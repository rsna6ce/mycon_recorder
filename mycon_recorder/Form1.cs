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

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBoxMessageLog.Items.Clear();
            labelLatestMessage.Text = "";
            _sw = System.Diagnostics.Stopwatch.StartNew();

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
                    //retry another port
                    port_offset++;
                }
            } while (!success);

            _button_color_default = buttonRec.BackColor;
            labelPlayTime.Text = "";
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient udp = (UdpClient)ar.AsyncState;

            System.Net.IPEndPoint remoteEP = null;
            byte[] rcvBytes;
            try
            {
                rcvBytes = udp.EndReceive(ar, ref remoteEP);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Console.WriteLine("ERROR: udp receive({0}/{1})", ex.Message, ex.ErrorCode);
                return;
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine("ERROR: udp socket closed.({0})", ex.Message);
                return;
            }

            string rcvMsg = System.Text.Encoding.UTF8.GetString(rcvBytes);
            string recvTimeBase = String.Format("{0, 8}", (_sw.ElapsedMilliseconds / 1000.0).ToString("0.000"));
            if (_recording && _recording_waiting)
            {
                _recording_started = _sw.ElapsedMilliseconds;
                _recording_waiting = false;
            }
            this.Invoke(new Action<string>(this.SetMessage), recvTimeBase + " " + rcvMsg);

            if (rcvMsg.EndsWith("H"))
            {
                string sendMsg = "H";
                byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(sendMsg);
                udp.Send(sendBytes, sendBytes.Length, remoteEP.Address.ToString(), remoteEP.Port);
            }
            else if (_recording)
            {
                string recvTimeElapsed = String.Format("{0, 8}", ((_sw.ElapsedMilliseconds- _recording_started) / 1000.0).ToString("0.000"));
                this.Invoke(new Action<string>(this.AppendMessageLog), recvTimeElapsed + " " + rcvMsg);
            }
            udp.BeginReceive(ReceiveCallback, udp);
        }

        private void SetMessage(string msg)
        {
            this.labelLatestMessage.Text = msg;
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
                // stop rec
                _recording = false;
                buttonRec.BackColor = _button_color_default;
                fileToolStripMenuItem.Enabled = true;
                buttonPlay.Enabled = true;
            }
            else
            {
                // start rec
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
                //stop play
                buttonPlay.BackColor = _button_color_default;
                buttonRec.Enabled = true;
                textBoxIPAddr.ReadOnly = false;
                listBoxMessageLog.Enabled = true;
                fileToolStripMenuItem.Enabled = true;
                _playing = false;
            }
            else
            {
                // start play
                buttonPlay.BackColor = Color.Green;
                buttonRec.Enabled = false;
                _playing_started = _sw.ElapsedMilliseconds + (long)(numericUpDown1.Value * 1000);
                textBoxIPAddr.ReadOnly = true;
                listBoxMessageLog.Enabled = false;
                fileToolStripMenuItem.Enabled = false;
                _playing_index = 0;
                _playing = true;
            }

        }

        private void timerPlay_Tick(object sender, EventArgs e)
        {
            if (!_playing)
            {
                return;
            }
            if (listBoxMessageLog.Items.Count <= _playing_index)
            {
                SetPlay(false);
                return;
            }
            if (listBoxMessageLog.SelectedIndex != _playing_index)
            {
                listBoxMessageLog.SelectedIndex = _playing_index;
            }
            long time_curr = _sw.ElapsedMilliseconds;
            long time_elap_ms = time_curr - _playing_started;
            labelPlayTime.Text = ((double)time_elap_ms / 1000.0).ToString("0.000");
            double time_record_sec = double.Parse(listBoxMessageLog.Items[_playing_index].ToString().Substring(0,8));
            long time_record_ms = (long)(time_record_sec * 1000);
            if (time_record_ms <= time_elap_ms)
            {
                System.Net.IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(textBoxIPAddr.Text), _port + _port_offset_for_debug);
                string sendMsg = listBoxMessageLog.Items[_playing_index].ToString().Substring(9);
                byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes(sendMsg);
                _udpClient.Send(sendBytes, sendBytes.Length, remoteEP.Address.ToString(), remoteEP.Port);
                _playing_index++;
            }
        }

        private void saveAsSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxMessageLog.Items.Count == 0)
            {
                return;
            }
            DateTime now = DateTime.Now;
            var time_now = now.ToString("yyyyMMdd_HHmmss");
            string exe_dir = Path.GetDirectoryName(Application.ExecutablePath) + @"\";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = time_now + ".txt";
            sfd.InitialDirectory = exe_dir;
            sfd.Filter = "TEXTファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            sfd.Title = "Select file to save.";
            sfd.RestoreDirectory = true;
            sfd.OverwritePrompt = true;
            sfd.CheckPathExists = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(sfd.FileName, false))
                {
                    foreach(var item in listBoxMessageLog.Items) {
                        writer.WriteLine(item);
                    }
                }
            }
        }

        private void openOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string exe_dir = Path.GetDirectoryName(Application.ExecutablePath) + @"\";

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "";
            ofd.InitialDirectory = exe_dir;
            ofd.Filter = "TEXTファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            ofd.Title = "Select file to open.";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                listBoxMessageLog.Items.Clear();
                using (StreamReader reader = new StreamReader(ofd.FileName, false))
                {
                    while (reader.Peek() >= 0)
                    {
                        listBoxMessageLog.Items.Add(reader.ReadLine());
                    }
                }
            }
        }
    }
}
