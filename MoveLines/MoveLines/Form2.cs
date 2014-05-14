using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoveLines
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public List<GraphLine> Lines = new List<GraphLine>();
        GraphLine SelectedLine = null;
        MoveInfo Moving = null;

        private void RefreshLineSelection(Point point)
        {
            var selectedLine = FindLineByPoint(Lines, point);
            if (selectedLine != this.SelectedLine)
            {
                this.SelectedLine = selectedLine;
                this.Invalidate();
            }
            if (Moving != null)
                this.Invalidate();

            this.Cursor =
                Moving != null ? Cursors.Hand :
                SelectedLine != null ? Cursors.SizeAll :
                  Cursors.Default;

        }

        static GraphLine FindLineByPoint(List<GraphLine> lines, Point p)
        {
            var size = 10;
            var buffer = new Bitmap(size * 2, size * 2);
            foreach (var line in lines)
            {
                //draw each line on small region around current point p and check pixel in point p 

                using (var g = Graphics.FromImage(buffer))
                {
                    g.Clear(Color.Black);
                    g.DrawLine(new Pen(Color.Green, 3), line.StartPoint.X - p.X + size, line.StartPoint.Y - p.Y + size, line.EndPoint.X - p.X + size, line.EndPoint.Y - p.Y + size);
                }

                if (buffer.GetPixel(size, size).ToArgb() != Color.Black.ToArgb())
                    return line;
            }
            return null;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            RefreshLineSelection(e.Location);

            if (this.SelectedLine != null && Moving == null)
            {
                //this.Capture = true;
                Moving = new MoveInfo
                {
                    Line = this.SelectedLine,
                    StartLinePoint = SelectedLine.StartPoint,
                    EndLinePoint = SelectedLine.EndPoint,
                    StartMoveMousePoint = e.Location
                };
            }

            RefreshLineSelection(e.Location);

            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Moving != null)
            {
                Moving.Line.StartPoint = new PointF(Moving.StartLinePoint.X + e.X - Moving.StartMoveMousePoint.X, Moving.StartLinePoint.Y + e.Y - Moving.StartMoveMousePoint.Y);
                Moving.Line.EndPoint = new PointF(Moving.EndLinePoint.X + e.X - Moving.StartMoveMousePoint.X, Moving.EndLinePoint.Y + e.Y - Moving.StartMoveMousePoint.Y);
            }

            RefreshLineSelection(e.Location);

            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (Moving != null)
            {
                this.Capture = false;
                Moving = null;
            }

            RefreshLineSelection(e.Location);

            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
           
            foreach (var line in Lines)
            {
                var color = line == SelectedLine ? Color.Gold : Color.GreenYellow;
                var pen = new Pen(color, 2);
                e.Graphics.DrawLine(pen, line.StartPoint, line.EndPoint);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //var image1 = (Bitmap)Image.FromFile(@"IMG-20140402-WA0000.jpg", true);

                Image image = Image.FromFile("IMG-20140402-WA0007.jpg");
                pictureBox1.Image = image;
                pictureBox1.Height = image.Height;
                pictureBox1.Width = image.Width;

            }
            catch (FileNotFoundException exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Lines = new List<GraphLine>() { new GraphLine(10, 10, 10, 200), new GraphLine(20, 10, 20, 200) };
        }
    }
}
