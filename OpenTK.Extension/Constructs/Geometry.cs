using System;
using OpenTK;

namespace OpenTK.Extension
{
    public class Geometry
    {
        /// <summary>
        /// Calculate the array of vertex normals based on vertex and face information (assuming triangle polygons).
        /// </summary>
        /// <param name="vertexArray">The vertex data to find the normals for.</param>
        /// <param name="triangleArray">The element array describing the order in which vertices are drawn.</param>
        /// <returns></returns>
        public static Vector3[] CalculateNormals(Vector3[] vertexArray, uint[] triangleArray)
        {
            Vector3 b1, b2, normal;
            Vector3[] normalData = new Vector3[vertexArray.Length];

            for (uint i = 0; i < triangleArray.Length / 3; i++)
            {
                uint cornerA = triangleArray[i * 3];
                uint cornerB = triangleArray[i * 3 + 1];
                uint cornerC = triangleArray[i * 3 + 2];

                b1 = vertexArray[cornerB] - vertexArray[cornerA];
                b2 = vertexArray[cornerC] - vertexArray[cornerA];

                normal = Vector3.Cross(b1, b2).Normalize();

                normalData[cornerA] += normal;
                normalData[cornerB] += normal;
                normalData[cornerC] += normal;
            }

            for (int i = 0; i < normalData.Length; i++) normalData[i] = normalData[i].Normalize();

            return normalData;
        }
        /// <summary>
        /// Calculate the Tangent array based on the Vertex, Face, Normal and UV data.
        /// </summary>
        public static Vector3[] CalculateTangents(Vector3[] vertices, Vector3[] normals, uint[] triangles, Vector2[] uvs)
        {
            Vector3[] tangents = new Vector3[vertices.Length];
            Vector3[] tangentData = new Vector3[vertices.Length];

            for (int i = 0; i < triangles.Length / 3; i++)
            {
                Vector3 v1 = vertices[triangles[i * 3]];
                Vector3 v2 = vertices[triangles[i * 3 + 1]];
                Vector3 v3 = vertices[triangles[i * 3 + 2]];

                Vector2 w1 = uvs[triangles[i * 3]];
                Vector2 w2 = uvs[triangles[i * 3] + 1];
                Vector2 w3 = uvs[triangles[i * 3] + 2];

                float x1 = v2.X - v1.X;
                float x2 = v3.X - v1.X;
                float y1 = v2.Y - v1.Y;
                float y2 = v3.Y - v1.Y;
                float z1 = v2.Z - v1.Z;
                float z2 = v3.Z - v1.Z;

                float s1 = w2.X - w1.X;
                float s2 = w3.X - w1.X;
                float t1 = w2.Y - w1.Y;
                float t2 = w3.Y - w1.Y;
                float r = 1.0f / (s1 * t2 - s2 * t1);
                Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);

                tangents[triangles[i * 3]] += sdir;
                tangents[triangles[i * 3 + 1]] += sdir;
                tangents[triangles[i * 3 + 2]] += sdir;
            }

            for (int i = 0; i < vertices.Length; i++)
                tangentData[i] = (tangents[i] - normals[i] * Vector3.Dot(normals[i], tangents[i])).Normalize();

            return tangentData;
        }
        /// <summary>
        /// Create a basic quad by storing two triangles into a VAO.
        /// This quad includes UV co-ordinates from 0,0 to 1,1.
        /// </summary>
        /// <param name="program">The ShaderProgram assigned to this quad.</param>
        /// <param name="location">The location of the VAO (assigned to the vertices).</param>
        /// <param name="size">The size of the VAO (assigned to the vertices).</param>
        /// <returns>The VAO object representing this quad.</returns>
        public static VAO CreateQuad(ShaderProgram program, Vector2 location, Vector2 size)
        {
            Vector3[] vertices = new Vector3[] { new Vector3(location.X ,location.Y, 0), new Vector3(location.X + size.X, location.Y, 0), 
                new Vector3(location.X + size.X, location.Y + size.Y, 0), new Vector3(location.X , location.Y + size.Y , 0) };
            Vector2[] uvs = new Vector2[] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
            uint[] triangles = new uint[] { 0, 1, 2, 2, 3, 0 };

            return new VAO(program, new VBO<Vector3>("Quad vertices", vertices), new VBO<Vector2>("Quad UVS", uvs), new VBO<uint>("Quad triangles", triangles, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticRead));
        }

