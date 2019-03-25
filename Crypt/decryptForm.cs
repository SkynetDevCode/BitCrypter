using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace Crypt
{
    public partial class decryptForm : Form
    {
        Thread m_thread;
        DecryptThread m_Decrypt;
        bool m_bThreadRun;

        dialogs diag = new dialogs();
        public decryptForm()
        {
            InitializeComponent();
            m_bThreadRun = false;
        }

        private void buttonFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Encryped File (*.bcfile)|*.bcfile";
            dlg.RestoreDirectory = true;
            dlg.Title = "Open Encryped File";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBoxFile.Text = dlg.FileName;
            }
            else
            {
                textBoxFile.Text = "";
                return;
            }
        }

        private void buttonKey_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Private Key File (*.bcrsaprv)|*.bcrsaprv";
            dlg.RestoreDirectory = true;
            dlg.Title = "Open RSA Key File";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBoxKey.Text = dlg.FileName;
            }
            else
            {
                textBoxKey.Text = "";
                return;
            }
        }

        private void buttonDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBoxDirDest.Text = dlg.SelectedPath;
            }
            else
            {
                textBoxDirDest.Text = "";
                return;
            }
        }

        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            if (textBoxFile.Text.Length == 0)
            {
            
                return;
            }
            if (textBoxDirDest.Text.Length == 0)
            {

                return;
            }
            if (textBoxKey.Text.Length == 0)
            {
    
                return;
            }


            buttonDecrypt.Enabled = false;
            //buttonDecrypt.Visible = false;
            progressBar1.Visible = true;
            labelstate.Visible = true;
            UpdUI(false);
            m_Decrypt = new DecryptThread(this, DecryptCallback, textBoxFile.Text, textBoxDirDest.Text, textBoxKey.Text);
            m_thread = new Thread(new ThreadStart(m_Decrypt.DecryptStart));
            m_thread.Start();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_bThreadRun)
            {
                DialogResult dialogResult = MessageBox.Show("Decryption process is currently running. It cannot be cancelled." + Environment.NewLine + "If you click Yes, Decryption process will be aborted", "Are you sure to exit ?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (dialogResult == DialogResult.Yes)
                {
                    invkclosure = true;
                    if (m_bThreadRun) m_thread.Abort();


                }
                else if (dialogResult == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
                this.Dispose();
            }
        public static bool invkclosure = false;

        public static void DecryptCallback(object data, int iPct, string sStatus, bool bThreadRun)
        {
            decryptForm form = (decryptForm)data;
            if (invkclosure == true) return;

            if (form.InvokeRequired)
            {
              
                    DecryptCallback callback = new Crypt.DecryptCallback(DecryptCallback);
                    form.Invoke(callback, data, iPct, sStatus, bThreadRun);
               
            }
            else
            {
                if (sStatus == "Decryption Failure")
                {
                    form.UpdForm(100, sStatus, 2);
                    form.NotifIcnShow("Failed to decrypt " + form.textBoxFile.Text, "Decryption Failure", "error");
                }
                else
                {

                    form.UpdForm(iPct, sStatus, 1);
                    if (iPct == 100 && sStatus == "File Successfully Decrypted")
                    {
                        form.NotifIcnShow("File Directory : " + form.textBoxDirDest.Text, "File Successfully Decrypted", "info");
                    }
                }
                form.m_bThreadRun = bThreadRun;
                if (bThreadRun == false) form.UpdUI(true);
            }
        }

        protected void UpdForm(int iPct, string sStatus, int pbcolor)
        {
            progressBar1.Value = iPct;
            ModifyProgressBarColor.SetState(progressBar1, pbcolor);

            labelstate.Text = "Status:" + sStatus;
        }

        protected void UpdUI(bool bEnable)
        {
            buttonDecrypt.Enabled = bEnable;
            buttonFile.Enabled = bEnable;
            buttonDir.Enabled = bEnable;
            buttonKey.Enabled = bEnable;
        }
        protected void NotifIcnShow(string text, string title, string icon)
        {


            if (icon == "error")
            {
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Error;

            }
            else if (icon == "info")
            {
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            }
            else if (icon == "warning")
            {
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Warning;
            }
            notifyIcon1.BalloonTipText = text;
            notifyIcon1.BalloonTipTitle = title;
            notifyIcon1.ShowBalloonTip(5000);
        }
        private void Form3_Load(object sender, EventArgs e)
        {

        }
        public static class ModifyProgressBarColor
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
            static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
            public static void SetState(ProgressBar pBar, int state)
            {
                SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
  
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }
    }

    public delegate void DecryptCallback(object data, int iPct, string sStatus, bool bThreadRun);

    public class DecryptThread
    {
        object m_data;
        DecryptCallback m_DecryptCallback;
        string m_sFileSrc;
        string m_sDirDst;
        string m_sFileKey;

        public DecryptThread(object data, DecryptCallback callback, string sFileSrc, string sDirDst, string sFileKey)
        {
            m_data = data;
            m_DecryptCallback = callback;
            m_sFileSrc = sFileSrc;
            m_sDirDst = sDirDst;
            m_sFileKey = sFileKey;
        }

        public void DecryptStart()
        {
            try
            {
                Decrypt();
            }
            catch (ThreadAbortException)
            {
                return;
            }
        }

        private void Decrypt()
        {
            FileStream fs = null, fs1 = null;
            BinaryReader rd;
            int l, l1, iPct, iPct1;
            byte[] buf = new byte[8192];
            try
            {
                // Initialise RSA
                m_DecryptCallback(m_data, 0, "Initializing RSA...", true);
                fs = new FileStream(m_sFileKey, FileMode.Open, FileAccess.Read, FileShare.None);
                rd = new BinaryReader(fs);
                int iRSAkeySize = rd.ReadInt32();
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(iRSAkeySize);
                RSAParameters RSAKey = new RSAParameters();
                RSAKey = RSA.ExportParameters(true);
                RSAKey.D = rd.ReadBytes(RSAKey.D.Length);
                RSAKey.DP = rd.ReadBytes(RSAKey.DP.Length);
                RSAKey.DQ = rd.ReadBytes(RSAKey.DQ.Length);
                RSAKey.Exponent = rd.ReadBytes(RSAKey.Exponent.Length);
                RSAKey.InverseQ = rd.ReadBytes(RSAKey.InverseQ.Length);
                RSAKey.Modulus = rd.ReadBytes(RSAKey.Modulus.Length);
                RSAKey.P = rd.ReadBytes(RSAKey.P.Length);
                RSAKey.Q = rd.ReadBytes(RSAKey.Q.Length);
                rd.Close();
                fs.Close();
                RSA.ImportParameters(RSAKey);

                // Charge les données cryptées RSA (clé AES, signature de hachage et nom du fichier)
                fs = new FileStream(m_sFileSrc, FileMode.Open, FileAccess.Read, FileShare.None);
                rd = new BinaryReader(fs);
                l1 = 0;
                l = rd.ReadInt32(); l1 += 4;
                byte[] byEnAesKey = rd.ReadBytes(l); l1 += byEnAesKey.Length;
                l = rd.ReadInt32(); l1 += 4;
                byte[] byEnSha512 = rd.ReadBytes(l); l1 += byEnSha512.Length;
                l = rd.ReadInt32(); l1 += 4;
                byte[] byEnFileName = rd.ReadBytes(l); l1 += byEnFileName.Length;
                rd.Close();
                fs.Close();

                // Decrypte RSA
                m_DecryptCallback(m_data, 0, "Decrypting RSA...", true);
                byte[] byAesKey = RSA.Decrypt(byEnAesKey, false);
                byte[] bySha512 = RSA.Decrypt(byEnSha512, false);
                byte[] byFileName = RSA.Decrypt(byEnFileName, false);
                StringBuilder sbFileName = new StringBuilder();
                for (l = 0; l < byFileName.Length; l++) sbFileName.Append(Convert.ToChar(byFileName[l]));
                string sFileName = sbFileName.ToString();

                // Decrypte AES
                fs = new FileStream(m_sFileSrc, FileMode.Open, FileAccess.Read, FileShare.None);
                fs.Position = (long)l1;
                fs1 = new FileStream(m_sDirDst + "\\" + sFileName, FileMode.Create, FileAccess.Write, FileShare.None);
                byte[] IV = new byte[16];
                RijndaelManaged csp = new RijndaelManaged();
                CryptoStream cs = new CryptoStream(fs1, csp.CreateDecryptor(byAesKey, IV), CryptoStreamMode.Write);
                l1 = iPct1 = 0;
                while ((l = fs.Read(buf, 0, 8192)) > 0)
                {
                    cs.Write(buf, 0, l);

                    // Pour progress bar
                    l1 += l;
                    iPct = (int)((double)l1 / (double)fs.Length * 100.0);
                    if (iPct > iPct1)
                    {
                        m_DecryptCallback(m_data, iPct, "Decrypting AES...", true);
                        iPct1 = iPct;
                    }
                }
                cs.Close();
                fs.Close();
                fs1.Close();

                // Contrôle de la signature de hachage
                m_DecryptCallback(m_data, 0, "Hashing SHA512...", true);
                fs = new FileStream(m_sDirDst + "\\" + sFileName, FileMode.Open, FileAccess.Read, FileShare.None);
                SHA512Managed sha = new SHA512Managed();
                byte[] bySha512verif = new byte[64];
                bySha512verif = sha.ComputeHash(fs);
                fs.Close();
                bool bValid = true;
                for (l = 0; l < 64; l++)
                {
                    if (bySha512verif[l] != bySha512[l])
                    {
                        bValid = false;
                        break;
                    }
                }
                if (bValid)
                {
                    m_DecryptCallback(m_data, 100, "File Successfully Decrypted", false);
                    fs.Close();
                    fs1.Close();
                }
                else
                {
                    m_DecryptCallback(m_data, 100, "Decryption Failure", false);
                }
            }
            //MessageBox.Show("Decryption Failure !", "BitCrypter", MessageBoxButtons.OK, MessageBoxIcon.Error);

            catch (CryptographicException)
            {
                m_DecryptCallback(m_data, 100, "Decryption Failure", false);
                MessageBox.Show("Decryption Failure !", "BitCrypter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fs.Close();
                fs1.Close();
            }
            catch (Exception e)
            {

                m_DecryptCallback(m_data, 100, "Decryption Failure", false);
                MessageBox.Show("Decryption Failure !" + Environment.NewLine + Environment.NewLine + e.Message, "BitCrypter", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}