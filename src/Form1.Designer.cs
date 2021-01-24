
namespace LicznikObiektow
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_WczytajObraz = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_AnalizaObrazu = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_WczytajObraz
            // 
            this.btn_WczytajObraz.Location = new System.Drawing.Point(12, 12);
            this.btn_WczytajObraz.Name = "btn_WczytajObraz";
            this.btn_WczytajObraz.Size = new System.Drawing.Size(112, 23);
            this.btn_WczytajObraz.TabIndex = 0;
            this.btn_WczytajObraz.Text = "Wczytaj Obraz";
            this.btn_WczytajObraz.UseVisualStyleBackColor = true;
            this.btn_WczytajObraz.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(65, 141);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(248, 221);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // btn_AnalizaObrazu
            // 
            this.btn_AnalizaObrazu.Location = new System.Drawing.Point(12, 44);
            this.btn_AnalizaObrazu.Name = "btn_AnalizaObrazu";
            this.btn_AnalizaObrazu.Size = new System.Drawing.Size(112, 23);
            this.btn_AnalizaObrazu.TabIndex = 2;
            this.btn_AnalizaObrazu.Text = "Analiza obrazu";
            this.btn_AnalizaObrazu.UseVisualStyleBackColor = true;
            this.btn_AnalizaObrazu.Click += new System.EventHandler(this.btn_AnalizaObrazu_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(439, 141);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(248, 221);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btn_AnalizaObrazu);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_WczytajObraz);
            this.Name = "Form1";
            this.Text = "Wykrywanie Obiektow na Obrazach";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_WczytajObraz;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_AnalizaObrazu;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

