using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MULTI_UIDis1 : NetworkBehaviour {

     GameObject UI1;
    public GameObject UI2;

    public NetworkView UiView;
    bool startup;

    // Use this for initialization
    void Start () {
        startup = false;
        UiView = GetComponent<NetworkView>();

        UI1 = GameObject.FindGameObjectWithTag("T1Gbutton");
	}
	
	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer)
        {
            return;
        }
        if (startup == false)
        {
            EnableDisplay();
            startup = true;
        }
	}

    void EnableDisplay()
    {
        Debug.Log("Team 1 Button Off....");
        UI1.SetActive(false);
    }





}
