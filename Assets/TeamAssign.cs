using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TeamAssign : NetworkManager
{
    public GameObject spawn1;
    public GameObject spawn2;

    public GameObject point1;
    public GameObject point2;

   public GameObject buttonSet1;
    public GameObject buttonSet2;

    NetworkPlayer play;

    NetworkPlayer[] playersInGame;


    public GameObject setP_Info;

    GameObject positionPlayer;

    bool firstPlayer;
    bool secondPlayer;

    void Start()
    {
        setP_Info = GameObject.FindGameObjectWithTag("NetMan");
        Debug.Log(setP_Info);
        firstPlayer = false;
        secondPlayer = false;
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (firstPlayer == false)
        {
            
            positionPlayer = (GameObject)Instantiate(spawn1, point1.transform.position, Quaternion.identity);
            setP_Info.GetComponent<MULTI_GameInfo>().SetPlayerInfo(playerControllerId, 1, 0, conn);
            NetworkServer.AddPlayerForConnection(conn, positionPlayer, playerControllerId);

                firstPlayer = true;

            Debug.Log(playersInGame);
             Debug.Log("PLAYER 1 ADDED!!!");

        }
        else
        {
            GameObject player2 = (GameObject)Instantiate(spawn2, point2.transform.position, Quaternion.identity);
            setP_Info.GetComponent<MULTI_GameInfo>().SetPlayerInfo(playerControllerId, 2, 1, conn);
            NetworkServer.AddPlayerForConnection(conn, player2, playerControllerId);
            secondPlayer = true;
           // Debug.Log(Network.player.ToString());

            Debug.Log("PLAYER 2 ADDED!!!");

        }

    //    NetworkServer.AddPlayerForConnection(conn, positionPlayer, playerControllerId);

       // Debug.Log();

    }


    void OnConnectedToServer()
    {
        print("Player ID is " + Network.player.ToString());
    }




}