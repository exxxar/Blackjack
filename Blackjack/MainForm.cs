using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blackjack
{
    public partial class MainForm : Form
    {
        Bitmap bufferedBitmap = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void DrawTable()
        {
            bufferedBitmap = new Bitmap(this.Width, this.Height);
            Graphics bufferedGraphics = Graphics.FromImage(bufferedBitmap);

            bufferedGraphics.FillRectangle(new LinearGradientBrush(new Point(0, 0), new Point(bufferedBitmap.Width, bufferedBitmap.Height),
                                                                        Color.LightGreen, Color.DarkGreen),
                                                   0, 0, bufferedBitmap.Width, bufferedBitmap.Height);

            Pen whitePen = new Pen(Color.White, 3);
            Brush whiteBrush = new SolidBrush(Color.LightSalmon);
            Font textFont = new Font("Broadway", 15);
            for (int i = 0; i < 7; i++)
            {
                bufferedGraphics.DrawRectangle(whitePen, 30 + 105 * i, 270, 90, 120);
                bufferedGraphics.DrawString("Player" + (i + 1), textFont, whiteBrush, 30 + 105 * i, 240);
            }

            bufferedGraphics.DrawRectangle(whitePen, 345, 50, 90, 120);
            bufferedGraphics.DrawRectangle(whitePen, 450, 50, 90, 120);
            bufferedGraphics.DrawString("Dealer", textFont, whiteBrush, 400, 20);

            whitePen.Dispose();
            whiteBrush.Dispose();
            textFont.Dispose();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            DrawTable();
        }


        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            if ( bufferedBitmap != null )
                e.Graphics.DrawImage(bufferedBitmap, 0, 0);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            DrawTable();
            this.Invalidate();
        }
    }
}
