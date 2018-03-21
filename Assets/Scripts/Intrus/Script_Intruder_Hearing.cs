using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Intruder_Hearing : MonoBehaviour
{
    public Script_Intruder associateScript;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entendu");
            associateScript.HearSomething(other.gameObject);
        }
    }
}
