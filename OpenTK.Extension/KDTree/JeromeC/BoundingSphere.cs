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
	public class BoundingSphere : IIntersector
	{
		
		/// <summary>
		/// BoundingSphere Radius
		/// </summary>
		public float Radius { get { return radius_; } set { radius_ = value; radius2_ = radius_ * radius_; } }
		float radius_;

		/// <summary>
		/// BoundingSphere Radius^2
		/// </summary>
		public float Radius2 { get { return radius2_; } }
		float radius2_;

		/// <summary>
		/// BoundingSphere center
		/// </summary>
		public Vector3 Center 
        { 
            get; set; 
        }
		

		public BoundingSphere()
        {
            Center = new Vector3();
			Radius = 0.0f;
		}

        public BoundingSphere(Vector3 center, float radius)
		{
			Center = center;
			Radius = radius;
		}
		public BoundingSphere(BoundingSphere bs)
		{
			Center = bs.Center;
			Radius = bs.Radius;
		}

        public bool Contains(Vector3 point) 
        {
            Vector3 direction = Center - point;
			return (direction.Length < Radius2);
		}


		/// <summary>
		/// BoudingSphere intersects another BoundingSphere
		/// </summary>
		/// <param name="bs"></param>
		/// <returns>True if sphere intersects another sphere, else FALSE</returns>
		public bool Intersects(BoundingSphere bs)
		{
			return Intersects(bs, IntersectionTypes.HOLLOW);
		}

		/// <summary>
		/// Concrete Intersection: Boundingsphere intersects another bounding sphere with 'intersection_type'
		/// </summary>
		/// <param name="bs"></param>
		/// <param name="intersection_type">Not used.</param>
		/// <returns>True if sphere intersects another sphere, else FALSE</returns>
		public bool Intersects(BoundingSphere bs, IntersectionTypes intersection_type)
        {
            Vector3 direction = Center - bs.Center;
			float distance = direction.LengthSquared;
			float radii = Radius + bs.Radius;
			float radii2 = (float)Math.Pow(radii, 2.0);

			if (distance > radii2)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Does BoundingSphere intersect BoundingBoxAxisAligned
		/// </summary>
		/// <notes>
		/// Code modified from: http://tog.acm.org/resources/GraphicsGems/gems/BoxSphere.c 
		///	Support for mixed hollow/solid intersections was dropped.  Only hollow-hollow and solid-solid
		///	are supported
		/// </notes>
		/// <param name="intersection_type">Defines intersection method</param>
		/// <returns>True if sphere intersects bounding box, else FALSE</returns>
		public bool Intersects(BoundingBoxAxisAligned aabb, IntersectionTypes intersection_type)
        {
			float a, b;
			float r2 = Radius2;
			bool face;

			switch (intersection_type)
			{
				case IntersectionTypes.HOLLOW: // Hollow Box - Hollow Sphere
					{
						float dmin = 0;
						float dmax = 0;
						face = false;
						for( int i = 0; i < 3; i++ ) {
							a = (float)Math.Pow(Center[i] - aabb.MinCorner[i],2.0 );
                            b = (float)Math.Pow(Center[i] - aabb.MaxCorner[i], 2.0);

							dmax += Math.Max( a, b );
							if( Center[i] < aabb.MinCorner[i] ) {
								face = true;
								dmin += a;
							} else if( Center[i] > aabb.MaxCorner[i] ) {
								face = true;
								dmin += b;
							} else if (Math.Min(a, b) <= r2) {
								face = true;
							}
						}
						if (face && (dmin <= r2) && (r2 <= dmax)) return true;
						break;
					}
				case IntersectionTypes.SOLID: // Solid Box - Solid Sphere
					{
						float dmin = 0;
						for (int i = 0; i < 3; i++){
							if( Center[i] < aabb.MinCorner[i] ) {
								dmin += (float)Math.Pow(Center[i] - aabb.MinCorner[i],2.0 );
							} else if( Center[i] > aabb.MaxCorner[i] ) {
								dmin += (float)Math.Pow(Center[i] - aabb.MaxCorner[i],2.0 );
							}
						}
						if (dmin <= r2) return true;
						break;
					}
			} 

			return false;
		} 
	}
}
