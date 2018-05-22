using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Script_Armory : NetworkBehaviour {

    public Animator myAnimator;

	// Use this for initialization
	void Start ()
    {
        
	}

	public void OpenArmory()
    {
        myAnimator.SetTrigger("Open");
        myAnimator.GetComponent<NetworkAnimator>().SetTrigger("Open");
        GetComponent<BoxCollider>().isTrigger = true;
        
    }
}
