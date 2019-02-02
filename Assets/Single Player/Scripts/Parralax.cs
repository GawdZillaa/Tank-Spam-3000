using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralax : MonoBehaviour {


    public Transform[] background;
    private float[] parralaxScale;
    public float smoothing;

    private Transform cameraX;
    private Vector3 previousCamPos;

   public GameObject LOL;
    Transform follow;


	// Use this for initialization
	void Start () {

        cameraX = Camera.main.transform;

        follow = LOL.transform;
        previousCamPos = LOL.transform.position;

        parralaxScale = new float[background.Length];
        
        for (int i = 0; i < background.Length; i++)
        {
            parralaxScale[i] = background[i].position.z * -1;
        }  
	}
	
	// Update is called once per frame
	void LateUpdate () {

       // Debug.Log(previousCamPos);
        for (int i = 0; i < background.Length; i++)
        {

//            Debug.Log("Scale : " + parralaxScale[i]);
//            Debug.Log("Prev: " + previousCamPos.x + " Curr: " + cameraX.position.x);

            float parralax = (previousCamPos.x - LOL.transform.position.x) * parralaxScale[i];
//            Debug.Log("Parralax: " + parralax);

            float bacgroundTargetPosX = background[i].position.x + parralax;

            Vector3 backgroundTargetPosition = new Vector3(bacgroundTargetPosX, background[i].position.y, background[i].position.z);

            background[i].position = Vector3.Lerp(background[i].position, backgroundTargetPosition, smoothing * Time.deltaTime);

 //           Debug.Log(background[i].position);


        }


        previousCamPos = LOL.transform.position;
	}
}
