using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters;

public class Script_Bourreau_Attack_Trigger : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Script_Player_Moves>())
        {
            Debug.Log("TOUCHED SOMEONE : " + other.gameObject.name);
            other.GetComponent<Script_Player_Moves>().Die();
            
        }

        if(other.gameObject.GetComponent<Destructible_Wall>()) //destructible wall
        {
        	//Debug.Log("tg");
        	other.GetComponent<Destructible_Wall>().DestroyWall();
        }
    }
}
