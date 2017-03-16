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
