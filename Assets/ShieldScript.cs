using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour {
    public GameObject shield;
    public GameObject ShieldImage;
    public Collider2D ShieldCollider;

    public GameObject Deflector;

    public GameObject Commander;

    public float MaxShield;
    private float ShieldHealth;
    bool stopHealth = false;
    bool takingDamage;
    bool ShieldDown;
    float timer;

    public float RechargeTempo;

    public float healTimer;
    float resetHeal;

    public GameObject BarHealth;

    // Use this for initialization
    void Start () {
        Deflector.SetActive(false);
        ShieldHealth = MaxShield;
        resetHeal = 0;
        timer = 0;
        ShieldDown = false;

        Commander.GetComponent<ShieldTankG>().Shield_isOn();

    }
	
	// Update is called once per frame
	void Update () {

        resetHeal -= Time.deltaTime;
        timer += Time.deltaTime;

		if (takingDamage == true)
        {
            Deflector.SetActive(true);
            resetHeal = healTimer;
            takingDamage = false;
        }

        else 
        {
            Deflector.SetActive(false);
        }

        

        if (ShieldHealth < MaxShield - 100 &&ShieldDown != true)
        {
            if (resetHeal < 0 || resetHeal == 0)
            {
                if (timer > RechargeTempo)
                {
                    ShieldHealth += 100;
                    float calc_health = ShieldHealth / MaxShield;

                    setHealthBar(calc_health);

                    timer = 0;
                }
            }
        }


        if (ShieldDown ==true)
        {
            if (timer > RechargeTempo)
            {
                ShieldHealth += 300;
                float calc_health = ShieldHealth / MaxShield;
                Debug.Log(ShieldHealth);

                setHealthBar(calc_health);
                timer = 0;
                if (ShieldHealth > MaxShield/2 ||
                    ShieldHealth == MaxShield/2)
                {
                    Commander.GetComponent<ShieldTankG>().Shield_isOn();

                    ShieldImage.SetActive(true);
                    ShieldCollider.enabled = true;
                    ShieldDown = false;
                    stopHealth = false;
                }
            }
        }

	}



    public void takeDamage(int amount, GameObject lol)
    {
        ShieldHealth -= amount;
        float calc_health = ShieldHealth / MaxShield;
        takingDamage = true;


        //        Debug.Log("friendly Unit Damaged!" + " " + health);
        if (ShieldHealth <= 0 || ShieldHealth == 0)
        {
 //           Debug.Log("FRIENDLY UNIT DESTROYED");
            if (stopHealth == false)
            {



                //     shield.SetActive(false);
                Debug.Log("Shields Disabled!!!!!!!");

                Commander.GetComponent<ShieldTankG>().Shield_isOff();
                ShieldImage.SetActive(false);
                ShieldCollider.enabled = false;

                setHealthBar(calc_health);
                ShieldDown = true;
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
}
