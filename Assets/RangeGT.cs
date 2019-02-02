using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeGT : MonoBehaviour {

    public GameObject Commander;
    List<string> Enemies = new List<string> { "BadTank", "BadTankT2", "BadTankT3", "TurrBad", "BadResourceCollector" };


    GameObject tempHq;
    GameObject temp;


    bool aftermathCheck;
    bool first;
    ComponentFind script;

    // Use this for initialization
    void Start()
    {
        script = Commander.GetComponent<ComponentFind>();

    }

    // Update is called once per frame
    void Update()
    {


    }


    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.name == "Stop Tank G")
        {
            Debug.Log(" was triggered");
            //            tempHq = other.gameObject;
            temp = other.gameObject;
            script.SetStop(temp, tempHq, 0);

        }


        if (Enemies.Contains(other.tag))
        {
            //           Debug.Log("ENEMY TANK DETECTED");

 //           Debug.Log("Target pso: " + other.transform.position.x);
 //           Debug.Log("Detect Pos: " + FinalDetect.transform.position.x);
//            if (other.transform.position.x > FinalDetect.transform.position.x)
  //          {
                temp = other.gameObject;
                //           Debug.Log(script);

                if (script != null)
                {
                    script.SetStop(temp, tempHq, 0);
                }
//            }

        }

    }



}
