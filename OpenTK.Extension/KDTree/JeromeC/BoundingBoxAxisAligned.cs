//
//
// KD Tree in C# by Jeremy C.
//https://github.com/Jerdak/KDTree2
// lincensed under GNU LESSER GENERAL PUBLIC LICENSE ,Version 3, 29 June 2007

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;


namespace OpenTKExtension
{
	public class BoundingBoxAxisAligned : IIntersector
	{
       

        Vector3 minV;
        Vector3 maxV ;
        Vector3 centerV;
        Vector3 half_sizeV;
        Vector3 sizeV ;


        public override string ToString()
        {
            return this.minV.ToString() + " : " + this.maxV.ToString();
        }

		/// <summary>
		/// Minimum corner of bounding box
		/// </summary>
        public Vector3 MinCorner	
        { 
			get {
				return minV;
			}
			set{
				minV = value;
				sizeV = maxV - minV;
				centerV = (maxV + minV) / 2.0f;
			}
		}
		
		/// <summary>
		/// Maximum corner of bounding box (MinCorner + Size)
		/// </summary>
        public Vector3 MaxCorner
		{
			get
			{
				return maxV;
			}
			set
			{
				maxV = value;
				sizeV = maxV - minV;
				centerV = (maxV + minV) / 2.0f;
			}
		}
		
		/// <summary>
		/// Center of bounding box (MaxCorner + MinCorner) / 2
		/// </summary>
		public Vector3 Center
		{
			get
			{
				return centerV;
			}
		}
	
		/// <summary>
		/// Size of bounding box (MaxCorner - MinCorner)
		/// </summary>
		public Vector3 Size
		{
			get
			{
				return sizeV;
			}
			set
			{
				sizeV = value;
				half_sizeV = sizeV / 2.0f;
				maxV = minV + Size;
				centerV = (maxV + minV) / 2.0f;
			}
		}
		
		/// <summary>
		/// Half-Size of BoundingBox (Size / 2)
		/// </summary>
		public Vector3 HalfSize
		{
			get
			{
				return half_sizeV;
			}
			private set
			{
				half_sizeV = value;
			}
		}
		
		public BoundingBoxAxisAligned()
        {
            //MinCorner = new Vector3();
            //MaxCorner = new Vector3();
		}

        public BoundingBoxAxisAligned(Vector3 min, Vector3 max)
		{
			MinCorner = min;
			MaxCorner = max;
		}

		public BoundingBoxAxisAligned(BoundingBoxAxisAligned aabb)
		{
			MinCorner = aabb.MinCorner;
			MaxCorner = aabb.MaxCorner;
		}
		/// <summary>
		/// BoundingBox contains 'point'
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public bool Contains(Vector3 point)
        {
			return (point.X >= minV.X && point.X <= maxV.X &&
					point.Y >= minV.Y && point.Y <= maxV.Y &&
					point.Y >= minV.Y && point.Y <= maxV.Y);
		}

		/// <summary>
		/// BoundingBox intersects another BoundingBox
		/// </summary>
		/// <param name="aabb"></param>
		/// <returns></returns>
		public bool Intersects(BoundingBoxAxisAligned aabb){
			Vector3 v1 = aabb.MinCorner;
			Vector3 v2 = aabb.MaxCorner;

			Vector3 v3 = MinCorner;
			Vector3 v4 = MaxCorner;

			return ((v4.X >= v1.X) && (v3.X <= v2.X) &&		//x-axis overlap
					(v4.Y >= v1.Y) && (v3.Y <= v2.Y) &&		//y-axis overlap
					(v4.Y >= v1.Y) && (v3.Y <= v2.Y));		//z-axis overlap
		}
		public bool Intersects(BoundingBoxAxisAligned aabb, IntersectionTypes type)
		{
			return Intersects(aabb);
		}

		/// <summary>
		/// Does BoundingBoxAxisAligned intersect BoundingSphere
		/// </summary>
		/// <notes>
		/// Code modified from: http://tog.acm.org/resources/GraphicsGems/gems/BoxSphere.c 
		///	Support for mixed hollow/solid intersections was dropped.  Only hollow-hollow and solid-solid
		///	are supported
		/// </notes>
		/// <param name="intersection_type">Defines intersection method</param>
		/// <returns>True if sphere intersects bounding box, else FALSE</returns>
		public bool Intersects(BoundingSphere bs, IntersectionTypes intersection_type)
		{
			float a, b;
			float r2 = bs.Radius2;
			bool face;

			switch (intersection_type)
			{
				case IntersectionTypes.HOLLOW: // Hollow Box - Hollow Sphere
					{
						float dmin = 0;
						float dmax = 0;
						face = false;
						for (int i = 0; i < 3; i++)
						{
							a = (float)Math.Pow(bs.Center[i] - MinCorner[i], 2.0);
							b = (float)Math.Pow(bs.Center[i] - MaxCorner[i], 2.0);

							dmax += Math.Max(a, b);
							if (bs.Center[i] < MinCorner[i])
							{
								face = true;
								dmin += a;
							}
							else if (bs.Center[i] > MaxCorner[i])
							{
								face = true;
								dmin += b;
							}
							else if (Math.Min(a, b) <= r2)
							{
								face = true;
							}
						}
						if (face && (dmin <= r2) && (r2 <= dmax)) return true;
						break;
					}
				case IntersectionTypes.SOLID: // Solid Box - Solid Sphere
					{
						float dmin = 0;
						for (int i = 0; i < 3; i++)
						{
							if (bs.Center[i] < MinCorner[i])
							{
								dmin += (float)Math.Pow(bs.Center[i] - MinCorner[i], 2.0);
							}
							else if (bs.Center[i] > MaxCorner[i])
							{
								dmin += (float)Math.Pow(bs.Center[i] - MaxCorner[i], 2.0);
							}
						}
						if (dmin <= r2) return true;
						break;
					}
			}

			return false;
		}
		public bool Intersects(BoundingSphere bs)
		{
			return Intersects(bs, IntersectionTypes.SOLID);
		}
	}
}
