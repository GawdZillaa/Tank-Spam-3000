//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------
using UnityEngine;
using System.Collections;

namespace Falcon
{
    class GLLine
    {
        public Vector3 startVertex = Vector3.zero;
        public Vector3 endVertex = Vector3.zero;

        public GLLine(Vector3 startVertex, Vector3 endVertex)
        {
            this.startVertex = startVertex;
            this.endVertex = endVertex;
        }

        public void Render(Material mat)
        {
            GL.PushMatrix();
            mat.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.LINES);
            GL.Color(Color.white);
            GL.Vertex(startVertex);
            GL.Vertex(endVertex);
            GL.End();
            GL.PopMatrix();
        }
    }
}