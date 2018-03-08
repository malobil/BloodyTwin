using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spam_Right_Character : MonoBehaviour {

	public GameObject spectre;
	public GameObject bourreau;
	private bool spectreOk = true;
	private bool bourreauOk = false;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void SpawnSpectre()
	{
		if(spectreOk = true)
		{
			Instantiate(spectre, new Vector3(0,0,0), Quaternion.identity);
			spectreOk = false;
		}
	}

	public void SpawnBourreau()
	{
		if(bourreauOk = true)
		{
			Instantiate(spectre, new Vector3(5,0,5), Quaternion.identity);
			bourreauOk = false;
		}
	}
}
