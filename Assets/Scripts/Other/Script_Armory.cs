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
        Sync();
        GetComponent<BoxCollider>().isTrigger = true;
    }

    void Sync()
    {
        myAnimator.GetComponent<NetworkAnimator>().SetTrigger("Open");
    }
}
