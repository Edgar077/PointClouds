//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenTK;
//using OpenTKExtension;

//namespace OpenTKExtension.Triangulation
//{
//    public class Context
//    {
//        //------------------------------------------------------------------;

//        public int doPrint = 0;
//        public int debug	= 0;
//        public float[] extent= new float[4];//tuple (xmin, xmax, ymin, ymax);
//        public bool triangulate = false;
//        public List<Vector3> vertices  = new List<Vector3>();// list of vertex 2-tuples: (x,y);
//        public List<Vector3> lines	 = new List<Vector3>(); // equation of line 3-tuple (a b c), for the equation of the line a*x+b*y = c;
//        public List<int[] > edges	 = new List<int[]>();// edge 3-tuple: (line index, vertex 1 index, vertex 2 index)	if either vertex index is -1, the edge extends to infinity;
//        public List<int[] > triangles = new List<int[]>();// 3-tuple of vertex indices;
//        public Dictionary<string, int[] > polygons  = new Dictionary<string,int[]>();// a dict of site:[edges] pairs;

//    public Context()
// {
		
// }

//    private bool inExtent(float x , float y)
// {
//            float xmin = this.extent[0];
//        float xmax = this.extent[1];
//        float ymin= this.extent[2];
//        float ymax = this.extent[3];
//        bool b = false;
//        if( x >=xmin && x <= xmax && y >= ymin && y<= ymax)
//            b = true;
//        return b;

// }
//    private void getClipEdges()
// {
//        float xmin = this.extent[0];
//        float xmax = this.extent[1];
//        float ymin= this.extent[2];
//        float ymax = this.extent[3];

		
//        for(int i = 0; i < this.edges.Count; i++)
//  {
//            int[] edge = edges[i];
//            Vector3 equation = this.lines[edge[0]];//line equation;
//            if (edge[1] !=-1 && edge[2] != -1)//finite line;
//   {
//                float x1 = this.vertices[edge[1]][0];
//                float y1= this.vertices[edge[1]][1];
//                float z1= this.vertices[edge[1]][2];

//                float x2 = this.vertices[edge[2]][0];
//                float y2= this.vertices[edge[2]][1];
//                float z2= this.vertices[edge[2]][2];

//                Vector3 pt1 = new Vector3(x1, y1, z1);
//                Vector3 pt2 = new Vector3(x2, y2, z2);

//                bool inExtentP1 = this.inExtent(x1,y1);
//                bool inExtentP2 = this.inExtent(x2,y2);

//                if( inExtentP1 && inExtentP2)
//    {
//                    clipEdges.Add((pt1, pt2));
//    }
//                else if( inExtentP1 and not inExtentP2)
//    {
//                    pt2=this.clipLine(x1, y1, equation, leftDir=False);
//                    clipEdges.Add((pt1, pt2));
//    }
//                else if( not inExtentP1 and inExtentP2)
//    {
//                    pt1=this.clipLine(x2, y2, equation, leftDir=True);
//                    clipEdges.Add((pt1, pt2));
//    }
//   }
//            else://infinite line;
//   {
//                if( edge[1]!=-1)
//    {
//                    x1, y1 = this.vertices[edge[1]][0], this.vertices[edge[1]][1];
//                    leftDir=False;
//    }
//                else
//    {
//                    x1, y1 = this.vertices[edge[2]][0], this.vertices[edge[2]][1];
//                    leftDir=True;
//    }
//                if( this.inExtent(x1,y1))
//    {
//                    pt1=(x1,y1);
//                    pt2=this.clipLine(x1, y1, equation, leftDir);
//                    clipEdges.Add((pt1, pt2));
//    }
//   }
//  }
//        return clipEdges;
// }


