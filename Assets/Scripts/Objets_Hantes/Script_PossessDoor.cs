using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Script_PossessDoor : NetworkBehaviour {

    private bool isLock;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.A))
        {
            LockDoor();
        }
	}

    [ClientRpc]
    public void LockDoor ()
    {
        isLock = !isLock;
    }
}
