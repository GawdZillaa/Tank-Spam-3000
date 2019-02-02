using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(AudioSource))]

public class T3GoodManager : MonoBehaviour, ComponentFind {

    public  float movespeed;
    float moveTotalSpeed;

   public GameObject Sensor;
    Component TankC;

    public Vector3 userDirection = Vector3.left;
    public Animator anim;
    public Transform bullPoint;
    public Transform bullPoint2;

    bool shouldMove = true;

    public GameObject ProjectilePrefab;
    public GameObject ProjectilePrefab2;
    public GameObject spawnPointGood;
    public float projectileVelo;

    private float canFireIn;
    private List<GameObject> ProjectilesR3G = new List<GameObject>();
    public bool killSwitch = false;

    bool wasMovingAlready = true;
    GameObject tempO;
    GameObject tempHqO;

    float TargetX;
    Vector3 TargetVector;
    Vector3 CurrentVector;

    public bool inBattle = false;
    public BoxCollider2D absorbBullet;
    public BoxCollider2D PlayerStop;
    public BoxCollider2D EnemyStop;
    public BoxCollider2D Rear;

    private float timer2;
    public float waitTime2;

    public AudioClip shot;
    public AudioClip shot2;
    public AudioClip explode;

    GameObject MoveFind;

    AudioSource audio;
    public int audioPLay;
     int tempAudio;

    bool Manual = true;

    bool initialCorrectionAuto;
    bool initialCorrectionManual;

    GameObject EnemyResource;

    GameObject GameOver;

