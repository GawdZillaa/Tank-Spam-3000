using UnityEngine;
using System.Collections;

public class BulletDamage : MonoBehaviour
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
        if (other.gameObject.name == "Blue Base" && hitCheck == false)
        {
            //  deal =other.GetComponent<BlueBaseHealth>();
            hitCheck = true;
            other.gameObject.GetComponent<BlueBaseHealth>().takeDamage(Damage);
            Destroy(gameObject);

            Debug.Log("BULLET HIT");

        }
        if (other.gameObject.tag == "BadTank" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<EvilTankManager>().takeDamage(Damage);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "BadTankT2" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<Tier2EvilManager>().takeDamage(Damage);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "BadTankT3" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<eT3Manager>().takeDamage(Damage);
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "TurrBad" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<BadTurrentManager>().takeDamage(Damage, other.gameObject);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "BadResourceCollector" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<AiResourceExtractor>().takeDamage(Damage, other.gameObject);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "BaseDefenceB" && hitCheck == false)
        {
            hitCheck = true;
            other.gameObject.GetComponent<AlphaTurrentB>().takeDamage(Damage, other.gameObject);
            Destroy(gameObject);
        }
    }




}
