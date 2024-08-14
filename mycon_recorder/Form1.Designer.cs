
namespace mycon_recorder
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRec = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.checkBoxWaiging = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.listBoxMessageLog = new System.Windows.Forms.ListBox();
            this.labelLatestMessage = new System.Windows.Forms.Label();
            this.timerPlay = new System.Windows.Forms.Timer(this.components);
            this.textBoxIPAddr = new System.Windows.Forms.TextBox();
            this.labelPlayTime = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(263, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openOToolStripMenuItem,
            this.saveAsSToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.fileToolStripMenuItem.Text = "File(&F)";
            // 
            // openOToolStripMenuItem
            // 
            this.openOToolStripMenuItem.Name = "openOToolStripMenuItem";
            this.openOToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openOToolStripMenuItem.Text = "Open(&O)";
            this.openOToolStripMenuItem.Click += new System.EventHandler(this.openOToolStripMenuItem_Click);
            // 
            // saveAsSToolStripMenuItem
            // 
            this.saveAsSToolStripMenuItem.Name = "saveAsSToolStripMenuItem";
            this.saveAsSToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsSToolStripMenuItem.Text = "Save as (&S)";
            this.saveAsSToolStripMenuItem.Click += new System.EventHandler(this.saveAsSToolStripMenuItem_Click);
            // 
            // buttonRec
            // 
            this.buttonRec.Location = new System.Drawing.Point(4, 28);
            this.buttonRec.Name = "buttonRec";
            this.buttonRec.Size = new System.Drawing.Size(84, 36);
            this.buttonRec.TabIndex = 1;
            this.buttonRec.Text = "REC";
            this.buttonRec.UseVisualStyleBackColor = true;
            this.buttonRec.Click += new System.EventHandler(this.buttonRec_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(4, 68);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(84, 36);
            this.buttonPlay.TabIndex = 1;
            this.buttonPlay.Text = "PLAY";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // checkBoxWaiging
            // 
            this.checkBoxWaiging.AutoSize = true;
            this.checkBoxWaiging.Checked = true;
            this.checkBoxWaiging.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxWaiging.Location = new System.Drawing.Point(96, 40);
            this.checkBoxWaiging.Name = "checkBoxWaiging";
            this.checkBoxWaiging.Size = new System.Drawing.Size(120, 16);
            this.checkBoxWaiging.TabIndex = 2;
            this.checkBoxWaiging.Text = "Wait first message";
            this.checkBoxWaiging.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "start delay[s]";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(176, 68);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(48, 19);
            this.numericUpDown1.TabIndex = 3;
            // 
            // listBoxMessageLog
            // 
            this.listBoxMessageLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxMessageLog.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBoxMessageLog.FormattingEnabled = true;
            this.listBoxMessageLog.ItemHeight = 16;
            this.listBoxMessageLog.Items.AddRange(new object[] {
            "999.999 UDLRABCDLlRrET12H"});
            this.listBoxMessageLog.Location = new System.Drawing.Point(4, 140);
            this.listBoxMessageLog.Name = "listBoxMessageLog";
            this.listBoxMessageLog.Size = new System.Drawing.Size(252, 164);
            this.listBoxMessageLog.TabIndex = 3;
            // 
            // labelLatestMessage
            // 
            this.labelLatestMessage.BackColor = System.Drawing.SystemColors.Window;
            this.labelLatestMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelLatestMessage.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelLatestMessage.Location = new System.Drawing.Point(4, 120);
            this.labelLatestMessage.Name = "labelLatestMessage";
            this.labelLatestMessage.Size = new System.Drawing.Size(252, 20);
            this.labelLatestMessage.TabIndex = 4;
            this.labelLatestMessage.Text = "000.000 UDLRABCDLlRrET12H";
            // 
            // timerPlay
            // 
            this.timerPlay.Enabled = true;
            this.timerPlay.Interval = 1;
            this.timerPlay.Tick += new System.EventHandler(this.timerPlay_Tick);
            // 
            // textBoxIPAddr
            // 
            this.textBoxIPAddr.Location = new System.Drawing.Point(96, 88);
            this.textBoxIPAddr.Name = "textBoxIPAddr";
            this.textBoxIPAddr.Size = new System.Drawing.Size(128, 19);
            this.textBoxIPAddr.TabIndex = 6;
            this.textBoxIPAddr.Text = "192.168.0.255";
            // 
            // labelPlayTime
            // 
            this.labelPlayTime.AutoSize = true;
            this.labelPlayTime.Location = new System.Drawing.Point(28, 103);
            this.labelPlayTime.Name = "labelPlayTime";
            this.labelPlayTime.Size = new System.Drawing.Size(43, 12);
            this.labelPlayTime.TabIndex = 7;
            this.labelPlayTime.Text = "999.999";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 309);
            this.Controls.Add(this.labelPlayTime);
            this.Controls.Add(this.textBoxIPAddr);
            this.Controls.Add(this.labelLatestMessage);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.checkBoxWaiging);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonRec);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.listBoxMessageLog);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "mycon_recorder";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsSToolStripMenuItem;
        private System.Windows.Forms.Button buttonRec;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.CheckBox checkBoxWaiging;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxMessageLog;
        private System.Windows.Forms.Label labelLatestMessage;
        private System.Windows.Forms.Timer timerPlay;
        private System.Windows.Forms.TextBox textBoxIPAddr;
        private System.Windows.Forms.Label labelPlayTime;
    }
}

