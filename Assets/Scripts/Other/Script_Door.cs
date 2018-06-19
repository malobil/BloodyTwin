using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Script_Door : NetworkBehaviour {

    private Animator doorAnimator;
    private Animation doorAnimation;
    [SyncVar(hook = "DoorState")]
    private bool opened = false;
    [SyncVar]
    private bool lockedDoor = false;

    [Header("Audio Door")]
	public AudioSource openDoor;
	public AudioSource closeDoor;
	public AudioSource doorIsLocked;
	public AudioSource lockingDoor;
	public AudioSource destroyDoor;

	// Use this for initialization
	void Start ()
    {
        doorAnimator = GetComponent<Animator>();
	}

    void DoorState (bool isOpening)
    {
        if(isOpening)
        {
            Debug.Log("DoorOpen");
            doorAnimator.SetTrigger("Open");
            openDoor.Play();
        }
        else if(!isOpening)
        {
            doorAnimator.SetTrigger("Close");
            Debug.Log("DoorClose");
        }
    }

    public void ChangeState ()
    {
        if(!lockedDoor)
        {
            Debug.Log("ChangeState");
            opened = !opened;
        }
        else
        {

        }
    }

    public void LockingDoor ()
    {
        lockedDoor = !lockedDoor;
        Debug.Log("Lock");
    }
}
