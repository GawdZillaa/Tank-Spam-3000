using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MULTI_CubeMove : NetworkBehaviour 
{
    GameObject UI1;

    Transform mainCamera;

    bool team1;
    bool team2;

    Vector3 xAx;
    bool startup;

    public float speed;
    public float factor;
    public float negFactor;

    public float PosBound;
    public float NegBound;

    float yKeep = -.77f;
    float positionTotal = 0;
    // Use this for initialization


    void DeterminePlacment()
    {
        if (xAx.x < 0)
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
    void Start()
    {
        checker = true;
 

        mainCamera = Camera.main.transform;
        moveCamera();
        startup = false;

        Startup();


    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!isLocalPlayer)
        {
            return;
                }


        if (positionTotal <= PosBound)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                positionTotal += (speed);


                transform.position = new Vector3(positionTotal, yKeep, 0);
 //                  Debug.Log("Movig cube: " + speed * factor);

            }
        }

        if (positionTotal >= NegBound)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                positionTotal += (-speed);
                transform.position = new Vector3(positionTotal, yKeep, 0);
//                    Debug.Log("Movig cube: " + speed * negFactor);

            }

        }

        moveCamera();
    }

    bool checker;
    void Update()
    {
        if (checker == true)
            {
            Debug.Log("isLocalPlayer == " + isLocalPlayer);

            checker = false;

        }
         if (!isLocalPlayer)
            return;


  




    }

    public override void OnStartLocalPlayer()
    {
        Debug.Log("OnStart");
        base.OnStartLocalPlayer();
        GetComponent<MeshRenderer>().material.color = Color.blue;
        Startup();
        DeterminePlacment();
        EnableDisplayPlayer1();

    }
    void Startup()
    {
        xAx = transform.position;
    }

    void EnableDisplayPlayer1()
    {
        Debug.Log("Team 1 Button Off....");

        if (team1 == true)
        {
            UI1.SetActive(false);
        }

        if (team2 == true)
        {
            UI1.SetActive(false);

        }
    }




    void moveCamera()
    {
        Vector3 temp = transform.position;
        temp.z -= 5;
        
        mainCamera.position = temp;
    }




}