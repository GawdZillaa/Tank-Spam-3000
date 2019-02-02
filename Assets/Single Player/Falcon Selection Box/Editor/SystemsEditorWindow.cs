//----------------------------------------------
//            Falcon Selection Box
// Copyright © 2016 Javier Falcon
//----------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

namespace Falcon
{
    public class SystemsEditorWindow : EditorWindow
    {
        private int selectedMode = 0;
        private string[] selectionGridStrings = { "3D", "2D" };
        private Camera camera = null;
        private GameObject draggable = null;

        [MenuItem("Window/Falcon/Systems")]
        static void Init()
        {
            SystemsEditorWindow window = (SystemsEditorWindow)EditorWindow.GetWindow(typeof(SystemsEditorWindow), false, "Falcon Systems");
            window.Show();
        }

        void OnGUI()
        {
            if (camera == null)
            {
                camera = GameObject.FindObjectOfType<Camera>();
            }
            draggable = Selection.activeGameObject;
            if (GUILayout.Button("Init Systems"))
            {
                if (GetUnityEventSystemInScene() == null)
                {
                    InstantiateSystemsWithEventSystem();
                }
                else
                {
                    InstantiateSystemsWithoutEventSystem();
                }
            }
            DrawGroupsUI();

            selectedMode = GUILayout.SelectionGrid(selectedMode, selectionGridStrings, 2);
            if (selectedMode == 0)
            {
                Draw3DGUI();
            }
            else
            {
                Draw2DGUI();
            }
        }

        void Draw3DGUI()
        {
            GUILayout.BeginHorizontal();
            camera = EditorGUILayout.ObjectField(camera, typeof(Camera), true) as Camera;
            if (GUILayout.Button("Init 3D Camera"))
            {
                PhysicsRaycaster raycaster = GameObject.FindObjectOfType<PhysicsRaycaster>() as PhysicsRaycaster;
                if (raycaster == null)
                {
                    raycaster = Undo.AddComponent<PhysicsRaycaster>(camera.gameObject);
                }
                AddGLSelectorTo(camera.gameObject);
            }
            GUILayout.EndHorizontal();

            if (Selection.activeGameObject)
            {
                EditorGUILayout.LabelField("Selected: " + Selection.activeGameObject.name);
                if (draggable != null)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Set 3D Selection Area"))
                    {
                        AddSelectionAreaTo(draggable.gameObject);
                        Add3DColliderTo(draggable.gameObject);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.LabelField("Select a game object to enable the rest of the menu");
            }

            if (Selection.activeGameObject != null)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Set 3D Selectable"))
                {
                    AddSelectableTo(Selection.activeGameObject);
                    //Event System needs a collider to work.
                    Add3DColliderTo(Selection.activeGameObject);
                }
                GUILayout.EndHorizontal();
            }

            if (Selection.activeGameObject != null)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Set 3D Selectable Example"))
                {
                    AddExampleSelectableTo(Selection.activeGameObject);
                    //Event System needs a collider to work.
                    Add3DColliderTo(Selection.activeGameObject);
                }
                GUILayout.EndHorizontal();
            }

            if (Selection.activeGameObject != null)
            {
                if (GUILayout.Button("Set 3D Click Listener"))
                {
                    AddClickHandlerTo(Selection.activeGameObject);
                    //Event System needs a collider to work.
                    Add3DColliderTo(Selection.activeGameObject);
                }
            }
        }

        void Draw2DGUI()
        {
            GUILayout.BeginHorizontal();
            camera = EditorGUILayout.ObjectField(camera, typeof(Camera), true) as Camera;
            if (GUILayout.Button("Init 2D Camera"))
            {
                Physics2DRaycaster raycaster = GameObject.FindObjectOfType<Physics2DRaycaster>() as Physics2DRaycaster;
                if (raycaster == null)
                {
                    raycaster = Undo.AddComponent<Physics2DRaycaster>(camera.gameObject);
                }
                AddGLSelectorTo(camera.gameObject);
            }
            GUILayout.EndHorizontal();

            if (Selection.activeGameObject)
            {
                EditorGUILayout.LabelField("Selected: " + Selection.activeGameObject.name);
                if (draggable != null)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Set 2D Selection Area"))
                    {
                        AddSelectionAreaTo(draggable.gameObject);
                        Add2DColliderTo(draggable.gameObject);
                    }
                    GUILayout.EndHorizontal();
                }
            }

