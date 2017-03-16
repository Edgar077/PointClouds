using System;
using System.Collections;

using System.Drawing;
using OpenTK;
using OpenTKExtension;
using System.Collections.Generic;


namespace FortuneVoronoi
{
	
	public abstract class Fortune
	{
        public static readonly Vector3 VVInfinite = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
        public static readonly Vector3 VVUnkown = Vector3.Zero;//new Vector3(float.NaN, float.NaN, float.NaN);
        public static readonly Vector3 VZero = Vector3.Zero;

        /// <summary>
        /// Visualization of Delaunay Triangulation
        /// </summary>
        /// <param name="weight">Weight of result image.</param>
        /// <param name="height">Height of result image.</param>
        /// <param name="Datapoints">Result bitmap.</param>
        /// <returns></returns>
        public static List<Triangle> GetDelaunayTriangulation(IEnumerable Datapoints)
        {
            System.Collections.Generic.List<Triangle> list = new System.Collections.Generic.List<Triangle>();
            VoronoiGraph voronoiGraph = Fortune.ComputeVoronoiGraph(Datapoints);
            //Graphics g = Graphics.FromImage(bmp);
            List<Triangle> triangles = new List<Triangle>();
            for (int i = voronoiGraph.Edges.Count - 1; i >= 0; i-- )
            {
                VoronoiEdge edge1 = voronoiGraph.Edges[i] ;
                for(int j = 0; j < i; j++)
                {
                    VoronoiEdge edge2 = voronoiGraph.Edges[j] ;
                    //if (edge1.LeftData.Index == edge2.LeftData.Index)
                    //{
                    //    triangles.Add(new Triangle(edge1.LeftData.Index, edge1.RightData.Index, edge2.RightData.Index));
                    //}
                    //if (edge1.RightData.Index == edge2.RightData.Index)
                    //{
                    //    triangles.Add(new Triangle(edge1.LeftData.Index, edge1.RightData.Index, edge2.LeftData.Index));
                    //}
                    //if (edge1.RightData.Index == edge2.LeftData.Index)
                    //{
                    //    triangles.Add(new Triangle(edge1.LeftData.Index, edge1.RightData.Index, edge2.RightData.Index));
                    //}
                    //if (edge1.LeftData.Index == edge2.RightData.Index)
                    //{
                    //    triangles.Add(new Triangle(edge1.LeftData.Index, edge1.RightData.Index, edge2.LeftData.Index));
                    //}

                }
                //if ((edge.LeftData[0] == v[0]) & (edge.LeftData[1] == v[1]))
                //{
                //    g.DrawLine(Pens.Black, (int)edge.LeftData[0], (int)edge.LeftData[1], (int)edge.RightData[0], (int)edge.RightData[1]);
                //}
            }


            ////Graphics g = Graphics.FromImage(bmp);
            //foreach (object o in Datapoints)
            //{
            //    Vector3 v = (Vector3)o;
            //    //g.DrawEllipse(Pens.Red, (int)v[0] - 1, (int)v[1] - 1, 2, 2);
            //    foreach (object obj in voronoiGraph.Edges)
            //    {
            //        VoronoiEdge edge = (VoronoiEdge)obj;
            //        if ((edge.LeftData[0] == v[0]) & (edge.LeftData[1] == v[1]))
            //        {
            //            g.DrawLine(Pens.Black, (int)edge.LeftData[0], (int)edge.LeftData[1], (int)edge.RightData[0], (int)edge.RightData[1]);
            //        }
            //    }
            //}
            return triangles;
        }

