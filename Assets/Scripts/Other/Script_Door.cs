using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Script_Door : NetworkBehaviour {

    private Animator doorAnimator;
    private Animation doorAnimation;
    [SyncVar(hook = "DoorState")]
    private bool opened = false;
    [SyncVar(hook = "RpcLock")]
    private bool lockedDoor = false;

    [Header("Door Audio")]
	public AudioSource doorSoundPlayer;
    public AudioClip[] doorSounds;
 

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
            PlayDoorSound(0);
        }
        else if(!isOpening)
        {
            doorAnimator.SetTrigger("Close");
            Debug.Log("DoorClose");
            PlayDoorSound(1);
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
            PlayDoorSound(2);
        }
    }

    public void SpectreChangeState()
    {
        opened = !opened;
    }

    public void OpeningDoor ()
    {
        PlayDoorSound(3);
    }

    public void ClosingDoor()
    {
        PlayDoorSound(4);
    }

    public void LockingDoor ()
    {
        lockedDoor = !lockedDoor;
        Debug.Log("Lock");
    }

    [ClientRpc]
    void RpcLock(bool state)
    {
        lockedDoor = state;
    }

    public void PlayDoorSound (int idx)
    {
        doorSoundPlayer.PlayOneShot(doorSounds[idx]);
    }
}
