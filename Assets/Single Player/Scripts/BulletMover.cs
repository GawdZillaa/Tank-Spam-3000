using UnityEngine;
using System.Collections;

public class BulletMover : MonoBehaviour {
    public float speed;

    // Use this for initialization
    void Start () {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;

    }


}
