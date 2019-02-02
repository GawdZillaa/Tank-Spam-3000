using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenu : MonoBehaviour {

    private List<GameObject> GoodBuildings = new List<GameObject>();
    private List<GameObject> t1List = new List<GameObject>();
    private List<GameObject> t2List = new List<GameObject>();
    private List<GameObject> t3List = new List<GameObject>();
    private List<GameObject> t4List = new List<GameObject>();
    private List<GameObject> tShList = new List<GameObject>();


    public GameObject WarningSystem;
    public GameObject DetectionSystem;
    bool DetectionState;

    public GameObject BuildingPanel;
    public GameObject BuildingPanel2;
    public GameObject InitialButton;
    public GameObject InitialButton2;

    Renderer rend;
    Renderer rend2;
    Renderer rend3;


    public GameObject dummy;
    public GameObject dummy2;

    public GameObject Detector;

    public int TurrentCost;
    public GameObject T1Turrent;
    public int ExtractorCost;
    public GameObject T1Resource;

    public GameObject Yplace_Hold;

    Vector3 origionalPosTurr;

    int BuildingType;
    bool openMenu;
    bool openMenuUnit;
    bool placeBuilding;

    public GameObject MouseManage;
    bool PlaceEnter = true;

    bool Manual;

    public GameObject AutoPanel;
    public GameObject ManualPanel;

    public GameObject BuildingSlotBad;
    public GameObject BuildingSlotGood;

    public float GatherLeftBound;
    public float gatherRightBound;


    public GameObject ResourceManager;


    void DisableScroll()
    {
        MouseManage.GetComponent<cameraMove>().stopScroll(true);

    }

    void EnableScroll()
    {
        MouseManage.GetComponent<cameraMove>().stopScroll(false);

    }

    public void MenuCheckOpen(bool option)
    {
        openMenu = option;
    }

    public void MenuCheckClose(bool option)
    {
        openMenu = option;
    }

    public void MenuCheckOpenUnit(bool option)
    {
        openMenuUnit = option;
    }

    public void MenuCheckCloseUnit(bool option)
    {
        openMenuUnit = option;
 //       Debug.Log(openMenuUnit);
    }

    public void SpawnTrial(int type)
    {
        BuildingType = type;
    }


	// Use this for initialization
	void Start () {
        Detector.SetActive(false);
        BuildingPanel.SetActive(false);
        ManualPanel.SetActive(false);
        BuildingPanel2.SetActive(false);

        Vector3 lol = dummy.transform.position;
        origionalPosTurr = Camera.main.ScreenToWorldPoint(lol);
        Manual = true;
        

    }

    // Update is called once per frame
    void Update () {

        if (openMenu == true || openMenuUnit == true)
        {
            if (openMenu == true)
            {
                BuildingPanel.SetActive(true);
                InitialButton.SetActive(false);
            }

            if (openMenuUnit == true)
            {
                BuildingPanel2.SetActive(true);
                InitialButton2.SetActive(false);
            }

        }


        if (openMenu == false || openMenuUnit == false)
        {
            if (openMenu == false)
            {
//                Debug.Log("closing 1....");

                BuildingPanel.SetActive(false);
                InitialButton.SetActive(true);
            }

            if (openMenuUnit == false)
            {
//                Debug.Log("closing 2....");
                BuildingPanel2.SetActive(false);
                InitialButton2.SetActive(true);
            }


        }

        if (BuildingType == 1)
        {
            if (PlaceEnter == true)
            {
                DisableScroll();
                PlaceEnter = false;

                rend = dummy.GetComponent<Renderer>();
                rend.enabled = true;

                rend2 = dummy2.GetComponent<Renderer>();
                rend2.enabled = true;

                rend3 = Detector.GetComponent<Renderer>();
                rend3.enabled = true;
            }

            Detector.SetActive(true);

            Vector3 mousPos = Input.mousePosition;
            Vector3 wantedPos = Camera.main.ScreenToWorldPoint(mousPos);
            Vector3 wantedPos2 = Camera.main.ScreenToWorldPoint(mousPos);
            Vector3 wantedPos3 = Camera.main.ScreenToWorldPoint(mousPos);



            Vector3 testD = dummy.transform.position;
            Vector3 testA = dummy2.transform.position;
   //         Debug.Log("Right: " + testD.x);

  //          Debug.Log("Left: " + testA.x);

            wantedPos.y = Yplace_Hold.transform.position.y;
            wantedPos.z = 0.0f;

            wantedPos2.y = Yplace_Hold.transform.position.y;
            wantedPos2.z = 0.0f;
            wantedPos2.x = wantedPos.x - .8f;

            wantedPos3.y = Yplace_Hold.transform.position.y - 1;
            wantedPos3.z = 0.0f;
            wantedPos3.x = wantedPos.x - .101f;

            dummy.transform.position = wantedPos;
            dummy2.transform.position = wantedPos2;
            Detector.transform.position = wantedPos3;

            mouseListe();

            if (BuildingType == 1)
            {
                BuildingSlotBad.GetComponent<Renderer>().enabled = true;
                BuildingSlotGood.GetComponent<Renderer>().enabled = true;
            }



            if (placeBuilding == true)
            {
                Debug.Log("Right: " + testD.x + " > " + gatherRightBound);

                Debug.Log("Left: " + testA.x + " < " + GatherLeftBound);
                int TempFunds = ResourceManager.GetComponent<ResourceManagerG>().getResourceG();

                if (TempFunds < TurrentCost)
                {
                    Debug.Log("Not Enough Funds");
                    WarningSystem.GetComponent<WarningMessageManager>().RecieveWarning("Insufficient Funds");
                    placeBuilding = false;
                }

                else if (testA.x > GatherLeftBound && testD.x < gatherRightBound)
                {
                    ResourceManager.GetComponent<ResourceManagerG>().deductResources(TurrentCost);
                    onMouseOver(wantedPos, T1Turrent);
                }

                else
                {
                    Debug.Log("PLACEMENT ERROR!!!!");
                    WarningSystem.GetComponent<WarningMessageManager>().RecieveWarning("Outta Bounds");
                    placeBuilding = false;
                }
            }


        }

        if (BuildingType == 2)
        {
            if (PlaceEnter == true)
            {
                DisableScroll();
                PlaceEnter = false;

                rend = dummy.GetComponent<Renderer>();
                rend.enabled = true;

                rend2 = dummy2.GetComponent<Renderer>();
                rend2.enabled = true;

                rend3 = Detector.GetComponent<Renderer>();
                rend3.enabled = true;
            }

            Detector.SetActive(true);

            Vector3 mousPos = Input.mousePosition;
            Vector3 wantedPos = Camera.main.ScreenToWorldPoint(mousPos);
            Vector3 wantedPos2 = Camera.main.ScreenToWorldPoint(mousPos);
            Vector3 wantedPos3 = Camera.main.ScreenToWorldPoint(mousPos);

            Vector3 wantedPos4 = Camera.main.ScreenToWorldPoint(mousPos); ;



            Vector3 testD = dummy.transform.position;
            Vector3 testA = dummy2.transform.position;
            //         Debug.Log("Right: " + testD.x);

            //          Debug.Log("Left: " + testA.x);

            wantedPos.y = Yplace_Hold.transform.position.y-.5f;
            wantedPos.z = 0.0f;

            wantedPos4.y = Yplace_Hold.transform.position.y;
            wantedPos4.z = 0.0f;

            wantedPos2.y = Yplace_Hold.transform.position.y;
            wantedPos2.z = 0.0f;
            wantedPos2.x = wantedPos.x - .8f;

            wantedPos3.y = Yplace_Hold.transform.position.y - 1;
            wantedPos3.z = 0.0f;
            wantedPos3.x = wantedPos.x - .101f;

            dummy.transform.position = wantedPos4;
            dummy2.transform.position = wantedPos2;
            Detector.transform.position = wantedPos3;

            mouseListe();

            if (BuildingType == 2)
            {
                BuildingSlotBad.GetComponent<Renderer>().enabled = true;
                BuildingSlotGood.GetComponent<Renderer>().enabled = true;
            }



            if (placeBuilding == true)
            {
//                Debug.Log("Right: " + testD.x + " > " + gatherRightBound);

 //               Debug.Log("Left: " + testA.x + " < " + GatherLeftBound);


                int TempFunds = ResourceManager.GetComponent<ResourceManagerG>().getResourceG();


                if (TempFunds < ExtractorCost)
                {
                    Debug.Log("Not Enough Funds");
                    WarningSystem.GetComponent<WarningMessageManager>().RecieveWarning("Insufficient Funds");
                    placeBuilding = false;
                }

                else if (testA.x > GatherLeftBound && testD.x < gatherRightBound)
                {
                    ResourceManager.GetComponent<ResourceManagerG>().deductResources(ExtractorCost);
                    onMouseOver(wantedPos, T1Resource);
                }
                else
                {

                    Debug.Log("PLACEMENT ERROR!!!!");
                    WarningSystem.GetComponent<WarningMessageManager>().RecieveWarning("Outta Bounds");
                    placeBuilding = false;
                }
            }


        }
    }



    void mouseListe( )
    {
        if (Input.GetMouseButtonDown(1))
        {
            Detector.SetActive(false);

            placeBuilding = false;
            CancelBuild();
        }

        if (Input.GetMouseButtonDown(0))
        {
           DetectionState = DetectionSystem.GetComponent<DetectBuilding>().DetectionState();

            if (DetectionState == true)
            {
                Detector.SetActive(false);

                placeBuilding = true;
            }

            else
            {
                WarningSystem.GetComponent<WarningMessageManager>().RecieveWarning("Overlaying Structure!");
            }


        }

    }

    void onMouseOver(Vector3 mousePosi, GameObject Building)
    {


        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("PLACED+_+__+_+");

            GameObject enterBuild = (GameObject)Instantiate(Building, mousePosi, Quaternion.identity);
            dummy.transform.position = origionalPosTurr;
            EnableScroll();
            PlaceEnter = true;
            BuildingType = 0;
            placeBuilding = false;
            rend.enabled = false;
            rend2.enabled = false;
            BuildingSlotBad.GetComponent<Renderer>().enabled = false;
            BuildingSlotGood.GetComponent<Renderer>().enabled = false;




        }
    }

    void CancelBuild()
    {

            dummy.transform.position = origionalPosTurr;
            EnableScroll();
            PlaceEnter = true;
            BuildingType = 0;
            placeBuilding = false;
            rend.enabled = false;
            rend2.enabled = false;
            BuildingSlotBad.GetComponent<Renderer>().enabled = false;
            BuildingSlotGood.GetComponent<Renderer>().enabled = false;
        BuildingType = 0;

        
    }



    public bool MovmentType()
    {
//        Debug.Log("IN BUILDING: " + Manual);
        return Manual;
    }

    public void AutoMove()
    {
        Manual = false;

        AutoPanel.SetActive(false);
        ManualPanel.SetActive(true);

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject t1 in allObjects)
        {
            if (t1.tag == "Tank")
            {
                t1List.Add(t1);
            }
            if (t1.tag == "Tank2")
            {
                t2List.Add(t1);
            }
            if (t1.tag == "Tank3")
            {
                t3List.Add(t1);
            }

            if (t1.tag == "Tank4")
            {
                t4List.Add(t1);
            }

            if (t1.tag == "TankShG")
            {
                tShList.Add(t1);
            }
        }

        foreach (GameObject t1 in t1List)
        {
            Debug.Log("IN AUTO~~~: " + t1);

            if (t1 != null)
            {
                t1.GetComponent<Movment>().AutoMove();
            }

        }

        foreach (GameObject t1 in t2List)
        {
            Debug.Log("IN AUTO~~~: " + t1);

            if (t1 != null)
            {
                t1.GetComponent<GoodTankR2Manager>().AutoMove();
            }

        }

        foreach (GameObject t1 in t3List)
        {
            Debug.Log("IN AUTO~~~: " + t1);

            if (t1 != null)
            {
                t1.GetComponent<T3GoodManager>().AutoMove();
            }
        

        }

        foreach (GameObject t1 in t4List)
        {
            Debug.Log("IN AUTO~~~: " + t1);

            if (t1 != null)
            {
                t1.GetComponent<SniperTankManager>().AutoMove();
            }


        }

        foreach (GameObject t1 in tShList)
        {
            Debug.Log("IN AUTO~~~: " + t1);

            if (t1 != null)
            {
                t1.GetComponent<ShieldTankG>().AutoMove();
            }


        }

    }



    public void ManualMove()
    {
        Manual = true;

        AutoPanel.SetActive(true);
        ManualPanel.SetActive(false);

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject t1 in allObjects)
        {
            if (t1.tag == "Tank")
            {
                t1List.Add(t1);
            }
            if (t1.tag == "Tank2")
            {
                t2List.Add(t1);
            }
            if (t1.tag == "Tank3")
            {
                t3List.Add(t1);
            }
            if (t1.tag == "Tank4")
            {
                t4List.Add(t1);
            }
            if (t1.tag == "TankShG")
            {
                tShList.Add(t1);
            }
        }

        foreach (GameObject t1 in t1List)
        {
            Debug.Log("IN AUTO~~~: " + t1);

            if (t1 != null)
            {
                t1.GetComponent<Movment>().ManualMove();
            }

        }

        foreach (GameObject t1 in t2List)
        {
            Debug.Log("IN AUTO~~~: " + t1);

            if (t1 != null)
            {
                t1.GetComponent<GoodTankR2Manager>().ManualMove();
            }

        }

        foreach (GameObject t1 in t3List)
        {
            Debug.Log("IN AUTO~~~: " + t1);

            if (t1 != null)
            {
                t1.GetComponent<T3GoodManager>().ManualMove();
            }

        }

        foreach (GameObject t1 in t4List)
        {
            Debug.Log("IN AUTO~~~: " + t1);

            if (t1 != null)
            {
                t1.GetComponent<SniperTankManager>().ManualMove();
            }


        }

        foreach (GameObject t1 in tShList)
        {
            Debug.Log("IN AUTO~~~: " + t1);

            if (t1 != null)
            {
                t1.GetComponent<ShieldTankG>().ManualMove();
            }


        }
    }



}


