using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiResourceExtractor : MonoBehaviour {

    public GameObject resources;
    public GameObject BarHealth;

    public int ExtractStack;

    public int[] TierCost = new int[6] {0, 50, 100, 150, 200, 250};
    public int[] TierStack = new int[7] { 0, 2, 4, 7, 8, 9, 10 };

    public Sprite[] UpgradeExtractSprites = new Sprite[7];
    public Animator anim;

    public float addResources;
    private float elapsed;

    bool stopHealth = false;
    bool build;

    bool SetUp;

    public float maxHealth;
    private float health;

    public float buildTimer;
    private float timer2;

    public bool killSwitch;

    GameObject Ai;

    int SlotId;

    float DeathCounter = 0;
    public int TimeTillReset;

    int Tier;
    public bool isUpgrade;
    public int TimeToUpgrade;
    float UpgradeReset = 0;
    bool FirstInUpgrade;

    void Start()
    {
        Tier = 1;
        FirstInUpgrade = true;
        isUpgrade = false;

        killSwitch = false;
        Ai = GameObject.FindGameObjectWithTag("AI");

        anim = GetComponent<Animator>();
        resources = GameObject.FindGameObjectWithTag("AI");
        health = maxHealth;

        SetUp = false;
        build = true;
    
        anim.Play("ResBuildBad");


    }

    void Update()
    {

        elapsed += Time.deltaTime;
        timer2 += Time.deltaTime;

        if (buildTimer < timer2 && build == true)
        {
            build = false;
            anim.Play("ResIdleBad");
        }

        if (addResources < elapsed && build == false && SetUp == false)
        {
            SetUp = true;
            int baseLine = resources.GetComponent<AiReources>().BaseLine;

            int Totals = TierStack[1] * baseLine;
//            Debug.Log("BaseLine: " + baseLine);
//            Debug.Log("Stack: " + TierStack[1]);

//            Debug.Log("Extractor: " + Totals);
            resources.GetComponent<AiReources>().UpExtractor(Totals);
            EconResume();

            elapsed = 0;


        }

        if (isUpgrade == true)
        {
            UpgradeReset += Time.deltaTime;

            if (FirstInUpgrade == true)
            {
//                Debug.Log("UPGRADING!!");
//                anim.Play("ResUpgrade");
                FirstInUpgrade = false;
            }

            if (TimeToUpgrade < UpgradeReset)
            {
//                Debug.Log("^^^^^^^^DONE UPGRADONG^^^^^^^");
                isUpgrade = false;
//                Debug.Log("NewTier-> " + Tier);
                setAnim(Tier);
                LinkNewExtractor(Tier);
                EconResume();
                GetComponent<SpriteRenderer>().sprite = UpgradeExtractSprites[Tier];

                UpgradeReset = 0;
            }
        }


        if (killSwitch == true)
        {
            DeathCounter += Time.deltaTime;
            Debug.Log(DeathCounter + " " + TimeTillReset);
            if (TimeTillReset < DeathCounter)
            {
                Destroy(gameObject);
                UnRegisterSlotId();

            }
        }


    }

    public void takeDamage(int amount, GameObject lol)
    {
        health -= amount;
        float calc_health = health / maxHealth;



        if (health <= 0 || health == 0)
        {
            if (stopHealth == false)
            {
                if (build != true)
                {
                    int TempTier = this.GetComponent<AiResourceExtractor>().Tier;
                    resources.GetComponent<AiReources>().DownExtractor(TempTier);
                }
                setHealthBar(calc_health);
                Destroy(gameObject);
                stopHealth = true;
                this.GetComponent<Renderer>().enabled = false;

                killSwitch = true;
                if (build == true)
                {
                    EconResume();
                }

            }

        }
        else
        {
            setHealthBar(calc_health);
        }



       
    }


    public bool UpgradeExtractor()
    {
//        Debug.Log("Recieved Ext Upgrade Request...");
        int RemaningEnergy = 0;
        int TempTier = this.GetComponent<AiResourceExtractor>().Tier;
        RemaningEnergy = resources.GetComponent<AiReources>().EnergyReserves;

//        Debug.Log("Energy: " + RemaningEnergy + " vs " + TierCost[Tier]);
        if (RemaningEnergy > TierCost[Tier])
        {
            Tier++;
            isUpgrade = true;
            FirstInUpgrade = true;
//            resources.GetComponent<AiReources>().DownExtractor(TempTier);
            return true;

        }

        else { return false; }

    }

    public void LinkNewExtractor(int tier)
    {
        int baseLine = resources.GetComponent<AiReources>().BaseLine;
        Debug.Log("Deducting...");

        switch (tier)
        {
            case 2:
                {
                    Debug.Log(TierStack[Tier - 1] * baseLine);
                    resources.GetComponent<AiReources>().DownExtractor(Tier-1);

                    Debug.Log("Adding...");
                    Debug.Log(TierStack[Tier] * baseLine);
                    resources.GetComponent<AiReources>().UpExtractor(TierStack[Tier] * baseLine);
                    break;
                }
            case 3:
                {
                    Debug.Log(TierStack[Tier - 1] * baseLine);
                    resources.GetComponent<AiReources>().DownExtractor(Tier - 1);

                    Debug.Log("Adding...");
                    Debug.Log(TierStack[Tier] * baseLine);
                    resources.GetComponent<AiReources>().UpExtractor(TierStack[Tier] * baseLine);
                    break;

                }
            case 4:
                {
                    Debug.Log(TierStack[Tier - 1] * baseLine);
                    resources.GetComponent<AiReources>().DownExtractor(Tier - 1);

                    Debug.Log("Adding...");
                    Debug.Log(TierStack[Tier] * baseLine);
                    resources.GetComponent<AiReources>().UpExtractor(TierStack[Tier] * baseLine);
                    break;

                }
            case 5:
                {
                    Debug.Log(TierStack[Tier - 1] * baseLine);
                    resources.GetComponent<AiReources>().DownExtractor(Tier - 1);

                    Debug.Log("Adding...");
                    Debug.Log(TierStack[Tier] * baseLine);
                    resources.GetComponent<AiReources>().UpExtractor(TierStack[Tier] * baseLine); break;

                }
            case 6:
                {
                    Debug.Log(TierStack[Tier - 1] * baseLine);
                    resources.GetComponent<AiReources>().DownExtractor(Tier - 1);

                    Debug.Log("Adding...");
                    Debug.Log(TierStack[Tier] * baseLine);
                    resources.GetComponent<AiReources>().UpExtractor(TierStack[Tier] * baseLine); break;

                }
        }
    }

    void setAnim(int Tier)
    {

        switch (Tier)
        {
            case 2:
                {
                    Debug.Log("Anim 2");
                    anim.Play("T2");
                    break;
                }
            case 3:
                {
                    Debug.Log("Anim 3");
                    anim.Play("T3");
                    break;

                }
            case 4:
                {
                    Debug.Log("Anim 4");
                    anim.Play("T4");
                    break;

                }
            case 5:
                {
                    Debug.Log("Anim 5");
                    anim.Play("T5");
                    break;

                }
            case 6:
                {
                    Debug.Log("Anim 6");
                    anim.Play("T6");
                    break;

                }
        }
    }

    public int getTier()
    {
        return Tier;
    }


    public void setHealthBar(float health)
    {
        BarHealth.transform.localScale = new Vector3(health, BarHealth.transform.localScale.y,
        BarHealth.transform.localScale.z);
    }


    public void EconResume()
    {
        resources.GetComponent<AI>().EconComms();
    }

    public void AssignSlotId(int id)
    {
        SlotId = id;
        Debug.Log("Extractor Reg Slot: " + SlotId);
    }

    public void UnRegisterSlotId()
    {
        Debug.Log("Un Reg Request Sent: " + SlotId);
        Ai.GetComponent<AI>().UnRegisterBuilding(1, SlotId);
    }

}
