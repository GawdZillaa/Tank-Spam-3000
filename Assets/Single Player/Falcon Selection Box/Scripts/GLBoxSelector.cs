//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Falcon
{
    /// <summary>
    /// Implements Selector. Draws a box on the screen using 4 screen points.
    /// The drawing is done with Unity's OpenGL API. 
    /// </summary>
    [RequireComponent (typeof(Camera))]
    [AddComponentMenu("Falcon/OpenGL Box Selector")]
    public class GLBoxSelector : Selector
    {
        private List<GLLine> glLines = new List<GLLine>();
        private GLQuad glQuad = new GLQuad(Vector3.zero, Vector3.zero);
        public bool filled = true;
        public Material rendererMaterial = null;
        public bool render = true;
        public bool showSelectablesBoundingBoxes = false;

        void OnPostRender()
        {
            if (render)
            {
                //Draw all lines.
                foreach (GLLine glLine in glLines)
                {
                    glLine.Render(rendererMaterial);
                }
                //Draw our quad.
                if (glQuad != null)
                {
                    glQuad.Render(rendererMaterial);
                }
            }
        }

        void Update()
        {
            if (showSelectablesBoundingBoxes)
            {
                showSelectablesBoundingBoxes = false;
                DrawBoundingBoxes();
            }
        }
        public override void SelectorMoveStart(Vector3 startWorldCoordinates, Vector3 currentWorldCoordinates, Vector2 startScreenCoordinates, Vector2 currentScreenCoordinates)
        {
            //Draw our box to represent the enclosed selected area.
            DrawSquare(new Vector3(startScreenCoordinates.x / Screen.width, startScreenCoordinates.y / Screen.height, 0f), new Vector3(currentScreenCoordinates.x / Screen.width, currentScreenCoordinates.y / Screen.height, 0f));
        }

        public override void SelectorMove(Vector3 startWorldCoordinates, Vector3 currentWorldCoordinates, Vector2 startScreenCoordinates, Vector2 currentScreenCoordinates)
        {
            //Clear any line that was drawn before.
            glLines.Clear();
            //Draw our box to represent the enclosed selected area.
            DrawSquare(new Vector3(startScreenCoordinates.x / Screen.width, startScreenCoordinates.y / Screen.height, 0f), new Vector3(currentScreenCoordinates.x / Screen.width, currentScreenCoordinates.y / Screen.height, 0f));
            //Try to select all objects within startScreenCoordinates and currentScreenCoordinates.
            SelectObjects(startScreenCoordinates, currentScreenCoordinates);
        }

        public override void SelectorMoveEnd(Vector3 startWorldCoordinates, Vector3 currentWorldCoordinates, Vector2 startScreenCoordinates, Vector2 currentScreenCoordinates)
        {
            //Clear lines and the quad.
            glQuad = null;
            glLines.Clear();
            onFinishSelecting(selectedObjects);
        }

        public override Selectable[] SelectObjects(Vector3 startPoint, Vector3 endPoint)
        {
            UnselectAll();
            bool success = false;

            foreach (Selectable localSelectable in allSelectables)
            {
                //Ask the selectable if it has been selected.
                success = localSelectable.IsSelected(startPoint, endPoint);

                if (success)
                {
                    //Save our selected object
                    AddToCurrentlySelected(localSelectable);
                    //Select our object.
                    localSelectable.Select();
                }
            }
            return _selectedObjects.ToArray();
        }

        //Convenient method to display all the bounding boxes the system is using to select game objects.
        void DrawBoundingBoxes()
        {
            bool localFilled = filled;
            filled = false; //Does not work properly when filled option is set to true.
            foreach (Selectable localSelectable in allSelectables)
            {
                Rect boundsRectangle = localSelectable.GetBoundsScreenCoordinates();
                //Vector3 v1 = Camera.main.WorldToScreenPoint(localSelectable.GetComponent<Renderer>().bounds.center);
                //Vector3 v2 = Camera.main.WorldToScreenPoint(localSelectable.GetComponent<Renderer>().bounds.center + Vector3.one);
                //DrawSquare(new Vector3(v1.x / Screen.width, v1.y / Screen.height, 0f), new Vector3(v2.x / Screen.width, v2.y / Screen.height, 0f));
                DrawSquare(new Vector3(boundsRectangle.xMin / Screen.width, boundsRectangle.yMin / Screen.height, 0f), new Vector3(boundsRectangle.xMax / Screen.width, boundsRectangle.yMax / Screen.height, 0f));
            }
            filled = localFilled;
        }

        void DrawLine(Vector3 startPosition, Vector3 endPosition)
        {   
            glLines.Add(new GLLine(startPosition, endPosition));
        }

        void DrawQuad(Vector3 vertex1, Vector3 vertex2)
        {
            if (glQuad == null)
            {
                glQuad = new GLQuad(vertex1, vertex2);
            }
            else
            {
                glQuad.Calculate(vertex1, vertex2);
            }
        }

        void DrawSquare(Vector3 vertex1, Vector3 vertex2)
        {
            if (!filled)
            {
                //Draw the square using lines, so it appears as an unfilled squad.
                glLines.Add(new GLLine(vertex1, new Vector3(vertex2.x, vertex1.y, 0f)));
                glLines.Add(new GLLine(vertex1, new Vector3(vertex1.x, vertex2.y, 0f)));
                glLines.Add(new GLLine(new Vector3(vertex2.x, vertex1.y, 0f), vertex2));
                glLines.Add(new GLLine(new Vector3(vertex1.x, vertex2.y, 0f), vertex2));
            }
            else
            {
                DrawQuad(vertex1, vertex2);
            }
        }
    }
}