using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine;

public class GameOverManager : MonoBehaviour {

    public GameObject GameOverPanel;

    public Text VictoryStatus;

    public GameObject getPlayerEcon;
    public GameObject getAiEcon;

    public Text AiT1;
    public Text AiT2;
    public Text AiT3;
    public Text AiKlls;
    public Text AiEcon;
    public Text AiTotatM;

    public Text PT1;
    public Text PT2;
    public Text PT3;
    public Text PKlls;
    public Text PEcon;
    public Text PTotatM;

    int at1_kill = 0;
    int at2_kill = 0;
    int at3_kill = 0;
    int at1_Enlist = 0;
    int at2_Enlist = 0;
    int at3_Enlist = 0;
    int aTotal_kills = 0;
    int a_econ = 0;
    int aTotal_military = 0;

    int pt1_kill = 0;
    int pt2_kill = 0;
    int pt3_kill = 0;
    int pt1_Enlist = 0;
    int pt2_Enlist = 0;
    int pt3_Enlist = 0;
    int pTotal_kills = 0;
    int p_econ = 0;
    int pTotal_military = 0;


    int Victory;
    // Use this for initialization
    void Start()
    {
        GameOverPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {


    }


    public void EndGame(int VictoryType)
    {
        int Akill = 0;
        int AmiL = 0;
        int Pkill = 0;
        int Pmil = 0;

        switch (VictoryType)
        {
            case 0:
                {
                    //Lose
                    Time.timeScale = 0;
                    GameOverPanel.SetActive(true);
                    VictoryStatus.text = "EPIC DEFEAT";

                    Akill = (at1_kill + at2_kill + at3_kill);
                    AmiL = (at1_Enlist + at2_Enlist + at3_Enlist);

                    AiKlls.text = "Kills: " + Akill;
                    AiTotatM.text = "Army Size: " + AmiL;
                    break;
                }
            case 1:
                {   //Win
                    Time.timeScale = 0;

                    GameOverPanel.SetActive(true);
                    VictoryStatus.text = "EPIC VICTORY";

                    Pkill = (pt1_kill + pt2_kill + pt3_kill);
                    Pmil = (pt1_Enlist + pt2_Enlist + pt3_Enlist);

                    AiKlls.text = "Kills: " + Pkill;
                    PTotatM.text = "Army Size: " + Pmil;
                    break;
                }
        }
    }

    public void AiRegisterKill(int Type)
    {
        switch (Type)
        {
            case 1:
                {
                    at1_kill++;
                    break;
                }
            case 2:
                {
                    at2_kill++;
                    break;
                }
            case 3:
                {
                    at3_kill++;
                    break;
                }
        }
    }

    public void PlayerRegisterKill(int Type)
    {
        switch (Type)
        {
            case 1:
                {
                    pt1_kill++;
                    break;
                }
            case 2:
                {
                    pt2_kill++;
                    break;
                }
            case 3:
                {
                    pt3_kill++;
                    break;
                }
        }
    }




    public void AiRegisterArmy(int Type)
    {
        switch (Type)
        {
            case 1:
                {
                    at1_Enlist++;
                    break;
                }
            case 2:
                {
                    at2_Enlist++;
                    break;
                }
            case 3:
                {
                    at3_Enlist++;
                    break;
                }
        }
    }

    public void PlayerRegisterArmy(int Type)
    {
        switch (Type)
        {
            case 1:
                {
                    pt1_Enlist++;
                    break;
                }
            case 2:
                {
                    pt2_Enlist++;
                    break;
                }
            case 3:
                {
                    pt3_Enlist++;
                    break;
                }
        }
    }

    void getAiEconomy()
    {
        a_econ = getAiEcon.GetComponent<AiReources>().BaseEconomy;
    }

    void getPlayerEconomy()
    {
        p_econ = getPlayerEcon.GetComponent<ResourceManagerG>().BaseEconomy;
    }
}
