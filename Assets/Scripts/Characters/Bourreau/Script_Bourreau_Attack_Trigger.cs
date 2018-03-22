using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Bourreau_Attack_Trigger : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Intru"))
        {
            other.transform.parent.GetComponent<Script_Intruder_Online>().IntruderDeath();
            Debug.Log("TOUCHED SOMEONE");
        }
    }
}
