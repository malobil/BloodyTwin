﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;

public class Script_Spawn_Camera_Online : NetworkBehaviour {

    public GameObject cameraToInstantiate;

    public override void OnStartLocalPlayer()
    {
        GameObject cameraI = Instantiate(cameraToInstantiate, transform.position, transform.rotation);
        cameraI.GetComponent<FreeLookCam>().SetCamera(transform);
        if(GetComponent<Script_Spectre_Moves_Online>() != null)
        {
            GetComponent<Script_Spectre_Moves_Online>().SettingCamera(cameraI);
        }
        cameraI.tag = "MainCamera" ;
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