    private void Auto_Composure_Corrector()
    {

        if (isFacing_NonNormal == true)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

            isFacing_NonNormal = false;

        }

    }

    public void AutoMove()
    {
        Manual = false;
    }

    public void ManualMove()
    {
        Manual = true;
    }

    void start_Movement_Type()
    {
        MoveFind = GameObject.FindGameObjectWithTag("Display");
        Manual = MoveFind.GetComponent<BuildingMenu>().MovmentType();
        Debug.Log("SET MOVMENT TYPE+!+!+!+!+!+!+!+! " + Manual);
    }


    void Awake()
    {
        tempO = null;
        tempHqO = null;
    }

    private bool hasFlipped = false;
    private bool isFacing_NonNormal = false;

    private void Flip(float diffrence)
    {
        if (diffrence < 0 && isFacing_NonNormal == true)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;

            transform.localScale = theScale;

            isFacing_NonNormal = false;
        }

        if (diffrence > 0 && isFacing_NonNormal == false)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;

            transform.localScale = theScale;

            isFacing_NonNormal = true;
        }

        hasFlipped = true;
    }


    private void EnemyAtSix(GameObject flanker)
    {
        Vector3 FlankerPosition = flanker.transform.position;

        if (FlankerPosition.x < transform.position.x && isFacing_NonNormal == true)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;

            isFacing_NonNormal = false;



        }

    }


    void FixedUpdate()
    {

    }
    // Use this for initialization
    void Start()
    {
        GameOver = GameObject.FindGameObjectWithTag("Display");
        GameOver.GetComponent<GameOverManager>().PlayerRegisterArmy(3);
        //        transform.Translate(userDirection * movespeed * Time.deltaTime);
        initialCorrectionAuto = false;
        initialCorrectionManual = false;
        start_Movement_Type();
        anim = GetComponent<Animator>();
        health = maxxHealth;

        audio = GetComponent<AudioSource>();

        EnemyResource = GameObject.FindGameObjectWithTag("AI");


        if (Manual == true)
        {
            anim.Play("T3GoodIdle");
        }

        if (Manual == false)
        {
            anim.Play("T3GoodMove");
        }

    }

    // Update is called once per frame
    float timer;
    public float duration;
    public float waitTime;


    void Update()
    {  


        timer += Time.deltaTime;
        timer2 += Time.deltaTime;
        canFireIn -= Time.deltaTime;



        if (Manual == true)   //MANUAL MOVE START~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        {
            if (initialCorrectionManual == false)
            {
                initialCorrectionAuto = false;
                initialCorrectionManual = true;
                theClick = false;
                anim.Play("T3GoodIdle");
            }

            if (onTrigger == true)
            {
                theClick = false;
            }

            if ((Vector3.Distance(transform.position, TargetVector) > .1f) && theClick == true && killSwitch == false)
            {
                if (hasFlipped == false)
                {
                    float diffrence = TargetVector.x - transform.position.x;
                    Flip(diffrence);
                }

                moveTotalSpeed = movespeed * Time.deltaTime;

                transform.position = Vector3.MoveTowards(transform.position, TargetVector, moveTotalSpeed);


                transform.Translate(userDirection * movespeed * 0);
                anim.Play("T3GoodMove");

                inBattle = false;
                if ((Vector3.Distance(transform.position, TargetVector) < .1f))
                {
                    theClick = false;
                    shouldMove = false;

                }



            }

            else
            {

                if (killSwitch == false && theClick == false && shouldMove == false)
                {
                    // transform.Translate(userDirection * movespeed * 0);
                    anim.Play("T3GoodIdle");
                    moveManager(tempO, tempHqO);

                    if (shouldMove == false && timer >= waitTime && onTrigger == true)
                    {
                        inBattle = true;
                        GameObject bullet = (GameObject)Instantiate(ProjectilePrefab, bullPoint.position, Quaternion.identity);
                        ProjectilesR3G.Add(bullet);
                        tempAudio++;
                        Debug.Log("SHOOT");

                        audio.PlayOneShot(shot);


                        timer = 0;
                        Debug.Log(tempO);

//                        Debug.Log("CHECK AREA");
                        moveManager(tempO, tempHqO);


                    }


                    if (shouldMove == false && timer2 > waitTime2 && onTrigger == true)
                    {
                        GameObject bullet2 = (GameObject)Instantiate(ProjectilePrefab2, bullPoint2.position, Quaternion.identity);
                        ProjectilesR3G.Add(bullet2);
                        audio.PlayOneShot(shot2);

                        timer2 = 0;
                    }


                    //onTrigger = false;
                    inBattle = false;
                }

            }


        }

        if (Manual == false) //AUTO MOVE START~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        {
            if (initialCorrectionAuto == false)
            {
                Auto_Composure_Corrector();
                initialCorrectionAuto = true;
                initialCorrectionManual = false;
                anim.Play("T3GoodMove");


            }

            if (shouldMove == true && health >= 0 && killSwitch == false)
            {
                transform.Translate(userDirection * movespeed * Time.deltaTime);

                //animationController(wasMovingAlready);
                if (wasMovingAlready == false)
                {
                    anim.Play("T3GoodMove");
                    wasMovingAlready = true;
                }



            }

            else
            {
                if (killSwitch == false)
                {
                    transform.Translate(userDirection * movespeed * 0);
                    anim.Play("T3GoodIdle");
                    wasMovingAlready = false;

                    moveManager(tempO, tempHqO);

                    if (shouldMove == false && timer > waitTime && health >= 0)
                    {
                        GameObject bulletE = (GameObject)Instantiate(ProjectilePrefab, bullPoint.position, Quaternion.identity);
                        ProjectilesR3G.Add(bulletE);
                        //                  Debug.Log("SHOOT");
                        audio.PlayOneShot(shot);

                        timer = 0;
                        moveManager(tempO, tempHqO);


                    }

                    if (shouldMove == false && timer2 > waitTime2)
                    {
                        GameObject bullet2 = (GameObject)Instantiate(ProjectilePrefab2, bullPoint2.position, Quaternion.identity);
                        ProjectilesR3G.Add(bullet2);
                        audio.PlayOneShot(shot2);

                        timer2 = 0;
                    }


                }


            }

        }


            for (int i = 0; i < ProjectilesR3G.Count; i++)
        {
            GameObject goBullet = ProjectilesR3G[i];

            // Debug.Log("projectile counter" + " " + Projectiles.Count);

            if (goBullet != null)
            {
                goBullet.transform.Translate(new Vector3(-1, 0) * Time.deltaTime * projectileVelo);
                //  Physics.IgnoreCollision(goBullet.GetComponent<Collider>(), GetComponent<Collider>());

            }

        }



    }

    bool theClick = false;

    public void ListMover(float xCoord, bool ShouldMove)
    {
        theClick = true;
        hasFlipped = false;
        TargetVector = new Vector3(xCoord, -1.75f, 0);
        CurrentVector = transform.position;

//        Debug.Log("----Inside Movment----");
//        Debug.Log("----" + " Target: " + TargetVector);
//        Debug.Log("----" + " Current: " + CurrentVector);

        transform.Translate(userDirection * movespeed * Time.deltaTime);
    }

    bool onTrigger = false;
    bool shouldShoot = false;




    void OnTriggerExit2D(Collider2D other)
    {
//        Debug.Log("EXIT COLLIDER");
    }


    public void moveManager(GameObject lol, GameObject hqLol)
    {
        // Debug.Log(lol.GetComponent<EvilTankManager>().health);
//        Debug.Log("~~~~In Move Manager~~~~");

 //       Debug.Log("lol: " + lol + " " + "HQ: " + hqLol);

        if (lol == null && hqLol == null)
        {
 //           Debug.Log("RESET MOVEEEEE");
            shouldMove = true;
            onTrigger = false;


        }

        if (lol != null && lol.tag == ("BadTank"))
        {
            if (lol.GetComponent<EvilTankManager>().killSwitch == true)
            {
                Debug.Log("RESET MOVEEEEE");
                shouldMove = true;
                onTrigger = false;


            }
        }

        if (lol != null && lol.tag == ("BadTankT2"))
        {
            if (lol.GetComponent<Tier2EvilManager>().killSwitch == true)
            {
                Debug.Log("RESET MOVEEEEE");
                shouldMove = true;
                onTrigger = false;


            }
        }

        if (lol != null && lol.tag == ("BadTankT3"))
        {
            if (lol.GetComponent<eT3Manager>().killSwitch == true)
            {
                Debug.Log("RESET MOVEEEEE");
                shouldMove = true;
                onTrigger = false;


            }
        }

        if (lol != null && lol.tag == ("TurrBad"))
        {
            if (lol.GetComponent<BadTurrentManager>().killSwitch == true)
            {
                Debug.Log("RESET MOVEEEEE");
                shouldMove = true;
                onTrigger = false;


            }
        }
        if (lol != null && hqLol == null)
        {



        }


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
                GameOver.GetComponent<GameOverManager>().AiRegisterKill(3);

                audio.PlayOneShot(explode);

                absorbBullet.enabled = false;
//                PlayerStop.enabled = false;
                EnemyStop.enabled = false;
                Rear.enabled = false;

                killSwitch = true;
                Sensor.SetActive(false);

                EnemyResource.GetComponent<AiReources>().killResource(45);

                setHealthBar(calc_health);
                Falcon.AInputManager.instance.selector.onFinishSelecting += unregister_Death;
                anim.Play("T3Boom");

                Destroy(gameObject, 6f);
                stopHealth = true;
            }

        }
        else
        {
            //            Debug.Log("Freindly Health" + " " + health);
            setHealthBar(calc_health);
        }


    }

    void unregister_Death(Falcon.Selectable[] selected)
    {
        foreach (Falcon.Selectable selectable in selected)
        {
            if (selectable.gameObject == this.gameObject)
            {
                Falcon.AInputManager.instance.selector.UnregisterSelectable(selectable);
                Falcon.AInputManager.instance.selector.UnselectAll();
            }
        }
    }

    void deathAnim()
    {
        if (killSwitch == true)
        {
            //  anim.Play("GoodTankDie");
        }
    }

    public void setHealthBar(float health)
    {
        BarHealth.transform.localScale = new Vector3(health, BarHealth.transform.localScale.y,
        BarHealth.transform.localScale.z);
    }

    public void SetStop(GameObject temp, GameObject tempHq, int id)
    {
        Debug.Log("Stopping...");
        shouldMove = false;
        onTrigger = true;

        tempO = temp;
        tempHqO = tempHq;

        if (temp != null)
        {
            EnemyAtSix(temp);
        }
        moveManager(temp, tempHq);
    }

    public Component ComponentScout()
    {
        TankC = this.GetComponent<Movment>();
        return TankC;
    }



    public bool getOnTrigger()
    {
        return onTrigger;
    }
}


