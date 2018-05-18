using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters;

public class Script_Bourreau_Attack_Trigger : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Intru"))
        {
            other.transform.parent.GetComponent<Script_Player_Moves>().Die();
            Debug.Log("TOUCHED SOMEONE");
        }
    }
}
