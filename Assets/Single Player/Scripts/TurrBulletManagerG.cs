using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrBulletManagerG : MonoBehaviour {

    public Animator anim;
    float animTimer = 2f;
    float timeKeep;
    bool starter = true;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        anim.Play("BulletAnim");

    }

    // Update is called once per frame
    void Update () {
        timeKeep += Time.deltaTime;

   //     Debug.Log(timeKeep);
        if (timeKeep > animTimer && starter == true)
        {
//            Debug.Log("PLAYYYYYYYYYYYYYY");
            anim.Play("Reg");

            starter = false;
        }

    }

    void startUp()
    {        

    }
}
