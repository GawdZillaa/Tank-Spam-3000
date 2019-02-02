using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BadTurrentManager : MonoBehaviour, ComponentFind {

    public List<GameObject> AllObjects = new List<GameObject>();

    private List<GameObject> Projectiles = new List<GameObject>();
    private List<GameObject> DetectedEnemies = new List<GameObject>();

//    Queue<GameObject> DeathNote = new Queue<GameObject>();

    List<GameObject> DeathNote = new List<GameObject>();

    public GameObject ProjectilePrefab;
    public Animator anim;
    public Transform bullPoint;

    public Component turb;

    public Collider2D detectCollider;

    public Collider2D absorbBullet;
    public Collider2D PlayerStop;

    GameObject temp;

    AudioSource audio;
    public AudioClip shot;

    bool onTrigger;
   public bool killSwitch;
    public float projectileVelo;

    public float waitTime;
    float timer;

    bool build;
    public float buildTimer;
    private float timer2;

    int SlotId;

     GameObject Ai;

    float DeathCounter = 0;
    public int TimeTillReset;

    double BackupHealth;

    bool hasTarget = false;

    bool hasBeenPowered = false;



    // Use this for initialization
    void Start()
    {
        TurrentNetwork();

        Ai = GameObject.FindGameObjectWithTag("AI");
        health = maxxHealth;
        build = true;
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        anim.Play("TurrBBuild");
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        timer2 += Time.deltaTime;

        if (buildTimer < timer2 && build == true)
        {
            Debug.Log("B Turr Build Done");
            build = false;
            anim.Play("TurrBIdle");
        }



        if (killSwitch == false && build == false)
        {

            if (timer >= waitTime && onTrigger == true)
            {
                GameObject bullet = (GameObject)Instantiate(ProjectilePrefab, bullPoint.position, Quaternion.identity);
                Projectiles.Add(bullet);
                audio.PlayOneShot(shot);

                Debug.Log("SHOOT");

                timer = 0;
                Debug.Log(temp);

                Debug.Log("CHECK AREA");
                moveManager(temp);



            }

        }







        for (int i = 0; i < Projectiles.Count; i++)
        {
            GameObject goBullet = Projectiles[i];

            // Debug.Log("projectile counter" + " " + Projectiles.Count);

            if (goBullet != null)
            {
                goBullet.transform.Translate(new Vector3(1, 0) * Time.deltaTime * projectileVelo);
                //  Physics.IgnoreCollision(goBullet.GetComponent<Collider>(), GetComponent<Collider>());

            }

        }

        if (onTrigger == true)
        {
            Ai.GetComponent<AI>().OffenciveDefence();
        }


        if (killSwitch == true)
        {
            DeathCounter += Time.deltaTime;
//            Debug.Log(DeathCounter + " " + TimeTillReset);
        if (TimeTillReset < DeathCounter)
            {
                Destroy(gameObject);
                UnRegisterSlotId();

            }
        }


        if (hasTarget == true)
        {
            if (temp == null)
            {
 //               Debug.Log("tmep Null? -> " + temp);

                if (DeathNote.Count > 0)
                {
                    int null_Count = 0;
                    foreach (GameObject listed in DeathNote)
                    {
                        if (listed != null)
                        {
                            Debug.Log(">>>>>New Temp<<<<<");
                            temp = listed;
                            moveManager(temp);
                            break;
                        }

                        else { null_Count++; }
                    }


                    if (null_Count == DeathNote.Count)
                    {
                        Debug.Log("Clear List");
                        hasTarget = false;
                        DeathNote.Clear();
                        tempManager();
                    }
                }



                else if (DeathNote.Count == 0)
                {
//                    Debug.Log("Count 0");
                    tempManager();

                }



            }
        }
    }




    public void moveManager(GameObject lol)
    {
        // Debug.Log(lol.GetComponent<EvilTankManager>().health);
        Debug.Log("~~~~In Move Manager~~~~");

        Debug.Log("lol: " + lol);

        if (lol == null)
        {
            Debug.Log("STOPfIRE");
            DestroyedEnemy(lol);

        }

        if (lol != null && lol.tag == ("Tank"))
        {
            if (lol.GetComponent<Movment>().killSwitch == true)
            {
                DestroyedEnemy(lol);


            }
        }

        if (lol != null && lol.tag == ("Tank2"))
        {
            if (lol.GetComponent<GoodTankR2Manager>().killSwitch == true)
            {
                DestroyedEnemy(lol);


            }
        }

        if (lol != null && lol.tag == ("Tank3"))
        {
            if (lol.GetComponent<T3GoodManager>().killSwitch == true)
            {
                DestroyedEnemy(lol);


            }
        }

        if (lol != null && lol.tag == ("TankShG"))
        {
            if (lol.GetComponent<ShieldTankG>().killSwitch == true)
            {
                DestroyedEnemy(lol);


            }
        }

        if (lol != null)
        {

            //Debug.Log("CHECKING" + " " + "Health at: " + temp.GetComponent<EvilTankManager>().health);
            // Debug.Log(lol.activeInHierarchy);

        }

    }

    void DestroyedEnemy(GameObject dead)
    {
        DeathNote.Remove(dead);
        tempManager();
    }




    public float maxxHealth;
    public float health;
    bool stopHealth = false;
    public GameObject BarHealth;

    public void takeDamage(int amount, GameObject lol)
    {
        health -= amount;
        float calc_health = health / maxxHealth;



        //        Debug.Log("friendly Unit Damaged!" + " " + health);
        if (health <= 0 || health == 0)
        {
            Debug.Log("FRIENDLY UNIT DESTROYED");
            if (stopHealth == false)
            {

 //               UnRegisterSlotId(); //UNREGISTER SLOT

                anim.Play("Expld");
                absorbBullet.enabled = false;
                PlayerStop.enabled = false;

                killSwitch = true;
                setHealthBar(calc_health);
                stopHealth = true;
                this.GetComponent<Renderer>().enabled = false;

  //              Destroy(gameObject, 10f);
    //            UnRegisterSlotId();
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

    void tempManager()
    {
        if (DeathNote.Count == 0)
        {

//            Debug.Log("RESET MOVEEEEE");
            onTrigger = false;

        }
    }

    public void SetStop(GameObject temp1, GameObject tempHq1, int functionId)
    {
        Debug.Log("***SET STOP***");
        Debug.Log("Curr Temp: " + temp);
        Debug.Log(DeathNote.Count);

        if (!DeathNote.Contains(temp1)  && functionId == 0)
        {
            Debug.Log("Adding Target");
            hasTarget = true;
            onTrigger = true;
            DeathNote.Add(temp1);
        }


        else if ((DeathNote.Contains(temp1)) && functionId == 1)
        {
            Debug.Log("Recieved Exit....");
            Debug.Log(DeathNote.Count);
            Debug.Log(temp);
            DeathNote.Remove(temp1);
            Debug.Log(DeathNote.Count);
            Debug.Log(temp);

            if (temp == temp1)
            {
                Debug.Log("BLAHHHHHHHHHHHHHHHHHH");

                temp = null;
            }

        }


    }

    public Component ComponentScout()
    {
        turb = this.GetComponent<BadTurrentManager>();
        return turb;
    }



    public void AssignSlotId(int id)
    {
        SlotId = id;
        Debug.Log("Turrent Reg Slot: " + SlotId);

    }

    public void UnRegisterSlotId()
    {
        Debug.Log("Un Reg Request Sent: " + SlotId);
        Ai.GetComponent<AI>().UnRegisterBuilding(0, SlotId);
    }


    void TurrentNetwork()
    {
        Scene scene = SceneManager.GetActiveScene();

        scene.GetRootGameObjects(AllObjects);

        int TotalTurrents = 0;
        foreach (GameObject listed in AllObjects)
        {
            if (listed.tag == "TurrBad")
            {
                TotalTurrents++;
            }
        }
        Debug.Log("Count: " + TotalTurrents);

        if (TotalTurrents == 5)
        {
            Debug.Log("5 Linked");
            PowerAll();
        }
    }


    public void PowerUp()
    {
        if (hasBeenPowered == false)
        {
            Debug.Log("Powered...");
            GetComponent<BadTurrentManager>().projectileVelo -= 1;
            GetComponent<BadTurrentManager>().waitTime -= .6f;
            hasBeenPowered = true;
        }
    }

    public void PowerAll()
    {
        foreach (GameObject listed in AllObjects)
        {
            if (listed.tag == "TurrBad")
            {
                listed.GetComponent<BadTurrentManager>().PowerUp();
            }
        }
    }
       
}

