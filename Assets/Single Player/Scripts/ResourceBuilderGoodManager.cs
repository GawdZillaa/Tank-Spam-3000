using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuilderGoodManager : MonoBehaviour {

    private List<GameObject> resourceBuilding = new List<GameObject>();
    public Transform building_spawnGood;

    public GameObject building;

    public float movespeed;
    float moveTotalSpeed;
    public Vector3 userDirection = Vector3.left;
    public Vector3 userDirection2;
    public Animator anim;
    bool shouldMove = true;

    public GameObject spawnPointGood;

    public bool killSwitch = false;

    bool wasMovingAlready = true;
    GameObject temp;
    GameObject tempHq;

    bool builder = false;

    float TargetX;
    Vector3 TargetVector;
    Vector3 CurrentVector;

    public bool inBattle = false;

    void Awake()
    {
        temp = null;
        tempHq = null;
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


    void FixedUpdate()
    {

    }
    // Use this for initialization
    void Start()
    {
        //        transform.Translate(userDirection * movespeed * Time.deltaTime);

        anim = GetComponent<Animator>();
        health = maxxHealth;
        anim.Play("ResourceGoodIdle");

    }

    // Update is called once per frame
    float timer;
    public float duration;


    void Update()
    {  //UPDATEEEE```````````````

 //       Debug.Log(theClick);

        timer += Time.deltaTime;



        if (onTrigger == true)
        {
            theClick = false;
        }

        if ((Vector3.Distance(transform.position, TargetVector) > .1f) && theClick == true &&
            builder == false)
        {
            if (hasFlipped == false)
            {
                float diffrence = TargetVector.x - transform.position.x;
                Flip(diffrence);
            }

            moveTotalSpeed = movespeed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, TargetVector, moveTotalSpeed);


            transform.Translate(userDirection * movespeed * 0);
            anim.Play("ResourceGoodMove");
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

            if (killSwitch == false && theClick == false && shouldMove == false && builder == false)
            {
                // transform.Translate(userDirection * movespeed * 0);
                anim.Play("ResourceGoodIdle");
                //                Debug.Log("Idle");

              

                onTrigger = false;
                inBattle = false;
            }

        }



        
        if (doSpawn == true)
        {
            stageCount += Time.deltaTime;
            if (builderplay1 == false)
            {
                Debug.Log("play 1st");
                anim.Play("ResourceGoodLower");
                 builderplay = true;

            }
            if (lowerTimer <= stageCount)
            {
                if (builderplay == false)
                {
                    Debug.Log("play 2nd");

                    anim.Play("ResourceGoodBuild");
                    builderplay = true;
                }

                if (BuildTimer < stageCount)
                {

                    GameObject bTo_go = (GameObject)Instantiate(building, building_spawnGood.position, Quaternion.identity);

                    resourceBuilding.Add(bTo_go);
                    doSpawn = false;
                    Destroy(gameObject);
                }
            }


        }




    }
    bool builderplay1 = false;

    bool builderplay = false;
    void continueBuildAnim()
    {

    }

    float stageCount = 0;
    float lowerTimer = 6;
    float BuildTimer = 20;

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
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.name == "Stop Tank G")
        {
            //            Debug.Log(" was triggered");
            shouldMove = false;
            tempHq = other.gameObject;
            moveManager(other.gameObject, tempHq);

        }



        if (other.gameObject.tag == "BadTank" ||
            other.gameObject.tag == "BadTankT2")
        {
            //            Debug.Log("ENEMY TANK DETECTED");
            shouldMove = false;

            onTrigger = true;
            temp = other.gameObject;
            moveManager(temp, tempHq);

        }

    }

    //void OnTriggerExit2D(Collider2D other)
    //{
    //   Debug.Log("EXIT COLLIDER");
    //  shouldMove = true;
    //}

    // public AnimationClip deathEvil;

    public void moveManager(GameObject lol, GameObject hqLol)
    {
        // Debug.Log(lol.GetComponent<EvilTankManager>().health);
        //        Debug.Log("~~~~In Move Manager~~~~");

        //        Debug.Log("lol: " + lol + " " + "HQ: " + hqLol);

        if (lol == null && hqLol == null)
        {
            Debug.Log("RESET MOVEEEEE");
            shouldMove = true;


        }

        if (lol != null && hqLol == null)
        {
            //           Debug.Log("CHECKING" + " " + "Health at: " + temp.GetComponent<EvilTankManager>().health);
            //          Debug.Log(lol.activeInHierarchy);


        }


    }


    public float maxxHealth = 100;
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
            //            Debug.Log("FRIENDLY UNIT DESTROYED");
            if (stopHealth == false)
            {
                killSwitch = true;
                setHealthBar(calc_health);
                deathAnim();
                Destroy(gameObject, .7f);
                stopHealth = true;

            }

        }
        else
        {
            //            Debug.Log("Freindly Health" + " " + health);
            setHealthBar(calc_health);
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

    // void checkTarget ()

    bool doSpawn;

    public void spawnCheck(bool changer)
    {
        doSpawn = changer;
        builder = true;
        //   TierTank = TierType;
    }




}


