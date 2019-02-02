//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Falcon
{
    /*
        System class that encapsulates all the logic for storing and removing groups of selections. 
        This class does not handle the input event to trigger the storing of groups.
    */
    public class SelectableGroupsManager : MonoBehaviour
    {
        private Dictionary<string, List<Selectable>> _selectableGroupsMap = new Dictionary<string, List<Selectable>>();

        //Events
        public delegate void DelegateOnGroupChange(string name, List<Selectable> group);
        public DelegateOnGroupChange onGroupChange;
        public delegate void DelegateOnSetGroup(string name, List<Selectable> group);
        public DelegateOnSetGroup onSetGroup;

        protected virtual void OnEnable()
        {
            onGroupChange += StubOnGroupChange;
            onSetGroup += StubOnSetGroup;
        }

        protected virtual void OnDisable()
        {
            onGroupChange -= StubOnGroupChange;
            onSetGroup -= StubOnGroupChange;
        }

        private void InitGroup(string name)
        {
            if (!_selectableGroupsMap.ContainsKey(name))
            {
                _selectableGroupsMap[name] = new List<Selectable>();
            }
            else
            {
                //Has the key but the list is null.
                if(_selectableGroupsMap[name] == null)
                {
                    _selectableGroupsMap[name] = new List<Selectable>();
                }
            }
        }

        public void SetGroup(string name, List<Selectable> group)
        {
            if (group != null)
            {
                InitGroup(name);
                _selectableGroupsMap[name].Clear();
                _selectableGroupsMap[name].AddRange(group);
                onGroupChange(name, _selectableGroupsMap[name]);
                onSetGroup(name, _selectableGroupsMap[name]);
            }
        }

        public void ClearGroup(string name)
        {
            if (_selectableGroupsMap.ContainsKey(name) && _selectableGroupsMap[name] != null)
            {
                _selectableGroupsMap[name].Clear();
                onGroupChange(name, _selectableGroupsMap[name]);

            }
        }

        public List<Selectable> GetGroup(string name)
        {
            List<Selectable> selectable = null;
            if (_selectableGroupsMap.ContainsKey(name))
            {
                selectable = _selectableGroupsMap[name];
            }
            else
            {
                selectable = null;
            }
            return selectable;
        }

        //Support for adding 1 or many to the group.
        public void AddToGroup(string name, Selectable selectable)
        {
            InitGroup(name);
            _selectableGroupsMap[name].Add(selectable);
            onGroupChange(name, _selectableGroupsMap[name]);
        }

        public void AddToGroup(string name, List<Selectable> group)
        {
            InitGroup(name);
            _selectableGroupsMap[name].AddRange(group);
            onGroupChange(name, _selectableGroupsMap[name]);
        }

        //Support for removing one or many from the group.
        public void RemoveFromGroup(string name, Selectable selectable)
        {
            if (_selectableGroupsMap.ContainsKey(name) && _selectableGroupsMap[name] != null)
            {
                _selectableGroupsMap[name].Remove(selectable);
                onGroupChange(name, _selectableGroupsMap[name]);
            }
        }

        public void RemoveFromGroup(string name, List<Selectable> group)
        {
            if (_selectableGroupsMap.ContainsKey(name) && _selectableGroupsMap[name] != null)
            {
                foreach(Selectable selectable in group)
                {
                    _selectableGroupsMap[name].Remove(selectable);
                }
                onGroupChange(name, _selectableGroupsMap[name]);
            }
        }

        public Dictionary<string, List<Selectable>> getMap()
        {
            return _selectableGroupsMap;
        }

        //Stubs
        public void StubOnGroupChange(string name, List<Selectable> group) { }
        public void StubOnSetGroup(string name, List<Selectable> group) { }
    }
}
