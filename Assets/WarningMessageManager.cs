using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WarningMessageManager : MonoBehaviour {

    public GameObject Panel;
    public GameObject text;
    public GameObject image;

    public float MessageTimer;
    float time;
    bool MessageUp;

    public Text WarningText;
	// Use this for initialization
	void Start () {
        Panel.SetActive(false);
        text.SetActive(false);
        image.SetActive(false);
        MessageUp = false;
    }
	
	// Update is called once per frame
	void Update () {
        
		if (MessageUp == true)
        {
            time += Time.deltaTime;
           time = WarningTimer(time);
        }
	}

   public void RecieveWarning(string warning)
    {
        Debug.Log("RECIEVE MESSAGE.......");
        MessageUp = true;
        Panel.SetActive(true);
        text.SetActive(true);
        image.SetActive(true);

        WarningText.text = warning;
    }

    float WarningTimer(float time)
    {
        if (MessageTimer < time)
        {
            MessageUp = false;
            Panel.SetActive(false);
            text.SetActive(false);
            image.SetActive(false);
            WarningText.text = null;
            time = 0;
            return time;
        }
        return time;

    }
}
