using UnityEngine;
using System.Collections.Generic;

public class SpawnEnemy : MonoBehaviour {
    public GameObject Bad_TankT1;
    public GameObject Bad_TankT2;
    public GameObject Bad_TankT3;

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




    bool doSpawn;

    public void spawnCheck(bool changer)
    {
        doSpawn = changer;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        spwantimeK += Time.deltaTime;
        spwantimeK2 += Time.deltaTime;
        spwantimeK3 += Time.deltaTime;


        //         Debug.Log(spwantimeK + " " + "vs" + " " + spawnWait);
        if (spwantimeK >= spawnWaitT1)
        {
            
                GameObject aTo_goE = (GameObject)Instantiate(Bad_TankT1, new Vector3 (tankSpawnPoint_bad.position.x, -1.88f, tankSpawnPoint_bad.position.z), Quaternion.identity);
                //  Physics.IgnoreCollision(aTo_go.GetComponent<Collider>(), GetComponent<Collider>());

                badTanks.Add(aTo_goE);
                Debug.Log("Tank Spawned");
                doSpawn = false;
                spwantimeK = 0;
         }

        if (spwantimeK2 >= spawnWaitT2)
        {

            GameObject aTo_goE = (GameObject)Instantiate(Bad_TankT2, new Vector3(tankSpawnPoint_bad.position.x, -1.85f, tankSpawnPoint_bad.position.z), Quaternion.identity);
            //  Physics.IgnoreCollision(aTo_go.GetComponent<Collider>(), GetComponent<Collider>());

            badTanks.Add(aTo_goE);
            Debug.Log("Tank Spawned");
            doSpawn = false;
            spwantimeK2 = 0;
        }

        if (spwantimeK3 >= spawnWaitT3)
        {

            GameObject aTo_goE = (GameObject)Instantiate(Bad_TankT3, new Vector3(tankSpawnPoint_bad.position.x, -1.8f, tankSpawnPoint_bad.position.z), Quaternion.identity);
            //  Physics.IgnoreCollision(aTo_go.GetComponent<Collider>(), GetComponent<Collider>());

            badTanks.Add(aTo_goE);
            Debug.Log("Tank Spawned");
            doSpawn = false;
            spwantimeK3 = 0;
        }


    }



    void AiSpawnCall(int t1, int t2, int t3)
    {

    }
}
