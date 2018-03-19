using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Bourreau_Attack_Trigger : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Script_Intruder_Online>())
        {
            other.GetComponent<Script_Intruder_Online>().IntruderDeath();
            Debug.Log("TOUCHED SOMEONE");
        }
    }
}
