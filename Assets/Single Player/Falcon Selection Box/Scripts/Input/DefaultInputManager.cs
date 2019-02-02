//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Falcon
{
    //Default implementation of the event system.
    public class DefaultInputManager : AInputManager
    {
        private bool isDragging = false;

        void OnEnable()
        {
            //Register default events so we don't get a null pointer exception if an event gets called with no registered functions.
            onPointerBeginMove += StubOnPointerBeginMove;
            onPointerMoved += StubOnPointerMove;
            onPointerEndMove += StubOnPointerEndMove;
            onPointerClick += ProcessClickAction;
        }

        void OnDisable()
        {
            //Make sure the events get unregistered.

            onPointerBeginMove -= StubOnPointerBeginMove;
            onPointerMoved -= StubOnPointerMove;
            onPointerEndMove -= StubOnPointerEndMove;
            onPointerClick -= ProcessClickAction;
        }

        protected override void Init()
        {
            base.Init();
     //       Debug.Log("Default Input -> Recieve Object!");

            //Try to find references to the systems.
            if (_selector == null )
            {
                _selector = GameObject.FindObjectOfType<Selector>();
            }
        }

        public override void OnReceiveClick(GameObject clickedObject, PointerEventData eventData)
        {
            //Do not notify the click event if the pointer is being dragged.
            Debug.Log("Default Input -> Recieve Object!");

            if (!isDragging)
            {
                onPointerClick(clickedObject, eventData);
            }
        }

        //Function that handles all clicks done to a GameObject with a 'Click Handler' component.
        void ProcessClickAction(GameObject clickEventSender, UnityEngine.EventSystems.PointerEventData eventData)
        {
            Debug.Log("Default Input -> ProcessClickAction");

            Selectable selectable = clickEventSender.GetComponent<Selectable>();

            //Is the user trying to click a selectable?
            if (selectable != null)
            {
                //Unselect all previously selected units and select the current one.
                _selector.UnselectAll();
                selectable.Select();
                _selector.AddToCurrentlySelected(selectable);
                _selector.NotifyOnFinishSelecting();
                return;
            }
        }

        public void StubOnPointerBeginMove(Vector3 startWorldCoordinates, Vector3 currentWorldCoordinates, Vector2 startScreenCoordinates, Vector2 currentScreenCoordinates)
        {
 //           Debug.Log("Default Input -> StubOnPointerBeginMove");

            //Raise the dragging flag.
            isDragging = true;
        }

        public void StubOnPointerMove(Vector3 startWorldCoordinates, Vector3 currentWorldCoordinates, Vector2 startScreenCoordinates, Vector2 currentScreenCoordinates){}

        public void StubOnPointerEndMove(Vector3 startWorldCoordinates, Vector3 currentWorldCoordinates, Vector2 startScreenCoordinates, Vector2 currentScreenCoordinates)
        {
//            Debug.Log("Default Input -> StubOnPointerEndMove");

            //Lower the dragging flag.
            isDragging = false;
        }
    }
}