            if (Selection.activeGameObject != null)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Set 2D Selectable"))
                {
                    AddSelectableTo(Selection.activeGameObject);
                    //Event System needs a collider to work.
                    Add2DColliderTo(Selection.activeGameObject);
                }
                GUILayout.EndHorizontal();
            }

            if (Selection.activeGameObject != null)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Set 2D Selectable Example"))
                {
                    AddExampleSelectableTo(Selection.activeGameObject);
                    //Event System needs a collider to work.
                    Add2DColliderTo(Selection.activeGameObject);
                }
                GUILayout.EndHorizontal();
            }

            if (Selection.activeGameObject != null)
            {
                if (GUILayout.Button("Set 2D Click Listener"))
                {
                    AddClickHandlerTo(Selection.activeGameObject);
                    //Event System needs a collider to work.
                    Add2DColliderTo(Selection.activeGameObject);
                }
            }
        }

        void DrawGroupsUI()
        {
            if(GUILayout.Button("Init Groups"))
            {
                InitGroups();
            }
        }

        void AddGLSelectorTo(GameObject localGameObject)
        {
            CheckIfSystemsAreInitialized();
            Selector selector = GameObject.FindObjectOfType<Selector>() as Selector;
            if (selector == null)
            {
                GLBoxSelector glBoxSelector = null;
                Material mat = AssetDatabase.LoadAssetAtPath("Assets/Falcon Selection Box/Art/selector material.mat", typeof(Material)) as Material;
                glBoxSelector = Undo.AddComponent<GLBoxSelector>(localGameObject);
                glBoxSelector.rendererMaterial = mat;
                Debug.Log("Added a GLBoxSelector to the GameObject - " + localGameObject.name);
            }
        }

        void AddSelectionAreaTo(GameObject localGameObject)
        {
            CheckIfSystemsAreInitialized();
            DragHandler dragHandler = localGameObject.GetComponent<DragHandler>();
            if (dragHandler == null)
            {
                dragHandler = Undo.AddComponent<DragHandler>(localGameObject);
                Debug.Log("Added a DragHandler to the GameObject - " + localGameObject.name);
            }
        }

        void AddSelectableTo(GameObject localGameObject)
        {
            CheckIfSystemsAreInitialized();
            DefaultSelectable selectable = localGameObject.GetComponent<DefaultSelectable>();
            if (selectable == null)
            {
                selectable = Undo.AddComponent<DefaultSelectable>(localGameObject);
                Debug.Log("Added a 'Default Selectable' to the GameObject - " + localGameObject.name);
            }
        }

        void AddExampleSelectableTo(GameObject localGameObject)
        {
            CheckIfSystemsAreInitialized();
            ExampleSelectable selectable = localGameObject.GetComponent<ExampleSelectable>();
            if (selectable == null)
            {
                selectable = Undo.AddComponent<ExampleSelectable>(localGameObject);
                Debug.Log("Added a 'Example Selectable' to the GameObject - " + localGameObject.name);
            }
        }

        void AddClickHandlerTo(GameObject localGameObject)
        {
            CheckIfSystemsAreInitialized();
            ClickHandler clickListener = localGameObject.GetComponent<ClickHandler>();
            if (clickListener == null)
            {
                clickListener = Undo.AddComponent<ClickHandler>(localGameObject);
                Debug.Log("Added a 'Click Handler' to the GameObject - " + localGameObject.name);
            }
        }

        void Add3DColliderTo(GameObject localGameObject)
        {
            Collider col = localGameObject.GetComponent<Collider>();
            if (col == null)
            {
                Undo.AddComponent<MeshCollider>(localGameObject);
                Debug.Log("Added a MeshCollider to the GameObject - " + localGameObject.name);
            }
        }

        void Add2DColliderTo(GameObject localGameObject)
        {
            Collider2D col2D = localGameObject.GetComponent<Collider2D>();
            if (col2D == null)
            {
                Undo.AddComponent<BoxCollider2D>(localGameObject);
                Debug.Log("Added a BoxCollider2D to the GameObject - " + localGameObject.name);
            }
        }

        void InstantiateSystemsWithoutEventSystem()
        {
            GameObject systemsGameObject = new GameObject("Falcon Systems");
            Undo.RegisterCreatedObjectUndo(systemsGameObject, "Created Falcon Systems");
            systemsGameObject.AddComponent<DontDestroyOnLoad>();

            AInputManager inputManager = InitInputManager();
            inputManager.transform.parent = systemsGameObject.transform;
        }

        void InstantiateSystemsWithEventSystem()
        {
            GameObject systemsGameObject = new GameObject("Falcon Systems");
            Undo.RegisterCreatedObjectUndo(systemsGameObject, "Created a Falcon Systems Object");
            systemsGameObject.AddComponent<DontDestroyOnLoad>();

            AInputManager inputManager = InitInputManager();
            inputManager.transform.parent = systemsGameObject.transform;

            EventSystem eventSystem = InitEventSystem();
            eventSystem.transform.parent = inputManager.transform;
        }

        AInputManager InitInputManager()
        {
            AInputManager inputManager = GetInputManagerInScene();
            if (inputManager == null)
            {
                GameObject inputObject = new GameObject("Input Manager");
                inputManager = inputObject.AddComponent<DefaultInputManager>();
                Debug.Log("Added [" + inputObject.name + " - 'Default Input Manager'] to the scene.");
            }
            return inputManager;
        }

        EventSystem InitEventSystem()
        {
            EventSystem eventSystem = GetUnityEventSystemInScene();
            if (eventSystem == null)
            {
                GameObject eventSystemObject = new GameObject("EventSystem");
                Undo.RegisterCreatedObjectUndo(eventSystemObject, "Created Event System");
                eventSystem = eventSystemObject.AddComponent<EventSystem>();
                Debug.Log("Added [" + eventSystemObject.name + " - EventSystem] to the scene.");
                if (eventSystemObject.GetComponent<StandaloneInputModule>() == null)
                {
                    eventSystemObject.AddComponent<StandaloneInputModule>();
                    Debug.Log("Added [" + eventSystemObject.name + " - StandaloneInputModule] to the scene.");
                }
#if UNITY_5_1 || UNITY_5_0
                if (eventSystemObject.GetComponent<TouchInputModule>() == null)
                {
                    eventSystemObject.AddComponent<TouchInputModule>();
                    Debug.Log("Added [" + eventSystemObject.name + " - TouchInputModule] to the scene.");
                }
                
#endif
            }
            return eventSystem;
        }

        void InitGroups()
        {
            CheckIfSystemsAreInitialized();
            Selector selectorInScene = GetSelector();
            GameObject objectToAddComponent = null;

            if(selectorInScene != null)
            {
                //Try to add it to the selector.
                objectToAddComponent = selectorInScene.gameObject;
            }
            if(objectToAddComponent != null)
            {
                Undo.AddComponent<SelectableGroupsManager>(objectToAddComponent);
                Debug.Log("Added SelectableGroupsManager to " + objectToAddComponent.gameObject);
            }
        }

        bool CheckIfSystemsAreInitialized()
        {
            bool success = true;
            if (GetInputManagerInScene() == null && GetUnityEventSystemInScene() == null)
            {
                success = false;
                InstantiateSystemsWithEventSystem();
            }
            else if (GetInputManagerInScene() == null && GetUnityEventSystemInScene() != null)
            {
                success = false;
                InstantiateSystemsWithoutEventSystem();
            }
            if (GetInputManagerInScene() == null)
            {
                success = false;
                InitInputManager();
            }
            return success;
        }

        bool ShouldInitSystems()
        {
            return GetInputManagerInScene() == null || GetUnityEventSystemInScene() == null;
        }

        bool ShouldInit3DCamera()
        {
            return GetRaycaster() == null || GetSelector() == null;
        }

        bool ShouldInit2DCamera()
        {
            return Get2DRaycaster() == null || GetSelector() == null;
        }

        bool CanAddSelectionAreaTo(GameObject go)
        {
            return go.GetComponent<DragHandler>() == null && go.GetComponent<Camera>() == null;
        }

        bool CanAddSelectableTo(GameObject go)
        {
            return go.GetComponent<Selectable>() == null && go.GetComponent<Camera>() == null;
        }

        bool CanAddClickListenerTo(GameObject go)
        {
            return go.GetComponent<ClickHandler>() == null && go.GetComponent<Camera>() == null;
        }

        AInputManager GetInputManagerInScene()
        {
            return GameObject.FindObjectOfType<AInputManager>();
        }

        EventSystem GetUnityEventSystemInScene()
        {
            return GameObject.FindObjectOfType<EventSystem>();
        }

        PhysicsRaycaster GetRaycaster()
        {
            return GameObject.FindObjectOfType<PhysicsRaycaster>();
        }

        Physics2DRaycaster Get2DRaycaster()
        {
            return GameObject.FindObjectOfType<Physics2DRaycaster>();
        }

        Selector GetSelector()
        {
            return GameObject.FindObjectOfType<Selector>();
        }
    }
}