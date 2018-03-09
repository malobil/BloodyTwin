using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;

public class Script_Bourreau_Camera : NetworkBehaviour {

    public GameObject cameraToInstantiate;

    public override void OnStartLocalPlayer()
    {
        GameObject cameraI = Instantiate(cameraToInstantiate, transform.position, transform.rotation);
        cameraI.GetComponent<FreeLookCam>().SetCamera(transform);
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
