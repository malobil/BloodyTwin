using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Script_Armory : NetworkBehaviour {

    public Animator myAnimator;

    [SyncVar(hook = "OpenDoor")]
    private bool isOpen = false;

	// Use this for initialization
	void Start ()
    {
        
	}

	public void OpenArmory()
    {
        isOpen = true;
        /*myAnimator.SetTrigger("Open");
        myAnimator.GetComponent<NetworkAnimator>().SetTrigger("Open");
        GetComponent<BoxCollider>().isTrigger = true;*/
    }

    public void OpenDoor(bool toOpen)
    {
        myAnimator.SetBool("OpenArmory",toOpen);
    }

}
