//
//A kd-tree implementation in C++ (and Fortran) by Matthew B. Kennel
//Article: https://arxiv.org/abs/physics/0408067
//C++, Fortran code : https://github.com/jmhodges/kdtree2
//ported to C# by Edgar Maass
//The KDTREE2 software is licensed under the terms of the Academic Free
//Software License, listed herein.  In addition, users of this software
//must give appropriate citation in relevant technical documentation or
//journal paper to the author, Matthew B. Kennel, Institute For
//Nonlinear Science, preferably via a reference to the www.arxiv.org
//repository of this document, {\tt www.arxiv.org e-print:
//physics/0408067}.  This requirement will be deemed to be advisory and
//not mandatory as is necessary to permit the free inclusion of the
//present software with any software licensed under the terms of any
//version of the GNU General Public License, or GNU Library General
//Public License.



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;


namespace OpenTKExtension
{

    public partial class KDTreeNodeKennell
    {
        //if thresholdForApproximateTree == 0 => binary tree, if larger then more points per leaf
        public const int thresholdForApproximateTree = 0; 

        public int cutAxis; // dimension to cut;
        public float cut_val; //cut value
        public float cut_val_left;
        public float cut_val_right;
        public int IndexLeafVector; // extents in index array for searching
        public int endIndex;

        public Box box = new Box(); // [min,max] of the box enclosing all points
        //public interval[] box = new interval[3]; // [min,max] of the box enclosing all points

        public KDTreeNodeKennell ChildLeft; 
        public KDTreeNodeKennell ChildRight;

        public KDTreeKennell Parent;

        


        public KDTreeNodeKennell(KDTreeKennell parent)
        {
            Parent = parent;
        }

