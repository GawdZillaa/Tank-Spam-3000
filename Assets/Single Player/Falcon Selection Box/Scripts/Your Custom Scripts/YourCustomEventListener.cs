using UnityEngine;
using System.Collections;
using Falcon;
/*
    Convinient script that allows to list for selection events. Easy to modifiy.
*/
public class YourCustomEventListener : MonoBehaviour {

    private Selector selector = null;

    void Awake()
    {
        selector = GameObject.FindObjectOfType<Selector>();
        if(selector == null)
        {
            Debug.LogError("Could not find a Selector component in the scene, add one and try again.");
        }
    }

    void OnEnable()
    {
        if(selector != null)
        {
            selector.onObjectSelected += MyCustomOnObjectSelectMethod;
            selector.onObjectUnselected += MyCustomOnObjectUnSelectMethod;
            selector.onFinishSelecting += MyCustomOnMultiselectFinished;
        }
    }

    void OnDisable()
    {
        if (selector != null)
        {
            selector.onObjectSelected -= MyCustomOnObjectSelectMethod;
            selector.onObjectUnselected -= MyCustomOnObjectUnSelectMethod;
            selector.onFinishSelecting -= MyCustomOnMultiselectFinished;
        }
    }

    void MyCustomOnObjectSelectMethod(Selectable selectable)
    {
        Debug.Log("[MyCustomOnObjectSelectMethod] detected - " + selectable + " has been selected.");
    }

    void MyCustomOnObjectUnSelectMethod(Selectable selectable)
    {
        Debug.Log("[MyCustomOnObjectUnSelectMethod] detected - " + selectable + " has been unselected.");
    }

    void MyCustomOnMultiselectFinished(Selectable[] selectables)
    {
        foreach(Selectable selectable in selectables)
        {

            Debug.Log("[MyCustomOnMultiselectFinished] detected - " + selectable + " has been selected.");
        }
    }
}
