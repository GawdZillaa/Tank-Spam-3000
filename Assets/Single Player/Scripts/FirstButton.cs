using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FirstButton : MonoBehaviour {

    public Button spawnTank; 


	// Use this for initialization
	void Start () {

        Button sTank = spawnTank.GetComponent<Button>();
        sTank.onClick.AddListener(TaskWhenClicked);

    }
	
	// Update is called once per frame
	void Update () {
	
	}



  void  TaskWhenClicked()
    {
        Debug.Log("BUTTON CLICKED");
    }
}
