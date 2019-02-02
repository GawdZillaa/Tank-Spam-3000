using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class MULTI_InitialP1 : NetworkBehaviour {


    GameObject toSpawn;

public void Init(GameObject go)
    {
        CmdStart(go);
    }


    [Command]
    public void CmdStart(GameObject obj)
    {
        NetworkServer.SpawnWithClientAuthority(obj, connectionToClient);
    }
}
