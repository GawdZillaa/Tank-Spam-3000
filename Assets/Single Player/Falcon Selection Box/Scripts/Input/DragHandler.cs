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
    //Class that handles pointer drag over a surface. Requires unity's event system and a collider to detect events.
    [AddComponentMenu("Falcon/Input/Selection Area")]
    public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private AInputManager inputHandler = null;
        private DeselectAllClickHandler deselectClickHandler = null;

        void Awake()
        {
            inputHandler = GameObject.FindObjectOfType<AInputManager>();
            deselectClickHandler = GameObject.FindObjectOfType<DeselectAllClickHandler>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (deselectClickHandler != null)
            {
                deselectClickHandler.enabled = false;
            }
            inputHandler.onPointerBeginMove(eventData.pointerPressRaycast.worldPosition, eventData.pointerPressRaycast.worldPosition, eventData.pressPosition, eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            inputHandler.onPointerMoved(eventData.pointerPressRaycast.worldPosition, eventData.pointerPressRaycast.worldPosition, eventData.pressPosition, eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(deselectClickHandler != null)
            {
                deselectClickHandler.enabled = true;
            }
            inputHandler.onPointerEndMove(eventData.pointerPressRaycast.worldPosition, eventData.pointerPressRaycast.worldPosition, eventData.pressPosition, eventData.position);
        }
    }
}