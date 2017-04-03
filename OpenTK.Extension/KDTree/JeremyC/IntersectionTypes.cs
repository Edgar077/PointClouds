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
	public enum IntersectionTypes { HOLLOW, SOLID };

	// Avoid using Visitor pattern here, no need to separate intersector classes from intersection logic.  
	interface IIntersector {
		bool Intersects(BoundingBoxAxisAligned aabb, IntersectionTypes type);
		bool Intersects(BoundingSphere bs, IntersectionTypes type);
	};
}