//    def getClipPolygons(this, closePoly)
// {
//        xmin, xmax, ymin, ymax = this.extent;
//        poly={};
//        for( inPtsIdx, edges in this.polygons.items())
//  {
//            clipEdges=[];
//            for( edge in edges)
//   {
//                equation=this.lines[edge[0]]//line equation;
//                if edge[1]!=-1 and edge[2]!=-1://finite line;
//    {
//                    x1, y1=this.vertices[edge[1]][0], this.vertices[edge[1]][1];
//                    x2, y2=this.vertices[edge[2]][0], this.vertices[edge[2]][1];
//                    pt1, pt2 = (x1,y1), (x2,y2);
//                    inExtentP1, inExtentP2 = this.inExtent(x1,y1), this.inExtent(x2,y2);
//                    if( inExtentP1 and inExtentP2)
//     {
//                        clipEdges.Add((pt1, pt2));
//     }
//                    else if( inExtentP1 and not inExtentP2)
//     {
//                        pt2=this.clipLine(x1, y1, equation, leftDir=False);
//                        clipEdges.Add((pt1, pt2));
//     }
//                    else if( not inExtentP1 and inExtentP2)
//     {
//                        pt1=this.clipLine(x2, y2, equation, leftDir=True);
//                        clipEdges.Add((pt1, pt2));
//     }
//    }
//                else://infinite line;
//    {
//                    if( edge[1]!=-1)
//     {
//                        x1, y1 = this.vertices[edge[1]][0], this.vertices[edge[1]][1];
//                        leftDir=False;
//     }
//                    else
//     {
//                        x1, y1 = this.vertices[edge[2]][0], this.vertices[edge[2]][1];
//                        leftDir=True;
//     }
//                    if( this.inExtent(x1,y1))
//     {
//                        pt1=(x1,y1);
//                        pt2=this.clipLine(x1, y1, equation, leftDir);
//                        clipEdges.Add((pt1, pt2));
//     }
//    }
//   }
//            //create polygon definition from edges and check if polygon is completely closed;
//            polyPts, complete=this.orderPts(clipEdges);
//            if( not complete)
//   {
//                startPt=polyPts[0];
//                endPt=polyPts[-1];
//                if startPt[0]==endPt[0] or startPt[1]==endPt[1]: //if start & end points are collinear then they are along an extent border;
//    {
//                    polyPts.Add(polyPts[0])//simple close;
//    }
//                else://close at extent corner;
//    {
//                    if (startPt[0]==xmin and endPt[1]==ymax) or (endPt[0]==xmin and startPt[1]==ymax): //upper left;
//     {
//                        polyPts.Add((xmin, ymax))//corner point;
//                        polyPts.Add(polyPts[0])//close polygon;
//     }
//                    if (startPt[0]==xmax and endPt[1]==ymax) or (endPt[0]==xmax and startPt[1]==ymax): //upper right;
//     {
//                        polyPts.Add((xmax, ymax));
//                        polyPts.Add(polyPts[0]);
//     }
//                    if (startPt[0]==xmax and endPt[1]==ymin) or (endPt[0]==xmax and startPt[1]==ymin): //bottom right;
//     {
//                        polyPts.Add((xmax, ymin));
//                        polyPts.Add(polyPts[0]);
//     }
//                    if (startPt[0]==xmin and endPt[1]==ymin) or (endPt[0]==xmin and startPt[1]==ymin): //bottom left;
//     {
//                        polyPts.Add((xmin, ymin));
//                        polyPts.Add(polyPts[0]);
//     }
//    }
//   }
//            if not closePoly://unclose polygon;
//   {
//                polyPts=polyPts[:-1];
//   }
//            poly[inPtsIdx]=polyPts;
//  }
//        return poly;
// }

//    def clipLine(this, x1, y1, equation, leftDir)
// {
//        xmin, xmax, ymin, ymax = this.extent;
//        a,b,c=equation;
//        if b==0://vertical line;
//  {
//            if leftDir://left is bottom of vertical line;
//   {
//                return (x1,ymax);
//   }
//            else
//   {
//                return (x1,ymin);
//   }
//  }
//        elif a==0://horizontal line;
//  {
//            if( leftDir)
//   {
//                return (xmin,y1);
//   }
//            else
//   {
//                return (xmax,y1);
//   }
//  }
//        else
//  {
//            y2_at_xmin=(c-a*xmin)/b;
//            y2_at_xmax=(c-a*xmax)/b;
//            x2_at_ymin=(c-b*ymin)/a;
//            x2_at_ymax=(c-b*ymax)/a;
//            intersectPts=[];
//            if ymin<=y2_at_xmin<=ymax://valid intersect point;
//   {
//                intersectPts.Add((xmin, y2_at_xmin));
//   }
//            if( ymin<=y2_at_xmax<=ymax)
//   {
//                intersectPts.Add((xmax, y2_at_xmax));
//   }
//            if( xmin<=x2_at_ymin<=xmax)
//   {
//                intersectPts.Add((x2_at_ymin, ymin));
//   }
//            if( xmin<=x2_at_ymax<=xmax)
//   {
//                intersectPts.Add((x2_at_ymax, ymax));
//   }
//            //delete duplicate (happens if intersect point is at extent corner);
//            intersectPts=set(intersectPts);
//            //choose target intersect point;
//            if( leftDir)
//   {
//                pt=min(intersectPts)//smaller x value;
//   }
//            else
//   {
//                pt=max(intersectPts);
//   }
//            return pt;
//  }
// }



