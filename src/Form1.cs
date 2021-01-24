using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LicznikObiektow
{
    public partial class Form1 : Form
    {
        private Color[,] _image { get; set; }

        public Form1()
        {
            InitializeComponent();
            btn_AnalizaObrazu.Visible = false;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();

            if (pictureBox2.Image != null)
                pictureBox2.Image.Dispose();

            btn_AnalizaObrazu.Visible = false;
            using (var fd = new OpenFileDialog())
            {
                fd.Filter = "png files (*.png)|*.png|jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
                fd.RestoreDirectory = true;

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(fd.FileName);
                    _image = Imaging.ConvertFileTo2DArrayWithGrayScale(fd.FileName);
                    btn_AnalizaObrazu.Visible = true;
                }
            }
        }

        private async void btn_AnalizaObrazu_Click(object sender, EventArgs e)
        {
            Color[,] newImage = null;
            ImageInfo info = null;
            await Task.Run(() =>
            {
                info = Imaging.GetImageDetails2(_image);
                newImage = Imaging.DrawEdges2(_image, info.Groups);
            });

            var sfd = new SaveFileDialog();
            sfd.Filter = "bmp (*.bmp)|*.bmp|All files (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Imaging.DrawImage(sfd.FileName, newImage);
                pictureBox2.Image = Image.FromFile(sfd.FileName);
            }

            lbl_Found.Text = $"Odnaleziono {info.Groups.Count} obiektów";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBox1.Dispose();
            pictureBox2.Dispose();
        }
    }
}
