using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTP_Snipping_Tool
{
    public partial class configure : Form
    {
        public configure()
        {
            InitializeComponent();
        }

        private void configure_Load(object sender, EventArgs e)
        {
            txtHost.Text = Properties.Settings.Default.host;
            txtPort.Text = Properties.Settings.Default.port.ToString();
            txtUsername.Text = Properties.Settings.Default.username;
            txtPassword.Text = Properties.Settings.Default.password;
            txtScreenshotpath.Text = Properties.Settings.Default.screenshotPath;
            txtWebURL.Text = Properties.Settings.Default.webURL;
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            txtHost.Text = Properties.Settings.Default.host;
            txtPort.Text = Properties.Settings.Default.port.ToString();
            txtUsername.Text = Properties.Settings.Default.username;
            txtPassword.Text = Properties.Settings.Default.password;
            txtScreenshotpath.Text = Properties.Settings.Default.screenshotPath;
            txtWebURL.Text = Properties.Settings.Default.webURL;
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            txtHost.Text = "IP or domain";
            txtPort.Text = "22";
            txtUsername.Text = "username";
            txtPassword.Text = "password";
            txtScreenshotpath.Text = "E.G. /var/www/screenshots.darcyjprojects.xyz/";
            txtWebURL.Text = "E.G. https://screenshots.darcyjprojects.xyz";
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtPort.ForeColor == Color.Red)
            {
                MessageBox.Show("Invalid port! Must be a number between 0 and 65536 with no letters, symbols or special characters - numbers only!\n\nYour configuration has not been saved. Please fix the port then try again.", "Invalid Port!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            } else
            {
                Properties.Settings.Default.host = txtHost.Text;
                Properties.Settings.Default.port = int.Parse(txtPort.Text);
                Properties.Settings.Default.username = txtUsername.Text;
                Properties.Settings.Default.password = txtPassword.Text;
                Properties.Settings.Default.screenshotPath = txtScreenshotpath.Text;
                Properties.Settings.Default.webURL = txtWebURL.Text;
                Properties.Settings.Default.Save();
                MessageBox.Show("Saved!", "Configuration Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Form snippedfrm = new snipped();
            snippedfrm.Show();
            this.Hide();
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int.Parse(txtPort.Text);
                txtPort.ForeColor = Color.Black;
            } catch
            {
                txtPort.ForeColor = Color.Red;
            }
        }

        private void configure_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                txtPassword.PasswordChar = '*';
            } else
            {
                Char nullChar = new char();
                txtPassword.PasswordChar = nullChar;
            }
        }
    }
}
