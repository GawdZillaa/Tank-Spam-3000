//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------
using UnityEngine;
using System.Collections;
using Falcon;

/*
    This example demonstrates how to interact with the groups API.
    By holding the leftShift+(any number from 1-9), the current selection is stored in that number.
    By holding the leftAlt+(any number from 1-9), the group assigned to that number is cleared.
    By pressing any number from 1-9 the group with that number is set as currently selected.
    Note that all delegates are triggered as if each object would have been selected individually, that way you have granular control over the selection.
*/
public class KeyboardGroupExample : MonoBehaviour {

    private Selector selector = null;

    void Awake()
    {
        selector = GameObject.FindObjectOfType<Selector>();
        if (selector == null)
        {
            Debug.LogError("Could not find a Selector component in the scene, add one and try again.");
            this.enabled = false;
        }
    }

    void Update()
    {
        GroupLogic();
    }

    void GroupLogic()
    {
        //The key combinations can also be configured through Unity's input editor but I preferred to not override your input with this example :)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selector.SaveCurrentSelectionToGroup("1");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selector.SaveCurrentSelectionToGroup("2");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                selector.SaveCurrentSelectionToGroup("3");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                selector.SaveCurrentSelectionToGroup("4");
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                selector.SaveCurrentSelectionToGroup("5");
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                selector.SaveCurrentSelectionToGroup("6");
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                selector.SaveCurrentSelectionToGroup("7");
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                selector.SaveCurrentSelectionToGroup("8");
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                selector.SaveCurrentSelectionToGroup("9");
            }
        }
        else if(Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selector.groupsManager.ClearGroup("1");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selector.groupsManager.ClearGroup("2");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                selector.groupsManager.ClearGroup("3");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                selector.groupsManager.ClearGroup("4");
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                selector.groupsManager.ClearGroup("5");
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                selector.groupsManager.ClearGroup("6");
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                selector.groupsManager.ClearGroup("7");
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                selector.groupsManager.ClearGroup("8");
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                selector.groupsManager.ClearGroup("9");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selector.SetSelectedObjectsFromGroup("1");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selector.SetSelectedObjectsFromGroup("2");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                selector.SetSelectedObjectsFromGroup("3");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                selector.SetSelectedObjectsFromGroup("4");
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                selector.SetSelectedObjectsFromGroup("5");
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                selector.SetSelectedObjectsFromGroup("6");
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                selector.SetSelectedObjectsFromGroup("7");
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                selector.SetSelectedObjectsFromGroup("8");
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                selector.SetSelectedObjectsFromGroup("9");
            }
        }
    }
}
