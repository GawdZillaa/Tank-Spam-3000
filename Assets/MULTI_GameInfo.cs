using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MULTI_GameInfo : MonoBehaviour {

    struct playerInfo
    {
       public int NetworkPlayer;
        public NetworkConnection connect;
       public int Team;
    }

    playerInfo[] infoPlayers = new playerInfo[2];


    public void SetPlayerInfo(int playId, int team,int index, NetworkConnection conn)
    {
        infoPlayers[index].NetworkPlayer = playId;
        infoPlayers[index].Team = team;
        infoPlayers[index].connect = conn;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
