using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeB : MonoBehaviour {

    public GameObject Commander;

    List<string> Enemies = new List<string> { "Tank", "Tank2", "Tank3", "Gresource", "GoodResourceCollector",
                                                "Turr", "Tank4", "TankShG","ShGs","MechG", "BaseDefenceG" };

    GameObject tempHq;
    GameObject temp;



    public Collider detector;

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


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.name == "Stop Tank B")
        {
            Debug.Log(" was triggered");
            tempHq = other.gameObject;
            temp = null;
            script.SetStop(temp, tempHq, 0);

        }


        if (Enemies.Contains(other.gameObject.tag)) 
        {
            Debug.Log("ENEMY TANK DETECTED");
            temp = other.gameObject;


            
            Debug.Log(temp);

            //           Debug.Log(script);

            if (script != null)
            {
                script.SetStop(temp, tempHq, 0);
            }
            
        }

    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (Enemies.Contains(other.gameObject.tag))
        {
            Debug.Log("??????????EXITING??????????");
            Debug.Log(temp);
            Debug.Log(other.gameObject);
            temp = other.gameObject;
            script.SetStop(temp, tempHq, 1);
        }
        
    }


}
