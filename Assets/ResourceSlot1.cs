using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSlot1 : MonoBehaviour {

    public GameObject slot;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        slot.GetComponent<Renderer>().material.color = new Color(1, 1, 1, -100);

    }
}
