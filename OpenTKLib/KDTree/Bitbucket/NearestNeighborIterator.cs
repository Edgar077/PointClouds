using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKLib
{
    
    public class NearestNeighborIterator<T> // Iterator<T>, Iterable<T> 
    {
    private IDistanceFunction distanceFunction;
    private double[] searchPoint;
    private KDTreeRednaxela.MinHeap<KdNode<T>> pendingPaths;
    private KDTreeRednaxela.IntervalHeap<T> evaluatedPoints;
    private int pointsRemaining;
    private double lastDistanceReturned;

    protected NearestNeighborIterator(KdNode<T> treeRoot, double[] searchPoint, int maxPointsReturned, IDistanceFunction distanceFunction) 
    {
        this.searchPoint = Arrays.copyOf(searchPoint, searchPoint.Length);
        this.pointsRemaining = Math.Min(maxPointsReturned, treeRoot.size());
        this.distanceFunction = distanceFunction;
        this.pendingPaths = new BinaryHeap.Min<KdNode<T>>();
        this.pendingPaths.offer(0, treeRoot);
        this.evaluatedPoints = new KDTreeRednaxela.IntervalHeap<T>();
    }

    /* -------- INTERFACE IMPLEMENTATION -------- */

    
    public boolean hasNext() {
        return pointsRemaining > 0;
    }

   
    public T next() {
        if (!hasNext()) {
            throw new IllegalStateException("NearestNeighborIterator has reached end!");
        }

        while (pendingPaths.size() > 0 && (evaluatedPoints.size() == 0 || (pendingPaths.getMinKey() < evaluatedPoints.getMinKey()))) {
            KdTree.nearestNeighborSearchStep(pendingPaths, evaluatedPoints, pointsRemaining, distanceFunction, searchPoint);
        }

        // Return the smallest distance point
        pointsRemaining--;
        lastDistanceReturned = evaluatedPoints.getMinKey();
        T value = evaluatedPoints.getMin();
        evaluatedPoints.removeMin();
        return value;
    }

    public double distance() {
        return lastDistanceReturned;
    }

    @Override
    public void remove() {
        throw new UnsupportedOperationException();
    }

    @Override
    public Iterator<T> iterator() {
        return this;
    }
}
}
