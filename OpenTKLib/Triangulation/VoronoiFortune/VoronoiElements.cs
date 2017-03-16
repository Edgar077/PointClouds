/*
 * Created by SharpDevelop.
 * User: Burhan
 * Date: 17/06/2014
 * Time: 09:29 م
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

/*
	  Copyright 2011 James Humphreys. All rights reserved.
	
	Redistribution and use in source and binary forms, with or without modification, are
	permitted provided that the following conditions are met:
	
	   1. Redistributions of source code must retain the above copyright notice, this list of
	      conditions and the following disclaimer.
	
	   2. Redistributions in binary form must reproduce the above copyright notice, this list
	      of conditions and the following disclaimer in the documentation and/or other materials
	      provided with the distribution.
	
	THIS SOFTWARE IS PROVIDED BY James Humphreys ``AS IS'' AND ANY EXPRESS OR IMPLIED
	WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
	FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
	CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
	CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
	SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
	ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
	NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
	ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
	
	The views and conclusions contained in the software and documentation are those of the
	authors and should not be interpreted as representing official policies, either expressed
	or implied, of James Humphreys.
 */

/*
 * C# Version by Burhan Joukhadar
 * 
 * Permission to use, copy, modify, and distribute this software for any
 * purpose without fee is hereby granted, provided that this entire notice
 * is included in all copies of any software which is or includes a copy
 * or modification of this software and in all copies of the supporting
 * documentation for such software.
 * THIS SOFTWARE IS BEING PROVIDED "AS IS", WITHOUT ANY EXPRESS OR IMPLIED
 * WARRANTY.  IN PARTICULAR, NEITHER THE AUTHORS NOR AT&T MAKE ANY
 * REPRESENTATION OR WARRANTY OF ANY KIND CONCERNING THE MERCHANTABILITY
 * OF THIS SOFTWARE OR ITS FITNESS FOR ANY PARTICULAR PURPOSE.
 */


using System;
using System.Collections.Generic;
using OpenTK;
using OpenTKExtension;

namespace VoronoiFortune
{
    //public class VectorWithIndex
    //{
    //    public double X, Y;
    //    public int Index;

    //    public VectorWithIndex()
    //    {
           
    //    }

    //    public VectorWithIndex (double x, double y, int Index)
    //    {
    //        X = x;
    //        Y = y;
    //        Index = Index;
    //    }
		
       
    //}
	
	
	public class Halfedge
	{
        
        public VertexKDTree vertex;
		public Halfedge ELleft, ELright;
        public Halfedge PQnext;
        public EdgeF ELedge;
		public bool deleted;
		public int ELpm;

        public float ystar;
		
		
		public Halfedge ()
		{
			PQnext = null;
		}
	}
    public class EdgeF
    {
        public float a = 0, b = 0, c = 0;
        public VertexKDTree[] ep;
        public VertexKDTree[] reg;
        public int edgenbr;

        public EdgeF()
        {
            ep = new VertexKDTree[2];
            reg = new VertexKDTree[2];
        }
    }
	
	
	public class EdgeFortune
	{
        public double x1, y1, x2, y2;
		public int PointIndex1, PointIndex2;
	}
	
	public class VectorWithIndexSorterYX : IComparer<VertexKDTree>
	{
		public int Compare ( VertexKDTree p1, VertexKDTree p2 )
		{
			
			if ( p1.Vector.Y < p2.Vector.Y )	return -1;
            if (p1.Vector.Y > p2.Vector.Y) return 1;
            if (p1.Vector.X < p2.Vector.X) return -1;
            if (p1.Vector.X > p2.Vector.X) return 1;
			return 0;
		}
	}
}
