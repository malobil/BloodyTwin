using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Intruder_FieldOfView : MonoBehaviour

    

{
    public Script_Intruder associateScript;

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Touché");
            RaycastHit hit;
            Debug.DrawRay(transform.position,other.transform.position -transform.position,  Color.green, 100f);
            Debug.Log(transform.position);

            if (Physics.Raycast(transform.position, other.transform.position - transform.position,  out hit, 100f))
            {
                Debug.Log(hit.collider.gameObject.name);
                if(hit.collider.CompareTag("Player"))
                {
                    associateScript.SeeSomething(hit.collider.gameObject);
                }
            }
        }
    }


}
