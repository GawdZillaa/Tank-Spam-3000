//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------
using UnityEngine;
using System;
using System.Collections;

namespace Falcon
{
    public enum SelectionDetectionMethod
    {
        Edge,
        Center,
        Custom
        
    };
    /*
        Base class for all selectables Selectable.
        Enables the game object to be selected by the system. 
        Supports finding the bounds of a renderer and a collider.
        Notifies when the object gets selected or unselected.
    */
    [RequireComponent(typeof(Renderer))]
    public abstract class Selectable : MonoBehaviour
    {
        protected bool _selected = false;
        protected Selector selector = null;
        public SelectionDetectionMethod selectionDetectionMethod;

        //Events
        public delegate void DelegateOnSelect(Selectable gameObject);
        public DelegateOnSelect onSelect;
        public delegate void DelegateOnUnselect(Selectable gameObject);
        public DelegateOnUnselect onUnselect;

        //Abstract methods
        public abstract Bounds GetBounds();
        public abstract Rect GetBoundsScreenCoordinates();

        //Method that checks if this game object is selected by the Selector
        public virtual bool IsSelected(Vector3 selectionStartPoint, Vector3 selectionEndPoint)
        {
            bool success = false;
            //Get the bounds using screen coordinates.
            Rect boundsRectangle = GetBoundsScreenCoordinates();
            Vector3 minVec = Vector3.Min(selectionStartPoint, selectionEndPoint);
            Vector3 maxVec = Vector3.Max(selectionStartPoint, selectionEndPoint);

            Rect selectionRectangle = Rect.MinMaxRect(minVec.x, minVec.y, maxVec.x, maxVec.y);

            //Call each different algorithm to detect if I am selected.
            switch (selectionDetectionMethod)
            {
                case SelectionDetectionMethod.Edge:
                    if (selectionRectangle.Overlaps(boundsRectangle, true))
                    {
                        success = true;
                    }
                    break;
                case SelectionDetectionMethod.Center:
                    if (selectionRectangle.Contains(boundsRectangle.center, true))
                    {
                        success = true;
                    }
                    break;
                case SelectionDetectionMethod.Custom:
                    success = CustomSelectionDetectionMethod(boundsRectangle, selectionRectangle);
                    break;
            }

            return success;
        }

        //Use this method when Edge or Center detection is not enough and you want to implement your own.
        protected virtual bool CustomSelectionDetectionMethod(Rect boundsRectangle, Rect selectionRectangle)
        {
            Debug.LogWarning("You are calling the default implementation of 'CustomSelectionDetectionMethod', which always returns true. Implement it or override it in the subclass.");
            return true;
        }

        protected virtual void OnEnable()
        {
            //Register default events to our delegates. This way we don't get a null reference exception if the delegate doesn't have a registered function.
            onSelect += StubOnSelect;
            onUnselect += StubOnUnselect;
            //This class needs a Selector to connect to the system. Find it.
            FindSelector();
            if (selector != null)
            {
                //Register this selectable to the system.
                selector.RegisterSelectable(this);
            }
        }

        protected virtual void OnDisable()
        {
            //Unregister our events to the delegates.
            onSelect -= StubOnSelect;
            onUnselect -= StubOnUnselect;
            //This class needs a Selector to connect to the system. Find it.
            FindSelector();
            if (selector != null)
            {
                //Unregister this selectable from the system.
                selector.UnregisterSelectable(this);
            }
        }

        //Function to select this object. This function is the one to customize selected behavior.
        public virtual void Select()
        {
            onSelect(this);
            _selected = true;
        }

        //Function to select this object. This function is the one to customize selected behavior.
        public virtual void Unselect()
        {
            if (this == null)
            {
                Debug.Log("UNSELECT~~~~~~");
                //                      Falcon.AInputManager.instance.selector.UnselectAll();
                _selected = false;
                return;
            }
//            Debug.Log("UNSELECT NORMAL++++++");
            onUnselect(this);
            _selected = false;
        }

        //Methods
        protected void FindSelector()
        {
            if (selector == null)
            {
                selector = GameObject.FindObjectOfType<Selector>();
            }
        }

        //Stubs
        protected virtual void StubOnSelect(Selectable localObject) {}
        protected virtual void StubOnUnselect(Selectable localObject) {}

        public bool selected
        {
            get
            {
                return _selected;
            }
        }
    }
}
