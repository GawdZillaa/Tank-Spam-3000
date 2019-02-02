//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;

namespace Falcon
{
    //Base class for all the input events. All classes that receive input events should notify to this class.
    public abstract class AInputManager : MonoBehaviour
    {
        private static AInputManager _instance;
        protected Selector _selector = null;
        
        //Event that gets called when a drag has started.
        public delegate void DelegateOnPointerBeginMove(Vector3 startWorldCoordinates, Vector3 currentWorldCoordinates, Vector2 startScreenCoordinates, Vector2 currentScreenCoordinates);
        public DelegateOnPointerMoved onPointerBeginMove;
        //Event that gets called when the pointer is being dragged.
        public delegate void DelegateOnPointerMoved(Vector3 startWorldCoordinates, Vector3 currentWorldCoordinates, Vector2 startScreenCoordinates, Vector2 currentScreenCoordinates);
        public DelegateOnPointerMoved onPointerMoved;
        //Event that gets called when the pointer has stopped dragging.
        public delegate void DelegateOnPointerEndMove(Vector3 startWorldCoordinates, Vector3 currentWorldCoordinates, Vector2 startScreenCoordinates, Vector2 currentScreenCoordinates);
        public DelegateOnPointerEndMove onPointerEndMove;
        //Event that gets called when a pointer click event has happened.
        public delegate void DelegateOnPointerClick(GameObject clickedObject, PointerEventData eventData);
        public DelegateOnPointerClick onPointerClick;
        //Convenient function to handle click events while other events are happening. 
        //This function should check if other exclusive events are happening and only notify the click event when necessary.
        public abstract void OnReceiveClick(GameObject clickedObject, PointerEventData eventData);

        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            //Remove duplicate Input Managers.
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AInputManager>();
            }
            else
            {
                if (AInputManager.instance.GetInstanceID() != this.GetInstanceID())
                {
                    Destroy(gameObject);
                }
            }
        }

        //Properties
        public Selector selector
        {
            get
            {
                return _selector;
            }
        }

        public static AInputManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<AInputManager>();
                }
                return _instance;
            }
        }
    }
}
