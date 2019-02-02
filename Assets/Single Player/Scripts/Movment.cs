using UnityEngine;
using System.Collections.Generic;

public class Movment : MonoBehaviour, ComponentFind {
    Component TankC;


    public float movespeed;
    float moveTotalSpeed;
    public Vector3 userDirection = Vector3.left;
    public Vector3 userDirection2;
    public Animator anim;
    public Transform bullPoint;
    bool shouldMove = true;

    public GameObject ProjectilePrefab;
    public GameObject spawnPointGood;
    public float projectileVelo;

    private float canFireIn;
    private List<GameObject> Projectiles = new List<GameObject>();
    public bool killSwitch = false;

    bool wasMovingAlready = true;
    GameObject tempO;
    GameObject tempHqO;

    AudioSource audio;
    public AudioClip shot;

    bool initialCorrectionAuto;
    bool initialCorrectionManual;


    float TargetX;
    Vector3 TargetVector;
    Vector3 CurrentVector;

    public bool inBattle = false;
    public BoxCollider2D absorbBullet;
    public BoxCollider2D PlayerStop;
    public BoxCollider2D EnemyStop;
    public BoxCollider2D Rear;

    GameObject MoveFind;

    GameObject EnemyResource;
    bool Manual;
    GameObject GameOver;


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


    void FixedUpdate()
    {

    }
    // Use this for initialization
    void Start()
    {
        GameOver = GameObject.FindGameObjectWithTag("Display");
        GameOver.GetComponent<GameOverManager>().PlayerRegisterArmy(1);

        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        initialCorrectionAuto = false;
        initialCorrectionManual = false;

        start_Movement_Type();

        health = maxxHealth;
        Debug.Log(Manual);

        EnemyResource = GameObject.FindGameObjectWithTag("AI");


        if (Manual == true)
        {
            anim.Play("Idle");
        }

        if (Manual == false)
        {
            anim.Play("Move");
        }


    }

    // Update is called once per frame
    float timer;
    public float duration;
    public float waitTime;


    void Update()
    { 
        timer += Time.deltaTime;
        canFireIn -= Time.deltaTime;


        if (Manual == true)   //MANUAL MOVE START~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        {
            if (initialCorrectionManual == false)
            {
                initialCorrectionAuto = false;
                initialCorrectionManual = true;
                theClick = false;
                anim.Play("Idle");
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
                anim.Play("Move");
                //            transform.Translate(userDirection * movespeed * Time.deltaTime);

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
                    anim.Play("Idle");
                    //                Debug.Log("Idle");


 //                   Debug.Log("shouldmove; " + shouldMove);
 //                   Debug.Log("timer; " + timer + " WaitTime: " + waitTime);
//                    Debug.Log("onTrigger; " + onTrigger);

                    if (shouldMove == false && timer >= waitTime && onTrigger == true)
                    {
                        inBattle = true;
                        GameObject bullet = (GameObject)Instantiate(ProjectilePrefab, bullPoint.position, Quaternion.identity);
                        Projectiles.Add(bullet);
                        audio.PlayOneShot(shot);

   //                     Debug.Log("SHOOT");

                        timer = 0;
  //                      Debug.Log(tempO);

 //                       Debug.Log("CHECK AREA");
                        moveManager(tempO, tempHqO);


                    }

                    //onTrigger = false;
                    inBattle = false;
                }

            }

        } //MANUAL MOVE END~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        if(Manual == false) //AUTO MOVE START~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        {
            if (initialCorrectionAuto == false)
            {
                Auto_Composure_Corrector();
                initialCorrectionAuto = true;
                initialCorrectionManual = false;
                anim.Play("Move");


            }

            if (shouldMove == true && health >= 0 && killSwitch == false)
            {
                transform.Translate(userDirection * movespeed * Time.deltaTime);

                //animationController(wasMovingAlready);
                if (wasMovingAlready == false)
                {
                    anim.Play("Move");
                    wasMovingAlready = true;
                }



            }

            else
            {
                if (killSwitch == false)
                {
                    transform.Translate(userDirection * movespeed * 0);
                    anim.Play("Idle");
                    wasMovingAlready = false;

                    if (shouldMove == false && timer > waitTime && health >= 0)
                    {
                        GameObject bulletE = (GameObject)Instantiate(ProjectilePrefab, bullPoint.position, Quaternion.identity);
                        Projectiles.Add(bulletE);
                        //                  Debug.Log("SHOOT");
                        Debug.Log("_+_+_+_+_+_+_+_+_+_+_+_+-SHOOT+_+_+_+_+_+_+_++_+_+_+_+_+_+_=");

                        audio.PlayOneShot(shot);

                        timer = 0;
                        moveManager(tempO, tempHqO);


                    }


                }


            }

        } //AUTO MOVE END~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        for (int i = 0; i < Projectiles.Count; i++)
        {
            GameObject goBullet = Projectiles[i];

            if (goBullet != null)
            {
                goBullet.transform.Translate(new Vector3(-1, 0) * Time.deltaTime * projectileVelo);

            }

        }



    }

    bool theClick = false;

    public void ListMover(float xCoord, bool ShouldMove)
    {
        theClick = true;
        hasFlipped = false;
        TargetVector = new Vector3(xCoord, -1.9f, 0);
        CurrentVector = transform.position;

        Debug.Log("----Inside Movment----");
        Debug.Log("----" + " Target: " + TargetVector);
        Debug.Log("----" + " Current: " + CurrentVector);

        transform.Translate(userDirection * movespeed * Time.deltaTime);
    }

    bool onTrigger = false;
    bool shouldShoot = false;


    //void OnTriggerEnter2D(Collider2D other)
    //{

    //    if (other.gameObject.name == "Stop Tank G")
    //    {
    //        Debug.Log(" was triggered");
    //        shouldMove = false;
    //        onTrigger = true;
    //        tempHq = other.gameObject;

    //        moveManager(other.gameObject, tempHq);

    //    }


    //    if (other.gameObject.tag == "BadTank" ||
    //        other.gameObject.tag == "BadTankT2" ||
    //        other.gameObject.tag == "BadTankT3")
    //    {
    //        Debug.Log("ENEMY TANK DETECTED");
    //        shouldMove = false;

    //        onTrigger = true;
    //        temp = other.gameObject;
    //        EnemyAtSix(temp);

    //        moveManager(temp, tempHq);

    //    }

    //}

    void OnTriggerExit2D(Collider2D other)
    {
    }


    public void moveManager(GameObject lol, GameObject hqLol)
    {
 //       Debug.Log("~~~~In Move Manager~~~~");
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

        if (health <= 0 || health == 0)
        {
            Debug.Log("FRIENDLY UNIT DESTROYED");
            if (stopHealth == false)
            {
                GameOver.GetComponent<GameOverManager>().AiRegisterKill(1);

                absorbBullet.enabled = false;
                PlayerStop.enabled = false;
                Rear.enabled = false;

                EnemyResource.GetComponent<AiReources>().killResource(10);
                killSwitch = true;
                setHealthBar(calc_health);
                Falcon.AInputManager.instance.selector.onFinishSelecting += unregister_Death;
                anim.Play("Boom");

                Destroy(gameObject, 4f);
                stopHealth = true;
            }

        }
        else
        {
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
            anim.Play("GoodTankDie");
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


