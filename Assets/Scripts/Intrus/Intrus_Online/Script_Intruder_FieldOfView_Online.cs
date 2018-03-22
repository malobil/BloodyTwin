﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Characters.ThirdPerson;

public class Script_Intruder_FieldOfView_Online : MonoBehaviour
{
    public Script_Intruder_Online associateScript;

    public float timeBetweenObjectSeen = 5f;
    private float currentCDObject;
    private bool canSee = true;

    public float timeBetweenBourreauSeen = 5f;
    private float currentCDBourreau;

    private void Update()
    {
        if(currentCDObject > 0)
        {
            currentCDObject -= Time.deltaTime;
        }

        if(currentCDBourreau > 0)
        {
            currentCDBourreau -= Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Possess") && currentCDObject<=0)
        {
            if(other.GetComponent<Script_Spectre_Possess_Move_Online>().ReturnIsMoving())
            {
                Debug.Log("ON SEE");
                associateScript.SeeSomething(other.gameObject);
                currentCDObject = timeBetweenObjectSeen;
                //Debug.Log(GameObject.FindGameObjectWithTag("Spectre"));
                other.GetComponent<Script_Possession_Online>().ReturnPlayer().GetComponent<Script_Spectre_Moves_Online>().AddFearToIntruder(10f, transform.parent.parent.gameObject);
            } 
        }

        if (other.CompareTag("Bourreau") && currentCDBourreau <= 0 && associateScript.ReturnCurrentState().ToString("") != "Fleeing")
        {
            
            RaycastHit hit;
            Debug.DrawRay(transform.position, other.transform.position - transform.position, Color.green, 100f);
            //Debug.Log(transform.position);

            if (Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, 100f))
            {
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Bourreau"))
                {
                    other.GetComponent<Script_Bourreau_Moves>().AddFearToIntruder(50f,transform.parent.parent.gameObject);
                    other.GetComponent<Script_Bourreau_Moves>().ActivateNavMashObstacle();
                    associateScript.SeeSomething(other.gameObject);
                    currentCDBourreau = timeBetweenBourreauSeen;
                }
            }
        }
    }


}
