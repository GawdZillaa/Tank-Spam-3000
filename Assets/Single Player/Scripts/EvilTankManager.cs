using UnityEngine;
using System.Collections.Generic;

public class EvilTankManager : MonoBehaviour {
    private List<GameObject> Projectiles = new List<GameObject>();

    public float maxxHealth = 100;
    public float health;
    bool stopHealth = false;
    public GameObject BarHealth;
    public Vector3 userDirection = Vector3.right;
    public Transform bullPoint;

    public GameObject ProjectilePrefab;
    private List<GameObject> ProjectilesE = new List<GameObject>();
    public float projectileVelo;
    private float canFireIn;

    public GameObject resources;
    //   private ResourceManagerG output;

    GameObject tempHq;
    GameObject temp;

    public BoxCollider2D absorbBullet;
    public BoxCollider2D PlayerStop;
    public BoxCollider2D EnemyStop;

    //ANIMATION 
    public float movespeed;
    float moveTotalSpeed;

    AudioSource audio;
    public AudioClip shot;
    public AudioClip explode;

    bool isAuto;
    bool CommandRecieved;

    Vector3 TargetFrontline;
    

    public GameObject MovmentManager;
    public GameObject Ai;

    public Animator anim;
 //   int dieHash = Animator.StringToHash("BoomBoom");
    bool shouldMove = true;
    public bool inBattle = false;

    private bool hasFlipped = false;
    private bool isFacing_NonNormal = false;

    GameObject GameOver;


    void Start()
    {
        GameOver = GameObject.FindGameObjectWithTag("Display");
        GameOver.GetComponent<GameOverManager>().AiRegisterArmy(1);

        MovmentManager = GameObject.FindGameObjectWithTag("AI");
        Ai = GameObject.FindGameObjectWithTag("AI"); 

        resources = GameObject.Find("Text");
        anim.Play("Idle");
        wasMovingAlready = true;
        audio = GetComponent<AudioSource>();

        isAuto = MovmentManager.GetComponent<AiMovmentState>().RequestMovmentType();
 //       Debug.Log("isAuto: " + isAuto);
        if (isAuto == false)
        {
            CommandRecieved = true;
            TargetFrontline = MovmentManager.GetComponent<AiMovmentState>().RequestFrontlinePosition();
        }
        else
        {
            TargetFrontline = new Vector3(0,0,0);
        }

    }

    // Use this for initialization
    void Awake () {
        health = maxxHealth;
        temp = null;
        tempHq = null;
 //       output = GoodResource.GetComponent<ResourceManagerG>();



    }
    public  bool killSwitch = false;
    void FixedUpdate()
    {
 

    }
    float timer;
    public float waitTime;

    // Update is called once per frame
    void Update () {

        timer += Time.deltaTime;
        canFireIn -= Time.deltaTime;

        if (isAuto == true)
        {
            if (shouldMove == true && health >= 0 && killSwitch == false)
            {
                transform.Translate(userDirection * movespeed * Time.deltaTime);

                //animationController(wasMovingAlready);
                if (wasMovingAlready == false)
                {
                    anim.Play("EvileMove");
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
                        ProjectilesE.Add(bulletE);
                        //                  Debug.Log("SHOOT");
                        audio.PlayOneShot(shot);

                        timer = 0;
                        moveManager(temp, tempHq);


                    }


                }


            }
        }

        if (isAuto == false)
        {
            
            
                if ((Vector3.Distance(transform.position, TargetFrontline) > .1f) && killSwitch == false && CommandRecieved == true 
                && shouldMove == true)
                {
                    if (hasFlipped == false)
                    {
                        float diffrence = TargetFrontline.x - transform.position.x;
                        Flip(diffrence);
                    }

  //              Debug.Log("Manual Move...");

                    //               Debug.Log(transform.position + " vs " + TargetVector);

                    moveTotalSpeed = movespeed * Time.deltaTime;

                    transform.position = Vector3.MoveTowards(transform.position, TargetFrontline, moveTotalSpeed);


                    transform.Translate(userDirection * movespeed * 0);
                    anim.Play("EvileMove");
                    //            transform.Translate(userDirection * movespeed * Time.deltaTime);

                    inBattle = false;
                    if ((Vector3.Distance(transform.position, TargetFrontline) < .1f))
                    {
                        shouldMove = false;
                        CommandRecieved = false;

                    }


                }
                else
                {

                    if ((killSwitch == false  && shouldMove == false) || (killSwitch ==false && CommandRecieved == false))
                    {
  //                  Debug.Log("Manual Idle...");

                    anim.Play("Idle");

                        moveManager(temp, tempHq);
                        if (shouldMove == false && timer >= waitTime)
                        {
                            inBattle = true;
                            GameObject bullet = (GameObject)Instantiate(ProjectilePrefab, bullPoint.position, Quaternion.identity);
                            Projectiles.Add(bullet);
                            audio.PlayOneShot(shot);

                            //                       Debug.Log("SHOOT");

                            timer = 0;
                            Debug.Log(temp);

                            //                       Debug.Log("CHECK AREA");
                            moveManager(temp, tempHq);


                        }

                        //onTrigger = false;
                        inBattle = false;
                    }

                }
            }

        


