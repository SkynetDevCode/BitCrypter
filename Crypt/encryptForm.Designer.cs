namespace Crypt
{
    partial class encryptForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(encryptForm));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFile = new System.Windows.Forms.TextBox();
            this.buttonFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonKey = new System.Windows.Forms.Button();
            this.textBoxFileDest = new System.Windows.Forms.TextBox();
            this.buttonFileDest = new System.Windows.Forms.Button();
            this.buttonEncrypt = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelstate = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.buttonAbort = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File to Encrypt :";
            // 
            // textBoxFile
            // 
            this.textBoxFile.Location = new System.Drawing.Point(15, 25);
            this.textBoxFile.Name = "textBoxFile";
            this.textBoxFile.ReadOnly = true;
            this.textBoxFile.Size = new System.Drawing.Size(339, 20);
            this.textBoxFile.TabIndex = 1;
            // 
            // buttonFile
            // 
            this.buttonFile.Location = new System.Drawing.Point(361, 23);
            this.buttonFile.Name = "buttonFile";
            this.buttonFile.Size = new System.Drawing.Size(26, 23);
            this.buttonFile.TabIndex = 2;
            this.buttonFile.Text = "...";
            this.buttonFile.UseVisualStyleBackColor = true;
            this.buttonFile.Click += new System.EventHandler(this.buttonFile_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "Public RSA Key File :";
            // 
            // textBoxKey
            // 
            this.textBoxKey.Location = new System.Drawing.Point(15, 79);
            this.textBoxKey.Name = "textBoxKey";
            this.textBoxKey.ReadOnly = true;
            this.textBoxKey.Size = new System.Drawing.Size(339, 20);
            this.textBoxKey.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(15, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(375, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "File Encrypted :";
            // 
            // buttonKey
            // 
            this.buttonKey.Location = new System.Drawing.Point(361, 77);
            this.buttonKey.Name = "buttonKey";
            this.buttonKey.Size = new System.Drawing.Size(26, 23);
            this.buttonKey.TabIndex = 2;
            this.buttonKey.Text = "...";
            this.buttonKey.UseVisualStyleBackColor = true;
            this.buttonKey.Click += new System.EventHandler(this.buttonKey_Click);
            // 
            // textBoxFileDest
            // 
            this.textBoxFileDest.Location = new System.Drawing.Point(18, 142);
            this.textBoxFileDest.Name = "textBoxFileDest";
            this.textBoxFileDest.ReadOnly = true;
            this.textBoxFileDest.Size = new System.Drawing.Size(336, 20);
            this.textBoxFileDest.TabIndex = 1;
            // 
            // buttonFileDest
            // 
            this.buttonFileDest.Location = new System.Drawing.Point(361, 140);
            this.buttonFileDest.Name = "buttonFileDest";
            this.buttonFileDest.Size = new System.Drawing.Size(26, 23);
            this.buttonFileDest.TabIndex = 2;
            this.buttonFileDest.Text = "...";
            this.buttonFileDest.UseVisualStyleBackColor = true;
            this.buttonFileDest.Click += new System.EventHandler(this.buttonFileDest_Click);
            // 
            // buttonEncrypt
            // 
            this.buttonEncrypt.Location = new System.Drawing.Point(15, 221);
            this.buttonEncrypt.Name = "buttonEncrypt";
            this.buttonEncrypt.Size = new System.Drawing.Size(372, 23);
            this.buttonEncrypt.TabIndex = 5;
            this.buttonEncrypt.Text = "Encrypt";
            this.buttonEncrypt.UseVisualStyleBackColor = true;
            this.buttonEncrypt.Click += new System.EventHandler(this.buttonEncrypt_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 192);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(370, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // labelstate
            // 
            this.labelstate.AutoSize = true;
            this.labelstate.Location = new System.Drawing.Point(15, 176);
            this.labelstate.Name = "labelstate";
            this.labelstate.Size = new System.Drawing.Size(37, 13);
            this.labelstate.TabIndex = 7;
            this.labelstate.Text = "Status";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "BitCrypter";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // buttonAbort
            // 
            this.buttonAbort.Enabled = false;
            this.buttonAbort.Location = new System.Drawing.Point(15, 250);
            this.buttonAbort.Name = "buttonAbort";
            this.buttonAbort.Size = new System.Drawing.Size(372, 23);
            this.buttonAbort.TabIndex = 8;
            this.buttonAbort.Text = "Abort";
            this.buttonAbort.UseVisualStyleBackColor = true;
            this.buttonAbort.Visible = false;
            this.buttonAbort.Click += new System.EventHandler(this.button1_Click);
            // 
            // encryptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 247);
            this.Controls.Add(this.buttonAbort);
            this.Controls.Add(this.labelstate);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.buttonEncrypt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonFileDest);
            this.Controls.Add(this.buttonKey);
            this.Controls.Add(this.buttonFile);
            this.Controls.Add(this.textBoxFileDest);
            this.Controls.Add(this.textBoxKey);
            this.Controls.Add(this.textBoxFile);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "encryptForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BitCrypter - File Encryption";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFile;
        private System.Windows.Forms.Button buttonFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonKey;
        private System.Windows.Forms.TextBox textBoxFileDest;
        private System.Windows.Forms.Button buttonFileDest;
        private System.Windows.Forms.Button buttonEncrypt;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label labelstate;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button buttonAbort;
    }
}