//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------
using UnityEngine;
using System.Collections;

namespace Falcon
{
    class GLQuad
    {
        public Vector3  vertex1 = Vector3.zero,
                        vertex2 = Vector3.zero,
                        vertex3 = Vector3.zero,
                        vertex4 = Vector3.zero;

        public GLQuad(Vector3 startVertex, Vector3 endVertex)
        {
            Calculate(startVertex, endVertex);
        }

        public void Calculate(Vector3 startVertex, Vector3 endVertex)
        {
            vertex1 = startVertex;
            vertex2 = new Vector3(endVertex.x, vertex1.y, 0f);
            vertex3 = endVertex;
            vertex4 = new Vector3(vertex1.x, endVertex.y, 0f);
        }

        public void Render(Material mat)
        {
            GL.PushMatrix();
            mat.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);
            GL.Color(Color.white);
            GL.Vertex(vertex1);
            GL.Vertex(vertex2);
            GL.Vertex(vertex3);
            GL.Vertex(vertex4);
            GL.End();
            GL.PopMatrix();
        }
    }
}
