using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{
    public partial class Form1 : Form
    {
        private System.Drawing.Bitmap myBitmap; // Our Bitmap declaration

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Graphics graphicsObj;
            myBitmap = new Bitmap(
                800,
                600,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            Forms.Program.loop(ref myBitmap);
            graphicsObj = Graphics.FromImage(myBitmap);
            graphicsObj.Dispose();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphicsObj = e.Graphics;

            graphicsObj.DrawImage(myBitmap, 0, 0, myBitmap.Width, myBitmap.Height);

            graphicsObj.Dispose();
        }
    }
}

