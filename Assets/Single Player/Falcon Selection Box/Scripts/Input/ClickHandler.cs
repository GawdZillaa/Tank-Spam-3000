//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;

namespace Falcon
{
    //Class that handles a click from the Unity event system.
    [AddComponentMenu("Falcon/Input/Click Listener")]
    public class ClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            //Let the input manager know that a click event occurred.
            //Note: On pointer click can also be used, but it won't check for other events. OnReceiveClick will make sure the click event isn't colliding with other exclusive events.
            Debug.Log("Click Manager");
            AInputManager.instance.OnReceiveClick(gameObject, eventData);
        }
    }
}