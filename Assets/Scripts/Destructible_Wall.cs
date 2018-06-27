using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Destructible_Wall : MonoBehaviour 
{

public GameObject boom;
public Transform boomPosition;
public GameObject wall;
public AudioSource doorDestroyed;

 public void DestroyWall()
 {
    GameObject objectToSpawn = Instantiate(boom, boomPosition.position , Quaternion.identity);
    NetworkServer.Spawn(objectToSpawn);
    doorDestroyed.Play();
 	NetworkServer.Destroy(wall);
 	//Debug.Log("tg2");
 }

}
