using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Destructible_Wall : MonoBehaviour 
{

public ParticleSystem boom;
public Transform boomPosition;
public GameObject wall;

 public void DestroyWall()
 {
	Instantiate(boom, boomPosition);
 	NetworkServer.Destroy(wall);
 	Debug.Log("tg2");
 }

}
