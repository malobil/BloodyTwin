using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Bourreau_Attack_Trigger : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Script_Intruder>())
        {
            other.GetComponent<Script_Intruder>().IntruderDeath();
        }
    }
}
