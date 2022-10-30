using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Renci.SshNet;

namespace FTP_Snipping_Tool
{
    public partial class snipped : Form
    {
        string host;
        string url;
        int port;
        string username;
        string password;
        string initialPath;

        string saveTo;

        const string defaultHost = @"IP or domain";
        const string defaultScreenshotPath = @"E.G. /var/www/screenshots.darcyjprojects.xyz/";
        const string defaultwebURL = @"E.G. https://screenshots.darcyjprojects.xyz";

        string filename = "";

        Image screenshot = Properties.Resources.placeholder;
        public snipped()
        {
            InitializeComponent();
        }

        private void snipped_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists(saveTo))
            {
                File.Delete(saveTo); //delete temp image
            }
            Application.Exit();
        }

        private void snipped_Load(object sender, EventArgs e)
        {
            string host = Properties.Settings.Default.host;
            string url = Properties.Settings.Default.webURL;
            int port = Properties.Settings.Default.port;
            string username = Properties.Settings.Default.username;
            string password = Properties.Settings.Default.password;
            string initialPath = Properties.Settings.Default.screenshotPath;

            if (host == defaultHost || initialPath == defaultScreenshotPath || url == defaultwebURL)
            {
                MessageBox.Show("Default values still remain in the configuration!\nPlease open the configuration and enter your SFTP server details!", "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            pictureBox1.Image = Main.img;
            screenshot = Main.img;
            txtStatus.Text = "Image file copied to clipboard.";
            txtURL.Text = "Please click upload to generate link.";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (File.Exists(saveTo))
            {
                File.Delete(saveTo); //delete temp image
            }
            Application.Exit();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int randomInt = rnd.Next(1, 1000);
            filename = DateTime.Now.ToString("ddMMyyyyHHmmss") + randomInt.ToString() + ".png";
            string tempDirectory = Environment.CurrentDirectory + "\\temp\\";
            saveTo = tempDirectory + filename;
            screenshot.Save(saveTo, ImageFormat.Png);
            txtStatus.Text = "Uploading image to SFTP server...";
            txtURL.Text = "Please wait...";
            upload();
        }

        public void upload()
        {   try
            {
                using (SftpClient client = new SftpClient(new PasswordConnectionInfo(host, port, username, password)))
                {
                    try
                    {
                        client.Connect();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occured when trying to connect to the supplied SFTP credentials.\n\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    try
                    {
                        client.ChangeDirectory(initialPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occured when trying to change the directory to the screenshot path provided (on the SFTP server).\n\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    try
                    {
                        using (Stream stream = File.OpenRead(saveTo))
                        {
                            client.BufferSize = 4 * 1024;
                            client.UploadFile(stream, Path.GetFileName(saveTo));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occured when trying to upload the screenshot to the SFTP server.\n\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    client.Disconnect(); //disconenct SFTP client
                }
                if (File.Exists(saveTo))
                {
                    File.Delete(saveTo); //delete temp image
                }
                Clipboard.SetText(url + "\\" + filename);
                txtStatus.Text = "Uploaded! Link copied to clipboard.";
                txtURL.Text = url + "\\" + filename;
            } catch (Exception ex)
            {
                MessageBox.Show("An error occured. Please check your configuration.\n\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtStatus.Text = "An error occured.";
                txtURL.Text = "An error occured.";
            }
            
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form configurefrm = new configure();
            configurefrm.Show();
            this.Hide();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form aboutfrm = new about();
            aboutfrm.Show();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Form main = new Main();
            main.Show();
            this.Hide();
        }
    }
}
