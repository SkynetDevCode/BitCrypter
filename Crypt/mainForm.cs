using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections;
using System.Diagnostics;
using Microsoft.Win32;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.IO;

namespace Crypt
{

    public partial class mainForm : Form
    {
        private const string APP_EXTENSION = ".bcrsapb";
        private const string PROG_ID = "BitCrypterPublic";
private const string FILE_DESCRIPTION = "BitCrypter RSA Public Key";

        private const string APP_EXTENSION1 = ".bcrsaprv";
        private const string PROG_ID1 = "BitCrypterPrivate";
        private const string FILE_DESCRIPTION1 = "BitCrypter RSA Private Key";

        private const string APP_EXTENSION2 = ".bcfile";
        private const string PROG_ID2 = "BitCrypterFile";
        private const string FILE_DESCRIPTION2 = "BitCrypter File";

        int m_iRSAkeySize = 2048; // Valeurs usuelles : 1024 2048 4096

        public mainForm()
        {
            InitializeComponent();
        }

        private void buttonKey_Click(object sender, EventArgs e)
        {
            string sFilePublicKey, sFilePrivateKey;

            m_iRSAkeySize = Int32.Parse(comboBoxSize.Text);

            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Public Key File (*.bcrsapb)|*.bcrsapb";
                dlg.RestoreDirectory = true;
                dlg.Title = "Save RSA Public Key";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    sFilePublicKey = dlg.FileName;
                }
                else
                {
                    return;
                }
            }
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Save RSA Private Key (*.bcrsaprv)|*.bcrsaprv";
                dlg.RestoreDirectory = true;
                dlg.Title = "Save RSA Private Key";
                if ((dlg.ShowDialog() == DialogResult.OK))
                {
                    sFilePrivateKey = dlg.FileName;
                }
                else
                {
                    return;
                }
            }

            Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            RSACryptoServiceProvider RSA1 = new RSACryptoServiceProvider(m_iRSAkeySize);
            RSAParameters RSAKey1 = new RSAParameters();
            RSAParameters RSAKey2 = new RSAParameters();

            RSAKey1 = RSA1.ExportParameters(false);           //  Public / encrypt
            RSAKey2 = RSA1.ExportParameters(true);            //  Private / decrypt

            FileStream fs = new FileStream(sFilePublicKey, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            BinaryWriter swb = new BinaryWriter(fs);
            swb.Write(m_iRSAkeySize);
            swb.Write(RSAKey1.Modulus);
            swb.Write(RSAKey1.Exponent);
            swb.Close();
            fs.Close();

            fs = new FileStream(sFilePrivateKey, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            swb = new BinaryWriter(fs);
            swb.Write(m_iRSAkeySize);
            swb.Write(RSAKey2.D);
            swb.Write(RSAKey2.DP);
            swb.Write(RSAKey2.DQ);
            swb.Write(RSAKey2.Exponent);
            swb.Write(RSAKey2.InverseQ);
            swb.Write(RSAKey2.Modulus);
            swb.Write(RSAKey2.P);
            swb.Write(RSAKey2.Q);
            swb.Close();
            fs.Close();
            Cursor = Cursors.Default;

            MessageBox.Show("RSA Key Pair successfully created", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void SetAssociation(string Extension, string KeyName, string OpenWith, string FileDescription)
        {
            try
            {
                RegistryKey CurrentUser;
          

                // The stuff that was above here is basically the same
                RegistryKey BaseKey;
                RegistryKey OpenMethod;
                RegistryKey Shell;
 

                BaseKey = Registry.ClassesRoot.CreateSubKey(Extension);
                BaseKey.SetValue("", KeyName);

                OpenMethod = Registry.ClassesRoot.CreateSubKey(KeyName);
                OpenMethod.SetValue("", FileDescription);
                OpenMethod.CreateSubKey("DefaultIcon").SetValue("", Application.StartupPath + "\\bcicons.dll" + ",0");
                Shell = OpenMethod.CreateSubKey("Shell");
                Shell.CreateSubKey("edit").CreateSubKey("command").SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
                Shell.CreateSubKey("open").CreateSubKey("command").SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
                BaseKey.Close();
                OpenMethod.Close();
                Shell.Close();


                // Delete the key instead of trying to change it
                CurrentUser = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + Extension, true);
                CurrentUser.DeleteSubKey("UserChoice", false);

                CurrentUser.Close();

                // Tell explorer the file association has been changed
                SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);

                MessageBox.Show("Registry File Association Done", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
            catch (Exception ex)
            {
                MessageBox.Show("Registry File Association Error" + Environment.NewLine + ex.Message + ex.Source, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

}

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
        public void CreateDefaultIconAssociation()
        {
            try


            {
                if (!File.Exists(Application.StartupPath + "\\bcicons.dll"))
                {
                    MessageBox.Show("Registry File Association Error" + Environment.NewLine + "'bcicons.dll' doesn't exists anymore", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }



                //Check To See If Extension Exists. Create New Keys If It Doesn't
         
                if (Registry.CurrentUser.OpenSubKey("Software\\Classes\\" + APP_EXTENSION, true) == null)
                {
                    //Create The Extension Key
                    Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + APP_EXTENSION).SetValue("", PROG_ID, Microsoft.Win32.RegistryValueKind.String);

                    //Create The PROG_ID Key and File Description Value
                    Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + PROG_ID).SetValue("", FILE_DESCRIPTION);

                    //Create DefaultIcon And Set Default String To Icon File (ICO o r EXE and DLL With Perameters)
                    Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + PROG_ID + "\\DefaultIcon").SetValue("", Application.StartupPath + "\\bcicons.dll" + ",1");


                }


                if (Registry.CurrentUser.OpenSubKey("Software\\Classes\\" + APP_EXTENSION1, true) == null)
                {
                    //Create The Extension Key
                    Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + APP_EXTENSION1).SetValue("", PROG_ID1, Microsoft.Win32.RegistryValueKind.String);

                    //Create The PROG_ID Key and File Description Value
                    Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + PROG_ID1).SetValue("", FILE_DESCRIPTION1);

                    //Create DefaultIcon And Set Default String To Icon File (ICO o r EXE and DLL With Perameters)
                    Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + PROG_ID1 + "\\DefaultIcon").SetValue("", Application.StartupPath + "\\bcicons.dll" + ",2");


                }

                //if (Registry.CurrentUser.OpenSubKey("Software\\Classes\\" + APP_EXTENSION2, true) == null)
                //{
                //    //Create The Extension Key
                //    Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + APP_EXTENSION2).SetValue("", PROG_ID2, Microsoft.Win32.RegistryValueKind.String);

                //    //Create The PROG_ID Key and File Description Value
                //    Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + PROG_ID2).SetValue("", FILE_DESCRIPTION2);

                //    //Create DefaultIcon And Set Default String To Icon File (ICO o r EXE and DLL With Perameters)
                //    Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + PROG_ID2 + "\\DefaultIcon").SetValue("", Application.StartupPath + "\\bcicons.dll" + ",0");


                //}

                SetAssociation(".bcfile", "BitCrypterFile", Application.ExecutablePath + " /bcdecrypt /file=", "BitCrypter File");

              
            }
            catch (Exception)
            {
            }

        }

        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            encryptForm dlg = new encryptForm();
            dlg.ShowDialog();
            this.Enabled = true;
        }

        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            decryptForm dlg = new decryptForm();
            dlg.ShowDialog();
            this.Enabled = true;
        }
        public static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                      .IsInRole(WindowsBuiltInRole.Administrator);
        }

        dialogs diag = new dialogs();

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxSize.SelectedIndex = 2;

            if (IsAdministrator())
            {
                buttonReg.Visible = true;
                buttonReg.Enabled = true;
                buttonElev.Visible = false;
                buttonElev.Enabled = false;
            }
            else
            {
                buttonReg.Visible = false;
                buttonReg.Enabled = false;
                buttonElev.Visible = true;
                buttonElev.Enabled = true;
                AddShieldToButton(buttonElev);
            }

        }
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
    uint Msg, int wParam, int lParam);

        // Make the button display the UAC shield.
        public static void AddShieldToButton(Button btn)
        {
            const Int32 BCM_SETSHIELD = 0x160C;

            // Give the button the flat style and make it
            // display the UAC shield.
            btn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            SendMessage(btn.Handle, BCM_SETSHIELD, 0, 1);
        }
        private void button1_Click(object sender, System.EventArgs e)
        {
            CreateDefaultIconAssociation();
        }

        private void buttonElev_Click(object sender, EventArgs e)
        {
            buttonElev.Enabled = false;
            Cursor = Cursors.WaitCursor;
            elevateRights();
            Cursor = Cursors.Default;
            buttonElev.Enabled = true;

        }
        public static void elevateRights()
        {
              try
            {
                Process p = new Process();
                p.StartInfo.FileName = Application.ExecutablePath;
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.Verb = "runas";
                p.Start();
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Elevation Request Failed" + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
}

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process p = new Process();
                p.Start();
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            about frm = new about();
            frm.ShowDialog();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}