//    def orderPts(this, edges)
// {
//        poly=[]//returned polygon points list [pt1, pt2, pt3, pt4 ....];
//        pts=[];
//        //get points list;
//        for( edge in edges)
//  {
//            pts.extend([pt for pt in edge]);
//  }
//        //try to get start & end point;
//        try
//  {
//            startPt, endPt = [pt for pt in pts if pts.count(pt)<2]//start and end point aren';t duplicate
//  }
//        except://all points are duplicate --> polygon is complete --> append some or other edge points;
//  {
//            complete=True;
//            firstIdx=0;
//            poly.Add(edges[0][0]);
//            poly.Add(edges[0][1]);
//  }
//        else://incomplete --> append the first edge points;
//  {
//            complete=False;
//            //search first edge;
//            for( i, edge in enumerate(edges))
//   {
//                if startPt in edge://find;
//    {
//                    firstIdx=i;
//                    break;
//    }
//   }
//            poly.Add(edges[firstIdx][0]);
//            poly.Add(edges[firstIdx][1]);
//            if poly[0]!=startPt: poly.reverse();
//  }
//        //append next points in list;
//        del edges[firstIdx];
//        while edges://all points will be treated when edges list will be empty;
//  {
//            currentPt = poly[-1]//last item;
//            for( i, edge in enumerate(edges))
//   {
//                if( currentPt==edge[0])
//    {
//                    poly.Add(edge[1]);
//                    break;
//    }
//                else if( currentPt==edge[1])
//    {
//                    poly.Add(edge[0]);
//                    break;
//    }
//   }
//            del edges[i];
//  }
//        return poly, complete;
// }

//    def setClipBuffer(this, xpourcent, ypourcent)
// {
//        xmin, xmax, ymin, ymax = this.extent;
//        witdh=xmax-xmin;
//        height=ymax-ymin;
//        xmin=xmin-witdh*xpourcent/100;
//        xmax=xmax+witdh*xpourcent/100;
//        ymin=ymin-height*ypourcent/100;
//        ymax=ymax+height*ypourcent/100;
//        this.extent=xmin, xmax, ymin, ymax;
// }
//}

//////////////////End clip functions////////////////;
//{

//    def outSite(this,s)
// {
//        if(this.debug)
//  {
//            print("site (%d) at %f %f" % (s.sitenum, s.x, s.y));
//  }
//        else if(this.triangulate)
//  {
//            pass;
//  }
//        else if(this.doPrint)
//  {
//            print("s %f %f" % (s.x, s.y));
//  }
// }

//    def outVertex(this,s)
// {
//        this.vertices.Add((s.x,s.y));
//        if(this.debug)
//  {
//            print("vertex(%d) at %f %f" % (s.sitenum, s.x, s.y));
//  }
//        else if(this.triangulate)
//  {
//            pass;
//  }
//        else if(this.doPrint)
//  {
//            print("v %f %f" % (s.x,s.y));
//  }
// }

//    def outTriple(this,s1,s2,s3)
// {
//        this.triangles.Add((s1.sitenum, s2.sitenum, s3.sitenum));
//        if(this.debug)
//  {
//            print("circle through left=%d right=%d bottom=%d" % (s1.sitenum, s2.sitenum, s3.sitenum));
//  }
//        else if(this.triangulate and this.doPrint)
//  {
//            print("%d %d %d" % (s1.sitenum, s2.sitenum, s3.sitenum));
//  }
// }

//    def outBisector(this,edge)
// {
//        this.lines.Add((edge.a, edge.b, edge.c));
//        if(this.debug)
//  {
//            print("line(%d) %gx+%gy=%g, bisecting %d %d" % (edge.edgenum, edge.a, edge.b, edge.c, edge.reg[0].sitenum, edge.reg[1].sitenum));
//  }
//        else if(this.doPrint)
//  {
//            print("l %f %f %f" % (edge.a, edge.b, edge.c));
//  }
// }

//    def outEdge(this,edge)
// {
//        sitenumL = -1;
//        if( edge.ep[Edge.LE] is not None)
//  {
//            sitenumL = edge.ep[Edge.LE].sitenum;
//  }
//        sitenumR = -1;
//        if( edge.ep[Edge.RE] is not None)
//  {
//            sitenumR = edge.ep[Edge.RE].sitenum;
//  }

//        //polygons dict add by CF;
//        if( edge.reg[0].sitenum not in this.polygons)
//  {
//            this.polygons[edge.reg[0].sitenum] = [];
//  }
//        if( edge.reg[1].sitenum not in this.polygons)
//  {
//            this.polygons[edge.reg[1].sitenum] = [];
//  }
//        this.polygons[edge.reg[0].sitenum].Add((edge.edgenum,sitenumL,sitenumR));
//        this.polygons[edge.reg[1].sitenum].Add((edge.edgenum,sitenumL,sitenumR));

//        this.edges.Add((edge.edgenum,sitenumL,sitenumR));

//        if(not this.triangulate)
//  {
//            if(this.doPrint)
//   {
//                print("e %d" % edge.edgenum);
//                print(" %d " % sitenumL);
//                print("%d" % sitenumR);
//   }
//  }
// }
//}


//    }
//}
