using UnityEngine;
using System.Collections.Generic;

public class ApplyDamage : MonoBehaviour {

    public int Damage;


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.name == "Blue Base")
        {
            Debug.Log(" was triggered");

            Destroy(gameObject);
        }

    }



}
