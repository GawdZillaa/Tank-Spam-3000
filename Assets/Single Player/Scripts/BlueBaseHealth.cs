using UnityEngine;
using System.Collections;

public class BlueBaseHealth : MonoBehaviour {

    public float maxxHealth = 100;
    public float health;
    bool stopHealth = false;
    public GameObject BarHealth;

    public GameObject GameOver;

    void Start()
    {
        GameOver = GameObject.FindGameObjectWithTag("Display");
    }

    void Awake ()
    {
        health = maxxHealth;
    }





   public void takeDamage (int amount)
    {
        health -= amount;
        float calc_health = health / maxxHealth;



        Debug.Log("Blue Base Damaged!" + " " + health);
        if (health <= 0)
        {
            Debug.Log("BLUE BASE DESTROYED");
            if (stopHealth == false)
            {
                
                setHealthBar(calc_health);
                Destroy(gameObject);
                stopHealth = true;

                GameOver.GetComponent<GameOverManager>().EndGame(1);
            }

            Die();
        }
        else
        {
            setHealthBar(calc_health);
        }


    }

    void Die ()
    {

    }


  public void setHealthBar(float health)
    {
        BarHealth.transform.localScale = new Vector3(health, BarHealth.transform.localScale.y, 
        BarHealth.transform.localScale.z);
    }

}
