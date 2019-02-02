using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UnitQ_Enemy : MonoBehaviour
{

    private List<GameObject> goodTanks = new List<GameObject>();
    GameObject[] inQ = new GameObject[5];

    public Image[] ShowUnit = new Image[5];
    public GameObject BuildBar;

    public GameObject WarningSystem;

    public GameObject AI;
    public GameObject Spawn;

    int t1cost;
    int t2cost;
    int t3cost;

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

    int LIMIT_Q = 20;
    int CURR_Q;

    public Transform SpawnPoint;

    public GameObject AiResources;

    int EfficiencyTier = 0;

    // Use this for initialization
    void Start()
    {
        t1cost = Spawn.GetComponent<AiSpawnManager>().t1cost;
        t2cost = Spawn.GetComponent<AiSpawnManager>().t2cost;
        t3cost = Spawn.GetComponent<SpawnEnemy>().t3cost;

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


            if (temp.tag == "BadTank")
            {
                TimerStart = true;
                barT1 = true;

                if (T1BuildTime < Timer)
                {
                    Vector3 yCustom = new Vector3(SpawnPoint.position.x, SpawnPoint.position.y + .05f, SpawnPoint.position.z);
                    GameObject aTo_go = (GameObject)Instantiate(temp, yCustom, Quaternion.identity);

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

            else if (temp.tag == "BadTankT2")
            {
                barT2 = true;
                TimerStart = true;

                FirstQueTemp = T2spr;

                if (T2BuildTime < Timer)
                {
                    Vector3 yCustom = new Vector3(SpawnPoint.position.x, SpawnPoint.position.y + .05f, SpawnPoint.position.z);
                    GameObject aTo_go = (GameObject)Instantiate(temp, yCustom, Quaternion.identity);

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

            else if (temp.tag == "BadTankT3")
            {
                barT3 = true;
                TimerStart = true;
                if (T3BuildTime < Timer)
                {
                    Vector3 yCustom = new Vector3(SpawnPoint.position.x, SpawnPoint.position.y + .1f, SpawnPoint.position.z);
                    GameObject aTo_go = (GameObject)Instantiate(temp, yCustom, Quaternion.identity);

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
            Spawn_To_Ai_BeginBuild();


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
         Spawn_To_Ai_HaltBuild();

        if (CURR_Q == LIMIT_Q)
        {
            Debug.Log("UNIT Q FULL!");
            WarningSystem.GetComponent<WarningMessageManager>().RecieveWarning("Q Is Full");


            if (tank.tag == "BadTank")
            {
                Debug.Log("Refund T1: " + t1cost);
                AI.GetComponent<AiReources>().killResource(t1cost);

            }
            else if (tank.tag == "BadTankT2")
            {
                Debug.Log("Refund T2: " + t2cost);
                AI.GetComponent<AiReources>().killResource(t2cost);

            }
            else if (tank.tag == "BadTankT3")
            {
                Debug.Log("Refund T3: " + t3cost);
                AI.GetComponent<AiReources>().killResource(t3cost);

            }
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



            if (FirstQueTemp != null && firstRound == true)
            {
 //               Debug.Log("First...");
                ShowUnit[i].sprite = FirstQueTemp;
                firstRound = false;
                i++;
            }
            if (i == ShowUnit.Length)
            {
//                Debug.Log("Break");
                break;
            }

            else if (listed.tag == "BadTank")
            {
//                Debug.Log("Applied...");

                ShowUnit[i].sprite = T1spr;
            }
            else if (listed.tag == "BadTankT2")
            {
                ShowUnit[i].sprite = T2spr;
            }
            else if (listed.tag == "BadTankT3")
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
//        Debug.Log("Refresh Q");
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

 //           Debug.Log(i + " vs " + ShowUnit.Length);
            if (i == ShowUnit.Length)
            {
//                Debug.Log("Break");
                break;
            }

           else if (listed.tag == "BadTank")
            {
                ShowUnit[i].sprite = T1spr;
            }
            else if (listed.tag == "BadTankT2")
            {
                ShowUnit[i].sprite = T2spr;
            }
           else if (listed.tag == "BadTankT3")
            {
                ShowUnit[i].sprite = T3spr;
            }

            i++;
        }
    }



    void ResetVisual()
    {
 //       Debug.Log("Refresh Q");
        foreach (Image provided in ShowUnit)
        {
            provided.sprite = null;
        }

    }
    void Spawn_To_Ai_HaltBuild()
    {
        AI.GetComponent<AI>().HoldBuild();
    }

    void Spawn_To_Ai_BeginBuild()
    {
        AI.GetComponent<AI>().InitiateBuild();

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

    public void RecallEfficiency()
    {
        Debug.Log("-Resetting Efficiency-");
        EfficiencyTier = 0;

        T1BuildTime = 5;
        T2BuildTime = 10;
        T3BuildTime = 15;
    }

}
