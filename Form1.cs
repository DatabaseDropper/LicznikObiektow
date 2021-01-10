using System;
using System.Drawing;
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
            btn_AnalizaObrazu.Visible = false;
            using (var fd = new OpenFileDialog())
            {
                fd.Filter = "png files (*.png)|*.png|jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
                fd.RestoreDirectory = true;

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(fd.FileName);
                    _image = Imaging.ConvertFileTo2DArray(fd.FileName);
                    btn_AnalizaObrazu.Visible = true;
                }
            }
        }

        private void btn_AnalizaObrazu_Click(object sender, EventArgs e)
        {
            var info = Imaging.GetImageDetails(_image);

            var newImage = Imaging.DrawEdges(_image, info.Edges);

            var sfd = new SaveFileDialog(); 
            sfd.Filter = "bmp (*.bmp)|*.bmp|All files (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Imaging.DrawImage(sfd.FileName, newImage);
                pictureBox2.Image = Image.FromFile(sfd.FileName);
            }
        }
    }
}
