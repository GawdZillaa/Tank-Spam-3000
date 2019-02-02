using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class MULTI_PlayerSetup : NetworkBehaviour {
    GameObject UI1;

    bool team1;
    bool team2;

    Vector3 xAx;
    bool startup;

    // Use this for initialization
    void Start () {

        startup = false;

        Startup();

        if (xAx.x < 1)
        {
            Debug.Log("Team 2!");
            UI1 = GameObject.FindGameObjectWithTag("T1Gbutton");

            team2 = true;
            team1 = false;

        }

        if (xAx.x > 1)
        {
            Debug.Log("Team 1!");
            UI1 = GameObject.FindGameObjectWithTag("T1Bbutton");

            team2 = false;
            team1 = true;
        }


    }

    // Update is called once per frame
    void Update () {

        if (!isLocalPlayer)
        {
            return;
        }

        if (startup == false && team1 == true)
        {
            EnableDisplayPlayer1();
            startup = true;
            startup = true;

        }


        if (startup == false && team2 == true)
        {
            EnableDisplayPlayer1();
            startup = true;
            startup = true;

        }


    }


    void Startup()
    {
        xAx = transform.position;
    }

    void EnableDisplayPlayer1()
    {
        Debug.Log("Team 1 Button Off....");

        UI1.SetActive(false);
    }

    void EnableDisplayPlayer2()
    {
        Debug.Log("Team 2 Button Off....");

        UI1.SetActive(false);
    }


}
