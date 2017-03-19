/*
 * Created by SharpDevelop.
 * User: Burhan
 * Date: 11/05/2014
 * Time: 01:02 ص
 * 
 * C# Program by Burhan Joukhadar
 * Permission to use, copy, modify, and distribute this software for any
 * purpose without fee is hereby granted, provided that this entire notice
 * is included in all copies of any software which is or includes a copy
 * or modification of this software and in all copies of the supporting
 * documentation for such software.
 * THIS SOFTWARE IS BEING PROVIDED "AS IS", WITHOUT ANY EXPRESS OR IMPLIED
 * WARRANTY.  IN PARTICULAR, NEITHER THE AUTHORS NOR AT&T MAKE ANY
 * REPRESENTATION OR WARRANTY OF ANY KIND CONCERNING THE MERCHANTABILITY
 * OF THIS SOFTWARE OR ITS FITNESS FOR ANY PARTICULAR PURPOSE.
 * 
 * This is a program that draws Voronoi Diagram using Fortune's Algorithm
 * This program is to evaluate the and view the resulting voronoi diagram
 * Also it gives an example of how to use the voronoi object, it's not optimized actually it's rushed.
 */

using System;
using System.Collections.Generic;
using CSPoint = System.Drawing.Point; 
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using OpenTK;
using OpenTKExtension;
namespace VoronoiFortune
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class FortuneVoronoiUI : Form
	{
		Bitmap bitmap;
		Bitmap background;
		Graphics g;
		Random seeder;
		Voronoi voronoi;
		static int numberOfPoints = 20;
		
		public FortuneVoronoiUI()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			seeder = new Random();
			pb.AutoSize = true;
			bitmap = new Bitmap (512,512);
			
			background = new Bitmap ( 512, 512 );
			Graphics g2 = Graphics.FromImage ( background );
			g2.Clear (Color.White);
			g2 = null;
			
			g = Graphics.FromImage (bitmap);
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.Clear (Color.White);
			pb.Image = bitmap;
			this.AutoSize = true;
			
			voronoi = new Voronoi ( 0.1f );
		}
        void TestVoronoi()
        {
            List<PointF> listPoints = CreatePoints();

            List<EdgeFortune> listEdges;
            listEdges = MakeVoronoiGraph(listPoints, bitmap.Width, bitmap.Height);

            DrawVoronoi(listEdges);

        }
		

        private List<PointF>  CreatePoints()
        {
            g.Clear(Color.White);

            List<PointF> sites = new List<PointF>();
            int seed = seeder.Next();
            Random rand = new Random(seed);

            richTextBox1.Text += "\nSEED: " + seed;

            for (int i = 0; i < numberOfPoints; i++)
            {
                sites.Add(new PointF((float)(rand.NextDouble() * 512), (float)(rand.NextDouble() * 512)));
            }

            // رسم المواقع
            for (int i = 0; i < sites.Count; i++)
            {
                g.FillEllipse(Brushes.Blue, sites[i].X - 1.5f, sites[i].Y - 1.5f, 3, 3);
            }
            return sites;
        }
        private void DrawVoronoi(List<EdgeFortune> myListEdges)
        {

            for (int i = 0; i < myListEdges.Count; i++)
            {
                try
                {
                    CSPoint p1 = new CSPoint((int)myListEdges[i].x1, (int)myListEdges[i].y1);
                    CSPoint p2 = new CSPoint((int)myListEdges[i].x2, (int)myListEdges[i].y2);
                    g.DrawLine(Pens.Black, p1.X, p1.Y, p2.X, p2.Y);
                }
                catch(Exception err)
                {
                    string s = "\nP " + i + ": " + myListEdges[i].x1 + ", " + myListEdges[i].y1 + " || " + myListEdges[i].x2 + ", " + myListEdges[i].y2;
                    richTextBox1.Text += s;
                    System.Diagnostics.Debug.WriteLine("Err :  " + err.Message);
                }
            }
            pb.Image = bitmap;
        }
	
		List<EdgeFortune> MakeVoronoiGraph ( List<PointF> myListPoints, int width, int height )
		{
			
            List<VertexKDTree> listPointsFortune = new List<VertexKDTree>();
			for ( int i = 0; i < myListPoints.Count; i++ )
			{
                listPointsFortune.Add(new VertexKDTree(new Vector3(myListPoints[i].X, myListPoints[i].Y, i), 0));
                
			}
            VertexKDTree max = new VertexKDTree(new Vector3(0, 0, 0), 0);
            VertexKDTree min = new VertexKDTree(new Vector3(width, height, 0), 0);

            return voronoi.GenerateVoronoi(listPointsFortune);
			
		}
		
		
		void Button1Click(object sender, EventArgs e)
		{
			this.richTextBox1.Text += "\n******* NEW TEST *******";
			TestVoronoi();
			//background = Clone32BPPBitmap ( bitmap );
		}
		void NumericUpDown1ValueChanged(object sender, EventArgs e)
		{
			numberOfPoints = (int)(numericUpDown1.Value);
			TestVoronoi();
			//background = Clone32BPPBitmap ( bitmap );
		}
		
		void PbMouseMove(object sender, MouseEventArgs e)
		{
			label1.Text = e.X + ", " + e.Y;
		}
	}
}
