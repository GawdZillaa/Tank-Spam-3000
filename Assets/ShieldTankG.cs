using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTankG : MonoBehaviour, ComponentFind
{
    float moveTotalSpeed;

    Component TankC;

    bool SheildActive;

    public static float movespeed = 1.0f;
    public Vector3 userDirection = Vector3.left;
    public Animator anim;
    public Transform bullPoint;
    GameObject tempO;
    GameObject tempHqO;

    public GameObject ProjectilePrefab;
    public GameObject spawnPointGood;
    public float projectileVelo;

    private float canFireIn;
    private List<GameObject> ProjectilesR2G = new List<GameObject>();
    public bool killSwitch = false;
    bool shouldMove = true;
    bool wasMovingAlready = true;


    public bool inBattle = false;

    float TargetX;
    Vector3 TargetVector;
    Vector3 CurrentVector;

    public AudioSource audio;

    public AudioClip shot;

    public BoxCollider2D absorbBullet;
    public BoxCollider2D EnemyStop;

    GameObject MoveFind;
    bool Manual;

    bool initialCorrectionAuto;
    bool initialCorrectionManual;

    public void Shield_isOn()
    {
        SheildActive = true;
    }

    public void Shield_isOff()
    {
        SheildActive = false;
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
        //        transform.Translate(userDirection * movespeed * Time.deltaTime);
        initialCorrectionAuto = false;
        initialCorrectionManual = false;
        start_Movement_Type();
        anim = GetComponent<Animator>();
        health = maxxHealth;
        SheildActive = true;

        if (Manual == true)
        {
            anim.Play("IdleSh");
        }

        if (Manual == false)
        {
            anim.Play("MoveSh");
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

                if (SheildActive == true)
                {
                    anim.Play("IdleSh");

                }

                if (SheildActive == false)
                {
                    anim.Play("IdleShD");

                }
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
                if (SheildActive == true)
                {
                    anim.Play("MoveSh");

                }

                if (SheildActive == false)
                {
                    anim.Play("MoveShD");

                }
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
                    if (SheildActive == true)
                    {
                        anim.Play("IdleSh");

                    }

                    if (SheildActive == false)
                    {
                        anim.Play("IdleShD");

                    }
                    //                Debug.Log("Idle");


                    Debug.Log("shouldmove; " + shouldMove);
                    Debug.Log("timer; " + timer + " WaitTime: " + waitTime);
                    Debug.Log("onTrigger; " + onTrigger);

                    if (shouldMove == false && timer >= waitTime && onTrigger == true)
                    {
                        inBattle = true;


                        timer = 0;
                        Debug.Log(tempO);

                        Debug.Log("CHECK AREA");
                        moveManager(tempO, tempHqO);


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
                if (SheildActive == true)
                {
                    anim.Play("MoveSh");

                }

                if (SheildActive == false)
                {
                    anim.Play("MoveShD");

                }
            }

            if (shouldMove == true && health >= 0 && killSwitch == false)
            {
                transform.Translate(userDirection * movespeed * Time.deltaTime);

                //animationController(wasMovingAlready);
                if (wasMovingAlready == false)
                {
                    if (SheildActive == true)
                    {
                        anim.Play("IdleSh");

                    }

                    if (SheildActive == false)
                    {
                        anim.Play("IdleShD");

                    }
                    wasMovingAlready = true;
                }



            }

            else
            {
                if (killSwitch == false)
                {
                    transform.Translate(userDirection * movespeed * 0);
                    if (SheildActive == true)
                    {
                        anim.Play("IdleSh");

                    }

                    if (SheildActive == false)
                    {
                        anim.Play("IdleShD");

                    }
                    wasMovingAlready = false;

                    if (shouldMove == false && health >= 0)
                    {

                        moveManager(tempO, tempHqO);


                    }


                }


            }


        }//AUTO MOVE END~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~







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




    void OnTriggerExit2D(Collider2D other)
    {
 //       Debug.Log("EXIT COLLIDER");
        //   shouldMove = true;
    }

    //    public AnimationClip deathEvil;

    public void moveManager(GameObject lol, GameObject hqLol)
    {
        // Debug.Log(lol.GetComponent<EvilTankManager>().health);
//        Debug.Log("~~~~In Move Manager~~~~");

 //       Debug.Log("lol: " + lol + " " + "HQ: " + hqLol);

        if (lol == null && hqLol == null)
        {
            Debug.Log("RESET MOVEEEEE");
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

        if (lol != null && hqLol == null)
        {
            //Debug.Log("CHECKING" + " " + "Health at: " + temp.GetComponent<EvilTankManager>().health);
            // Debug.Log(lol.activeInHierarchy);


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




                absorbBullet.enabled = false;
                EnemyStop.enabled = false;

                killSwitch = true;
                setHealthBar(calc_health);
                Falcon.AInputManager.instance.selector.onFinishSelecting += unregister_Death;
                anim.Play("Boom");
                //               explode();
                //               Destroy(explosion.gameObject);
                Destroy(gameObject, 4f);
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
}