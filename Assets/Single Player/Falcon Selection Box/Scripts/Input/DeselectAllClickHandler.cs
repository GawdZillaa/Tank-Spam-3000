//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;

namespace Falcon
{
    public class DeselectAllClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private Selector _selector = null;

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Deselect All Click Handler -> OnPointerClick");

            if (_selector == null)
            {
                _selector = GameObject.FindObjectOfType<Selector>();
            }
            if (_selector != null)
            {
                _selector.UnselectAll();
            }
        }
    }
}