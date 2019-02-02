using UnityEngine;
using System.Collections;

public class BulletDamageE : MonoBehaviour
{


    public int Damage;
    GameObject deal;
    bool hitCheck = false;


    public Animator BullMove;

    void Start()
    {
        if (BullMove != null)
        {
            BullMove.GetComponent<Animator>();

            BullMove.Play("Ggo");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.name == "Red Base" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<RedBase>().takeDamage(Damage);
            Destroy(gameObject);
 //           Debug.Log("BULLET HIT");

        }

        if (other.gameObject.tag == "Tank" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<Movment>().takeDamage(Damage, other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Tank2" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<GoodTankR2Manager>().takeDamage(Damage, other.gameObject);
            Destroy(gameObject);

        }

        if (other.gameObject.tag == "Tank3" && hitCheck == false)
        {

            hitCheck = true;
            other.gameObject.GetComponent<T3GoodManager>().takeDamage(Damage, other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Tank4" && hitCheck == false)
        {

            hitCheck = true;
            other.gameObject.GetComponent<SniperTankManager>().takeDamage(Damage, other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "GoodResourceCollector" && hitCheck == false)
        {

            hitCheck = true;
            other.gameObject.GetComponent<ResourceExtractor>().takeDamage(Damage, other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Gresource" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<ResourceBuilderGoodManager>().takeDamage(Damage, other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Turr" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<TurrentManager>().takeDamage(Damage, other.gameObject);
            Destroy(gameObject);

        }


        if (other.gameObject.tag == "TankShG" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<ShieldTankG>().takeDamage(Damage, other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "ShGs" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<ShieldScript>().takeDamage(Damage, other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "MechG" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<MechAR_G_Manager>().takeDamage(Damage, other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "BaseDefenceG" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<MechAR_G_Manager>().takeDamage(Damage, other.gameObject);
            Destroy(gameObject);
        }



    }
}

