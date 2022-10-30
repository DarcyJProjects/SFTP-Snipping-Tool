using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTP_Snipping_Tool
{
    public partial class about : Form
    {
        public about()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void link(string url)
        {
            var p = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(p);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string linkurl = "https://www.codeproject.com/info/cpol10.aspx";
            link(linkurl);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            link("https://www.codeproject.com/Articles/485883/Create-your-own-Snipping-Tool");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            link("https://www.codeproject.com");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            link("https://github.com/DarcyJProjects/SFTP-Snipping-Tool");
        }
    }
}
