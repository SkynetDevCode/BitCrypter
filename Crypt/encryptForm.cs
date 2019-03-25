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
    public partial class encryptForm : Form
    {
       

        Thread m_thread;
        EncryptThread m_Encrypt;
        bool m_bThreadRun;

        dialogs diag = new dialogs();
        public encryptForm()
        {
            InitializeComponent();
            m_bThreadRun = false;
        }

        private void buttonFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;
            dlg.Title = "File to Encrypt";
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
            dlg.Filter = "Public Key File (*.bcrsapb)|*.bcrsapb";
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

        private void buttonFileDest_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Encryped File (*.bcfile)|*.bcfile";
            dlg.RestoreDirectory = true;
            dlg.Title = "Save Encryped File";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBoxFileDest.Text = dlg.FileName;
            }
            else
            {
                textBoxFileDest.Text = "";
                return;
            }
        }
        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            if (textBoxFile.Text.Length == 0)
            {
             
                return;
            }
            if (textBoxFileDest.Text.Length == 0)
            {
          
                return;
            }
            if (textBoxKey.Text.Length == 0)
            {
               
                return;
            }

            invkclosure = false;
            buttonEncrypt.Enabled = false;
            progressBar1.Visible = true;
            labelstate.Visible = true;

            UpdUI(false);
            m_Encrypt = new EncryptThread(this, EncryptCallback, textBoxFile.Text, textBoxFileDest.Text, textBoxKey.Text);
            m_thread = new Thread(new ThreadStart(m_Encrypt.EncryptStart));
            m_thread.Start();
        }
       public static bool invkclosure = false;
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_bThreadRun)
            {
                
                DialogResult dialogResult = MessageBox.Show("Encryption process is currently running. It cannot be cancelled." + Environment.NewLine + "If you click Yes, Encryption process will be aborted", "Are you sure to exit ?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
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

        public static void EncryptCallback(object data, int iPct, string sStatus, bool bThreadRun)
        {
            encryptForm form = (encryptForm)data;
            if (invkclosure == true) return;
          


                if (form.InvokeRequired)
            {
              
                    EncryptCallback callback = new Crypt.EncryptCallback(EncryptCallback);
                    form.Invoke(callback, data, iPct, sStatus, bThreadRun);
                
            }
            else
            {
                if (sStatus == "Encryption Failure")
                {
                    form.UpdForm(iPct, sStatus, 2);
                    form.NotifIcnShow("Failed to encrypt " + form.textBoxFile.Text, "Encryption Failure", "error");
                }
                else
                {
                    form.UpdForm(iPct, sStatus, 1);
                    if (iPct == 100 && sStatus == "File Successfully Encrypted")
                    {
                        form.NotifIcnShow(form.textBoxFileDest.Text, "File Successfully Encrypted", "info");
                    }
                }
                
                form.m_bThreadRun = bThreadRun;
                if (bThreadRun == false) form.UpdUI(true);
            }
        }

        protected void UpdForm(int iPct, string sStatus,int pbcolor)
        {
            ModifyProgressBarColor.SetState(progressBar1, pbcolor);
            progressBar1.Value = iPct;
            labelstate.Text = "Status: " + sStatus;
            notifyIcon1.Text = sStatus;
        }

        protected void UpdUI(bool bEnable)
        {
            buttonEncrypt.Enabled = bEnable;
            buttonFile.Enabled = bEnable;
            buttonFileDest.Enabled = bEnable;
            buttonKey.Enabled = bEnable;
            if (bEnable == true){
                buttonAbort.Enabled = false;
            }
            else
            {
                buttonAbort.Enabled = true;
            }
           
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
        private void Form2_Load(object sender, EventArgs e)
        {

        }
      

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            invkclosure = true;
            if (m_bThreadRun) m_thread.Abort();
            if (m_bThreadRun == false)
            {
                buttonEncrypt.Enabled = true;
                buttonFile.Enabled = true;
                buttonFileDest.Enabled = true;
                buttonKey.Enabled = true;
                buttonAbort.Enabled = false;
            }

        }
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
    public delegate void EncryptCallback(object data, int iPct, string sStatus, bool bThreadRun);

    public class EncryptThread
    {
        object m_data;
        EncryptCallback m_EncryptCallback;
        string m_sFileSrc;
        string m_sFileDst;
        string m_sFileKey;

        public EncryptThread(object data, EncryptCallback callback, string sFileSrc, string sFileDst, string sFileKey)
        {
            m_data = data;
            m_EncryptCallback = callback;
            m_sFileSrc = sFileSrc;
            m_sFileDst = sFileDst;
            m_sFileKey = sFileKey;
        }

        public void EncryptStart()
        {
            try
            {
                Encrypt();
            }
            catch (ThreadAbortException)
            {
                return;
            }
        }

        private void Encrypt()
        {
            FileStream fs, fs1;
            int l, l1, iPct, iPct1;
            byte[] buf = new byte[8192];
            try
            {
                // Clé secrète aléatoire pour AES
                byte[] byAesKey = new byte[32];
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetBytes(byAesKey);

                // Signature de hachage SHA512
                m_EncryptCallback(m_data, 0, "Hashing SHA512", true);
                SHA512Managed sha = new SHA512Managed();
                byte[] bySha512 = new byte[64];
                fs = new FileStream(m_sFileSrc, FileMode.Open, FileAccess.Read);
                bySha512 = sha.ComputeHash(fs);
                fs.Close();

                // Initialise RSA
                m_EncryptCallback(m_data, 0, "Initializing RSA...", true);
                fs = new FileStream(m_sFileKey, FileMode.Open, FileAccess.Read, FileShare.None);
                BinaryReader rd = new BinaryReader(fs);
                int iRSAkeySize = rd.ReadInt32();
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(iRSAkeySize);
                RSAParameters RSAKey = new RSAParameters();
                RSAKey = RSA.ExportParameters(false);
                RSAKey.Modulus = rd.ReadBytes(RSAKey.Modulus.Length);
                RSAKey.Exponent = rd.ReadBytes(RSAKey.Exponent.Length);
                rd.Close();
                fs.Close();
                RSA.ImportParameters(RSAKey);

                // Encrypte RSA
                // ATTENTION : taille maxi du tampon d'encryptage = 245 pour iRSAkeySize = 2048 (501 pour 4096)
                // l = RSAKey.Modulus.Length - 11;
                // voir RSACryptoServiceProvider.Encrypt Method
                m_EncryptCallback(m_data, 0, "Encrypting RSA...", true);
                byte[] byEnAesKey = RSA.Encrypt(byAesKey, false);
                byte[] byEnSha512 = RSA.Encrypt(bySha512, false);
                string sFilename = Path.GetFileName(m_sFileSrc);
                byte[] byFileName = new byte[sFilename.Length];
                for (l = 0; l < sFilename.Length; l++) byFileName[l] = Convert.ToByte(sFilename[l]);
                byte[] byEnFileName = RSA.Encrypt(byFileName, false);

                fs = new FileStream(m_sFileDst, FileMode.Create, FileAccess.Write, FileShare.None);
                BinaryWriter wr = new BinaryWriter(fs);
                wr.Write(byEnAesKey.Length);
                wr.Write(byEnAesKey);
                wr.Write(byEnSha512.Length);
                wr.Write(byEnSha512);
                wr.Write(byEnFileName.Length);
                wr.Write(byEnFileName);
                wr.Close();
                fs.Close();

                // Encrypte AES
                fs = new FileStream(m_sFileDst, FileMode.Append, FileAccess.Write, FileShare.None);
                fs1 = new FileStream(m_sFileSrc, FileMode.Open, FileAccess.Read);
                byte[] IV = new byte[16];
                RijndaelManaged csp = new RijndaelManaged();
                CryptoStream cs = new CryptoStream(fs, csp.CreateEncryptor(byAesKey, IV), CryptoStreamMode.Write);
                l1 = iPct1 = 0;
                while ((l = fs1.Read(buf, 0, 8192)) > 0)
                {
                    cs.Write(buf, 0, l);

                    // Pour progress bar
                    l1 += l;
                    iPct = (int)((double)l1 / (double)fs1.Length * 100.0);
                    if (iPct > iPct1)
                    {
                        m_EncryptCallback(m_data, iPct, "Encrypting AES...", true);
                        iPct1 = iPct;
                    }
                }
                cs.Close();
                fs.Close();
                fs1.Close();

                
                m_EncryptCallback(m_data, 100, "File Successfully Encrypted", false);
             
            }
            catch (Exception e)
            {
             
                m_EncryptCallback(m_data, 100, "Encryption Failure", false);
                MessageBox.Show("Encryption Failure !" + Environment.NewLine + Environment.NewLine + e.Message, "BitCrypter", MessageBoxButtons.OK, MessageBoxIcon.Error);
             
            }
        }
    }

}