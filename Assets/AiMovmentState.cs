using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMovmentState : MonoBehaviour {

    private bool isAuto;
    private bool FrontlineUp;
    private Vector3 InitialPosition;
    private Vector3 FrontlinePosition;

    public GameObject SpawnPointPos;

    // Use this for initialization
    void Start () {
        isAuto = false;
        FrontlineUp = false;

        FrontlinePosition = SpawnPointPos.transform.position;
        InitialPosition = FrontlinePosition;

        Debug.Log("FRONTLINE STARTUP: " + FrontlinePosition);
	}
	
	// Update is called once per frame
	void Update () {
       //  if ()
		
	}


    public void ChangeMovmentState(bool Auto)
    {
        isAuto = Auto;
    }

    public bool RequestMovmentType()
    {
        return isAuto;
    }

    public Vector3 RequestFrontlinePosition()
    {
        return FrontlinePosition;
    }

    public void UpdateFrontLine(Vector3 NewFrontline)
    {
        Debug.Log("Movment Manager Updated");
//        Debug.Log("Old Front Line: " + FrontlinePosition);
        FrontlinePosition = NewFrontline;
//        Debug.Log("New Frontline: " + FrontlinePosition);
        if (FrontlinePosition != InitialPosition)
        {
            FrontlineUp = true;
        }
    }


}
