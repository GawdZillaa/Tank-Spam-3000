//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Falcon
{
    /*
        Helper window to find out information about the stored groups.     
    */
    public class GroupsEditorWindow : EditorWindow
    {
        private SelectableGroupsManager groupsManager = null;
        private Dictionary<string, List<Selectable>>.KeyCollection keys = null;
        private List<bool> keysFoldouts = new List<bool>();
        private List<Vector2> scrollsViewsPosition = new List<Vector2>();
        private int keysFoldoutsCounter = 0;

        [MenuItem("Window/Falcon/Groups")]
        static void Init()
        {
            GroupsEditorWindow window = (GroupsEditorWindow)EditorWindow.GetWindow(typeof(GroupsEditorWindow), false, "Groups");
            window.Show();
        }

        void OnGUI()
        {
            InitGroupsManager();
            DrawGroupsUI();
        }

        void InitGroupsManager()
        {
            if (groupsManager == null)
            {
                groupsManager = GameObject.FindObjectOfType<SelectableGroupsManager>();
            }
        }

        void DrawGroupsUI()
        {
            keysFoldoutsCounter = 0;

            if (groupsManager.getMap().Keys.Count > 0 && groupsManager.getMap().Keys.Count != keysFoldouts.Count)
            {
                //The list has changed, need to reload it.
                keys = groupsManager.getMap().Keys;
                keysFoldouts = new List<bool>();
                scrollsViewsPosition = new List<Vector2>();
                for (int a = 0; a < groupsManager.getMap().Keys.Count; a++) {
                    keysFoldouts.Add(false);
                    scrollsViewsPosition.Add(Vector2.zero);
                }
            }
            if (keys != null)
            {
                EditorGUILayout.PrefixLabel("Groups: ");
                foreach (string groupName in keys)
                {
                    if (groupsManager.GetGroup(groupName).Count > 0)
                    {
                        //Only draw the list if it has data.
                        EditorGUILayout.BeginVertical();
                        keysFoldouts[keysFoldoutsCounter] = EditorGUILayout.Foldout(keysFoldouts[keysFoldoutsCounter], groupName);
                        if (keysFoldouts[keysFoldoutsCounter])
                        {
                            scrollsViewsPosition[keysFoldoutsCounter] = EditorGUILayout.BeginScrollView(scrollsViewsPosition[keysFoldoutsCounter]);
                            foreach (Selectable selectable in groupsManager.GetGroup(groupName))
                            {
                                EditorGUILayout.LabelField(selectable.gameObject.name);
                            }
                            GUILayout.EndScrollView();
                        }
                        EditorGUILayout.EndVertical();
                        keysFoldoutsCounter++;
                    }
                }
            }
        }
	
	}
}
