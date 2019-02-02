using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSpawnManager : MonoBehaviour
{
    public GameObject Bad_TankT1;
    public GameObject Bad_TankT2;
    public GameObject Bad_TankT3;

    public GameObject AI;

    List<int> TankPackage = new List<int>();

    public GameObject Q;

    public int t1cost;
    public int t2cost;
    public int t3cost;


    public Transform tankSpawnPoint_bad;
    private List<GameObject> badTanks = new List<GameObject>();
    // Use this for initialization
    public int spawnWaitT1;
    public int spawnWaitT2;
    public int spawnWaitT3;


    float spwantimeK;
    float spwantimeK2;
    float spwantimeK3;

    public GameObject ResourceManager;

    bool Produce;
    bool inProduce;


    bool doSpawn;

    public void spawnCheck(bool changer)
    {
        doSpawn = changer;
    }

    // Use this for initialization
    void Start()
    {
        Produce = false;
        inProduce = false;
    }

    // Update is called once per frame
    void Update()
    {

        spwantimeK += Time.deltaTime;
        spwantimeK2 += Time.deltaTime;
        spwantimeK3 += Time.deltaTime;

        if (Produce == true && inProduce == false)
        {
 //           Debug.Log("Calling Production...");
            inProduce = true;
            CallQ();
        }
    }



   public void AiSpawnCall(int t1, int t2, int t3, int PackageTotal)
    {
        ResourceManager.GetComponent<AiReources>().AiUnitRequest(PackageTotal);
//        Debug.Log("PACKAGE RECIEVED");


        int t1Temp = t1;
        int t2Temp = t2;
        int t3Temp = t3;

        for (int i = 0; i < t1; i++)
        {
            TankPackage.Add(1);
        }
        for (int i = 0; i < t2; i++)
        {
            TankPackage.Add(2);
        }
        for (int i = 0; i < t3; i++)
        {
            TankPackage.Add(3);
        }

        Produce = true;
    }

    void CallQ()
    {
//        Debug.Log("In Production...");

        foreach (int listed in TankPackage)
        {

            if (listed == 1)
            {

                Q.GetComponent<UnitQ_Enemy>().RecieveUnitRequest(Bad_TankT1);
//                Debug.Log("Tank Spawned");
            }

            if (listed == 2)
            {
                Q.GetComponent<UnitQ_Enemy>().RecieveUnitRequest(Bad_TankT2);

                Debug.Log("Tank Spawned");
            }

            if (listed == 3)
            {

                Q.GetComponent<UnitQ_Enemy>().RecieveUnitRequest(Bad_TankT3);
                Debug.Log("Tank Spawned");
            }


        }


        ResetList();
        Produce = false;
        inProduce = false;

    }

    void ResetList()
    {
        TankPackage.Clear();
    }



}