        internal static float ParabolicCut(float x1, float y1, float x2, float y2, float ys)
		{
//			y1=-y1;
//			y2=-y2;
//			ys=-ys;
//			
			if(Math.Abs(x1-x2)<1e-10 && Math.Abs(y1-y2) < 1e-6)
			{

				throw new Exception("Identical datapoints are not allowed!");
			}

			if(Math.Abs(y1-ys)<1e-10 && Math.Abs(y2-ys)<1e-10)
				return (x1+x2)/2;
			if(Math.Abs(y1-ys)<1e-10)
				return x1;
			if(Math.Abs(y2-ys)<1e-10)
				return x2;
			float a1 = 1/(2*(y1-ys));
			float a2 = 1/(2*(y2-ys));
			if(Math.Abs(a1-a2)<1e-10)
				return (x1+x2)/2;
			float xs1 = 0.5f/(2*a1-2*a2)*(4*a1*x1-4*a2*x2+2 * Convert.ToSingle( Math.Sqrt(-8*a1*x1*a2*x2-2*a1*y1+2*a1*y2+4*a1*a2*x2*x2+2*a2*y1+4*a2*a1*x1*x1-2*a2*y2)));
			float xs2 = 0.5f/(2*a1-2*a2)*(4*a1*x1-4*a2*x2-2 * Convert.ToSingle(Math.Sqrt(-8*a1*x1*a2*x2-2*a1*y1+2*a1*y2+4*a1*a2*x2*x2+2*a2*y1+4*a2*a1*x1*x1-2*a2*y2)));
			xs1=Convert.ToSingle(Math.Round(xs1,10));
			xs2=Convert.ToSingle(Math.Round(xs2,10));
			if(xs1>xs2)
			{
				float h = xs1;
				xs1=xs2;
				xs2=h;
			}
			if(y1>=y2)
				return xs2;
			return xs1;
		}
		internal static Vector3 CircumCircleCenter( Vector3 A, Vector3 B, Vector3 C)
		{
			if(A==B || B==C || A==C)
				throw new Exception("Need three different points!");
			float tx = (A[0] + C[0])/2;
			float ty = (A[1] + C[1])/2;

			float vx = (B[0] + C[0])/2;
			float vy = (B[1] + C[1])/2;

			float ux,uy,wx,wy;
			
			if(A[0] == C[0])
			{
				ux = 1;
				uy = 0;
			}
			else
			{
				ux = (C[1] - A[1])/(A[0] - C[0]);
				uy = 1;
			}

			if(B[0] == C[0])
			{
				wx = -1;
				wy = 0;
			}
			else
			{
				wx = (B[1] - C[1])/(B[0] - C[0]);
				wy = -1;
			}

			float alpha = (wy*(vx-tx)-wx*(vy - ty))/(ux*wy-wx*uy);

            return new Vector3(tx + alpha * ux, ty + alpha * uy, 0);
		}	
		public static VoronoiGraph ComputeVoronoiGraph(IEnumerable Datapoints)
		{
			BinaryPriorityQueue PQ = new BinaryPriorityQueue();
			Hashtable CurrentCircles = new Hashtable();
			VoronoiGraph VG = new VoronoiGraph();
			VNode RootNode = null;
			foreach( Vector3 V in Datapoints)
			{
				PQ.Push(new VDataEvent(V));
			}
			while(PQ.Count>0)
			{
				VEvent VE = PQ.Pop() as VEvent;
				VDataNode[] CircleCheckList;
				if(VE is VDataEvent)
				{
					RootNode = VNode.ProcessDataEvent(VE as VDataEvent,RootNode,VG,VE.Y,out CircleCheckList);
				}
				else if(VE is VCircleEvent)
				{
					CurrentCircles.Remove(((VCircleEvent)VE).NodeN);
					if(!((VCircleEvent)VE).Valid)
						continue;
					RootNode = VNode.ProcessCircleEvent(VE as VCircleEvent,RootNode,VG,VE.Y,out CircleCheckList);
				}
				else throw new Exception("Got event of type "+VE.GetType().ToString()+"!");
				foreach(VDataNode VD in CircleCheckList)
				{
					if(CurrentCircles.ContainsKey(VD))
					{
						((VCircleEvent)CurrentCircles[VD]).Valid=false;
						CurrentCircles.Remove(VD);
					}
					VCircleEvent VCE = VNode.CircleCheckDataNode(VD,VE.Y);
					if(VCE!=null)
					{
						PQ.Push(VCE);
						CurrentCircles[VD]=VCE;
					}
				}
				if(VE is VDataEvent)
				{
					 Vector3 DP = ((VDataEvent)VE).DataPoint;
					foreach(VCircleEvent VCE in CurrentCircles.Values)
					{
						if(MathTools.Dist(DP[0],DP[1],VCE.Center[0],VCE.Center[1])<VCE.Y-VCE.Center[1] && Math.Abs(MathTools.Dist(DP[0],DP[1],VCE.Center[0],VCE.Center[1])-(VCE.Y-VCE.Center[1]))>1e-10)
							VCE.Valid = false;
					}
				}
			}
			return VG;
		}

        /// <summary>
        /// Visualization of 2D Voronoi map.
        /// </summary>
        /// <param name="weight">Weight of result image.</param>
        /// <param name="height">Height of result image.</param>
        /// <param name="Datapoints">Array of data points.</param>
        /// <returns>Result bitmap.</returns>
        public static Bitmap GetVoronoyMap(int weight, int height, IEnumerable Datapoints)
        {
            Bitmap bmp = new Bitmap(weight, height);
            VoronoiGraph graph = Fortune.ComputeVoronoiGraph(Datapoints);
            Graphics g = Graphics.FromImage(bmp);
            foreach (object o in graph.Vertizes)
            {
                Vector3 v = (Vector3)o;
                g.DrawEllipse(Pens.Black, (int)v[0]-2, (int)v[1]-2, 4, 4);
            }
            foreach (object o in Datapoints)
            {
                Vector3 v = (Vector3)o;
                g.DrawEllipse(Pens.Red, (int)v[0]-1, (int)v[1]-1, 2, 2);
            }
            foreach (object o in graph.Edges)
            {
                VoronoiEdge edge = (VoronoiEdge)o;
                try
                {
                    g.DrawLine(Pens.Brown, (int)edge.VVertexA[0], (int)edge.VVertexA[1], (int)edge.VVertexB[0], (int)edge.VVertexB[1]);
                }
                catch { }
            }
            return bmp;
        }
      
       
	}
}
