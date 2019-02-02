using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MULTI_CameraManager : MonoBehaviour {

    public GameObject camPoint;

    private Vector3 offset;
    // Use this for initialization


    public float zoomSpeed = 1;
    public float targetOrtho;
    public float smoothSpeed = 5f;
    public float minOrtho = 1.0f;
    public float maxOrtho = 13.0f;

    bool setScroll;

    bool scrollCheck = false;
    GameObject turrentPlace;
    void Start()
    {
        //offset = transform.position - camPoint.transform.position;
        //targetOrtho = Camera.main.orthographicSize;
        //setScroll = false;

    }

    // Update is called once per frame
    void LateUpdate()
    {

    }

    void Update()
    {

        //float scroll = Input.GetAxis("Mouse ScrollWheel");
        ////        Debug.Log(scroll);
        //if (scroll != 0.0f && scrollCheck == false)
        //{

        //    targetOrtho -= scroll * zoomSpeed;
        //    targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        //    setScroll = false;


        //}

        //if (scrollCheck == true)
        //{

        //    if (setScroll == false)
        //    {
        //        Debug.Log("IN()()()()()()()()()()()()()(");

        //        Debug.Log(targetOrtho);

        //        targetOrtho = 3.031f;
        //        targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        //      setScroll = true;
        //  }

        //}

        //        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
    }


    public void stopScroll(bool recieve)
    {
        scrollCheck = recieve;
    }

}