        public KDTreeNodeKennell build_tree_for_range(int startIndex, int endIndex, KDTreeNodeKennell parent)
        {
            // recursive function to build 
            KDTreeNodeKennell node = new KDTreeNodeKennell(this.Parent);
            // the newly created node. 
            if (endIndex < startIndex)
            {
                return (null); // no data in this node.
            }
            if ((endIndex - startIndex) <= thresholdForApproximateTree)
            {
                // create a leaf node. 
                // always compute true bounding box for leaf node. 
                for (int i = 0; i < 3; i++)
                {
                    node.box[i] = spread_in_coordinate(i, startIndex, endIndex);
                }
                node.cutAxis = 0;
                node.cut_val = 0f;
                node.IndexLeafVector = startIndex;
                node.endIndex = endIndex;
                node.ChildLeft = node.ChildRight = null;
            }
            else
            {
                //
                // Compute an APPROXIMATE bounding box for this node.
                // if parent == NULL, then this is the root node, and 
                // we compute for all dimensions.
                // Otherwise, we copy the bounding box from the parent for
                // all coordinates except for the parent's cut dimension.  
                // That, we recompute ourself.
                //
                int c = -1;
                //int c = 0;

                float maxspread = 0.0F;
                int mEndIndexToBuild;
                for (int i = 0; i < 3; i++)
                {
                    if ((parent == null) || (parent.cutAxis == i))
                    {
                        node.box[i] = spread_in_coordinate(i, startIndex, endIndex);
                    }
                    else
                    {
                        node.box[i] = parent.box[i];
                    }
                    float spread = node.box[i].upper - node.box[i].lower;
                    if (spread > maxspread)
                    {
                        maxspread = spread;
                        c = i;
                    }
                }

                if(c == -1)
                {
                    System.Windows.Forms.MessageBox.Show("Error in building the tree: The point cloud contains duplicates. Please remove the duplicates (see help)");
                    return null;

                }
                float sum;
                float average;


                sum = 0.0F;
                for (int k = startIndex; k <= endIndex; k++)
                {
                    sum += this.Parent.TreeVectors[Convert.ToInt32(this.Parent.Indices[k])].Vector[c];
                }
                average = sum / (float)(endIndex - startIndex + 1);


                mEndIndexToBuild = select_on_coordinate_value(c, average, startIndex, endIndex);

                // move the indices around to cut on dim 'c'.
                node.cutAxis = c;
                node.IndexLeafVector = startIndex;
                node.endIndex = endIndex;
                node.ChildLeft = build_tree_for_range(startIndex, mEndIndexToBuild, node);
                node.ChildRight = build_tree_for_range(mEndIndexToBuild + 1, endIndex, node);
                if (node.ChildRight == null && node.ChildLeft == null)
                {
                    System.Windows.Forms.MessageBox.Show("Error in building tree");
                }
                if (node.ChildRight == null)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        node.box[i] = node.ChildLeft.box[i];
                    }
                    node.cut_val = node.ChildLeft.box[c].upper;
                    node.cut_val_left = node.cut_val_right = node.cut_val;
                }
                else if (node.ChildLeft == null)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        node.box[i] = node.ChildRight.box[i];
                    }
                    node.cut_val = node.ChildRight.box[c].upper;
                    node.cut_val_left = node.cut_val_right = node.cut_val;
                }
                else
                {
                    node.cut_val_right = node.ChildRight.box[c].lower;
                    node.cut_val_left = node.ChildLeft.box[c].upper;
                    node.cut_val = (node.cut_val_left + node.cut_val_right) / 2f;
                    //
                    // now recompute true bounding box as union of subtree boxes.
                    // This is now faster having built the tree, being logarithmic in
                    // N, not linear as would be from naive method.
                    //
                    for (int i = 0; i < 3; i++)
                    {
                        interval intv = new interval(
                            Math.Min(node.ChildLeft.box[i].lower, node.ChildRight.box[i].lower),
                            Math.Max(node.ChildLeft.box[i].upper, node.ChildRight.box[i].upper)
                             );
                        node.box[i] = intv;

                    }
                }
            }
            return (node);
        }
        public int select_on_coordinate_value(int c, float alpha, int l, int u)
        {
            //
            //  Move indices in ind[l..u] so that the elements in [l .. return]
            //  are <= alpha, and hence are less than the [return+1..u]
            //  elmeents, viewed across dimension 'c'.
            // 
            int lb = l;
            int ub = u;

            while (lb < ub)
            {
                if (this.Parent.TreeVectors[Convert.ToInt32(this.Parent.Indices[lb])].Vector[c] <= alpha)
                {
                    lb++; // good where it is.
                }
                else
                {
                    //swap(ref Indices[lb], ref Indices[ub]);
                    swap(lb, ub);

                    ub--;
                }
            }

            // here ub=lb
            if (this.Parent.TreeVectors[Convert.ToInt32(this.Parent.Indices[lb])].Vector[c] <= alpha)
            {
                return (lb);
            }
            else
            {
                return (lb - 1);
            }

        }

        public interval spread_in_coordinate(int indexCoordinate, int startIndex, int endIndex)
        {
            // return the minimum and maximum of the indexed data between startIndex and endIndex in
            // smin_out and smax_out.
            float smin;
            float smax;
            float lmin;
            float lmax;
            int i;


            smin = this.Parent.TreeVectors[Convert.ToInt32(this.Parent.Indices[startIndex])].Vector[indexCoordinate];
            smax = smin;


            // process two at a time.
            for (i = startIndex + 2; i <= endIndex; i += 2)
            {
                lmin = this.Parent.TreeVectors[Convert.ToInt32(this.Parent.Indices[i - 1])].Vector[indexCoordinate];
                lmax = this.Parent.TreeVectors[Convert.ToInt32(this.Parent.Indices[i])].Vector[indexCoordinate];

                if (lmin > lmax)
                {
                    swap(ref lmin, ref lmax);
                    //      float t = lmin;
                    //      lmin = lmax;
                    //      lmax = t;
                }

                if (smin > lmin)
                {
                    smin = lmin;
                }
                if (smax < lmax)
                {
                    smax = lmax;
                }
            }
            // is there one more element? 
            if (i == endIndex + 1)
            {
                float last = this.Parent.TreeVectors[Convert.ToInt32(this.Parent.Indices[endIndex])].Vector[indexCoordinate];
                if (smin > last)
                {
                    smin = last;
                }
                if (smax < last)
                {
                    smax = last;
                }
            }

            return new interval(smin, smax);
            //  printf("Spread in coordinate %d=[%f,%f]\n",c,smin,smax);
        }
        public void select_on_coordinate(int c, int k, int l, int u)
        {
            //
            //  Move indices in ind[l..u] so that the elements in [l .. k] 
            //  are less than the [k+1..u] elmeents, viewed across dimension 'c'. 
            // 
            while (l < u)
            {
                uint t = this.Parent.Indices[l];
                int m = l;

                for (int i = l + 1; i <= u; i++)
                {
                    if (this.Parent.TreeVectors[Convert.ToInt32(this.Parent.Indices[i])].Vector[c] < this.Parent.TreeVectors[Convert.ToInt32(t)].Vector[c])
                    {
                        m++;
                        //swap(ref Indices[i], ref Indices[m]);
                        swap(i, m);

                    }
                } // for i
                //swap(ref Indices[l], ref Indices[m]);
                swap(l, m);

                if (m <= k)
                {
                    l = m + 1;
                }
                if (m >= k)
                {
                    u = m - 1;
                }
            } // while loop
        }
        public void search(SearchRecord sr)
        {
            // the core search routine.
            // This uses true distance to bounding box as the
            // criterion to search the secondary node. 
            //
            // This results in somewhat fewer searches of the secondary nodes
            // than 'search', which uses the vdiff vector,  but as this
            // takes more computational time, the overall performance may not
            // be improved in actual run time. 
            //

            if ((ChildLeft == null) && (ChildRight == null))
            {
                // we are on a leaf node
                if (sr.NumberOfNeighbours == 0)
                {
                    process_leaf_node_fixedball(sr);
                }
                else
                {
                    process_leaf_node(sr);
                }
            }
            else
            {
                KDTreeNodeKennell ncloser;
                KDTreeNodeKennell nfarther;

                float extra;
                float qval = sr.VectorTarget[cutAxis];
                // value of the wall boundary on the cut dimension. 
                if (qval < cut_val)
                {
                    ncloser = ChildLeft;
                    nfarther = ChildRight;
                    extra = cut_val_right - qval;
                }
                else
                {
                    ncloser = ChildRight;
                    nfarther = ChildLeft;
                    extra = qval - cut_val_left;
                };

                if (ncloser != null)
                {
                    ncloser.search(sr);
                }

                if ((nfarther != null) && (extra * extra < sr.Radius))
                {
                    // first cut
                    if (nfarther.box_in_search_range(sr))
                    {
                        nfarther.search(sr);
                    }
                }
            }
        }


        private float dis_from_bnd(float x, float amin, float amax)
        {
            if (x > amax)
            {
                return (x - amax);
            }
            else if (x < amin)
            {
                return (amin - x);
            }
            else
            {
                return 0f;
            }

        }

        public bool box_in_search_range(SearchRecord sr)
        {
            //
            // does the bounding box, represented by minbox[*],maxbox[*]
            // have any point which is within 'sr.ballsize' to 'sr.qv'??
            //


            float dis2 = 0.0F;
            float ballsize = sr.Radius;
            for (int i = 0; i < 3; i++)
            {
                float f = dis_from_bnd(sr.VectorTarget[i], box[i].lower, box[i].upper);

                dis2 += f * f;
                if (dis2 > ballsize)
                {
                    return (false);
                }
            }
            return (true);
        }

        private bool CheckDistance(Vector3 v1, Vector3 v2, float ballsize, out float distance)
        {

            bool early_exit = false;
            distance = 0f;

            for (int k = 0; k < 3; k++)
            {
                float f = v1[k] - v2[k];
                distance += f * f;
                if (distance >= ballsize)
                {
                    early_exit = true;
                    break;
                }
            }
            
            if (early_exit)
            {
                return true;
            }

            return false;
        }
        public void process_leaf_node(SearchRecord sr)
        {
           
            float ballsize = sr.Radius;
            //
            bool rearrange = sr.rearrange;
           
            for (int i = IndexLeafVector; i <= endIndex; i++)
            {
                uint indexofi; // sr.ind[i];
                float distance;
               
                if (rearrange)
                {
                    
                    
                    if(CheckDistance(this.Parent.TreeVectors[i].Vector,  sr.VectorTarget, ballsize, out distance))
                        continue;// next iteration of mainloop

                    // why do we do things like this?  because if we take an early
                    // exit (due to distance being too large) which is common, then
                    // we need not read in the actual point index, thus saving main
                    // memory bandwidth.  If the distance to point is less than the
                    // ballsize, though, then we need the index.
                    //
                    indexofi = this.Parent.Indices[i];
                }
                else
                {
                    // 
                    // but if we are not using the rearranged data, then
                    // we must always compare all
                    indexofi = this.Parent.Indices[i];

                    if (CheckDistance(this.Parent.TreeVectors[Convert.ToInt32(indexofi)].Vector, sr.VectorTarget, ballsize, out distance))
                        continue;// next iteration of mainloop

                } // end if rearrange.
              
                // here the point must be added to the list.
                //
                // two choices for any point.  The list so far is either
                // undersized, or it is not.
                //
                if (sr.SearchResult.Count < sr.NumberOfNeighbours)
                {
                    KDTreeResult e = new KDTreeResult(indexofi, distance);
                    if (!this.Parent.TakenAlgorithm)
                    {
                        sr.SearchResult.Add(e);
                        if (sr.SearchResult.Count == sr.NumberOfNeighbours)
                        {
                            ballsize = sr.SearchResult[0].Distance;
                        }
                    }
                    else
                    {
                        if (!this.Parent.TreeVectors[Convert.ToInt32(indexofi)].TakenInTree)
                        {
                            this.Parent.TreeVectors[Convert.ToInt32(indexofi)].TakenInTree = true;
                            sr.SearchResult.Add(e);
                            if (sr.SearchResult.Count == sr.NumberOfNeighbours)
                            {
                                ballsize = sr.SearchResult[0].Distance;
                            }
                        }
                        else
                        {

                        }

                    }
                    // Set the ball radius to the largest on the list (maximum priority).

                }
                else
                {
                    //
                    // if we get here then the current node, has a squared 
                    // distance smaller
                    // than the last on the list, and belongs on the list.
                    // 
                    KDTreeResult e = new KDTreeResult(indexofi, distance);

                    ballsize = sr.SearchResult.ReplaceLast_ReturnFirst(e);

                }
                if (distance == 0f)
                    break;

            } // main loop
            sr.Radius = ballsize;
        }

        public void process_leaf_node_fixedball(SearchRecord sr)
        {
         
            float ballsize = sr.Radius;
            //
            bool rearrange = sr.rearrange;

            for (int i = IndexLeafVector; i <= endIndex; i++)
            {
                uint indexofi = this.Parent.Indices[i];
                float distance;
                bool early_exit;

                if (rearrange)
                {
                    early_exit = false;
                    distance = 0.0F;
                    for (int k = 0; k < 3; k++)
                    {
                        float f = this.Parent.TreeVectors[i].Vector[k] - sr.VectorTarget[k];
                        distance += f * f;
                        if (distance > ballsize)
                        {
                            early_exit = true;
                            break;
                        }
                    }
                    if (early_exit)
                    {
                        continue; // next iteration of mainloop
                    }
                    // why do we do things like this?  because if we take an early
                    // exit (due to distance being too large) which is common, then
                    // we need not read in the actual point index, thus saving main
                    // memory bandwidth.  If the distance to point is less than the
                    // ballsize, though, then we need the index.
                    //
                    indexofi = this.Parent.Indices[i];
                }
                else
                {
                    // 
                    // but if we are not using the rearranged data, then
                    // we must always 
                    indexofi = this.Parent.Indices[i];
                    early_exit = false;
                    distance = 0.0F;
                    for (int k = 0; k < 3; k++)
                    {
                        float f = this.Parent.TreeVectors[Convert.ToInt32(indexofi)].Vector[k] - sr.VectorTarget[k];
                        distance += f * f;
                        if (distance > ballsize)
                        {
                            early_exit = true;
                            break;
                        }
                    }
                    if (early_exit)
                    {
                        continue; // next iteration of mainloop
                    }
                } // end if rearrange.


                KDTreeResult e = new KDTreeResult(indexofi, distance);
              
                //sr.result.push_back(e);

                sr.SearchResult.Add(e);


            }
        }

        public override string ToString()
        {
            if (this.Parent != null && this.Parent.TreeVectors != null)
            {

                int ind = Convert.ToInt32(this.Parent.Indices[IndexLeafVector]);
                string str = this.Parent.TreeVectors[ind].Vector.ToString();
                str =  ind.ToString() + " : " + str;// this.box.ToString();
                if (this.ChildLeft == null && this.ChildRight == null)
                    return str = "Leaf: " + str;

                return str;
                //int ind = Convert.ToInt32(this.Parent.Indices[IndexLeafVector]);
                //string str = this.Parent.TreeVectors[ind].Vector.ToString();
                //if (this.ChildLeft == null && this.ChildRight == null)
                //    return "Leaf: " + str;
                //else
                //    return this.box.ToString();

            }
            return string.Empty;
            //return base.ToString();
        }
        public void swap(int a, int b)
        {
            uint tmpInd = this.Parent.Indices[a];
            //VertexKDTree vTmp = this.Parent.TreeVectors[a];

            //this.Parent.TreeVectors[a] = this.Parent.TreeVectors[b];
            this.Parent.Indices[a] = this.Parent.Indices[b];

            //this.Parent.TreeVectors[b] = vTmp;
            this.Parent.Indices[b] = tmpInd;

        }

        public static void swap(ref float a, ref float b)
        {
            float tmp;
            tmp = a;
            a = b;
            b = tmp;
        }
    }
}
