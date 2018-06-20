using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Spectre_Moves_Door : MonoBehaviour {
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown("e"))
        {
            GetComponent<Script_Door>().SpectreChangeState();
            Debug.Log("state");
        }
	}
}
