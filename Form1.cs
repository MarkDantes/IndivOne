using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IndivOne
{
    public partial class Form1 : Form
    {
        Graphics g;
        public SolidBrush b1 = new SolidBrush(Color.Blue);
        public SolidBrush b2 = new SolidBrush(Color.White);
        public List<PointF> Points = new List<PointF>();
        bool dr = false;
        public Pen pen = new Pen(Color.Gold);
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonDraw_Click(object sender, EventArgs e)
        {
            DrawL();
        }
        public void DrawL()
        {
            dr = true;
            g = Graphics.FromHwnd(pictureBox1.Handle);
            if (Jarvis(Points).Count > 1)
            { 
                for (int i = 0; i < Jarvis(Points).Count - 1; i++)
                {

                    g.DrawLine(pen, Jarvis(Points)[i], Jarvis(Points)[i + 1]);
                   

                }
                g.DrawLine(pen, Jarvis(Points)[0], Jarvis(Points)[Jarvis(Points).Count - 1]);
            }
            g.Dispose();
            
        }
        private void buttonClear_Click(object sender, EventArgs e)
        {
            dr = false;
            g = Graphics.FromHwnd(pictureBox1.Handle);
            g.Clear(Color.White);
            Points.Clear();
            g.Dispose();
        }


        private static int Orientation(PointF p1, PointF p2, PointF p)
        {
            
            float Orin = (p2.X - p1.X) * (p.Y - p1.Y) - (p.X - p1.X) * (p2.Y - p1.Y);

            if (Orin > 0)
                return -1;  // Точка P лежит слева
            if (Orin < 0)
                return 1; // Точка P лежит справа

            return 0; 
        }
        public List<PointF> Jarvis(List<PointF> p)
        {
           
            List<PointF> res = new List<PointF>();
             var pointOnHull =  p.Where(point => point.X == p.Min(min => min.X)).First();

            PointF endpoint;
           
            do
            {
                res.Add(pointOnHull);
                 endpoint = p[0];
                for (int j = 1; j < p.Count; j++)
                {
                    if ((endpoint == pointOnHull) || Orientation(pointOnHull, endpoint, p[j])==-1)
                    {
                        endpoint = p[j];
                    }
                }
                if (endpoint != res[0])
                {
                    pointOnHull = endpoint;
                }
                else
                    break;

            } while (endpoint != res[0]);

            return res;
        }
        
        public void DrawDots()
        {
            g.Clear(Color.White);
            g = Graphics.FromHwnd(pictureBox1.Handle);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            for (int i = 0; i < Points.Count; i++)
                g.FillEllipse(b1, Points[i].X, Points[i].Y, 10, 10);

            g.Dispose();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            g = (sender as Control).CreateGraphics();
            PointF p = e.Location;
            if (e.Button == MouseButtons.Left)
            {
                for (int x = 0; x <= 10; x++)
                {
                    for (int y = 0; y <= 10; y++)
                    {
                        PointF p1 = new PointF();
                        p1.X = e.X + x;
                        p1.Y = e.Y + y;
                        if (Points.Contains(p1))
                        {

                            g.FillEllipse(b2, p1.X, p1.Y, 10, 10);
                            Points[Points.IndexOf(p1)] = p;
                            DrawDots();
                            if (dr)
                                DrawL();

                        }
                        else
                        {
                            p1.X = e.X - x;
                            p1.Y = e.Y - y;
                            if (Points.Contains(p1))
                            {


                                g.FillEllipse(b2, p1.X, p1.Y, 10, 10);
                                Points[Points.IndexOf(p1)] = p;
                                DrawDots();
                                if (dr)
                                    DrawL();
                            }
                        }

                    }
                }
            }
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            g = (sender as Control).CreateGraphics();
            PointF p = e.Location;


            Points.Add(p);
                    DrawDots();
                    if (dr)
                DrawL();
               
        }
    }
}
