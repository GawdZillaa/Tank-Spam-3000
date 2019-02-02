using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentManager : MonoBehaviour, ComponentFind {

    private List<GameObject> Projectiles = new List<GameObject>();
    private List<GameObject> DetectedEnemies = new List<GameObject>();
    List<GameObject> DeathNote = new List<GameObject>();
    bool hasTarget = false;

    public GameObject ProjectilePrefab;
    public Animator anim;
    public Transform bullPoint;

    public Collider2D detectCollider;

    public Collider2D absorbBullet;
    public Collider2D PlayerStop;

    GameObject temp;

    public Component turb;

    AudioSource audio;
    public AudioClip shot;

    bool onTrigger;
    bool killSwitch;
    public float projectileVelo;

    public float waitTime;
    float timer;

    bool build;
    public float buildTimer;
    private float timer2;
    
	// Use this for initialization
	void Start () {
        health = maxxHealth;
        build = true;
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        anim.Play("TurrBuild");
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        timer2 += Time.deltaTime;

        if (buildTimer < timer2 && build == true)
        {
            build = false;
            anim.Play("idle");
        }



        if (killSwitch == false && build == false)
        {

            if (timer >= waitTime && onTrigger == true)
            {
                GameObject bullet = (GameObject)Instantiate(ProjectilePrefab, bullPoint.position, Quaternion.identity);
                Projectiles.Add(bullet);
                audio.PlayOneShot(shot);

                //                Debug.Log("SHOOT");

                timer = 0;
                //                Debug.Log(temp);

                //               Debug.Log("CHECK AREA");
                moveManager(temp);



            }

        }





        for (int i = 0; i < Projectiles.Count; i++)
        {
            GameObject goBullet = Projectiles[i];

            // Debug.Log("projectile counter" + " " + Projectiles.Count);

            if (goBullet != null)
            {
                goBullet.transform.Translate(new Vector3(-1, 0) * Time.deltaTime * projectileVelo);
                //  Physics.IgnoreCollision(goBullet.GetComponent<Collider>(), GetComponent<Collider>());

            }

        }






    }




    //    void OnTriggerEnter2D(Collider2D other)
    //    {



    //        if (other.gameObject.tag == "BadTank" ||
    //            other.gameObject.tag == "BadTankT2"||
    //            other.gameObject.tag == "BadTankT3")
    //        {
    ////            Debug.Log("ENEMY TANK DETECTED");

    //            onTrigger = true;
    //            temp = other.gameObject;

    //            DetectedEnemies.Add(temp);
    //            moveManager(temp);

    //        }

    //    }



    public Component ComponentScout()
    {
        turb = this.GetComponent<BadTurrentManager>();
        return turb;
    }


    public void moveManager(GameObject lol)
    {
        // Debug.Log(lol.GetComponent<EvilTankManager>().health);
 //       Debug.Log("~~~~In Move Manager~~~~");

 //       Debug.Log("lol: " + lol);

        if (lol == null)
        {
            Debug.Log("STOPfIRE");
            DestroyedEnemy(lol);


        }
        if (lol != null && lol.tag == ("BadTank"))
        {
            if (lol.GetComponent<EvilTankManager>().killSwitch == true)
            {
                DestroyedEnemy(lol);




            }
        }

        if (lol != null && lol.tag == ("BadTankT2"))
        {
            if (lol.GetComponent<Tier2EvilManager>().killSwitch == true)
            {
                DestroyedEnemy(lol);


            }
        }

        if (lol != null && lol.tag == ("BadTankT3"))
        {
            if (lol.GetComponent<eT3Manager>().killSwitch == true)
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
        onTrigger = false;
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
//            Debug.Log("FRIENDLY UNIT DESTROYED");
            if (stopHealth == false)
            {



                anim.Play("Expld");
                absorbBullet.enabled = false;
                PlayerStop.enabled = false;

                killSwitch = true;
                setHealthBar(calc_health);

                Destroy(gameObject, 4f);
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

        temp = temp1;
            hasTarget = true;
            onTrigger = true;

        moveManager(temp);


    }
}


