using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace mycon_recorder
{
    public partial class Form1 : Form
    {
        private UdpClient _udpClient = null;
        private const int _port = 59630;
        private const int _port_offset_for_debug = 1;
        private System.Diagnostics.Stopwatch _sw;
        private bool _recording = false;
        private bool _recording_waiting = false;
        private long _recording_started = 0;
        private bool _playing = false;
        private long _playing_started = 0;
        private int _playing_index = 0;
        private Color _button_color_default;

        // 再生スレッド制御
        private Thread _playThread;
        private volatile bool _playThreadRunning = false;
        private readonly object _playLock = new object();

        // 高精度タイマー（winmm.dll）
        [DllImport("winmm.dll")]
        private static extern uint timeBeginPeriod(uint period);
        [DllImport("winmm.dll")]
        private static extern uint timeEndPeriod(uint period);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBoxMessageLog.Items.Clear();
            labelLatestMessage.Text = "";
            _sw = System.Diagnostics.Stopwatch.StartNew();

            // システムタイマー精度を1msに向上
            timeBeginPeriod(1);

            bool success = false;
            int port_offset = 0;
            do
            {
                try
                {
                    IPEndPoint localEP = new IPEndPoint(IPAddress.Any, _port + port_offset);
                    _udpClient = new UdpClient(localEP);
                    _udpClient.BeginReceive(ReceiveCallback, _udpClient);
                    success = true;
                }
                catch
                {
                    port_offset++;
                }
            } while (!success && port_offset < 100);

            _button_color_default = buttonRec.BackColor;
            labelPlayTime.Text = "";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timeEndPeriod(1);
            _playThreadRunning = false;
            _playThread?.Join();
            _udpClient?.Close();
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient udp = (UdpClient)ar.AsyncState;
            IPEndPoint remoteEP = null;
            byte[] rcvBytes;
            try
            {
                rcvBytes = udp.EndReceive(ar, ref remoteEP);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("ERROR: udp receive({0}/{1})", ex.Message, ex.ErrorCode);
                return;
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            string rcvMsg = Encoding.UTF8.GetString(rcvBytes);
            string recvTimeBase = String.Format("{0,8}", (_sw.ElapsedMilliseconds / 1000.0).ToString("0.000"));

            if (_recording && _recording_waiting)
            {
                _recording_started = _sw.ElapsedMilliseconds;
                _recording_waiting = false;
            }

            this.Invoke(new Action<string>(SetMessage), recvTimeBase + " " + rcvMsg);

            if (rcvMsg.EndsWith("H"))
            {
                string sendMsg = "H";
                byte[] sendBytes = Encoding.UTF8.GetBytes(sendMsg);
                udp.Send(sendBytes, sendBytes.Length, remoteEP.Address.ToString(), remoteEP.Port);
            }
            else if (_recording)
            {
                string recvTimeElapsed = String.Format("{0,8}", ((_sw.ElapsedMilliseconds - _recording_started) / 1000.0).ToString("0.000"));
                this.Invoke(new Action<string>(AppendMessageLog), recvTimeElapsed + " " + rcvMsg);
            }

            udp.BeginReceive(ReceiveCallback, udp);
        }

        private void SetMessage(string msg)
        {
            labelLatestMessage.Text = msg;
        }

        private void AppendMessageLog(string msg)
        {
            listBoxMessageLog.Items.Add(msg);
            listBoxMessageLog.SelectedIndex = listBoxMessageLog.Items.Count - 1;
        }

        private void buttonRec_Click(object sender, EventArgs e)
        {
            if (_recording)
            {
                _recording = false;
                buttonRec.BackColor = _button_color_default;
                fileToolStripMenuItem.Enabled = true;
                buttonPlay.Enabled = true;
            }
            else
            {
                listBoxMessageLog.Items.Clear();
                _recording_waiting = checkBoxWaiging.Checked;
                if (!_recording_waiting)
                {
                    _recording_started = _sw.ElapsedMilliseconds;
                }
                _recording = true;
                buttonRec.BackColor = Color.Red;
                buttonPlay.Enabled = false;
                fileToolStripMenuItem.Enabled = false;
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            SetPlay(!_playing);
        }

        private void SetPlay(bool play_mode)
        {
            if (!play_mode)
            {
                // === 停止処理 ===
                lock (_playLock)
                {
                    _playing = false;
                    _playThreadRunning = false;
                }

                _playThread?.Join();

                this.Invoke((MethodInvoker)(() =>
                {
                    buttonPlay.BackColor = _button_color_default;
                    buttonRec.Enabled = true;
                    textBoxIPAddr.ReadOnly = false;
                    listBoxMessageLog.Enabled = true;
                    fileToolStripMenuItem.Enabled = true;
                    labelPlayTime.Text = "";
                }));
            }
            else
            {
                // === 開始処理 ===
                lock (_playLock)
                {
                    _playing_started = _sw.ElapsedMilliseconds + (long)(numericUpDown1.Value * 1000);
                    _playing_index = 0;
                    _playing = true;
                    _playThreadRunning = true;
                }

                _playThread = new Thread(PlayWorker);
                _playThread.IsBackground = true;
                _playThread.Start();

                this.Invoke((MethodInvoker)(() =>
                {
                    buttonPlay.BackColor = Color.Green;
                    buttonRec.Enabled = false;
                    textBoxIPAddr.ReadOnly = true;
                    listBoxMessageLog.Enabled = false;
                    fileToolStripMenuItem.Enabled = false;
                }));
            }
        }

        private void PlayWorker()
        {
            IPEndPoint remoteEP = null;
            try
            {
                remoteEP = new IPEndPoint(IPAddress.Parse(textBoxIPAddr.Text), _port + _port_offset_for_debug);
            }
            catch
            {
                BeginInvoke((MethodInvoker)(() => SetPlay(false)));
                return;
            }

            long startGlobal = _sw.ElapsedMilliseconds;
            long delayMs = (long)(numericUpDown1.Value * 1000);

            while (_playThreadRunning)
            {
                long currentGlobal = _sw.ElapsedMilliseconds;
                long elapsedMs = currentGlobal - startGlobal + delayMs;

                bool shouldStop = false;
                int currentIndex = 0;
                long nextTargetMs = 0;

                lock (_playLock)
                {
                    if (!_playing || listBoxMessageLog.Items.Count <= _playing_index)
                    {
                        shouldStop = true;
                    }
                    else
                    {
                        currentIndex = _playing_index;
                        string item = listBoxMessageLog.Items[_playing_index].ToString();
                        double timeRecordSec = double.Parse(item.Substring(0, 8).Trim());
                        nextTargetMs = (long)(timeRecordSec * 1000);

                        if (nextTargetMs <= elapsedMs)
                        {
                            string sendMsg = item.Substring(9);
                            byte[] sendBytes = Encoding.UTF8.GetBytes(sendMsg);
                            try
                            {
                                _udpClient.Send(sendBytes, sendBytes.Length, remoteEP);
                            }
                            catch { }
                            _playing_index++;
                        }
                    }
                }

                if (shouldStop)
                {
                    BeginInvoke((MethodInvoker)(() => SetPlay(false)));
                    return;
                }

                // UI更新（非同期）
                try
                {
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        if (listBoxMessageLog.Items.Count > currentIndex)
                            listBoxMessageLog.SelectedIndex = currentIndex;
                        labelPlayTime.Text = (elapsedMs / 1000.0).ToString("0.000");
                    }));
                }
                catch { }

                // スピンロックで正確なタイミングまで待機
                if (nextTargetMs > elapsedMs)
                {
                    long targetGlobal = startGlobal + nextTargetMs - delayMs;
                    while (_sw.ElapsedMilliseconds < targetGlobal && _playThreadRunning)
                    {
                        Thread.SpinWait(500); // 約0.05～0.1ms待機
                    }
                }

                Thread.Yield();
            }
        }

        private void saveAsSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxMessageLog.Items.Count == 0) return;

            DateTime now = DateTime.Now;
            string time_now = now.ToString("yyyyMMdd_HHmmss");
            string exe_dir = Path.GetDirectoryName(Application.ExecutablePath) + @"\";

            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = time_now + ".txt",
                InitialDirectory = exe_dir,
                Filter = "TEXTファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*",
                Title = "保存先を選択",
                RestoreDirectory = true,
                OverwritePrompt = true,
                CheckPathExists = true
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(sfd.FileName, false))
                {
                    foreach (var item in listBoxMessageLog.Items)
                        writer.WriteLine(item);
                }
            }
        }

        private void openOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string exe_dir = Path.GetDirectoryName(Application.ExecutablePath) + @"\";

            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = exe_dir,
                Filter = "TEXTファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*",
                Title = "開くファイルを選択",
                RestoreDirectory = true,
                CheckFileExists = true,
                CheckPathExists = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                listBoxMessageLog.Items.Clear();
                using (StreamReader reader = new StreamReader(ofd.FileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBoxMessageLog.Items.Add(line);
                    }
                }
            }
        }
    }
}