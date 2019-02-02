using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeG : MonoBehaviour
{

    public GameObject Commander;


    GameObject tempHq;
    GameObject temp;

    bool aftermathCheck;
    bool first;
    ComponentFind script;

    // Use this for initialization
    void Start()
    {
        aftermathCheck = false;
        first = false;
        script = Commander.GetComponent<ComponentFind>();

    }

    // Update is called once per frame
    void Update()
    {
        if (aftermathCheck == true)
        {

        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.name == "Stop Tank G")
        {
            Debug.Log(" was triggered");
//            tempHq = other.gameObject;
            temp = other.gameObject;
            script.SetStop(temp, tempHq, 0);

        }


        if (other.gameObject.tag == "BadTank" ||
            other.gameObject.tag == "BadTankT2" ||
            other.gameObject.tag == "BadTankT3" ||
            other.gameObject.tag == "TurrBad" ||
            other.gameObject.tag == "BadResourceCollector" ||
            other.gameObject.tag == "BaseDefenceB")
        {
 //           Debug.Log("ENEMY TANK DETECTED");
            temp = other.gameObject;
 //           Debug.Log(script);
       
                script.SetStop(temp, tempHq, 0);
            

        }

    }



}