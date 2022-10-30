﻿using System;
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
    }
}
