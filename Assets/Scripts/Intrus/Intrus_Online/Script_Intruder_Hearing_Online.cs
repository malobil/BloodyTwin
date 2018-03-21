using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Script_Intruder_Hearing_Online : MonoBehaviour
{
    public Script_Intruder_Online associateScript;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Script_Bourreau_Moves>())
        {
            Debug.Log("Entendu");
            associateScript.HearSomething(other.gameObject);
        }
    }
}
