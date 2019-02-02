using UnityEngine;
using System.Collections;

public class CharacterMovment : MonoBehaviour {
    public static int movespeed = 5;
    public Vector3 userDirection = Vector3.left;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(userDirection * movespeed *Time.deltaTime) ;
	}
}
