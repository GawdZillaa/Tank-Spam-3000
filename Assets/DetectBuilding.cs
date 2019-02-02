using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBuilding : MonoBehaviour {

    bool BuildClerance = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
 //       Debug.Log(BuildClerance);
	}

    public bool DetectionState()
    {
        return BuildClerance;
    }

    List<string> Enemies = new List<string> { "BadTank", "BadTankT2", "BadTankT3" };


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Turr"||
            other.tag == "GoodResourceCollector" ||
            other.tag == "MyBullet")
        {
            Debug.Log("DETECTED: " + other);
            BuildClerance = false;

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Turr" ||
            other.tag == "GoodResourceCollector"||
            Enemies.Contains(other.gameObject.tag))
        {
            BuildClerance = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Turr" ||
            other.tag == "GoodResourceCollector" ||
            Enemies.Contains(other.gameObject.tag))
        {
//            Debug.Log("OVER SOMETING");
            BuildClerance = false;
        }
    }


}
