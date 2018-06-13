using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Script_Door : NetworkBehaviour {

    private Animator doorAnimator;
    private Animation doorAnimation;
    [SyncVar(hook = "DoorState")]
    private bool opened = false;

	// Use this for initialization
	void Start () {
        doorAnimator = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void DoorState (bool isOpening)
    {
        if(isOpening)
        {
            Debug.Log("DoorOpen");
            doorAnimator.SetTrigger("Open");
        }
        else if(!isOpening)
        {
            doorAnimator.SetTrigger("Close");
            Debug.Log("DoorClose");
        }
    }

    public void ChangeState ()
    {
        opened = !opened;
    }
}
