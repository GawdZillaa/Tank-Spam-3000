using UnityEngine;
using System.Collections;

public class cubemove : MonoBehaviour
{

    public float speed;
    public float factor;
    public float negFactor;

    public float PosBound;
    public float NegBound;

    float yKeep = -.77f;
    float positionTotal = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
//        Debug.Log(positionTotal);
        if (positionTotal <= PosBound)
        {
            if (Input.GetKey(KeyCode.D))
            {
                positionTotal += (speed);


                transform.position = new Vector3(positionTotal, yKeep, 0);
             //   Debug.Log("Movig cube: " + speed * factor);

            }
        }

        if (positionTotal >= NegBound)
        {
            if (Input.GetKey(KeyCode.A))
            {
                positionTotal += (-speed);
                transform.position = new Vector3(positionTotal, yKeep, 0);
            //    Debug.Log("Movig cube: " + speed * negFactor);

            }

        }
    }
}


