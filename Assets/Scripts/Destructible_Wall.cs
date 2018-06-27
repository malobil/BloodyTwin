using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Destructible_Wall : NetworkBehaviour 
{

public GameObject boom;
public Transform boomPosition;
public GameObject wall;
public AudioSource doorDestroyed;

 public void DestroyWall()
 {
    RpcCheckPossess();
    GameObject objectToSpawn = Instantiate(boom, boomPosition.position , Quaternion.identity);
    NetworkServer.Spawn(objectToSpawn);
    doorDestroyed.Play();
 	NetworkServer.Destroy(wall);
 }
    [ClientRpc]
    void RpcCheckPossess()
    {
        if(GetComponent<Script_Spectre_Moves_Door>())
        {
            if(GetComponent<Script_Spectre_Moves_Door>().enabled)
            {
                GetComponent<Script_Possession_Online>().UnPossessObject();
            }
        }
    }

}
