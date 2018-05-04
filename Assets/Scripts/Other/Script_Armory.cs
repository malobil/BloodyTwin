using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Armory : MonoBehaviour {

    public Animator myAnimator;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	public void OpenArmory()
    {
        myAnimator.SetTrigger("Open");
    }
}
