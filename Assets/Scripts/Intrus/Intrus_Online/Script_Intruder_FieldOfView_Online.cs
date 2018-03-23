using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Characters.ThirdPerson;

public class Script_Intruder_FieldOfView_Online : MonoBehaviour
{
    public Script_Intruder_Online associateScript;

    [Header("FEAR")]
    public float fearOnSeeBourreau;
    public float fearOnSeeSpectre;
    public LayerMask seeingLayer;

    [Header("FEARCD")]
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
                other.GetComponent<Script_Possession_Online>().ReturnPlayer().GetComponent<Script_Spectre_Moves_Online>().AddFearToIntruder(fearOnSeeSpectre, transform.parent.parent.gameObject);
            } 
        }

        if (other.CompareTag("Bourreau") && associateScript.ReturnCurrentState().ToString("") != "Fleeing")
        {
            
            RaycastHit hit;
            Debug.DrawRay(transform.position, other.transform.position - transform.position, Color.green, 100f);
            //Debug.Log(transform.position);

            if (Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, 100f, seeingLayer))
            {
                Debug.Log("Je voie :" + hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Bourreau"))
                {
                    if (currentCDBourreau <= 0)
                    {
                        other.GetComponent<Script_Bourreau_Moves>().AddFearToIntruder(fearOnSeeBourreau, transform.parent.parent.gameObject);
                        associateScript.SeeSomething(other.gameObject);
                    } 
                    other.GetComponent<Script_Bourreau_Moves>().ActivateNavMashObstacle();
                    currentCDBourreau = timeBetweenBourreauSeen;
                }
            }
        }
    }


}
