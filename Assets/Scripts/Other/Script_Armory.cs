using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Script_Armory : NetworkBehaviour {

    public Animator myAnimator;
    public GameObject door;

    [SyncVar(hook = "OpenDoor")]
    private bool isOpen = false;

    private AudioSource source;

	// Use this for initialization
	void Start ()
    {
        source = GetComponent<AudioSource>();
	}


	public void OpenArmory()
    {
        isOpen = true;
    }

    public void OpenDoor(bool toOpen)
    {
        myAnimator.SetBool("OpenArmory",toOpen);
        RpcPlayOpenSound();
    }

    [ClientRpc]
    void RpcPlayOpenSound()
    {
        door.layer = 2;
        source.Play();
        Destroy(this);
    }
}