            for (int i = 0; i < ProjectilesE.Count; i++)
        {
            GameObject goBulletE = ProjectilesE[i];
            if (goBulletE != null)
            {
                goBulletE.transform.Translate(new Vector3(1, 0) * Time.deltaTime * projectileVelo);

            }

        }
    }

    private void Flip(float diffrence)
    {
        if (diffrence < 0 && isFacing_NonNormal == false)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;

            transform.localScale = theScale;

            isFacing_NonNormal = true;
        }

        if (diffrence > 0 && isFacing_NonNormal == true)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;

            transform.localScale = theScale;

            isFacing_NonNormal = false;
        }

        hasFlipped = true;
    }

    bool wasMovingAlready = true;
    bool firstMove = false;



    public void takeDamage(int amount)
    {
        health -= amount;
        float calc_health = health / maxxHealth;


 //       Debug.Log("ENEMY Unit Damaged!" + " " + health);
        if (health <= 0 || health == 0)
        {
 //           Debug.Log("ENEMY UNIT DESTROYED");
            if (stopHealth == false)
            {
                //             anim.Play("EvilDie");

                GameOver.GetComponent<GameOverManager>().PlayerRegisterKill(1);

                audio.PlayOneShot(explode);
                resources.GetComponent<ResourceManagerG>().killResource(10);

                absorbBullet.enabled = false;
                PlayerStop.enabled = false;
                EnemyStop.enabled = false;

                killSwitch = true;
                setHealthBar(calc_health);
                deathAnim();
                Destroy(gameObject, 3.0f);
                stopHealth = true;
                
            }

 //           Die();
        }
        else
        {
            setHealthBar(calc_health);
        }


    }





    void deathAnim()
    {
        if (killSwitch == true)
        {
     //       resources.GetComponent<ResourceManagerG>().killResource(5);

            anim.Play("EvilDie");
        }
    }


    public void setHealthBar(float health)
    {
        BarHealth.transform.localScale = new Vector3(health, BarHealth.transform.localScale.y,
        BarHealth.transform.localScale.z);
    }

    float healtNotify()
    {
        return health;

    }


    void OnTriggerEnter2D(Collider2D other)
    {
 //       Debug.Log("GOOD TANK DETECTED");
        if (other.gameObject.tag == "Tank" ||
            other.gameObject.tag == "Tank2"||
            other.gameObject.tag == "Tank3"||
            other.gameObject.tag == "Gresource"||
            other.gameObject.tag == "GoodResourceCollector" ||
            other.gameObject.tag == "Turr" ||
            other.gameObject.tag == "Tank4" ||
            other.gameObject.tag == "TankShG" ||
            other.gameObject.tag == "ShGs" ||
            other.gameObject.tag == "MechG"
            )
        {
 //          Debug.Log("GOOD TANK DETECTED");

            shouldMove = false;
            tempHq = other.gameObject;
            moveManager(other.gameObject, tempHq);

        }

        if (other.gameObject.name == "Stop Tank E")
        {
 //           Debug.Log("ENEMY BASE DETECTED");
            shouldMove = false;
            temp = other.gameObject;
            moveManager(temp, tempHq);

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
      //  Debug.Log("EXIT COLLIDER");
       // shouldMove = true;
    }


    public void moveManager(GameObject lol, GameObject hqLol)
    {
//         Debug.Log(lol.GetComponent<EvilTankManager>().health);
//       Debug.Log("~~~~In Move Manager~~~~");

//        Debug.Log("lol: " + lol + " " + "HQ: " + hqLol);

        if (lol == null && hqLol == null)
        {
//            Debug.Log("RESET MOVEEEEE");
            shouldMove = true;


        }
        if (lol != null && lol.tag == ("Tank"))
        {
            if (lol.GetComponent<Movment>().killSwitch == true)
            {
                Debug.Log("RESET MOVEEEEE");
                shouldMove = true;


            }
        }
        if (lol != null && hqLol == null)
        {
            //           Debug.Log("CHECKING" + " " + "Health at: " + temp.GetComponent<EvilTankManager>().health);
  //          Debug.Log("2nD........");

 //           Debug.Log(lol.activeInHierarchy);


        }


    }


    void ChangeMovmentState(bool autois)
    {
       
        isAuto = autois;
    }

   public void AiMoveCommand(Vector3 newCoords)
    {
        TargetFrontline = newCoords;
        CommandRecieved = true;
    }

    public void AiOffenceCommand()
    {
        isAuto = true;
    }


}






