//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------
using UnityEngine;
using System.Collections;
using System;

namespace Falcon
{
    /*
        Default implementation of the Selectable class.
        Enables the game object to be selected by the system. 
        Supports finding the bounds of a renderer and a collider.
        Notifies when the object gets selected or unselected.
    */
    [AddComponentMenu("Falcon/Default Selectable")]
    public class DefaultSelectable : Selectable
    {
        private Renderer myRenderer = null;
        public Collider myCollider = null;
        public Collider2D myCollider2D = null;

        void Awake()
        {
            myRenderer = GetComponent<Renderer>();
            //Identify 3D colliders and 2D colliders.
            if(myCollider == null)
            {
                myCollider = GetComponent<Collider>();
            }
            if(myCollider2D == null)
            {
                myCollider2D = GetComponent<Collider2D>();
            }
        }
            
        //Gets the bounds of a collider, in world coordinates.
        public override Bounds GetBounds()
        {
            Bounds bounds = new Bounds();
            if(myCollider != null)
            {
                bounds = myCollider.bounds;
            }
            else if(myCollider2D != null)
            {
                bounds = myCollider2D.bounds;
            }
            return bounds;
        }

        //Get the bounds of the renderer, in screen coordinates.
        public override Rect GetBoundsScreenCoordinates()
        {
            Vector3 localCenter = myRenderer.bounds.center;
            Vector3 localExtents = myRenderer.bounds.extents;
            Vector2[] extentPoints = new Vector2[8]
             {
               Camera.main.WorldToScreenPoint(new Vector3(localCenter.x - localExtents.x, localCenter.y - localExtents.y, localCenter.z - localExtents.z)),
               Camera.main.WorldToScreenPoint(new Vector3(localCenter.x + localExtents.x, localCenter.y - localExtents.y, localCenter.z - localExtents.z)),
               Camera.main.WorldToScreenPoint(new Vector3(localCenter.x - localExtents.x, localCenter.y - localExtents.y, localCenter.z + localExtents.z)),
               Camera.main.WorldToScreenPoint(new Vector3(localCenter.x + localExtents.x, localCenter.y - localExtents.y, localCenter.z + localExtents.z)),
               Camera.main.WorldToScreenPoint(new Vector3(localCenter.x - localExtents.x, localCenter.y + localExtents.y, localCenter.z - localExtents.z)),
               Camera.main.WorldToScreenPoint(new Vector3(localCenter.x + localExtents.x, localCenter.y + localExtents.y, localCenter.z - localExtents.z)),
               Camera.main.WorldToScreenPoint(new Vector3(localCenter.x - localExtents.x, localCenter.y + localExtents.y, localCenter.z + localExtents.z)),
               Camera.main.WorldToScreenPoint(new Vector3(localCenter.x + localExtents.x, localCenter.y + localExtents.y, localCenter.z + localExtents.z))
             };
            Vector2 min = extentPoints[0];
            Vector2 max = extentPoints[0];
            foreach (Vector2 localVector in extentPoints)
            {
                min = Vector2.Min(min, localVector);
                max = Vector2.Max(max, localVector);
            }
            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }
    }
}