using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.ootii.Utilities.Debug
{
    public class DebugDraw
    {
        static Material material = new Material(
        @"Shader ""Custom/Draw"" {
        Properties {
            _Color (""Main Color"", Color) = (1,1,1,1)
        }
        SubShader {
            Pass {
                Color [_Color]
            }
        }
        }");

        static MeshCreator creator = new MeshCreator();
        static Mesh solidSphere;
        static Mesh solidCube;

        static List<Vector3> sLines = new List<Vector3>();

        static DebugDraw()
        {
            solidSphere = creator.CreateSphere(3);
            solidCube = creator.CreateCube(0);
        }

        /// <summary>
        /// Draws a sphere
        /// </summary>
        /// <param name="rCenter"></param>
        /// <param name="rRadius"></param>
        /// <param name="rColor"></param>
        public static void DrawSphere(Vector3 rCenter, float rRadius, Color rColor, bool rWireframe)
        {
            Color lOldColor = Gizmos.color;
            Matrix4x4 lOldMatrix = Gizmos.matrix;

            Gizmos.color = rColor;
            Gizmos.matrix = Matrix4x4.identity;

            if (rWireframe)
            {
                Gizmos.DrawWireSphere(rCenter, rRadius);
            }
            else
            {
                Gizmos.DrawSphere(rCenter, rRadius);
            }

            Gizmos.color = lOldColor;
            Gizmos.matrix = lOldMatrix;
        }

        /// <summary>
        /// Draws a cube
        /// </summary>
        /// <param name="rCenter"></param>
        /// <param name="rRadius"></param>
        /// <param name="rColor"></param>
        public static void DrawCube(Vector3 rCenter, Vector3 rSize, Color rColor, bool rWireframe)
        {
            Color lOldColor = Gizmos.color;
            Matrix4x4 lOldMatrix = Gizmos.matrix;

            Gizmos.color = rColor;
            Gizmos.matrix = Matrix4x4.identity;
            
            if (rWireframe)
            {
                Gizmos.DrawWireCube(rCenter, rSize);
            }
            else
            {
                Gizmos.DrawCube(rCenter, rSize);
            }

            Gizmos.color = lOldColor;
            Gizmos.matrix = lOldMatrix;
        }

        /// <summary>
        /// Draws a horizontal circle
        /// </summary>
        /// <param name="rCenter"></param>
        /// <param name="rRadius"></param>
        /// <param name="rColor"></param>
        public static void DrawCircle(Vector3 rCenter, float rRadius, Color rColor)
        {
            DrawArc(rCenter, Quaternion.identity, 0, 360, rRadius, rColor);
        }

        /// <summary>
        /// Draws a horizontal circle
        /// </summary>
        /// <param name="rCenter"></param>
        /// <param name="rRadius"></param>
        /// <param name="rColor"></param>
        public static void DrawArc(Vector3 rCenter, Quaternion rRotation, float rMinAngle, float rMaxAngle, float rRadius, Color rColor)
        {
            sLines.Clear();

            float lStep = 10f;
            Vector3 lPoint = Vector3.zero;

            // The circle wants to go from the +X direction and
            // rotate counter-clockwise as the theta increase.
            // Logically, I think +Z should be forward and 
            // it should rotate clockwise as theta increases. Hence the 'TrueTheta'.
            for (float lTheta = rMinAngle; lTheta <= rMaxAngle; lTheta += lStep)
            {
                float lTrueTheta = -(lTheta * Mathf.Deg2Rad) + 1.57079f; 
                lPoint.x = rRadius * Mathf.Cos(lTrueTheta);
                lPoint.y = 0;
                lPoint.z = rRadius * Mathf.Sin(lTrueTheta);
                sLines.Add(lPoint);
            }

            // Transform the points based on the center and rotation
            Matrix4x4 lMatrix = Matrix4x4.TRS(rCenter, rRotation, Vector3.one);
            for (int i = 0; i < sLines.Count; i++) { sLines[i] = lMatrix.MultiplyPoint3x4(sLines[i]); }

            // Draw out the lines
            DrawLines(sLines, rColor);
        }

        /// <summary>
        /// Draw a simple frustum
        /// </summary>
        /// <param name="rPosition"></param>
        /// <param name="rRotation"></param>
        /// <param name="rHAngle"></param>
        /// <param name="rVAngle"></param>
        /// <param name="rDistance"></param>
        public static void DrawFrustum(Vector3 rPosition, Quaternion rRotation, float rHAngle, float rVAngle, float rDistance, Color rColor)
        {
            Color lOldColor = Gizmos.color;
            Matrix4x4 lOldMatrix = Gizmos.matrix;

            Gizmos.color = rColor;
            Gizmos.matrix = Matrix4x4.TRS(rPosition, rRotation, Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, rVAngle, rDistance, 0.1f, rHAngle / rVAngle);

            Gizmos.color = lOldColor;
            Gizmos.matrix = lOldMatrix;
        }

        /// <summary>
        /// Draws a frustum
        /// </summary>
        /// <param name="rPosition"></param>
        /// <param name="rRotation"></param>
        /// <param name="rHAngle"></param>
        /// <param name="rVAngle"></param>
        /// <param name="rDistance"></param>
        /// <param name="rColor"></param>
        public static void DrawFrustumArc(Vector3 rPosition, Quaternion rRotation, float rHAngle, float rVAngle, float rDistance, Color rColor)
        {
            float lStep = 10f;
            Vector3 lPoint = Vector3.zero;

            float lHalfHAngle = rHAngle * 0.5f;
            float lHalfVAngle = rVAngle * 0.5f;

            List<Vector3> lCenters = new List<Vector3>(2) { Vector3.zero, Vector3.zero };
            List<Vector3> lLeftCorners = new List<Vector3>(5) { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
            List<Vector3> lRightCorners = new List<Vector3>(5) { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

            // bottom close arc 
            sLines.Clear();
            for (float lTheta = -lHalfHAngle; lTheta <= lHalfHAngle; lTheta += lStep)
            {
                float lTrueTheta = -(lTheta * Mathf.Deg2Rad) + 1.57079f;
                lPoint.x = 1.0f * Mathf.Cos(lTrueTheta);
                lPoint.y = 0;
                lPoint.z = 1.0f * Mathf.Sin(lTrueTheta);
                sLines.Add(lPoint);
            }

            // Transform the points based on the center and rotation
            Matrix4x4 lMatrix = Matrix4x4.TRS(rPosition, rRotation * Quaternion.AngleAxis(lHalfVAngle, Vector3.right), Vector3.one);
            for (int i = 0; i < sLines.Count; i++) { sLines[i] = lMatrix.MultiplyPoint3x4(sLines[i]); }

            lLeftCorners[1] = sLines[0]; // bottom close left
            lRightCorners[1] = sLines[sLines.Count - 1]; // bottom close right

            // Draw out the lines
            DrawLines(sLines, rColor);

            // Bottom far arc 
            sLines.Clear();
            for (float lTheta = -lHalfHAngle; lTheta <= lHalfHAngle; lTheta += lStep)
            {
                float lTrueTheta = -(lTheta * Mathf.Deg2Rad) + 1.57079f;
                lPoint.x = rDistance * Mathf.Cos(lTrueTheta);
                lPoint.y = 0;
                lPoint.z = rDistance * Mathf.Sin(lTrueTheta);
                sLines.Add(lPoint);
            }

            // Transform the points based on the center and rotation
            lMatrix = Matrix4x4.TRS(rPosition, rRotation * Quaternion.AngleAxis(lHalfVAngle, Vector3.right), Vector3.one);
            for (int i = 0; i < sLines.Count; i++) { sLines[i] = lMatrix.MultiplyPoint3x4(sLines[i]); }

            lLeftCorners[0] = sLines[0]; // bottom far left
            lLeftCorners[4] = sLines[0]; // bottom far left

            lRightCorners[0] = sLines[sLines.Count - 1]; // bottom far right
            lRightCorners[4] = sLines[sLines.Count - 1]; // bottom far right

            lCenters[0] = sLines[sLines.Count / 2]; // bottom far center

            // Draw out the lines
            DrawLines(sLines, rColor);

            // Top close arc 
            sLines.Clear();
            for (float lTheta = -lHalfHAngle; lTheta <= lHalfHAngle; lTheta += lStep)
            {
                float lTrueTheta = -(lTheta * Mathf.Deg2Rad) + 1.57079f;
                lPoint.x = 1.0f * Mathf.Cos(lTrueTheta);
                lPoint.y = 0;
                lPoint.z = 1.0f * Mathf.Sin(lTrueTheta);
                sLines.Add(lPoint);
            }

            // Transform the points based on the center and rotation
            lMatrix = Matrix4x4.TRS(rPosition, rRotation * Quaternion.AngleAxis(-lHalfVAngle, Vector3.right), Vector3.one);
            for (int i = 0; i < sLines.Count; i++) { sLines[i] = lMatrix.MultiplyPoint3x4(sLines[i]); }

            lLeftCorners[2] = sLines[0]; // top close left
            lRightCorners[2] = sLines[sLines.Count - 1]; // top close right

            // Draw out the lines
            DrawLines(sLines, rColor);

            // Top far arc 
            sLines.Clear();
            for (float lTheta = -lHalfHAngle; lTheta <= lHalfHAngle; lTheta += lStep)
            {
                float lTrueTheta = -(lTheta * Mathf.Deg2Rad) + 1.57079f;
                lPoint.x = rDistance * Mathf.Cos(lTrueTheta);
                lPoint.y = 0;
                lPoint.z = rDistance * Mathf.Sin(lTrueTheta);
                sLines.Add(lPoint);
            }

            // Transform the points based on the center and rotation
            lMatrix = Matrix4x4.TRS(rPosition, rRotation * Quaternion.AngleAxis(-lHalfVAngle, Vector3.right), Vector3.one);
            for (int i = 0; i < sLines.Count; i++) { sLines[i] = lMatrix.MultiplyPoint3x4(sLines[i]); }

            lLeftCorners[3] = sLines[0]; // top far left
            lRightCorners[3] = sLines[sLines.Count - 1]; // top far right
            lCenters[1] = sLines[sLines.Count / 2]; // top far center

            // Draw out the lines
            DrawLines(sLines, rColor);

            // Center vertical close arch
            sLines.Clear();
            for (float lTheta = -lHalfVAngle; lTheta <= lHalfVAngle; lTheta += lStep)
            {
                float lTrueTheta = -(lTheta * Mathf.Deg2Rad) + 1.57079f;
                lPoint.x = 0f;
                lPoint.y = 1.0f * Mathf.Cos(lTrueTheta);
                lPoint.z = 1.0f * Mathf.Sin(lTrueTheta);
                sLines.Add(lPoint);
            }

            // Transform the points based on the center and rotation
            lMatrix = Matrix4x4.TRS(rPosition, rRotation, Vector3.one);
            for (int i = 0; i < sLines.Count; i++) { sLines[i] = lMatrix.MultiplyPoint3x4(sLines[i]); }

            // Draw out the lines
            DrawLines(sLines, rColor);

            // Center vertical close arch
            sLines.Clear();
            for (float lTheta = -lHalfVAngle; lTheta <= lHalfVAngle; lTheta += lStep)
            {
                float lTrueTheta = -(lTheta * Mathf.Deg2Rad) + 1.57079f;
                lPoint.x = 0f;
                lPoint.y = rDistance * Mathf.Cos(lTrueTheta);
                lPoint.z = rDistance * Mathf.Sin(lTrueTheta);
                sLines.Add(lPoint);
            }

            // Transform the points based on the center and rotation
            lMatrix = Matrix4x4.TRS(rPosition, rRotation, Vector3.one);
            for (int i = 0; i < sLines.Count; i++) { sLines[i] = lMatrix.MultiplyPoint3x4(sLines[i]); }

            // Draw out the lines
            DrawLines(sLines, rColor);
            // Draw out the ends
            DrawLines(lLeftCorners, rColor);
            DrawLines(lRightCorners, rColor);
        }
        
        /// <summary>
        /// Draw a simple colored line
        /// </summary>
        /// <param name="rFrom"></param>
        /// <param name="rTo"></param>
        /// <param name="rColor"></param>
        public static void DrawLine(Vector3 rFrom, Vector3 rTo, Color rColor)
        {
            Color lOldColor = Gizmos.color;
            Matrix4x4 lOldMatrix = Gizmos.matrix;
            
            Gizmos.color = rColor;
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.DrawLine(rFrom, rTo);

            Gizmos.color = lOldColor;
            Gizmos.matrix = lOldMatrix;
        }

        /// <summary>
        /// Drawas a list of connected lines where i0->i1->i2 etc.
        /// </summary>
        /// <param name="rLines"></param>
        /// <param name="rColor"></param>
        public static void DrawLines(List<Vector3> rLines, Color rColor)
        {
            Color lOldColor = Gizmos.color;
            Matrix4x4 lOldMatrix = Gizmos.matrix;

            Gizmos.color = rColor;
            Gizmos.matrix = Matrix4x4.identity;

            for (int i = 1; i < rLines.Count; i++)
            {
                Gizmos.DrawLine(rLines[i - 1], rLines[i]);
            }

            Gizmos.color = lOldColor;
            Gizmos.matrix = lOldMatrix;
        }

        /// <summary>
        /// Draws an actual sphere mesh
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        public static void DrawSphereMesh(Vector3 position, float radius, Color color)
        {
            Matrix4x4 mat = Matrix4x4.TRS(position, Quaternion.identity, radius * 0.5f * Vector3.one);
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.AddColor("_Color", color);
            Graphics.DrawMesh(solidSphere, mat, material, 0, null, 0, block);
        }

        /// <summary>
        /// Draws an actual cube mesh
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public static void DrawCubeMesh(Vector3 position, Quaternion rotation, float size, Color color)
        {
            Matrix4x4 mat = Matrix4x4.TRS(position, rotation, size * Vector3.one);
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.AddColor("_Color", color);
            Graphics.DrawMesh(solidCube, mat, material, 0, null, 0, block);
        }

        #region MeshCreator
        public class MeshCreator
        {
            private List<Vector3> positions;
            private List<Vector2> uvs;
            private int index;
            private Dictionary<Int64, int> middlePointIndexCache;

            // add vertex to mesh, fix position to be on unit sphere, return index
            private int addVertex(Vector3 p, Vector2 uv)
            {
                positions.Add(p);
                uvs.Add(uv);
                return index++;
            }

            // return index of point in the middle of p1 and p2
            private int getMiddlePoint(int p1, int p2)
            {
                // first check if we have it already
                bool firstIsSmaller = p1 < p2;
                Int64 smallerIndex = firstIsSmaller ? p1 : p2;
                Int64 greaterIndex = firstIsSmaller ? p2 : p1;
                Int64 key = (smallerIndex << 32) + greaterIndex;

                int ret;
                if (this.middlePointIndexCache.TryGetValue(key, out ret))
                {
                    return ret;
                }

                // not in cache, calculate it
                Vector3 point1 = this.positions[p1];
                Vector3 point2 = this.positions[p2];
                Vector3 middle = new Vector3(
                    (point1.x + point2.x) / 2.0f,
                    (point1.y + point2.y) / 2.0f,
                    (point1.z + point2.z) / 2.0f);

                Vector2 uv1 = this.uvs[p1];
                Vector2 uv2 = this.uvs[p2];
                Vector2 uvmid = new Vector2(
                    (uv1.x + uv2.x) / 2.0f,
                    (uv1.y + uv2.y) / 2.0f);

                // add vertex makes sure point is on unit sphere
                int i = addVertex(middle, uvmid);

                // store it, return index
                this.middlePointIndexCache.Add(key, i);
                return i;
            }

            public Mesh CreateCube(int subdivisions)
            {
                positions = new List<Vector3>();
                uvs = new List<Vector2>();
                middlePointIndexCache = new Dictionary<long, int>();
                index = 0;

                var indices = new List<int>();

                // front
                addVertex(new Vector3(-1, -1, 1), new Vector2(1, 0));
                addVertex(new Vector3(-1, 1, 1), new Vector2(1, 1));
                addVertex(new Vector3(1, 1, 1), new Vector2(0, 1));
                addVertex(new Vector3(1, -1, 1), new Vector2(0, 0));
                indices.Add(0); indices.Add(3); indices.Add(2);
                indices.Add(2); indices.Add(1); indices.Add(0);

                // right
                addVertex(new Vector3(1, -1, 1), new Vector2(1, 0));
                addVertex(new Vector3(1, 1, 1), new Vector2(1, 1));
                addVertex(new Vector3(1, 1, -1), new Vector2(0, 1));
                addVertex(new Vector3(1, -1, -1), new Vector2(0, 0));
                indices.Add(4); indices.Add(7); indices.Add(6);
                indices.Add(6); indices.Add(5); indices.Add(4);

                // back
                addVertex(new Vector3(1, -1, -1), new Vector2(1, 0));
                addVertex(new Vector3(1, 1, -1), new Vector2(1, 1));
                addVertex(new Vector3(-1, 1, -1), new Vector2(0, 1));
                addVertex(new Vector3(-1, -1, -1), new Vector2(0, 0));
                indices.Add(8); indices.Add(11); indices.Add(10);
                indices.Add(10); indices.Add(9); indices.Add(8);

                // left
                addVertex(new Vector3(-1, -1, -1), new Vector2(1, 0));
                addVertex(new Vector3(-1, 1, -1), new Vector2(1, 1));
                addVertex(new Vector3(-1, 1, 1), new Vector2(0, 1));
                addVertex(new Vector3(-1, -1, 1), new Vector2(0, 0));
                indices.Add(12); indices.Add(15); indices.Add(14);
                indices.Add(14); indices.Add(13); indices.Add(12);

                // top
                addVertex(new Vector3(1, 1, 1), new Vector2(0, 0));
                addVertex(new Vector3(1, 1, -1), new Vector2(0, 1));
                addVertex(new Vector3(-1, 1, -1), new Vector2(1, 1));
                addVertex(new Vector3(-1, 1, 1), new Vector2(1, 0));
                indices.Add(16); indices.Add(17); indices.Add(18);
                indices.Add(18); indices.Add(19); indices.Add(16);

                // bottom
                addVertex(new Vector3(1, -1, 1), new Vector2(1, 0));
                addVertex(new Vector3(1, -1, -1), new Vector2(1, 1));
                addVertex(new Vector3(-1, -1, -1), new Vector2(0, 1));
                addVertex(new Vector3(-1, -1, 1), new Vector2(0, 0));
                indices.Add(21); indices.Add(20); indices.Add(23);
                indices.Add(23); indices.Add(22); indices.Add(21);

                for (int i = 0; i < subdivisions; i++)
                {
                    var indices2 = new List<int>();
                    for (int idx = 0; idx < indices.Count; idx += 3)
                    {
                        // replace triangle by 4 triangles
                        int a = getMiddlePoint(indices[idx + 0], indices[idx + 1]);
                        int b = getMiddlePoint(indices[idx + 1], indices[idx + 2]);
                        int c = getMiddlePoint(indices[idx + 2], indices[idx + 0]);

                        indices2.Add(indices[idx + 0]); indices2.Add(a); indices2.Add(c);
                        indices2.Add(indices[idx + 1]); indices2.Add(b); indices2.Add(a);
                        indices2.Add(indices[idx + 2]); indices2.Add(c); indices2.Add(b);
                        indices2.Add(a); indices2.Add(b); indices2.Add(c);
                    }
                    indices = indices2;
                }

                // done, create the mesh
                var mesh = new Mesh();

                mesh.vertices = positions.ToArray();
                mesh.triangles = indices.ToArray();
                mesh.uv = uvs.ToArray();
                mesh.RecalculateNormals();

                var colors = new Color[mesh.vertexCount];
                for (int i = 0; i < colors.Length; i++)
                    colors[i] = new Color(1.0f, 1.0f, 1.0f);
                mesh.colors = colors;

                RecalculateTangents(mesh);

                return mesh;
            }

            public Mesh CreateSphere(int subdivisions)
            {
                var sphere = CreateCube(subdivisions);
                var vertices = new List<Vector3>(sphere.vertices);

                for (int i = 0; i < vertices.Count; i++)
                    vertices[i] = vertices[i].normalized;

                sphere.vertices = vertices.ToArray();
                sphere.RecalculateNormals();
                RecalculateTangents(sphere);

                return sphere;
            }

            // Lengyel, Eric. “Computing Tangent Space Basis Vectors for an Arbitrary Mesh”.
            // Terathon Software 3D Graphics Library, 2001. http://www.terathon.com/code/tangent.html
            public static void RecalculateTangents(Mesh mesh)
            {
                var tan1 = new Vector3[mesh.vertexCount];
                var tan2 = new Vector3[mesh.vertexCount];

                for (int a = 0; a < mesh.triangles.Length; a += 3)
                {
                    int i1 = mesh.triangles[a + 0];
                    int i2 = mesh.triangles[a + 1];
                    int i3 = mesh.triangles[a + 2];

                    Vector3 v1 = mesh.vertices[i1];
                    Vector3 v2 = mesh.vertices[i2];
                    Vector3 v3 = mesh.vertices[i3];

                    Vector2 w1 = mesh.uv[i1];
                    Vector2 w2 = mesh.uv[i2];
                    Vector2 w3 = mesh.uv[i3];

                    float x1 = v2.x - v1.x;
                    float x2 = v3.x - v1.x;
                    float y1 = v2.y - v1.y;
                    float y2 = v3.y - v1.y;
                    float z1 = v2.z - v1.z;
                    float z2 = v3.z - v1.z;

                    float s1 = w2.x - w1.x;
                    float s2 = w3.x - w1.x;
                    float t1 = w2.y - w1.y;
                    float t2 = w3.y - w1.y;

                    float r = 1.0F / (s1 * t2 - s2 * t1);
                    var sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r,
                            (t2 * z1 - t1 * z2) * r);
                    var tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r,
                            (s1 * z2 - s2 * z1) * r);

                    tan1[i1] += sdir;
                    tan1[i2] += sdir;
                    tan1[i3] += sdir;

                    tan2[i1] += tdir;
                    tan2[i2] += tdir;
                    tan2[i3] += tdir;
                }

                var tangents = new Vector4[mesh.vertexCount];
                for (long a = 0; a < mesh.vertexCount; a++)
                {
                    Vector3 n = mesh.normals[a];
                    Vector3 t = tan1[a];

                    // Gram-Schmidt orthogonalize
                    tangents[a] = t - n * Vector3.Dot(n, t);
                    tangents[a].Normalize();

                    // Calculate handedness
                    tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
                }

                mesh.tangents = tangents;
            }
        #endregion
        }
    }
}