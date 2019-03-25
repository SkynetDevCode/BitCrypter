using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Crypt
{
    public partial class about : Form
    {
        public about()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void about_Load(object sender, EventArgs e)
        {
            Label2.Text = "Version : " + Application.ProductVersion;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://sdvcd.fr");
            }
            catch (Exception)
            {

            }
        }
    }
}
