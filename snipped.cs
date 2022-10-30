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

        //drawing
        int type = 0; //0 = pen, 1 = hl
        int colour = 0;
        Pen pen;
        Pen hl;
        int hlAlpha = 64;
        int penSize = 5;
        int hlSize = 30;
        Image cleanImage;
        Image cleanImage2;
        int f = 0;

        bool mouseDown;
        Point lastPoint;

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
            host = Properties.Settings.Default.host;
            url = Properties.Settings.Default.webURL;
            port = Properties.Settings.Default.port;
            username = Properties.Settings.Default.username;
            password = Properties.Settings.Default.password;
            initialPath = Properties.Settings.Default.screenshotPath;

            if (host == defaultHost || initialPath == defaultScreenshotPath || url == defaultwebURL)
            {
                MessageBox.Show("Default values still remain in the configuration!\nPlease open the configuration and enter your SFTP server details!", "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            pictureBox1.Image = Main.img;
            txtStatus.Text = "Image file copied to clipboard.";
            txtURL.Text = "Please click upload to generate link.";

            //set pen and hl to black
            pen = new Pen(Color.FromArgb(255, 0, 0, 0), penSize);
            hl = new Pen(Color.FromArgb(hlAlpha, 0, 0, 0), hlSize);
            resetText();
            btnBlack.Text = "•";

            //scale
            if (pictureBox1.Image.Width > pictureBox1.Width || pictureBox1.Image.Height > pictureBox1.Height)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            }
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
            pictureBox1.Image.Save(saveTo, ImageFormat.Png);
            txtStatus.Text = "Uploading image to SFTP server...";
            txtURL.Text = "Please wait...";
            upload();
        }

        public void upload()
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
                Clipboard.SetText(url + "/" + filename);
                txtStatus.Text = "Uploaded! Link copied to clipboard.";
                txtURL.Text = url + "/" + filename;
            
            
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

        private void btnHL_Click(object sender, EventArgs e)
        {
            if (btnPen.BackColor == Color.FromArgb(255, 128, 255, 128))
            {
                btnPen.BackColor = Color.White;
                btnHL.BackColor = Color.FromArgb(255, 128, 255, 128);
                type = 1;
            }
        }

        private void btnPen_Click(object sender, EventArgs e)
        {
            if (btnHL.BackColor == Color.FromArgb(255, 128, 255, 128))
            {
                btnHL.BackColor = Color.White;
                btnPen.BackColor = Color.FromArgb(255, 128, 255, 128);
                type = 0;
            }
        }

        public void resetText()
        {
            btnBlack.Text = "";
            btnRed.Text = "";
            btnOrange.Text = "";
            btnYellow.Text = "";
            btnGreen.Text = "";
            btnBlue.Text = "";
            btnPurple.Text = "";
            btnWhite.Text = "";
        }

        private void btnBlack_Click(object sender, EventArgs e)
        {
            pen = new Pen(Color.FromArgb(255, 0, 0, 0), penSize);
            hl = new Pen(Color.FromArgb(hlAlpha, 0, 0, 0), hlSize);
            resetText();
            btnBlack.Text = "•";
        }

        private void btnRed_Click(object sender, EventArgs e)
        {
            pen = new Pen(Color.FromArgb(255, 255, 0, 0), penSize);
            hl = new Pen(Color.FromArgb(hlAlpha, 255, 0, 0), hlSize);
            resetText();
            btnRed.Text = "•";
        }

        private void btnOrange_Click(object sender, EventArgs e)
        {
            pen = new Pen(Color.FromArgb(255, 255, 165, 0), penSize);
            hl = new Pen(Color.FromArgb(hlAlpha, 255, 165, 0), hlSize);
            resetText();
            btnOrange.Text = "•";
        }

        private void btnYellow_Click(object sender, EventArgs e)
        {
            pen = new Pen(Color.FromArgb(255, 255, 255, 0), penSize);
            hl = new Pen(Color.FromArgb(hlAlpha, 255, 255, 0), hlSize);
            resetText();
            btnYellow.Text = "•";
        }

        private void btnGreen_Click(object sender, EventArgs e)
        {
            pen = new Pen(Color.FromArgb(255, 0, 128, 0), penSize);
            hl = new Pen(Color.FromArgb(hlAlpha, 0, 128, 0), hlSize);
            resetText();
            btnGreen.Text = "•";
        }

        private void btnBlue_Click(object sender, EventArgs e)
        {
            pen = new Pen(Color.FromArgb(255, 0, 0, 255), penSize);
            hl = new Pen(Color.FromArgb(hlAlpha, 0, 0, 255), hlSize);
            resetText();
            btnBlue.Text = "•";
        }

        private void btnPurple_Click(object sender, EventArgs e)
        {
            pen = new Pen(Color.FromArgb(255, 128, 0, 128), penSize);
            hl = new Pen(Color.FromArgb(hlAlpha, 128, 0, 128), hlSize);
            resetText();
            btnPurple.Text = "•";
        }

        private void btnWhite_Click(object sender, EventArgs e)
        {
            pen = new Pen(Color.FromArgb(255, 255, 255, 255), penSize);
            hl = new Pen(Color.FromArgb(hlAlpha, 255, 255, 255), hlSize);
            resetText();
            btnWhite.Text = "•";
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = e.Location;
            mouseDown = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            lastPoint = e.Location;
            mouseDown = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown == true && lastPoint != null)
            {
                if (f == 0)
                {
                    cleanImage = (Image)Main.img.Clone();
                    cleanImage2 = (Image)cleanImage.Clone();
                    f = 1;
                } else if (f == 2)
                {
                    cleanImage = (Image)cleanImage2.Clone();
                }
                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {
                    switch (type)
                    {
                        case 0:
                            g.DrawLine(pen, lastPoint, e.Location);
                            break;
                        case 1:
                            g.DrawLine(hl, lastPoint, e.Location);
                            break;
                    }
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                }

                pictureBox1.Invalidate();
                lastPoint = e.Location;
            }
        }

        private void btnClearpen_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = cleanImage;
            f = 2;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
