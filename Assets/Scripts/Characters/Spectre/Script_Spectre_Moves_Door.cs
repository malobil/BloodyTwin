using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Script_Spectre_Moves_Door : NetworkBehaviour {
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown("e"))
        {
            CmdDoor();
            Debug.Log("state");
        }
	}

    [Command]
    void CmdDoor()
    {
        GetComponent<Script_Door>().SpectreChangeState();
        Debug.Log("FERME LA PORTE ");
    }
}
