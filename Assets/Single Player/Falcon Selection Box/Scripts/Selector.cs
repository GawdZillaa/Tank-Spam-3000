//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Falcon
{
    /*
        System class that handles all the selectable objects. This class also handles drawing a representation of the enclosed bounds of the selection. A box-type implementation is provided: GLSelector.
        All selectables should register and unregister themselves to this class by using the RegisterSelectable and UnregisterSelectable methods.
        It also has delegates to notify when any object got selected or unselected.

    */
    public abstract class Selector : MonoBehaviour
    {
        protected List<Selectable> _selectedObjects = new List<Selectable>();
        protected List<Selectable> allSelectables = new List<Selectable>();
        public SelectableGroupsManager groupsManager = null;
        protected bool needInit = true;

        //Events
        public delegate void DelegateOnObjectSelected(Selectable selectedObject);
        public DelegateOnObjectSelected onObjectSelected;
        public delegate void DelegateOnObjectUnselected(Selectable selectedObject);
        public DelegateOnObjectUnselected onObjectUnselected;
        public delegate void DelegateOnFinishSelecting(Selectable[] selectedObjects);
        public DelegateOnFinishSelecting onFinishSelecting;
        public delegate void DelegateOnFinishUnselectAll();
        public DelegateOnFinishUnselectAll onFinishUnselectAll;

        //Abstract
        //Methods to process input dragging events.
        public abstract void SelectorMoveStart(Vector3 startWorldCoordinates, Vector3 currentWorldCoordinates, Vector2 startScreenCoordinates, Vector2 currentScreenCoordinates);
        public abstract void SelectorMove(Vector3 startWorldCoordinates, Vector3 currentWorldCoordinates, Vector2 startScreenCoordinates, Vector2 currentScreenCoordinates);
        public abstract void SelectorMoveEnd(Vector3 startWorldCoordinates, Vector3 currentWorldCoordinates, Vector2 startScreenCoordinates, Vector2 currentScreenCoordinates);
        //Method to try and select objects within the enclosed points.
        public abstract Selectable[] SelectObjects(Vector3 startPoint, Vector3 endPoint);

        //MonoBehaviour methods
        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
            Init();
        }

        protected virtual void OnEnable()
        {
            Init();
        }

        protected virtual void OnDisable()
        {
            //Unregister events
            onObjectSelected -= StubOnObjectSelected;
            onObjectUnselected -= StubOnObjectUnselected;
            onFinishSelecting -= StubOnFinishSelecting;
            onFinishUnselectAll -= StubOnFinishUnselectAll;

            //sometimes the input manager instance is not there and Unity throws a null error.
            if (AInputManager.instance != null)
            {
                AInputManager.instance.onPointerBeginMove -= SelectorMoveStart;
                AInputManager.instance.onPointerMoved -= SelectorMove;
                AInputManager.instance.onPointerEndMove -= SelectorMoveEnd;
            }
            if(groupsManager != null)
            {
                //Listen when a group has been stored.
                groupsManager.onSetGroup -= OnSetGroup;
            }
            needInit = true;
        }
            
        //Methods
        //Convenient method to initialize this object.
        protected virtual void Init()
        {
            //Make sure we initialize once, but make sure it's always initialized.
            if (needInit)
            {
                onObjectSelected += StubOnObjectSelected;
                onObjectUnselected += StubOnObjectUnselected;
                onFinishSelecting += StubOnFinishSelecting;
                onFinishUnselectAll += StubOnFinishUnselectAll;

                if (AInputManager.instance != null)
                {
                    if (AInputManager.instance != null)
                    {
                        AInputManager.instance.onPointerBeginMove += SelectorMoveStart;
                        AInputManager.instance.onPointerMoved += SelectorMove;
                        AInputManager.instance.onPointerEndMove += SelectorMoveEnd;
                    }
                }
                InitGroups();
                needInit = false;
            }
        }

        protected void InitGroups()
        {
            if (groupsManager == null)
            {
                groupsManager = GameObject.FindObjectOfType<SelectableGroupsManager>();
            }
            //Double check there is an instance for groups manager.
            if(groupsManager != null)
            {
                groupsManager.onSetGroup += OnSetGroup;
            }
        }

        public bool AreThereAnySelectedObjects()
        {
            Debug.Log("Selector -> AreThereAnySelectedObjects");

            return _selectedObjects.Count > 0;
        }

        //Method called when a object has been selected.
        public void AddToCurrentlySelected(Selectable selectable)
        {
//            Debug.Log("Selector -> AddToCurrentlySelected");
 //           Debug.Log(selectable);
 //           if (selectable.gameObject.GetComponent<Movment>().inBattle != true)
 //           {

                _selectedObjects.Add(selectable);
 //           }
        }

        //Method called when a objecty has been unselected.
        public void RemoveFromCurrentlySelected(Selectable selectable)
        {
            Debug.Log("Selector -> RemoveFromCurrentlySelected");

            _selectedObjects.Remove(selectable);
        }

        //Method for selectables to register themselves to this _selector.
        public void RegisterSelectable(Selectable selectable)
        {
 //           Debug.Log("Selector -> RegisterSelectable");
 //           Debug.Log(selectable.gameObject.GetComponent<Movment>().inBattle + "CHECK~~~~~~~~~~~~~~~~~~~++++++~~~~~~~");

     //       if (selectable.gameObject.GetComponent<Movment>().inBattle != true)
    //        {


                selectable.onSelect += RelaySelectEvent;
                selectable.onUnselect += RelayUnselectEvent;
                allSelectables.Add(selectable);

      //      }




        }

        //Method for selectables to unregsiter themselves to this _selector.
        public void UnregisterSelectable(Selectable selectable)
        {
 //           Debug.Log("Selector -> UnregisterSelectable");

            selectable.onSelect -= RelaySelectEvent;
            selectable.onUnselect -= RelayUnselectEvent;
            allSelectables.Remove(selectable);
        }

        public void NotifyOnFinishSelecting()
        {
            onFinishSelecting(selectedObjects);
        }

        /// <summary>
        /// Notify to listeners that a object has been selected.
        /// Calls onObjectSelected delegate.
        /// </summary>
        /// <param name="selectable"></param>
        private void RelaySelectEvent(Selectable selectable)
        {
            onObjectSelected(selectable);
        }

        /// <summary>
        /// Notify to listeners that a object has been unselected.
        /// Calls onObjectUnselected delegate.
        /// </summary>
        /// <param name="selectable"></param>
        private void RelayUnselectEvent(Selectable selectable)
        {
            onObjectUnselected(selectable);
        }

        /// <summary>
        /// Sets the state of all selected objects to unselected and clears the selected objects list.
        /// </summary>
        public void UnselectAll()
        {
 //           Debug.Log("Selector -> UnselectAll");

            if (_selectedObjects.Count > 0)
            {
                foreach (Selectable selectable in _selectedObjects)
                {
                    selectable.Unselect();
                }
                _selectedObjects.Clear();
            }
            onFinishUnselectAll();
        }

        //Groups methods
        //Listen when a group has been stored in SelectableGroupsManager.
        protected virtual void OnSetGroup(string groupName, List<Selectable> group)
        {
        }

        //Convenient method to store whatever is currently selected.
        //Overrides the group it is being set to.
        public void SaveCurrentSelectionToGroup(string groupName)
        {
            groupsManager.SetGroup(groupName, _selectedObjects);
        }

        //Convenient method to store whatever is currently selected.
        //Does not override the group, instead it just appends to it.
        public void AppendCurrentSelectionToGroup(string groupName)
        {
            groupsManager.AddToGroup(groupName, _selectedObjects);
        }

        //Convenient method to set the currently selected objects to a group.
        public void SetSelectedObjectsFromGroup(string groupName)
        {
            List<Selectable> group = groupsManager.GetGroup(groupName);
            if (group != null)
            {
                UnselectAll();
                foreach(Selectable localSelectable in group)
                {
                    _selectedObjects.Add(localSelectable);
                    localSelectable.Select();
                }
            }
        }

        //Stubs
        public void StubOnObjectSelected(Selectable localObject) { }
        public void StubOnObjectUnselected(Selectable localObject) { }
        public void StubOnFinishSelecting(Selectable[] selectedObjects) { }
        public void StubOnFinishUnselectAll() { }

        //Properties
        public Selectable[] selectedObjects
        {
            get
            {
                return _selectedObjects.ToArray();
            }
        }
        public List<Selectable> selectedObjectsList
        {
            get
            {
                return _selectedObjects;
            }
        }
    }
}