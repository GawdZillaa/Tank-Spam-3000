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
        Example implementation of the Selectable class.
        Shows how to customize selected behavior.
        This class changes the color the selected game object when selected and unselected.
        Enables the game object to be selected by the system. 
        Supports finding the bounds of a renderer and a collider.
        Notifies when the object gets selected or unselected.
    */
    [AddComponentMenu("Falcon/Example Selectable")]
    public class ExampleSelectable : Selectable
    {
        private Renderer myRenderer = null;
        public Collider myCollider = null;
        public Collider2D myCollider2D = null;
        public Color selectedColor = Color.red;
        public Color unselectedColor = Color.white;

        void Awake()
        {
            myRenderer = GetComponent<Renderer>();
            if (myCollider == null)
            {
                myCollider = GetComponent<Collider>();
            }
            if (myCollider2D == null)
            {
                myCollider2D = GetComponent<Collider2D>();
            }
        }

        public override void Select()
        {
            base.Select();
            //Customized behavior of the select method.
            //Change the color of the selected object to selectedColor.
            myRenderer.material.color = selectedColor;
        }

        public override void Unselect()
        {
            base.Unselect();
            //Change the color of the selected object to unselectedColor.
            if(myRenderer == null)
            {
                Debug.Log("COLORRRR FAIL");
                return;
           }
            myRenderer.material.color = unselectedColor;
        }

        public override Bounds GetBounds()
        {
            Bounds b = new Bounds();
            if (myCollider != null)
            {
                b = myCollider.bounds;
            }
            else if (myCollider2D != null)
            {
                b = myCollider2D.bounds;
            }
            return b;
        }

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