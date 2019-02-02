using System.Collections.Generic;
using UnityEngine;

public class ResourceExtractor : MonoBehaviour {

    public int[] TierCost = new int[6] { 0, 50, 100, 150, 200, 250 };
    public int[] TierStack = new int[7] { 0, 2, 4, 7, 8, 9, 10 };
    public Sprite[] UpgradeExtractSprites = new Sprite[7];
    int Tier;
    bool FirstInUpgrade;
    float UpgradeReset = 0;
    public int TimeToUpgrade;
    public bool isUpgrade;


    public GameObject resources;
    public GameObject BarHealth;

    public int ExtractStack;

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

    public GameObject ResourceLinkManager;

    void Start()
    {
        Tier = 1;
        anim = GetComponent<Animator>();
        resources = GameObject.FindGameObjectWithTag("ResourceText");
        health = maxHealth;

        FirstInUpgrade = true;
        SetUp = false;
        build = true;
        anim.Play("ResBuild");

        ResourceLinkManager = GameObject.FindGameObjectWithTag("PlayerHome");
    }

    void Update () {

        elapsed += Time.deltaTime;
        timer2 += Time.deltaTime;

        if (buildTimer < timer2 && build == true)
        {
            build = false;
            anim.Play("ResIdle");
        }

        if (addResources < elapsed && build == false && SetUp == false)
        {
            SetUp = true;
            resources.GetComponent<ResourceManagerG>().UpExtractor(ExtractStack * 5);
//            Debug.Log("CALLLINGGGGGGGGGGGGGGGGG");
            Debug.Log(resources);
            ResourceLinkManager.GetComponent<ExtractorLink>().StartupLinkScan();

            elapsed = 0;
            

        }



        if (isUpgrade == true)
        {
            UpgradeReset += Time.deltaTime;

            if (FirstInUpgrade == true)
            {
//                Debug.Log("--UPGRADING!!");
                anim.Play("Upgrade");
                FirstInUpgrade = false;
            }

            if (TimeToUpgrade < UpgradeReset)
            {
//                Debug.Log("^^^^^^^^DONE UPGRADONG^^^^^^^");
                isUpgrade = false;
                Debug.Log("--NewTier-> " + Tier);
                setAnim(Tier);
                LinkNewExtractor(Tier);
                GetComponent<SpriteRenderer>().sprite = UpgradeExtractSprites[Tier];
//                Debug.Log("CALLLINGGGGGGGGGGGGGGGGG");

                ResourceLinkManager.GetComponent<ExtractorLink>().StartupLinkScan();

                UpgradeReset = 0;
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
                resources.GetComponent<ResourceManagerG>().DownExtractor(ExtractStack);
                setHealthBar(calc_health);
                Destroy(gameObject);
                stopHealth = true;

            }

        }
        else
        {
            setHealthBar(calc_health);
        }


    }



    public void setHealthBar(float health)
    {
        BarHealth.transform.localScale = new Vector3(health, BarHealth.transform.localScale.y,
        BarHealth.transform.localScale.z);
    }

    public void UpgradeExtractor()
    {
        //        Debug.Log("--Current Tier: " + Tier);

        if (build == false && isUpgrade == false)
        {
            int tempFunds = 0;
            tempFunds = resources.GetComponent<ResourceManagerG>().getResourceG();

            if (TierCost[Tier] < tempFunds)
            {
                resources.GetComponent<ResourceManagerG>().deductResources(TierCost[Tier]);
                FirstInUpgrade = true;
                isUpgrade = true;
                Tier++;
                //            Debug.Log("--Upgrading To Tier: " + Tier);

            }
        }

    }

    void setAnim(int Tier)
    {

        switch (Tier)
        {
            case 2:
                {
                    Debug.Log("--Anim 2");
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


    public void LinkNewExtractor(int tier)
    {
        int baseLine = resources.GetComponent<ResourceManagerG>().BaseLine;
        Debug.Log("--Deducting...");

        switch (tier)
        {
            case 2:
                {
                    Debug.Log(TierStack[Tier - 1] * baseLine);
                    resources.GetComponent<ResourceManagerG>().DownExtractor(Tier - 1);

                    Debug.Log("--Adding...");
                    Debug.Log(TierStack[Tier] * baseLine);
                    resources.GetComponent<ResourceManagerG>().UpExtractor(TierStack[Tier] * baseLine);
                    break;
                }
            case 3:
                {
                    Debug.Log(TierStack[Tier - 1] * baseLine);
                    resources.GetComponent<ResourceManagerG>().DownExtractor(Tier - 1);

                    Debug.Log("Adding...");
                    Debug.Log(TierStack[Tier] * baseLine);
                    resources.GetComponent<ResourceManagerG>().UpExtractor(TierStack[Tier] * baseLine);
                    break;

                }
            case 4:
                {
                    Debug.Log(TierStack[Tier - 1] * baseLine);
                    resources.GetComponent<ResourceManagerG>().DownExtractor(Tier - 1);

                    Debug.Log("Adding...");
                    Debug.Log(TierStack[Tier] * baseLine);
                    resources.GetComponent<ResourceManagerG>().UpExtractor(TierStack[Tier] * baseLine);
                    break;

                }
            case 5:
                {
                    Debug.Log(TierStack[Tier - 1] * baseLine);
                    resources.GetComponent<ResourceManagerG>().DownExtractor(Tier - 1);

                    Debug.Log("Adding...");
                    Debug.Log(TierStack[Tier] * baseLine);
                    resources.GetComponent<ResourceManagerG>().UpExtractor(TierStack[Tier] * baseLine);
                    break;

                }
            case 6:
                {
                    Debug.Log(TierStack[Tier - 1] * baseLine);
                    resources.GetComponent<ResourceManagerG>().DownExtractor(Tier - 1);

                    Debug.Log("Adding...");
                    Debug.Log(TierStack[Tier] * baseLine);
                    resources.GetComponent<ResourceManagerG>().UpExtractor(TierStack[Tier] * baseLine);
                    break;

                }
        }
    }


    public int getTier()
    {
        return Tier;
    }
}