        /// <summary>
        /// Create a basic quad by storing two triangles into a VAO.
        /// This quad includes UV co-ordinates from uvloc to uvloc+uvsize.
        /// </summary>
        /// <param name="program">The ShaderProgram assigned to this quad.</param>
        /// <param name="location">The location of the VAO (assigned to the vertices).</param>
        /// <param name="size">The size of the VAO (assigned to the vertices).</param>
        /// <param name="uvloc">The origin of the UV co-ordinates.</param>
        /// <param name="uvsize">The size of the UV co-ordinates.</param>
        /// <returns>The VAO object representing this quad.</returns>
        public static VAO CreateQuad(ShaderProgram program, Vector2 location, Vector2 size, Vector2 uvloc, Vector2 uvsize)
        {
            Vector3[] vertices = new Vector3[] { new Vector3(location.X ,location.Y, 0), new Vector3(location.X + size.X, location.Y ,0), 
                new Vector3(location.X + size.X ,location.Y + size.Y ,0), new Vector3(location.X ,location.Y + size.Y, 0) };
            Vector2[] uvs = new Vector2[] { uvloc, new Vector2(uvloc.X + uvsize.X, uvloc.Y), new Vector2(uvloc.X + uvsize.X ,uvloc.Y + uvsize.Y), new Vector2(uvloc.X ,uvloc.Y + uvsize.Y) };
            uint[] triangles = new uint[] { 0, 1, 2, 2, 3, 0 };

            return new VAO(program, new VBO<Vector3>("Quad vertices", vertices), new VBO<Vector2>("Quad vertices", uvs), new VBO<uint>("Quad triangles", triangles, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticRead));
        }

        /// <summary>
        /// Create a basic quad by storing two triangles into a VAO.
        /// This quad includes normals, and does not include UV co-ordinates.
        /// </summary>
        /// <param name="program">The ShaderProgram assigned to this quad.</param>
        /// <param name="location">The location of the VAO (assigned to the vertices).</param>
        /// <param name="size">The size of the VAO (assigned to the vertices).</param>
        /// <returns>The VAO object representing this quad.</returns>
        public static VAO CreateQuadWithNormals(ShaderProgram program, Vector2 location, Vector2 size)
        {
            Vector3[] vertex = new Vector3[] { new Vector3(location.X, location.Y, 0), new Vector3(location.X + size.X ,location.Y, 0), 
                new Vector3(location.X + size.X, location.Y + size.Y, 0), new Vector3(location.X, location.Y + size.Y ,0) };
            uint[] triangles = new uint[] { 0, 1, 2, 2, 3, 0 };
            Vector3[] normal = CalculateNormals(vertex, triangles);

            return new VAO(program, new VBO<Vector3>("Quad vertices", vertex), new VBO<Vector3>("Quad normals", normal), new VBO<uint>("Quad triangles", triangles, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticRead));
        }

        /// <summary>
        /// Create a basic cube and store into a VAO.
        /// This cube consists of 12 triangles and 6 faces.
        /// </summary>
        /// <param name="program">The ShaderProgram assigned to this cube.</param>
        /// <param name="min">The 3 minimum values of the cube (lower left back corner).</param>
        /// <param name="max">The 3 maximum values of the cube (top right front corner).</param>
        /// <returns></returns>
        public static VAO CreateCube(ShaderProgram program, Vector3 min, Vector3 max)
        {
            Vector3[] vertex = new Vector3[] {
                new Vector3(min.X, min.Y, max.Z),
                new Vector3(max.X, min.Y ,max.Z),
                new Vector3(min.X, max.Y ,max.Z),
                new Vector3(max.X ,max.Y, max.Z),
                new Vector3(max.X ,min.Y, min.Z),
                new Vector3(max.X ,max.Y, min.Z),
                new Vector3(min.X ,max.Y ,min.Z),
                new Vector3(min.X ,min.Y, min.Z)
            };

            uint[] triangles = new uint[] {
                0, 1, 2, 1, 3, 2,
                1, 4, 3, 4, 5, 3,
                4, 7, 5, 7, 6, 5,
                7, 0, 6, 0, 2, 6,
                7, 4, 0, 4, 1, 0,
                2, 3, 6, 3, 5, 6
            };

            return new VAO(program, new VBO<Vector3>("Cube vertices", vertex), new VBO<uint>("Cube triangles", triangles, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticRead));
        }

        /// <summary>
        /// Create a basic cube with normals and stores it in a VAO.
        /// This cube consists of 12 triangles and 6 faces.
        /// </summary>
        /// <param name="program">The ShaderProgram assigned to this cube.</param>
        /// <param name="min">The 3 minimum values of the cube (lower left back corner).</param>
        /// <param name="max">The 3 maximum values of the cube (top right front corner).</param>
        /// <returns></returns>
        public static VAO CreateCubeWithNormals(ShaderProgram program, Vector3 min, Vector3 max)
        {
            Vector3[] vertex = new Vector3[] {
                new Vector3(min.X ,min.Y, max.Z),
                new Vector3(max.X ,min.Y, max.Z),
                new Vector3(min.X ,max.Y, max.Z),
                new Vector3(max.X, max.Y ,max.Z),
                new Vector3(max.X, min.Y ,min.Z),
                new Vector3(max.X, max.Y ,min.Z),
                new Vector3(min.X, max.Y ,min.Z),
                new Vector3(min.X, min.Y, min.Z)
            };

            uint[] triangles = new uint[] {
                0, 1, 2, 1, 3, 2,
                1, 4, 3, 4, 5, 3,
                4, 7, 5, 7, 6, 5,
                7, 0, 6, 0, 2, 6,
                7, 4, 0, 4, 1, 0,
                2, 3, 6, 3, 5, 6
            };

            Vector3[] normal = CalculateNormals(vertex, triangles);

            return new VAO(program, new VBO<Vector3>("Cube vertices", vertex), new VBO<Vector3>("Cube normals", normal), new VBO<uint>("Cube normals", triangles, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticRead));
        }
    }
}
