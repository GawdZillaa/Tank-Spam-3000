using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MULTI_UIDisplay2 : NetworkBehaviour
{

    GameObject UI1;
    public GameObject UI2;

    public NetworkView UiView;

    bool startup;
    // Use this for initialization
    void Start()
    {
        startup = false;
        UiView = GetComponent<NetworkView>();

        UI1 = GameObject.FindGameObjectWithTag("T1Bbutton");
    }

    // Update is called once per frame
    void Update()
    {
      //  Debug.Log("IN TEAM 2 ....");

  //      if (!isLocalPlayer)
 //       {
 //           return;
 //       }

        if (startup == false)
        {
            EnableDisplay();
            startup = true;
        }
    }

    void EnableDisplay()
    {
        Debug.Log("Team 2 Button Off....");

        UI1.SetActive(false);
    }


}
