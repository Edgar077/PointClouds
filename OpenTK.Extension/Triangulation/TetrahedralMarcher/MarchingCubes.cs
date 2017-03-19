
//from : Open Development Cookbook Source

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace OpenTKExtension
{


    /// <summary>
    /// A vertex is a simple class that stores the postion of a point, node or vertex.
    /// </summary>
    public class MarchingCubes : RenderableObject
    {
        //
        // Marching Cubes Example Program 
        // by Cory Bloyd (corysama@yahoo.com)
        //
        // A simple, portable and complete implementation of the Marching Cubes
        // and Marching Tetrahedrons algorithms in a single source file.
        // There are many ways that this code could be made faster, but the 
        // intent is for the code to be easy to understand.
        //
        // For a description of the algorithm go to
        // http://astronomy.swin.edu.au/pbourke/modelling/polygonise/
        //
        // This code is public domain.
        //


        //These tables are used so that everything can be done in little loops that you can look at all at once
        // rather than in pages and pages of unrolled code.

        //TablesMarchingCube.a2fVertexOffset lists the positions, relative to vertex0, of each of the 8 vertices of a cube


        //PolygonMode polygonMode = PolygonMode.Fill;

       
        
        float fTargetValue = 48.0f;
        float fTime = 0.0f;
        Vector3[] vectors = null;
        //bool bSpin = true;
        //bool bMove = true;
        //bool bLight = true;

        public MarchingCubes()
        {
            //fStepSize = 1.0f / iDataSetSize;

        }
        void vMarchingCubes()
        {
            int iDataSetSize = this.PointCloud.Vectors.Length;
            float fStepSize = 1f/ iDataSetSize;
            
            int iX, iY, iZ;
            for (iX = 0; iX < iDataSetSize; iX++)
                for (iY = 0; iY < iDataSetSize; iY++)
                    for (iZ = 0; iZ < iDataSetSize; iZ++)
                    {
                        MarchCube(iX * fStepSize, iY * fStepSize, iZ * fStepSize, fStepSize);
                    }
        }

        //vMarchCube1 performs the Marching Cubes algorithm on a single cube
        void MarchCube(float fX, float fY, float fZ, float fScale)
        {
            int[] aiCubeEdgeFlags = new int[256];

            int iVertexTest, iEdge, iFlagIndex, iEdgeFlags;
            float fOffset;
            // Vector3 sColor;
            float[] cubeValues = new float[8];
            Vector3[] edgeVectors = new Vector3[12];
            Vector3[] edgeNormals = new Vector3[12];

            //Make a local copy of the values at the cube's corners
            for (int iVertex = 0; iVertex < 8; iVertex++)
            {
                cubeValues[iVertex] = distanceToPoints(fX + TablesMarchingCube.a2fVertexOffset[iVertex][0] * fScale,
                                                   fY + TablesMarchingCube.a2fVertexOffset[iVertex][1] * fScale,
                                                   fZ + TablesMarchingCube.a2fVertexOffset[iVertex][2] * fScale);
            }

            //Find which vertices are inside of the surface and which are outside
            iFlagIndex = 0;
            for (iVertexTest = 0; iVertexTest < 8; iVertexTest++)
            {
                if (cubeValues[iVertexTest] <= fTargetValue)
                    iFlagIndex |= 1 << iVertexTest;
            }

            //Find which edges are intersected by the surface
            iEdgeFlags = aiCubeEdgeFlags[iFlagIndex];

            //If the cube is entirely inside or outside of the surface, then there will be no intersections
            if (iEdgeFlags == 0)
            {
                return;
            }

            //Find the point of intersection of the surface with each edge
            //Then find the normal to the surface at those points
            for (iEdge = 0; iEdge < 12; iEdge++)
            {
                //if there is an intersection on this edge
                if (CheckIntersection(iEdgeFlags, iEdge))
                {
                    fOffset = fGetOffset(cubeValues[TablesMarchingCube.a2iEdgeConnection[iEdge][0]],
                                                 cubeValues[TablesMarchingCube.a2iEdgeConnection[iEdge][1]], fTargetValue);

                    edgeVectors[iEdge].X = fX + (TablesMarchingCube.a2fVertexOffset[TablesMarchingCube.a2iEdgeConnection[iEdge][0]][0] + fOffset * TablesMarchingCube.a2fEdgeDirection[iEdge][0]) * fScale;
                    edgeVectors[iEdge].Y = fY + (TablesMarchingCube.a2fVertexOffset[TablesMarchingCube.a2iEdgeConnection[iEdge][0]][1] + fOffset * TablesMarchingCube.a2fEdgeDirection[iEdge][1]) * fScale;
                    edgeVectors[iEdge].Z = fZ + (TablesMarchingCube.a2fVertexOffset[TablesMarchingCube.a2iEdgeConnection[iEdge][0]][2] + fOffset * TablesMarchingCube.a2fEdgeDirection[iEdge][2]) * fScale;

                   
                    edgeNormals[iEdge] = getNormal(edgeVectors[iEdge].X, edgeVectors[iEdge].Y, edgeVectors[iEdge].Z);
                }
            }
            //Draw the triangles that were found.  There can be up to five per cube
            for (int i = 0; i < 5; i++)
            {
                if (TablesMarchingCube.a2iTriangleConnectionTable[iFlagIndex][3 * i] < 0)
                    break;

                for (int j = 0; j < 3; j++)
                {
                    int vertexIndex = TablesMarchingCube.a2iTriangleConnectionTable[iFlagIndex][3 * i + j];
                    //VectorNormal v;
                    //v.normal = (edgeNormals[vertex]);
                    //v.vector = (edgeVectors[vertex]) * invDim;
                    //vertices.Add(v);
                }
            }
            //result: 
            //TablesMarchingCube.a2iTriangleConnectionTable

            ////Draw the triangles that were found.  There can be up to five per cube
            //for (iTriangle = 0; iTriangle < 5; iTriangle++)
            //{
            //    if (a2iTriangleConnectionTable[iFlagIndex][3 * iTriangle] < 0)
            //        break;

            //    for (iCorner = 0; iCorner < 3; iCorner++)
            //    {
            //        iVertex = a2iTriangleConnectionTable[iFlagIndex][3 * iTriangle + iCorner];

            //        vGetColor(sColor, asEdgeVertex[iVertex], asEdgeNorm[iVertex]);
            //        glColor3f(sColor.X, sColor.Y, sColor.Z);
            //        glNormal3f(asEdgeNorm[iVertex].X, asEdgeNorm[iVertex].Y, asEdgeNorm[iVertex].Z);
            //        glVertex3f(asEdgeVertex[iVertex].X, asEdgeVertex[iVertex].Y, asEdgeVertex[iVertex].Z);
            //    }
        }
        //MarchCubeTetrahedron performs the Marching Tetrahedrons algorithm on a single cube by making six calls to vMarchTetrahedron
        void MarchCubeTetrahedron(float fX, float fY, float fZ, float fScale)
        {
            int iVertex, iTetrahedron, iVertexInACube;
            Vector3[] asCubePosition = new Vector3[8];
            float[] afCubeValue = new float[8];
            Vector3[] asTetrahedronPosition = new Vector3[4];
            float[] afTetrahedronValue = new float[4];

            //Make a local copy of the cube's corner positions
            for (iVertex = 0; iVertex < 8; iVertex++)
            {
                asCubePosition[iVertex].X = fX + TablesMarchingCube.a2fVertexOffset[iVertex][0] * fScale;
                asCubePosition[iVertex].Y = fY + TablesMarchingCube.a2fVertexOffset[iVertex][1] * fScale;
                asCubePosition[iVertex].Z = fZ + TablesMarchingCube.a2fVertexOffset[iVertex][2] * fScale;
            }

            //Make a local copy of the cube's corner values
            for (iVertex = 0; iVertex < 8; iVertex++)
            {
                afCubeValue[iVertex] = distanceToPoints(asCubePosition[iVertex].X,
                                                   asCubePosition[iVertex].Y,
                                               asCubePosition[iVertex].Z);
            }

            for (iTetrahedron = 0; iTetrahedron < 6; iTetrahedron++)
            {
                for (iVertex = 0; iVertex < 4; iVertex++)
                {
                    iVertexInACube = TablesMarchingCube.a2iTetrahedronsInACube[iTetrahedron][iVertex];
                    asTetrahedronPosition[iVertex].X = asCubePosition[iVertexInACube].X;
                    asTetrahedronPosition[iVertex].Y = asCubePosition[iVertexInACube].Y;
                    asTetrahedronPosition[iVertex].Z = asCubePosition[iVertexInACube].Z;
                    afTetrahedronValue[iVertex] = afCubeValue[iVertexInACube];
                }
                MarchTetrahedron(asTetrahedronPosition, afTetrahedronValue);
            }
        }
        //vMarchTetrahedron performs the Marching Tetrahedrons algorithm on a single tetrahedron
        void MarchTetrahedron(Vector3[] pasTetrahedronPosition, float[] pafTetrahedronValue)
        {
            //int[] aiTetrahedronEdgeFlags = new int[16];
            //int[][] a2iTetrahedronTriangles = new int[16][7];

            int iEdge, iVert0, iVert1, iEdgeFlags, iVertex, iFlagIndex = 0;
            //int iTriangle, iCorner;
            float fOffset, fInvOffset;//, fValue = 0.0f;
            Vector3[] edgeVertex = new Vector3[6];
            Vector3[] edgeNorm = new Vector3[6];
            //Vector3 sColor;

            //Find which vertices are inside of the surface and which are outside
            for (iVertex = 0; iVertex < 4; iVertex++)
            {
                if (pafTetrahedronValue[iVertex] <= fTargetValue)
                    iFlagIndex |= 1 << iVertex;
            }

            //Find which edges are intersected by the surface
            iEdgeFlags = TablesMarchingCube.aiTetrahedronEdgeFlags[iFlagIndex];

            //If the tetrahedron is entirely inside or outside of the surface, then there will be no intersections
            if (iEdgeFlags == 0)
            {
                return;
            }
            //Find the point of intersection of the surface with each edge
            // Then find the normal to the surface at those points
            for (iEdge = 0; iEdge < 6; iEdge++)
            {

                if (CheckIntersection(iEdgeFlags, iEdge))
                {
                    iVert0 = TablesMarchingCube.a2iTetrahedronEdgeConnection[iEdge][0];
                    iVert1 = TablesMarchingCube.a2iTetrahedronEdgeConnection[iEdge][1];
                    fOffset = fGetOffset(pafTetrahedronValue[iVert0], pafTetrahedronValue[iVert1], fTargetValue);
                    fInvOffset = 1.0f - fOffset;

                    edgeVertex[iEdge].X = fInvOffset * pasTetrahedronPosition[iVert0].X + fOffset * pasTetrahedronPosition[iVert1].X;
                    edgeVertex[iEdge].Y = fInvOffset * pasTetrahedronPosition[iVert0].Y + fOffset * pasTetrahedronPosition[iVert1].Y;
                    edgeVertex[iEdge].Z = fInvOffset * pasTetrahedronPosition[iVert0].Z + fOffset * pasTetrahedronPosition[iVert1].Z;

                    edgeNorm[iEdge] = getNormal( edgeVertex[iEdge].X, edgeVertex[iEdge].Y, edgeVertex[iEdge].Z);
                }
            }
            ////Draw the triangles that were found.  There can be up to 2 per tetrahedron
            //for(iTriangle = 0; iTriangle< 2; iTriangle++)
            //{
            //        if(a2iTetrahedronTriangles[iFlagIndex][3 * iTriangle] < 0)
            //                break;

            //        for(iCorner = 0; iCorner< 3; iCorner++)
            //        {
            //                iVertex = a2iTetrahedronTriangles[iFlagIndex][3 * iTriangle + iCorner];

            //                vGetColor(sColor, asEdgeVertex[iVertex], asEdgeNorm[iVertex]);
            //                glColor3f(sColor.X, sColor.Y, sColor.Z);
            //                glNormal3f(asEdgeNorm[iVertex].X, asEdgeNorm[iVertex].Y, asEdgeNorm[iVertex].Z);
            //                glVertex3f(asEdgeVertex[iVertex].X, asEdgeVertex[iVertex].Y, asEdgeVertex[iVertex].Z);
            //        }
            //}

        }




        float fGetOffset(float fValue1, float fValue2, float fValueDesired)
        {
            float fDelta = fValue2 - fValue1;

            if (fDelta == 0.0)
            {
                return 0.5f;
            }
            return (fValueDesired - fValue1) / fDelta;
        }


        //vGetColor generates a color from a given position and normal of a point
        void generateColor(Vector3 rfColor, Vector3 rfPosition, Vector3 rfNormal)
        {
            float fX = rfNormal.X;
            float fY = rfNormal.Y;
            float fZ = rfNormal.Z;
            rfColor.X = (fX > 0.0f ? fX : 0.0f) + (fY < 0.0f ? -0.5f * fY : 0.0f) + (fZ < 0.0f ? -0.5f * fZ : 0.0f);
            rfColor.Y = (fY > 0.0f ? fY : 0.0f) + (fZ < 0.0f ? -0.5f * fZ : 0.0f) + (fX < 0.0f ? -0.5f * fX : 0.0f);
            rfColor.Z = (fZ > 0.0f ? fZ : 0.0f) + (fX < 0.0f ? -0.5f * fX : 0.0f) + (fY < 0.0f ? -0.5f * fY : 0.0f);
        }

        void normalizeVector(Vector3 rfVectorResult, Vector3 rfVectorSource)
        {
            float fOldLength;
            float fScale;

            fOldLength = Convert.ToSingle(Math.Sqrt((rfVectorSource.X * rfVectorSource.X) +
                                (rfVectorSource.Y * rfVectorSource.Y) +
                                (rfVectorSource.Z * rfVectorSource.Z)));

            if (fOldLength == 0.0)
            {
                rfVectorResult.X = rfVectorSource.X;
                rfVectorResult.Y = rfVectorSource.Y;
                rfVectorResult.Z = rfVectorSource.Z;
            }
            else
            {
                fScale = 1.0f / fOldLength;
                rfVectorResult.X = rfVectorSource.X * fScale;
                rfVectorResult.Y = rfVectorSource.Y * fScale;
                rfVectorResult.Z = rfVectorSource.Z * fScale;
            }
        }


        //Generate a sample data set.  fSample1(), fSample2() and fSample3() define three scalar fields whose
        // values vary by the X,Y and Z coordinates and by the fTime value set by vSetTime()
        void setTime(float fNewTime)
        {
            float fOffset;
            int iSourceNum;

            for (iSourceNum = 0; iSourceNum < 3; iSourceNum++)
            {
                vectors[iSourceNum].X = 0.5f;
                vectors[iSourceNum].Y = 0.5f;
                vectors[iSourceNum].Z = 0.5f;
            }

            fTime = fNewTime;
            fOffset = 1.0f + Convert.ToSingle(Math.Sin(fTime));
            vectors[0].X *= fOffset;
            vectors[1].Y *= fOffset;
            vectors[2].Z *= fOffset;
        }

        //fSample1 finds the distance of (fX, fY, fZ) from three moving points
        float distanceToPoints(float fX, float fY, float fZ)
        {
            float fResult = 0.0f;
            float fDx, fDy, fDz;
            fDx = fX - vectors[0].X;
            fDy = fY - vectors[0].Y;
            fDz = fZ - vectors[0].Z;
            fResult += 0.5f / (fDx * fDx + fDy * fDy + fDz * fDz);

            fDx = fX - vectors[1].X;
            fDy = fY - vectors[1].Y;
            fDz = fZ - vectors[1].Z;
            fResult += 1.0f / (fDx * fDx + fDy * fDy + fDz * fDz);

            fDx = fX - vectors[2].X;
            fDy = fY - vectors[2].Y;
            fDz = fZ - vectors[2].Z;
            fResult += 1.5f / (fDx * fDx + fDy * fDy + fDz * fDz);

            return fResult;
        }

        //fSample2 finds the distance of (fX, fY, fZ) from three moving lines
        float distanceToLines(float fX, float fY, float fZ)
        {
            float fResult = 0.0f;
            float fDx, fDy, fDz;
            fDx = fX - vectors[0].X;
            fDy = fY - vectors[0].Y;
            fResult += 0.5f / (fDx * fDx + fDy * fDy);

            fDx = fX - vectors[1].X;
            fDz = fZ - vectors[1].Z;
            fResult += 0.75f / (fDx * fDx + fDz * fDz);

            fDy = fY - vectors[2].Y;
            fDz = fZ - vectors[2].Z;
            fResult += 1.0f / (fDy * fDy + fDz * fDz);

            return fResult;
        }


        //fSample2 defines a height field by plugging the distance from the center into the sin and cos functions
        float height(float fX, float fY, float fZ)
        {
            float fHeight = 20.0f * Convert.ToSingle((fTime + Math.Sqrt((0.5f - fX) * (0.5f - fX) + (0.5f - fY) * (0.5f - fY))));
            fHeight = 1.5f + 0.1f * Convert.ToSingle((Math.Sin(fHeight) + Math.Cos(fHeight)));
            float fResult = (fHeight - fZ) * 50.0f;

            return fResult;
        }


        //vGetNormal() finds the gradient of the scalar field at a point
        //This gradient can be used as a very accurate vertex normal for lighting calculations
        Vector3 getNormal( float fX, float fY, float fZ)
        {
            Vector3 rfNormal = new Vector3();
            rfNormal.X = distanceToPoints(fX - 0.01f, fY, fZ) - distanceToPoints(fX + 0.01f, fY, fZ);
            rfNormal.Y = distanceToPoints(fX, fY - 0.01f, fZ) - distanceToPoints(fX, fY + 0.01f, fZ);
            rfNormal.Z = distanceToPoints(fX, fY, fZ - 0.01f) - distanceToPoints(fX, fY, fZ + 0.01f);
            normalizeVector(rfNormal, rfNormal);
            return rfNormal;
        }


      

        private bool CheckIntersection(int iEdgeFlags, int iEdge)
        {
            //if there is an intersection on this edge
            int edgeShift = 1 << iEdge;
            int result = iEdgeFlags & edgeShift;
            byte bResult = Convert.ToByte(result);
            if (bResult == 255)
                return true;
            else
                return false;
        }
      

        // For any edge, if one vertex is inside of the surface and the other is outside of the surface
        //  then the edge intersects the surface
        // For each of the 4 vertices of the tetrahedron can be two possible states : either inside or outside of the surface
        // For any tetrahedron the are 2^4=16 possible sets of vertex states
        // This table lists the edges intersected by the surface for all 16 possible vertex states
        // There are 6 edges.  For each entry in the table, if edge #n is intersected, then bit #n is set to 1


        void testapp()
        {
            float[] afPropertiesAmbient = { 0.50f, 0.50f, 0.50f, 1.00f };
            float[] afPropertiesDiffuse = { 0.75f, 0.75f, 0.75f, 1.00f };
            float[] afPropertiesSpecular = { 1.00f, 1.00f, 1.00f, 1.00f };

            //int iWidth = 640;
            //int iHeight = 480;

            //glutInit(&argc, argv);
            //glutInitWindowPosition(0, 0);
            //glutInitWindowSize(iWidth, iHeight);
            //glutInitDisplayMode(UT_RGB | UT_DEPTH | UT_DOUBLE);
            //glutCreateWindow("Marching Cubes");
            //glutDisplayFunc(vDrawScene);
            //glutIdleFunc(vIdle);
            //glutReshapeFunc(vResize);
            //glutKeyboardFunc(vKeyboard);
            //glutSpecialFunc(vSpecial);

            //glClearColor(0.0, 0.0, 0.0, 1.0);
            //glClearDepth(1.0);

            //glEnable(_DEPTH_TEST);
            //glEnable(_LIGHTING);
            //glPolygonMode(_FRONT_AND_BACK, polygonMode);

            //glLightfv(_LIGHT0, _AMBIENT, afPropertiesAmbient);
            //glLightfv(_LIGHT0, _DIFFUSE, afPropertiesDiffuse);
            //glLightfv(_LIGHT0, _SPECULAR, afPropertiesSpecular);
            //glLightModelf(_LIGHT_MODEL_TWO_SIDE, 1.0);

            //glEnable(_LIGHT0);

            //glMaterialfv(_BACK, _AMBIENT, afAmbientGreen);
            //glMaterialfv(_BACK, _DIFFUSE, afDiffuseGreen);
            //glMaterialfv(_FRONT, _AMBIENT, afAmbientBlue);
            //glMaterialfv(_FRONT, _DIFFUSE, afDiffuseBlue);
            //glMaterialfv(_FRONT, _SPECULAR, afSpecularWhite);
            //glMaterialf(_FRONT, _SHININESS, 25.0);

            //vResize(iWidth, iHeight);

            //vPrintHelp();
            //glutMainLoop();
        }
        //        void vIdle();
        //        void vDrawScene();
        //        void vResize(int, int);
        //        void vKeyboard(unsigned char cKey, int iX, int iY);
        //        void vSpecial(int iKey, int iX, int iY);

        //        void vPrintHelp();
        //        void vSetTime(float fTime);
        //        float fSample1(float fX, float fY, float fZ);
        //        float fSample2(float fX, float fY, float fZ);
        //        float fSample3(float fX, float fY, float fZ);
        //float(fSample)(float fX, float fY, float fZ) = fSample1;

        //void vMarchingCubes();
        //void vMarchCube1(float fX, float fY, float fZ, float fScale);
        //void vMarchCube2(float fX, float fY, float fZ, float fScale);
        //void(*vMarchCube)(float fX, float fY, float fZ, float fScale) = vMarchCube1;



        //void vPrintHelp()
        //{
        //    printf("Marching Cubes Example by Cory Bloyd (dejaspaminacan@my-deja.com)\n\n");

        //    printf("+/-  increase/decrease sample density\n");
        //    printf("PageUp/PageDown  increase/decrease surface value\n");
        //    printf("s  change sample function\n");
        //    printf("c  toggle marching cubes / marching tetrahedrons\n");
        //    printf("w  wireframe on/off\n");
        //    printf("l  toggle lighting / color-by-normal\n");
        //    printf("Home  spin scene on/off\n");
        //    printf("End  source point animation on/off\n");
        //}


        //void vResize(int iWidth, int iHeight)
        //{
        //    float fAspect, fHalfWorldSize = (1.4142135623730950488016887242097f / 2);

        //    glViewport(0, 0, iWidth, iHeight);
        //    glMatrixMode(_PROJECTION);
        //    glLoadIdentity();

        //    if (iWidth <= iHeight)
        //    {
        //        fAspect = (float)iHeight / (float)iWidth;
        //        glOrtho(-fHalfWorldSize, fHalfWorldSize, -fHalfWorldSize * fAspect,
        //                fHalfWorldSize * fAspect, -10 * fHalfWorldSize, 10 * fHalfWorldSize);
        //    }
        //    else
        //    {
        //        fAspect = (float)iWidth / (float)iHeight;
        //        glOrtho(-fHalfWorldSize * fAspect, fHalfWorldSize * fAspect, -fHalfWorldSize,
        //                fHalfWorldSize, -10 * fHalfWorldSize, 10 * fHalfWorldSize);
        //    }

        //    glMatrixMode(_MODELVIEW);
        //}

        //void vKeyboard(unsigned char cKey, int iX, int iY)
        //{
        //    switch (cKey)
        //    {
        //        case 'w':
        //            {
        //                if (polygonMode == _LINE)
        //                {
        //                    polygonMode = _FILL;
        //                }
        //                else
        //                {
        //                    polygonMode = _LINE;
        //                }
        //                glPolygonMode(_FRONT_AND_BACK, polygonMode);
        //            }
        //            break;
        //        case '+':
        //        case '=':
        //            {
        //                ++iDataSetSize;
        //                fStepSize = 1.0 / iDataSetSize;
        //            }
        //            break;
        //        case '-':
        //            {
        //                if (iDataSetSize > 1)
        //                {
        //                    --iDataSetSize;
        //                    fStepSize = 1.0 / iDataSetSize;
        //                }
        //            }
        //            break;
        //        case 'c':
        //            {
        //                if (vMarchCube == vMarchCube1)
        //                {
        //                    vMarchCube = vMarchCube2;//Use Marching Tetrahedrons
        //                }
        //                else
        //                {
        //                    vMarchCube = vMarchCube1;//Use Marching Cubes
        //                }
        //            }
        //            break;
        //        case 's':
        //            {
        //                if (fSample == fSample1)
        //                {
        //                    fSample = fSample2;
        //                }
        //                else if (fSample == fSample2)
        //                {
        //                    fSample = fSample3;
        //                }
        //                else
        //                {
        //                    fSample = fSample1;
        //                }
        //            }
        //            break;
        //        case 'l':
        //            {
        //                if (bLight)
        //                {
        //                    glDisable(_LIGHTING);//use vertex colors
        //                }
        //                else
        //                {
        //                    glEnable(_LIGHTING);//use lit material color
        //                }

        //                bLight = !bLight;
        //            };
        //    }
        //}


        //void vSpecial(int iKey, int iX, int iY)
        //{
        //    switch (iKey)
        //    {
        //        case UT_KEY_PAGE_UP:
        //            {
        //                if (fTargetValue < 1000.0)
        //                {
        //                    fTargetValue *= 1.1;
        //                }
        //            }
        //            break;
        //        case UT_KEY_PAGE_DOWN:
        //            {
        //                if (fTargetValue > 1.0)
        //                {
        //                    fTargetValue /= 1.1;
        //                }
        //            }
        //            break;
        //        case UT_KEY_HOME:
        //            {
        //                bSpin = !bSpin;
        //            }
        //            break;
        //        case UT_KEY_END:
        //            {
        //                bMove = !bMove;
        //            }
        //            break;
        //    }
        //}

        //void vIdle()
        //{
        //    glutPostRedisplay();
        //}

        //void vDrawScene()
        //{
        //    static float fPitch = 0.0;
        //    static float fYaw = 0.0;
        //    static float fTime = 0.0;

        //    glClear(_COLOR_BUFFER_BIT | _DEPTH_BUFFER_BIT);

        //    glPushMatrix();

        //    if (bSpin)
        //    {
        //        fPitch += 4.0;
        //        fYaw += 2.5;
        //    }
        //    if (bMove)
        //    {
        //        fTime += 0.025;
        //    }

        //    vSetTime(fTime);

        //    glTranslatef(0.0, 0.0, -1.0);
        //    glRotatef(-fPitch, 1.0, 0.0, 0.0);
        //    glRotatef(0.0, 0.0, 1.0, 0.0);
        //    glRotatef(fYaw, 0.0, 0.0, 1.0);

        //    glPushAttrib(_LIGHTING_BIT);
        //    glDisable(_LIGHTING);
        //    glColor3f(1.0, 1.0, 1.0);
        //    glutWireCube(1.0);
        //    glPopAttrib();


        //    glPushMatrix();
        //    glTranslatef(-0.5, -0.5, -0.5);
        //    glBegin(_TRIANES);
        //    vMarchingCubes();
        //    glEnd();
        //    glPopMatrix();


        //    glPopMatrix();

        //    glutSwapBuffers();
        //}

        //fGetOffset finds the approximate point of intersection of the surface
        // between two points with the values fValue1 and fValue2
    }

}
