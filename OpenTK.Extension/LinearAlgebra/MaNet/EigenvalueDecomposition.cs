// original from the Java matrix package JAMA http://math.nist.gov/javanumerics/jama/
// converted to C# by Ken Johnson, added units tests, (2010)
// http://www.codeproject.com/Articles/91458/MaNet-A-matrix-library-for-NET-Rational-Computing
//adapted to use OpenTK 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace OpenTKExtension
{


    /// <summary>
    /// Eigenvalues and eigenvectors of a real matrix. 
    /// 
    ///   If A is symmetric, then A = V*D*V' where the eigenvalue matrix D is
    ///   diagonal and the eigenvector matrix V is orthogonal.
    ///   I.e. A = V.times(D.times(V.transpose())) and 
    ///   V.times(V.transpose()) equals the identity matrix.
    /// 
    ///   If A is not symmetric, then the eigenvalue matrix D is block diagonal
    /// with the real eigenvalues in 1-by-1 blocks and any complex eigenvalues,
    ///  lambda + i*mu, in 2-by-2 blocks, [lambda, mu; -mu, lambda].  The
    ///   columns of V represent the eigenvectors in the sense that A*V = V*D,
    ///   i.e. A.times(V) equals V.times(D).  The matrix V may be badly
    ///    conditioned, or even singular, so the validity of the equation
    ///    A = V*D*inverse(V) depends upon V.cond().
    /// </summary>
    [Serializable]
    public class EigenvalueDecomposition
    {
        /** Eigenvalues and eigenvectors of a real matrix. 
        <P>
            If A is symmetric, then A = V*D*V' where the eigenvalue matrix D is
            diagonal and the eigenvector matrix V is orthogonal.
            I.e. A = V.times(D.times(V.transpose())) and 
            V.times(V.transpose()) equals the identity matrix.
        <P>
            If A is not symmetric, then the eigenvalue matrix D is block diagonal
            with the real eigenvalues in 1-by-1 blocks and any complex eigenvalues,
            lambda + i*mu, in 2-by-2 blocks, [lambda, mu; -mu, lambda].  The
            columns of V represent the eigenvectors in the sense that A*V = V*D,
            i.e. A.times(V) equals V.times(D).  The matrix V may be badly
            conditioned, or even singular, so the validity of the equation
            A = V*D*inverse(V) depends upon V.cond().
        **/
        /* ------------------------
           Class variables
         * ------------------------ */

        /** Row and column dimension (square matrix).
        @serial matrix dimension.
        */
        private int n;

        /** Symmetry flag.
        @serial internal symmetry flag.
        */
        private bool issymmetric;

        /** Arrays for internal storage of eigenvalues.
        @serial internal storage of eigenvalues.
        */
        private float[] d, e;

        /** Array for internal storage of eigenvectors.
        @serial internal storage of eigenvectors.
        */
        private float[,] Varray;

        /** Array for internal storage of nonsymmetric Hessenberg form.
        @serial internal storage of nonsymmetric Hessenberg form.
        */
        private float[,] H;

        /** Working storage for nonsymmetric algorithm.
        @serial working storage for nonsymmetric algorithm.
        */
        private float[] ort;

        /* ------------------------
           Private Methods
         * ------------------------ */

        /// <summary>
        /// Symmetric Householder reduction to tridiagonal form
        /// </summary>
        private void tred2()
        {

            //  This is derived from the Algol procedures tred2 by
            //  Bowdler, Martin, Reinsch, and Wilkinson, Handbook for
            //  Auto. Comp., Vol.ii-Linear Algebra, and the corresponding
            //  Fortran subroutine in EISPACK.

            for (int j = 0; j < n; j++)
            {
                d[j] = Varray[n - 1, j];
            }

            // Householder reduction to tridiagonal form.

            for (int i = n - 1; i > 0; i--)
            {

                // Scale to avoid under/overflow.

                float scale = 0.0f;
                float h = 0.0f;
                for (int k = 0; k < i; k++)
                {
                    scale = scale + Math.Abs(d[k]);
                }
                if (scale == 0)
                {
                    e[i] = d[i - 1];
                    for (int j = 0; j < i; j++)
                    {
                        d[j] = Varray[i - 1, j];
                        Varray[i, j] = 0.0f;
                        Varray[j, i] = 0.0f;
                    }
                }
                else
                {

                    // Generate Householder vector.

                    for (int k = 0; k < i; k++)
                    {
                        d[k] /= scale;
                        h += d[k] * d[k];
                    }
                    float f = d[i - 1];
                    float g = Convert.ToSingle(Math.Sqrt(h));
                    if (f > 0)
                    {
                        g = -g;
                    }
                    e[i] = scale * g;
                    h = h - f * g;
                    d[i - 1] = f - g;
                    for (int j = 0; j < i; j++)
                    {
                        e[j] = 0.0f;
                    }

                    // Apply similarity transformation to remaining columns.

                    for (int j = 0; j < i; j++)
                    {
                        f = d[j];
                        Varray[j, i] = f;
                        g = e[j] + Varray[j, j] * f;
                        for (int k = j + 1; k <= i - 1; k++)
                        {
                            g += Varray[k, j] * d[k];
                            e[k] += Varray[k, j] * f;
                        }
                        e[j] = g;
                    }
                    f = 0.0f;
                    for (int j = 0; j < i; j++)
                    {
                        e[j] /= h;
                        f += e[j] * d[j];
                    }
                    float hh = f / (h + h);
                    for (int j = 0; j < i; j++)
                    {
                        e[j] -= hh * d[j];
                    }
                    for (int j = 0; j < i; j++)
                    {
                        f = d[j];
                        g = e[j];
                        for (int k = j; k <= i - 1; k++)
                        {
                            Varray[k, j] -= (f * e[k] + g * d[k]);
                        }
                        d[j] = Varray[i - 1, j];
                        Varray[i, j] = 0.0f;
                    }
                }
                d[i] = h;
            }

            // Accumulate transformations.

            for (int i = 0; i < n - 1; i++)
            {
                Varray[n - 1, i] = Varray[i, i];
                Varray[i, i] = 1.0f;
                float h = d[i + 1];
                if (h != 0)
                {
                    for (int k = 0; k <= i; k++)
                    {
                        d[k] = Varray[k, i + 1] / h;
                    }
                    for (int j = 0; j <= i; j++)
                    {
                        float g = 0.0f;
                        for (int k = 0; k <= i; k++)
                        {
                            g += Varray[k, i + 1] * Varray[k, j];
                        }
                        for (int k = 0; k <= i; k++)
                        {
                            Varray[k, j] -= g * d[k];
                        }
                    }
                }
                for (int k = 0; k <= i; k++)
                {
                    Varray[k, i + 1] = 0.0f;
                }
            }
            for (int j = 0; j < n; j++)
            {
                d[j] = Varray[n - 1, j];
                Varray[n - 1, j] = 0.0f;
            }
            Varray[n - 1, n - 1] = 1.0f;
            e[0] = 0.0f;
        }


        /// <summary>
        /// Symmetric tridiagonal QL algorithm.
        /// </summary>
        private void tql2()
        {

            //  This is derived from the Algol procedures tql2, by
            //  Bowdler, Martin, Reinsch, and Wilkinson, Handbook for
            //  Auto. Comp., Vol.ii-Linear Algebra, and the corresponding
            //  Fortran subroutine in EISPACK.

            for (int i = 1; i < n; i++)
            {
                e[i - 1] = e[i];
            }
            e[n - 1] = 0.0f;

            float f = 0.0f;
            float tst1 = 0.0f;
            float eps =  Convert.ToSingle(Math.Pow(2.0, -52));
            for (int l = 0; l < n; l++)
            {

                // Find small subdiagonal element

                tst1 =  Convert.ToSingle(Math.Max(tst1, Math.Abs(d[l]) + Math.Abs(e[l])) );
                int m = l;
                while (m < n)
                {
                    if (Math.Abs(e[m]) <= eps * tst1)
                    {
                        break;
                    }
                    m++;
                }

                // If m == l, d[l] is an eigenvalue,
                // otherwise, iterate.

                if (m > l)
                {
                    int iter = 0;
                    do
                    {
                        iter = iter + 1;  // (Could check iteration count here.)

                        // Compute implicit shift

                        float g = d[l];
                        float p = (d[l + 1] - g) / (2* e[l]);
                        float r = Convert.ToSingle(Maths.Hypot(p, 1));
                        if (p < 0)
                        {
                            r = -r;
                        }
                        d[l] = e[l] / (p + r);
                        d[l + 1] = e[l] * (p + r);
                        float dl1 = d[l + 1];
                        float h = g - d[l];
                        for (int i = l + 2; i < n; i++)
                        {
                            d[i] -= h;
                        }
                        f = f + h;

                        // Implicit QL transformation.

                        p = d[m];
                        float c = 1.0f;
                        float c2 = c;
                        float c3 = c;
                        float el1 = e[l + 1];
                        float s = 0.0f;
                        float s2 = 0.0f;
                        for (int i = m - 1; i >= l; i--)
                        {
                            c3 = c2;
                            c2 = c;
                            s2 = s;
                            g = c * e[i];
                            h = c * p;
                            r = Convert.ToSingle(Maths.Hypot(p, e[i]));
                            e[i + 1] = s * r;
                            s = e[i] / r;
                            c = p / r;
                            p = c * d[i] - s * g;
                            d[i + 1] = h + s * (c * g + s * d[i]);

                            // Accumulate transformation.

                            for (int k = 0; k < n; k++)
                            {
                                h = Varray[k, i + 1];
                                Varray[k, i + 1] = s * Varray[k, i] + c * h;
                                Varray[k, i] = c * Varray[k, i] - s * h;
                            }
                        }
                        p = -s * s2 * c3 * el1 * e[l] / dl1;
                        e[l] = s * p;
                        d[l] = c * p;

                        // Check for convergence.

                    } while (Math.Abs(e[l]) > eps * tst1);
                }
                d[l] = d[l] + f;
                e[l] = 0.0f;
            }

            // Sort eigenvalues and corresponding vectors.

            for (int i = 0; i < n - 1; i++)
            {
                int k = i;
                float p = d[i];
                for (int j = i + 1; j < n; j++)
                {
                    if (d[j] < p)
                    {
                        k = j;
                        p = d[j];
                    }
                }
                if (k != i)
                {
                    d[k] = d[i];
                    d[i] = p;
                    for (int j = 0; j < n; j++)
                    {
                        p = Varray[j, i];
                        Varray[j, i] = Varray[j, k];
                        Varray[j, k] = p;
                    }
                }
            }
        }

        // Nonsymmetric reduction to Hessenberg form.

        private void orthes()
        {

            //  This is derived from the Algol procedures orthes and ortran,
            //  by Martin and Wilkinson, Handbook for Auto. Comp.,
            //  Vol.ii-Linear Algebra, and the corresponding
            //  Fortran subroutines in EISPACK.

            int low = 0;
            int high = n - 1;

            for (int m = low + 1; m <= high - 1; m++)
            {

                // Scale column.

                float scale = 0.0f;
                for (int i = m; i <= high; i++)
                {
                    scale = scale + Math.Abs(H[i, m - 1]);
                }
                if (scale != 0)
                {

                    // Compute Householder transformation.

                    float h = 0.0f;
                    for (int i = high; i >= m; i--)
                    {
                        ort[i] = H[i, m - 1] / scale;
                        h += ort[i] * ort[i];
                    }
                    float g = Convert.ToSingle(Math.Sqrt(h));
                    if (ort[m] > 0)
                    {
                        g = -g;
                    }
                    h = h - ort[m] * g;
                    ort[m] = ort[m] - g;

                    // Apply Householder similarity transformation
                    // H = (I-u*u'/h)*H*(I-u*u')/h)

                    for (int j = m; j < n; j++)
                    {
                        float f = 0.0f;
                        for (int i = high; i >= m; i--)
                        {
                            f += ort[i] * H[i, j];
                        }
                        f = f / h;
                        for (int i = m; i <= high; i++)
                        {
                            H[i, j] -= f * ort[i];
                        }
                    }

                    for (int i = 0; i <= high; i++)
                    {
                        float f = 0.0f;
                        for (int j = high; j >= m; j--)
                        {
                            f += ort[j] * H[i, j];
                        }
                        f = f / h;
                        for (int j = m; j <= high; j++)
                        {
                            H[i, j] -= f * ort[j];
                        }
                    }
                    ort[m] = scale * ort[m];
                    H[m, m - 1] = scale * g;
                }
            }

            // Accumulate transformations (Algol's ortran).

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Varray[i, j] = (i == j ? 1: 0);
                }
            }

            for (int m = high - 1; m >= low + 1; m--)
            {
                if (H[m, m - 1] != 0)
                {
                    for (int i = m + 1; i <= high; i++)
                    {
                        ort[i] = H[i, m - 1];
                    }
                    for (int j = m; j <= high; j++)
                    {
                        float g = 0.0f;
                        for (int i = m; i <= high; i++)
                        {
                            g += ort[i] * Varray[i, j];
                        }
                        // float division avoids possible underflow
                        g = (g / ort[m]) / H[m, m - 1];
                        for (int i = m; i <= high; i++)
                        {
                            Varray[i, j] += g * ort[i];
                        }
                    }
                }
            }
        }


        // Complex scalar division.
        [NonSerialized]
        private float cdivr, cdivi;
        private void cdiv(float xr, float xi, float yr, float yi)
        {
            float r, d;
            if (Math.Abs(yr) > Math.Abs(yi))
            {
                r = yi / yr;
                d = yr + r * yi;
                cdivr = (xr + r * xi) / d;
                cdivi = (xi - r * xr) / d;
            }
            else
            {
                r = yr / yi;
                d = yi + r * yr;
                cdivr = (r * xr + xi) / d;
                cdivi = (r * xi - xr) / d;
            }
        }


        // Nonsymmetric reduction from Hessenberg to real Schur form.

        private void hqr2()
        {

            //  This is derived from the Algol procedure hqr2,
            //  by Martin and Wilkinson, Handbook for Auto. Comp.,
            //  Vol.ii-Linear Algebra, and the corresponding
            //  Fortran subroutine in EISPACK.

            // Initialize

            int nn = this.n;
            int n = nn - 1;
            int low = 0;
            int high = nn - 1;
            float eps =  Convert.ToSingle(Math.Pow(2.0, -52));
            float exshift = 0.0f;
            float p = 0, q = 0, r = 0, s = 0, z = 0, t, w, x, y;

            // Store roots isolated by balanc and compute matrix norm

            float norm = 0.0f;
            for (int i = 0; i < nn; i++)
            {
                if (i < low | i > high)
                {
                    d[i] = H[i, i];
                    e[i] = 0.0f;
                }
                for (int j =  Math.Max(i - 1, 0); j < nn; j++)
                {
                    norm = norm + Math.Abs(H[i, j]);
                }
            }

            // Outer loop over eigenvalue index

            int iter = 0;
            while (n >= low)
            {

                // Look for single small sub-diagonal element

                int l = n;
                while (l > low)
                {
                    s = Math.Abs(H[l - 1, l - 1]) + Math.Abs(H[l, l]);
                    if (s == 0)
                    {
                        s = norm;
                    }
                    if (Math.Abs(H[l, l - 1]) < eps * s)
                    {
                        break;
                    }
                    l--;
                }

                // Check for convergence
                // One root found

                if (l == n)
                {
                    H[n, n] = H[n, n] + exshift;
                    d[n] = H[n, n];
                    e[n] = 0.0f;
                    n--;
                    iter = 0;

                    // Two roots found

                }
                else if (l == n - 1)
                {
                    w = H[n, n - 1] * H[n - 1, n];
                    p = (H[n - 1, n - 1] - H[n, n]) / 2.0f;
                    q = p * p + w;
                    z = Convert.ToSingle(Math.Sqrt(Math.Abs(q)));
                    H[n, n] = H[n, n] + exshift;
                    H[n - 1, n - 1] = H[n - 1, n - 1] + exshift;
                    x = H[n, n];

                    // Real pair

                    if (q >= 0)
                    {
                        if (p >= 0)
                        {
                            z = p + z;
                        }
                        else
                        {
                            z = p - z;
                        }
                        d[n - 1] = x + z;
                        d[n] = d[n - 1];
                        if (z != 0)
                        {
                            d[n] = x - w / z;
                        }
                        e[n - 1] = 0.0f;
                        e[n] = 0.0f;
                        x = H[n, n - 1];
                        s = Math.Abs(x) + Math.Abs(z);
                        p = x / s;
                        q = z / s;
                        r = Convert.ToSingle(Math.Sqrt(p * p + q * q));
                        p = p / r;
                        q = q / r;

                        // Row modification

                        for (int j = n - 1; j < nn; j++)
                        {
                            z = H[n - 1, j];
                            H[n - 1, j] = q * z + p * H[n, j];
                            H[n, j] = q * H[n, j] - p * z;
                        }

                        // Column modification

                        for (int i = 0; i <= n; i++)
                        {
                            z = H[i, n - 1];
                            H[i, n - 1] = q * z + p * H[i, n];
                            H[i, n] = q * H[i, n] - p * z;
                        }

                        // Accumulate transformations

                        for (int i = low; i <= high; i++)
                        {
                            z = Varray[i, n - 1];
                            Varray[i, n - 1] = q * z + p * Varray[i, n];
                            Varray[i, n] = q * Varray[i, n] - p * z;
                        }

                        // Complex pair

                    }
                    else
                    {
                        d[n - 1] = x + p;
                        d[n] = x + p;
                        e[n - 1] = z;
                        e[n] = -z;
                    }
                    n = n - 2;
                    iter = 0;

                    // No convergence yet

                }
                else
                {

                    // Form shift

                    x = H[n, n];
                    y = 0.0f;
                    w = 0.0f;
                    if (l < n)
                    {
                        y = H[n - 1, n - 1];
                        w = H[n, n - 1] * H[n - 1, n];
                    }

                    // Wilkinson's original ad hoc shift

                    if (iter == 10)
                    {
                        exshift += x;
                        for (int i = low; i <= n; i++)
                        {
                            H[i, i] -= x;
                        }
                        s = Math.Abs(H[n, n - 1]) + Math.Abs(H[n - 1, n - 2]);
                        x = y = 0.75f * s;
                        w = -0.4375f * s * s;
                    }

                    // MATLAB's new ad hoc shift

                    if (iter == 30)
                    {
                        s = (y - x) / 2.0f;
                        s = s * s + w;
                        if (s > 0)
                        {
                            s = Convert.ToSingle(Math.Sqrt(s));
                            if (y < x)
                            {
                                s = -s;
                            }
                            s = x - w / ((y - x) / 2+ s);
                            for (int i = low; i <= n; i++)
                            {
                                H[i, i] -= s;
                            }
                            exshift += s;
                            x = y = w = 0.964f;
                        }
                    }

                    iter = iter + 1;   // (Could check iteration count here.)

                    // Look for two consecutive small sub-diagonal elements

                    int m = n - 2;
                    while (m >= l)
                    {
                        z = H[m, m];
                        r = x - z;
                        s = y - z;
                        p = (r * s - w) / H[m + 1, m] + H[m, m + 1];
                        q = H[m + 1, m + 1] - z - r - s;
                        r = H[m + 2, m + 1];
                        s = Math.Abs(p) + Math.Abs(q) + Math.Abs(r);
                        p = p / s;
                        q = q / s;
                        r = r / s;
                        if (m == l)
                        {
                            break;
                        }
                        if (Math.Abs(H[m, m - 1]) * (Math.Abs(q) + Math.Abs(r)) <
                           eps * (Math.Abs(p) * (Math.Abs(H[m - 1, m - 1]) + Math.Abs(z) +
                           Math.Abs(H[m + 1, m + 1]))))
                        {
                            break;
                        }
                        m--;
                    }

                    for (int i = m + 2; i <= n; i++)
                    {
                        H[i, i - 2] = 0.0f;
                        if (i > m + 2)
                        {
                            H[i, i - 3] = 0.0f;
                        }
                    }

                    // float QR step involving rows l:n and columns m:n

                    for (int k = m; k <= n - 1; k++)
                    {
                        bool notlast = (k != n - 1);
                        if (k != m)
                        {
                            p = H[k, k - 1];
                            q = H[k + 1, k - 1];
                            r = (notlast ? H[k + 2, k - 1] : 0);
                            x = Math.Abs(p) + Math.Abs(q) + Math.Abs(r);
                            if (x != 0)
                            {
                                p = p / x;
                                q = q / x;
                                r = r / x;
                            }
                        }
                        if (x == 0)
                        {
                            break;
                        }
                        s = Convert.ToSingle(Math.Sqrt(p * p + q * q + r * r));
                        if (p < 0)
                        {
                            s = -s;
                        }
                        if (s != 0)
                        {
                            if (k != m)
                            {
                                H[k, k - 1] = -s * x;
                            }
                            else if (l != m)
                            {
                                H[k, k - 1] = -H[k, k - 1];
                            }
                            p = p + s;
                            x = p / s;
                            y = q / s;
                            z = r / s;
                            q = q / p;
                            r = r / p;

                            // Row modification

                            for (int j = k; j < nn; j++)
                            {
                                p = H[k, j] + q * H[k + 1, j];
                                if (notlast)
                                {
                                    p = p + r * H[k + 2, j];
                                    H[k + 2, j] = H[k + 2, j] - p * z;
                                }
                                H[k, j] = H[k, j] - p * x;
                                H[k + 1, j] = H[k + 1, j] - p * y;
                            }

                            // Column modification

                            for (int i = 0; i <=  Math.Min(n, k + 3); i++)
                            {
                                p = x * H[i, k] + y * H[i, k + 1];
                                if (notlast)
                                {
                                    p = p + z * H[i, k + 2];
                                    H[i, k + 2] = H[i, k + 2] - p * r;
                                }
                                H[i, k] = H[i, k] - p;
                                H[i, k + 1] = H[i, k + 1] - p * q;
                            }

                            // Accumulate transformations

                            for (int i = low; i <= high; i++)
                            {
                                p = x * Varray[i, k] + y * Varray[i, k + 1];
                                if (notlast)
                                {
                                    p = p + z * Varray[i, k + 2];
                                    Varray[i, k + 2] = Varray[i, k + 2] - p * r;
                                }
                                Varray[i, k] = Varray[i, k] - p;
                                Varray[i, k + 1] = Varray[i, k + 1] - p * q;
                            }
                        }  // (s != 0)
                    }  // k loop
                }  // check convergence
            }  // while (n >= low)

            // Backsubstitute to find vectors of upper triangular form

            if (norm == 0)
            {
                return;
            }

            for (n = nn - 1; n >= 0; n--)
            {
                p = d[n];
                q = e[n];

                // Real vector

                if (q == 0)
                {
                    int l = n;
                    H[n, n] = 1.0f;
                    for (int i = n - 1; i >= 0; i--)
                    {
                        w = H[i, i] - p;
                        r = 0.0f;
                        for (int j = l; j <= n; j++)
                        {
                            r = r + H[i, j] * H[j, n];
                        }
                        if (e[i] < 0)
                        {
                            z = w;
                            s = r;
                        }
                        else
                        {
                            l = i;
                            if (e[i] == 0)
                            {
                                if (w != 0)
                                {
                                    H[i, n] = -r / w;
                                }
                                else
                                {
                                    H[i, n] = -r / (eps * norm);
                                }

                                // Solve real equations

                            }
                            else
                            {
                                x = H[i, i + 1];
                                y = H[i + 1, i];
                                q = (d[i] - p) * (d[i] - p) + e[i] * e[i];
                                t = (x * s - z * r) / q;
                                H[i, n] = t;
                                if (Math.Abs(x) > Math.Abs(z))
                                {
                                    H[i + 1, n] = (-r - w * t) / x;
                                }
                                else
                                {
                                    H[i + 1, n] = (-s - y * t) / z;
                                }
                            }

                            // Overflow control

                            t = Math.Abs(H[i, n]);
                            if ((eps * t) * t > 1)
                            {
                                for (int j = i; j <= n; j++)
                                {
                                    H[j, n] = H[j, n] / t;
                                }
                            }
                        }
                    }

                    // Complex vector

                }
                else if (q < 0)
                {
                    int l = n - 1;

                    // Last vector component imaginary so matrix is triangular

                    if (Math.Abs(H[n, n - 1]) > Math.Abs(H[n - 1, n]))
                    {
                        H[n - 1, n - 1] = q / H[n, n - 1];
                        H[n - 1, n] = -(H[n, n] - p) / H[n, n - 1];
                    }
                    else
                    {
                        cdiv(0 , -H[n - 1, n], H[n - 1, n - 1] - p, q);
                        H[n - 1, n - 1] = cdivr;
                        H[n - 1, n] = cdivi;
                    }
                    H[n, n - 1] = 0.0f;
                    H[n, n] = 1.0f;
                    for (int i = n - 2; i >= 0; i--)
                    {
                        float ra, sa, vr, vi;
                        ra = 0.0f;
                        sa = 0.0f;
                        for (int j = l; j <= n; j++)
                        {
                            ra = ra + H[i, j] * H[j, n - 1];
                            sa = sa + H[i, j] * H[j, n];
                        }
                        w = H[i, i] - p;

                        if (e[i] < 0)
                        {
                            z = w;
                            r = ra;
                            s = sa;
                        }
                        else
                        {
                            l = i;
                            if (e[i] == 0)
                            {
                                cdiv(-ra, -sa, w, q);
                                H[i, n - 1] = cdivr;
                                H[i, n] = cdivi;
                            }
                            else
                            {

                                // Solve complex equations

                                x = H[i, i + 1];
                                y = H[i + 1, i];
                                vr = (d[i] - p) * (d[i] - p) + e[i] * e[i] - q * q;
                                vi = (d[i] - p) * 2* q;
                                if (vr == 0.0 & vi == 0)
                                {
                                    vr = eps * norm * (Math.Abs(w) + Math.Abs(q) +
                                    Math.Abs(x) + Math.Abs(y) + Math.Abs(z));
                                }
                                cdiv(x * r - z * ra + q * sa, x * s - z * sa - q * ra, vr, vi);
                                H[i, n - 1] = cdivr;
                                H[i, n] = cdivi;
                                if (Math.Abs(x) > (Math.Abs(z) + Math.Abs(q)))
                                {
                                    H[i + 1, n - 1] = (-ra - w * H[i, n - 1] + q * H[i, n]) / x;
                                    H[i + 1, n] = (-sa - w * H[i, n] - q * H[i, n - 1]) / x;
                                }
                                else
                                {
                                    cdiv(-r - y * H[i, n - 1], -s - y * H[i, n], z, q);
                                    H[i + 1, n - 1] = cdivr;
                                    H[i + 1, n] = cdivi;
                                }
                            }

                            // Overflow control

                            t =  Convert.ToSingle(Math.Max(Math.Abs(H[i, n - 1]), Math.Abs(H[i, n])));
                            if ((eps * t) * t > 1)
                            {
                                for (int j = i; j <= n; j++)
                                {
                                    H[j, n - 1] = H[j, n - 1] / t;
                                    H[j, n] = H[j, n] / t;
                                }
                            }
                        }
                    }
                }
            }

            // Vectors of isolated roots

            for (int i = 0; i < nn; i++)
            {
                if (i < low | i > high)
                {
                    for (int j = i; j < nn; j++)
                    {
                        Varray[i, j] = H[i, j];
                    }
                }
            }

            // Back transformation to get eigenvectors of original matrix

            for (int j = nn - 1; j >= low; j--)
            {
                for (int i = low; i <= high; i++)
                {
                    z = 0.0f;
                    for (int k = low; k <=  Convert.ToSingle(Math.Min(j, high)); k++)
                    {
                        z = z + Varray[i, k] * H[k, j];
                    }
                    Varray[i, j] = z;
                }
            }
        }


        /* ------------------------
           Constructor
         * ------------------------ */



        ///<summary> Check for symmetry, then construct the eigenvalue decomposition</summary>
        ///<param name="mat3D">Square matrix</param>
        ///<returns>Structure to access D and V.</returns>
        public EigenvalueDecomposition(Matrix3 mat3D)
        {
            float[,] A = mat3D.ToFloatArray();
            this.n = mat3D.ColumnLength();

            int m = mat3D.RowLength();
            Varray = new float[n, m];

            d = new float[n];
            e = new float[n];

            issymmetric = true;
            for (int j = 0; (j < n) & issymmetric; j++)
            {
                for (int i = 0; (i < n) & issymmetric; i++)
                {
                    issymmetric = (A[i, j] == A[j, i]);
                }
            }

            if (issymmetric)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        Varray[i, j] = A[i, j];
                    }
                }

                // Tridiagonalize.
                tred2();

                // Diagonalize.
                tql2();

            }
            else
            {
                H = new float[n, m];

                ort = new float[n];

                for (int j = 0; j < n; j++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        H[i, j] = A[i, j];
                    }
                }

                // Reduce to Hessenberg form.
                orthes();

                // Reduce Hessenberg to real Schur form.
                hqr2();
            }
        }

        /* ------------------------
           Public Methods
         * ------------------------ */


        ///<summary>Return the eigenvector matrix</summary>
        ///<returns>V</returns>
        public Matrix3 V
        {
            get
            {
                Matrix3 mat = new Matrix3();
                mat = mat.FromFloatArray(Varray);
                return mat;
            }


        }

        ///<summary>Return the real parts of the eigenvalues</summary>
        ///<returns>real(diag(D))</returns>
        public float[] EV
        {
            get
            {
                return d;
            }
        }


        ///<Sumary>Return the imaginary parts of the eigenvalues</Sumary>
        ///<returns> imag(diag(D))</returns>
        public float[] EV_IMAG
        {
            get
            {
                return e;
            }
        }


        ///<summary>Return the block diagonal eigenvalue matrix</summary>
        ///<returns>D</returns>
        public Matrix3 getD()
        {
            Matrix3 X = new Matrix3();
            float[,] D = X.ToFloatArray();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    D[i, j] = 0.0f;
                }
                D[i, i] = d[i];
                if (e[i] > 0)
                {
                    D[i, i + 1] = e[i];
                }
                else if (e[i] < 0)
                {
                    D[i, i - 1] = e[i];
                }
            }
            return X;
        }
    }

}
