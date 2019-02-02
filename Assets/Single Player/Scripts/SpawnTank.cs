using UnityEngine;
using System.Collections.Generic;

public class SpawnTank : MonoBehaviour
{
    public GameObject TierOneTank;
    public GameObject TierTwoTank;
    public GameObject TierThreeTank;
    public GameObject TierResource;
    public GameObject TierSniper;
    public GameObject TierShield;
    public GameObject TierMechT1;


    public GameObject Q;

    public Transform tankSpawnPoint_good;
    private List<GameObject> goodTanks = new List<GameObject>();

    public int t1Amount;
    public int t2Amount;
    public int t3Amount;
    public int tResAmount;
    public int tSniperAmount;
    public int tShieldAmount;
    public int MechT1Amount;
   

    int spawnWait = 2;
    float spwantimeK;
    public GameObject resources;
    bool doSpawn;
    int TierTank;

    bool temp = false;

    public GameObject Q_Send;

    public void MULTIrecieveCommand(int tier)
    {
        doSpawn = true;
        TierTank = tier;
    }
    public void spawnCheck(bool changer)
    {
        doSpawn = changer;
        //   TierTank = TierType;
    }

    public void tierCheck(int TierType)
    {
        TierTank = TierType;
    }

    // Use this for initialization
    void Start()
    {
        resources = GameObject.FindGameObjectWithTag("ResourceText");
    }

    // Update is called once per frame
    void Update()
    {

    //    spwantimeK += Time.deltaTime;


        if (doSpawn == true)
        {
            if (TierTank == 1)
            {
                temp = resources.GetComponent<ResourceManagerG>().checkRequest(t1Amount);
                if (temp == true)
                {

                    Q_Send.GetComponent<UnitQ>().RecieveUnitRequest(TierOneTank);
//                    Debug.Log("T1 Tank Spawned");
                    doSpawn = false;
                }

                else
                    doSpawn = false;

            }

            if (TierTank == 2)
            {
                temp = resources.GetComponent<ResourceManagerG>().checkRequest(t2Amount);

                if (temp == true)
                {

                    Q.GetComponent<UnitQ>().RecieveUnitRequest(TierTwoTank);
//                    Debug.Log("T2 Tank Spawned");
                    doSpawn = false;
                }



            }

            if (TierTank == 3)
            {

                temp = resources.GetComponent<ResourceManagerG>().checkRequest(t3Amount);

                if (temp == true)
                {
                    Q.GetComponent<UnitQ>().RecieveUnitRequest(TierThreeTank);
//                    Debug.Log("T3 Tank Spawned");
                    doSpawn = false;
                }

                else
                    doSpawn = false;


            }

            if (TierTank == 4)
            {

                temp = resources.GetComponent<ResourceManagerG>().checkRequest(tSniperAmount);

                if (temp == true)
                {
                    GameObject aTo_go = (GameObject)Instantiate(TierSniper, new Vector3(tankSpawnPoint_good.position.x, -1.84f, tankSpawnPoint_good.position.z), Quaternion.identity);

                    goodTanks.Add(aTo_go);
//                    Debug.Log("T3 Tank Spawned");
                    doSpawn = false;
                }

                else
                    doSpawn = false;


            }

            if (TierTank == 5)
            {

                temp = resources.GetComponent<ResourceManagerG>().checkRequest(tShieldAmount);

                if (temp == true)
                {
                    GameObject aTo_go = (GameObject)Instantiate(TierShield, new Vector3(tankSpawnPoint_good.position.x, -1.84f, tankSpawnPoint_good.position.z), Quaternion.identity);

                    goodTanks.Add(aTo_go);
                    Debug.Log("T3 Tank Spawned");
                    doSpawn = false;
                }

                else
                    doSpawn = false;


            }


            if (TierTank == 6)
            {

                temp = resources.GetComponent<ResourceManagerG>().checkRequest(MechT1Amount);

                if (temp == true)
                {
                    GameObject aTo_go = (GameObject)Instantiate(TierMechT1, new Vector3(tankSpawnPoint_good.position.x, -1.70f, tankSpawnPoint_good.position.z), Quaternion.identity);

                    goodTanks.Add(aTo_go);
                    Debug.Log("Resource Tank Spawned");
                    doSpawn = false;
                }

                else
                    doSpawn = false;


            }


            if (TierTank == 10)
            {

                temp = resources.GetComponent<ResourceManagerG>().checkRequest(tResAmount);

                if (temp == true)
                {
                    GameObject aTo_go = (GameObject)Instantiate(TierResource, new Vector3(tankSpawnPoint_good.position.x, -1.85f, tankSpawnPoint_good.position.z), Quaternion.identity);

                    goodTanks.Add(aTo_go);
                    Debug.Log("Resource Tank Spawned");
                    doSpawn = false;
                }

                else
                    doSpawn = false;


            }



        }


    }
}