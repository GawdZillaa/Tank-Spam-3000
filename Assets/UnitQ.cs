using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UnitQ : MonoBehaviour
{

    private List<GameObject> goodTanks = new List<GameObject>();
    GameObject[] inQ = new GameObject[5];

    public Image[] ShowUnit = new Image[5];
    public GameObject BuildBar;

    public GameObject WarningSystem;

    public int T1BuildTime;
    public int T2BuildTime;
    public int T3BuildTime;

    public Sprite T1spr;
    public Sprite T2spr;
    public Sprite T3spr;

    Sprite FirstQueTemp;

    float Timer = 0;

    bool ActiveQ;
    bool TimerStart;
    bool hasDeQ;

    bool barT1;
    bool barT2;
    bool barT3;

    bool inBar;

    bool Refreshed;

    Queue<GameObject> Q = new Queue<GameObject>();
    GameObject temp;

    int LIMIT_Q = 10;
    int CURR_Q;

    public Transform SpawnPoint;

    int EfficiencyTier;

    // Use this for initialization
    void Start()
    {
        ActiveQ = false;
        TimerStart = false;
        hasDeQ = false;
        barT1 = false;
        barT2 = false;
        barT3 = false;
        inBar = false;

        CURR_Q = 0;
        temp = null;

        Refreshed = false;

        EfficiencyTier = 0;
    }

    void FixedUpdate()
    {
        if (TimerStart == true)
        {
            Timer += Time.deltaTime;

        }


        if (Q.Count == 0 && TimerStart == false)
        {
            ActiveQ = false;
        }

        if (ActiveQ == true)
        {
            if (hasDeQ == false)
            {
                temp = Q.Dequeue();
                CURR_Q--;
                hasDeQ = true;
            }

            if (barT1 == true)  //Set Up T1 Spawn Time
            {
                FirstQueTemp = T1spr;

                float Build_Points = Timer / T1BuildTime;
                BuildBarUp(Build_Points);

                if (inBar == false)
                {
                    RefreshQVisual();

                    inBar = true;
                }
            }

            if (barT2 == true) //Set Up T2 Spawn Time
            {
                FirstQueTemp = T2spr;
                float Build_Points = Timer / T2BuildTime;
                BuildBarUp(Build_Points);

                if (inBar == false)
                {
                    RefreshQVisual();

                    inBar = true;
                }
            }

            if (barT3 == true) //Set Up T3 Spawn Time
            {
                FirstQueTemp = T3spr;
                float Build_Points = Timer / T3BuildTime;
                BuildBarUp(Build_Points);

                if (inBar == false)
                {
                    RefreshQVisual();

                    inBar = true;
                }
            }


            if (temp.tag == "Tank")
            {
                TimerStart = true;
                barT1 = true;

                if (T1BuildTime < Timer)
                {
                    GameObject aTo_go = (GameObject)Instantiate(temp, SpawnPoint.position, Quaternion.identity);

                    TimerStart = false;
                    hasDeQ = false;
                    barT1 = false;
                    inBar = false;
                    Refreshed = false;

                    FirstQueTemp = null;
                    temp = null;

                    Timer = 0;
                    CURR_Q--;
                }
            }

            else if (temp.tag == "Tank2")
            {
                barT2 = true;
                TimerStart = true;

                FirstQueTemp = T2spr;

                if (T2BuildTime < Timer)
                {
                    GameObject aTo_go = (GameObject)Instantiate(temp, SpawnPoint.position, Quaternion.identity);

                    TimerStart = false;
                    inBar = false;
                    hasDeQ = false;
                    barT2 = false;
                    Refreshed = false;

                    temp = null;
                    FirstQueTemp = null;

                    Timer = 0;
                    CURR_Q--;


                }
            }

            else if (temp.tag == "Tank3")
            {
                barT3 = true;
                TimerStart = true;
                if (T3BuildTime < Timer)
                {
                    GameObject aTo_go = (GameObject)Instantiate(temp, SpawnPoint.position, Quaternion.identity);

                    TimerStart = false;
                    inBar = false;
                    hasDeQ = false;
                    barT3 = false;
                    Refreshed = false;

                    temp = null;
                    FirstQueTemp = null;

                    Timer = 0;
                    CURR_Q--;

                }
            }


        }


        if (Q.Count == 0 && ActiveQ == false && Refreshed == false)
        {
            ResetVisual();
            Refreshed = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RecieveUnitRequest(GameObject tank)
    {
        if (CURR_Q == LIMIT_Q)
        {
            Debug.Log("UNIT Q FULL!");
            WarningSystem.GetComponent<WarningMessageManager>().RecieveWarning("Q Is Full");
            return;
        }

        else
        {
            CURR_Q++;
            Q.Enqueue(tank);
//            Debug.Log("QUEUED~~~~~~~~~~~~~~~~~~~~");
            ActiveQ = true;
        }

        int i = 0;
        bool firstRound = true;

        foreach (Image provided in ShowUnit)
        {
            provided.sprite = null;

        }

        foreach (GameObject listed in Q)
        {


                inQ[i] = listed;

            if (FirstQueTemp != null && firstRound == true)
            {
//                Debug.Log("First...");
                ShowUnit[i].sprite = FirstQueTemp;
                firstRound = false;
                i++;
            }

            if (listed.tag == "Tank")
            {
 //               Debug.Log("Applied...");

                ShowUnit[i].sprite = T1spr;
            }
            if (listed.tag == "Tank2")
            {
                ShowUnit[i].sprite = T2spr;
            }
            if (listed.tag == "Tank3")
            {
                ShowUnit[i].sprite = T3spr;
            }

            i++;
        }
    }


    public void BuildBarUp(float progress)
    {
        BuildBar.transform.localScale = new Vector3(progress, BuildBar.transform.localScale.y,
        BuildBar.transform.localScale.x);
    }


    void RefreshQVisual()
    {
 //       Debug.Log("Refresh Q");
        foreach (Image provided in ShowUnit)
        {
                provided.sprite = null;
        }

        if (Q.Count == 0)
        {
            if (FirstQueTemp != null)
            {
 //               Debug.Log(FirstQueTemp);
                ShowUnit[0].sprite = FirstQueTemp;
            }
        }

        int i = 0;
        bool firstRound = true;

        foreach (GameObject listed in Q)
        {

            if (FirstQueTemp != null && firstRound == true)
            {
//                Debug.Log(FirstQueTemp);
                ShowUnit[i].sprite = FirstQueTemp;
                firstRound = false;
                i++;
            }

             if (listed.tag == "Tank")
            {
                ShowUnit[i].sprite = T1spr;
            }
             if (listed.tag == "Tank2")
            {
                ShowUnit[i].sprite = T2spr;
            }
             if (listed.tag == "Tank3")
            {
                ShowUnit[i].sprite = T3spr;
            }

            i++;
        }
    }



    void ResetVisual()
    {
        Debug.Log("Refresh Q");
        foreach (Image provided in ShowUnit)
        {
            provided.sprite = null;
        }

    }



    public void ExtractorEfficiency(int PowerTier)
    {
        if (EfficiencyTier == PowerTier)
        {
            return;
        }
        else { EfficiencyTier = PowerTier; }

        Debug.Log("New Efficiency: " + EfficiencyTier);

        switch (PowerTier)
        {
            case 1:
                {
                    T1BuildTime = 5;
                    T2BuildTime = 9;
                    T3BuildTime = 13;
                    break;
                }
            case 2:
                {
                    T1BuildTime = 4;
                    T2BuildTime = 8;
                    T3BuildTime = 11;
                    break;
                }
            case 3:
                {
                    T1BuildTime = 4;
                    T2BuildTime = 7;
                    T3BuildTime = 10;
                    break;
                }
            case 4:
                {
                    T1BuildTime = 3;
                    T2BuildTime = 6;
                    T3BuildTime = 9;
                    break;
                }
            case 5:
                {
                    T1BuildTime = 2;
                    T2BuildTime = 5;
                    T3BuildTime = 8;
                    break;
                }
            case 6:
                {
                    T1BuildTime = 1;
                    T2BuildTime = 3;
                    T3BuildTime = 5;
                    break;
                }
        }
    }


}
