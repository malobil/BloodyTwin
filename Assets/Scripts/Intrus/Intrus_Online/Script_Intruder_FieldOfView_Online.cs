using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Characters.ThirdPerson;

public class Script_Intruder_FieldOfView_Online : MonoBehaviour

    

{
    public Script_Intruder_Online associateScript;

    public float timeBetweenObjectSeen = 5f;
    private float currentCDObject;

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
        if(other.GetComponent<Script_Spectre_Possess_Move_Online>() && other.GetComponent<Script_Spectre_Possess_Move_Online>().ReturnIsMoving() && currentCDObject <= 0)
        {
            Debug.Log("ON SEE");
            associateScript.FearedImpact(10f);
            currentCDObject = timeBetweenObjectSeen;
        }

        if (other.GetComponent<Script_Bourreau_Moves>() && currentCDBourreau <= 0)
        {
            // Debug.Log("Touché");
            RaycastHit hit;
            Debug.DrawRay(transform.position, other.transform.position - transform.position, Color.green, 100f);
            //Debug.Log(transform.position);

            if (Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, 100f))
            {
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Player"))
                {
                    associateScript.SeeSomething(hit.collider.gameObject);
                    associateScript.FearedImpact(50f);
                    currentCDBourreau = timeBetweenBourreauSeen;
                }
            }
        }
    }


}
