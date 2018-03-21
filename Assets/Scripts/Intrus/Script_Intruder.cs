using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Script_Intruder : MonoBehaviour
{
    public float fearAdd;
    public float fearRemoved;

    public int intruderNum;
    public Image fearLevel;
    public float currentFear;

    // AI //

    public enum IntruderState { Armed, Composed, Scared, Panic };
    public IntruderState currentState;

    private GameObject objectSeen;
    private GameObject objectHeard;

    public float smoothness;
    public Transform body;

    public NavMeshAgent navMeshAI;
    public Transform waypoint;
    private Vector3 moveTo;

    // Use this for initialization
    public void Start()
    {
        ChooseRandomPoint(); 
      //navMeshAI.SetDestination(waypoint.position);
        Script_Global_Fear.Instance.IntruderAmount();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("f"))
        {
            FearedImpact(fearAdd);
        }

        if (Input.GetKeyDown("g"))
        {
            FearedImpact(fearRemoved);
        }


    
    }

    public void IntruderDeath()
    {
        Destroy(this.gameObject);
    }

    public void FearedImpact(float fearState)
    {

        currentFear += fearState;

        if (currentFear <= 20)
        {
            currentState = IntruderState.Composed;

        }
        else if (currentFear <= 75)
        {
            currentState = IntruderState.Scared;
        }
        else
        {
            currentState = IntruderState.Panic;

        }

        if (currentFear >= 100)
        {
            currentFear = 100;
        }
        else if (currentFear <= 0)
        {
            currentFear = 0;
        }

        Script_Global_Fear.Instance.FearGlobalState();
        UpdateFearFeedback();
    }

    public float CurrentFearState()
    {
        return currentFear;
    }

    private void UpdateFearFeedback()
    {
        fearLevel.fillAmount = currentFear / 100;
    }

    public void SeeSomething(GameObject target)
    {
        objectSeen = target;
        Debug.Log(objectSeen);

        if(currentState == IntruderState.Composed)
        {
            
        }

    }

    public void HearSomething(GameObject target)
    {
        objectHeard = target;
        Debug.Log(objectHeard + "Entendu");

        Vector3 relativePos = objectHeard.transform.position - body.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        body.rotation = rotation;
    }

    private void ChooseRandomPoint()
    {
        Debug.Log("222");
        Vector3 randomPoint = waypoint.position + Random.insideUnitSphere * 100;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
        {
            moveTo = hit.position;
            Debug.Log(moveTo);
            Navposition(moveTo);
        }
    }
    
    private void Navposition(Vector3 positionToGo)
    {
        navMeshAI.SetDestination(positionToGo);
    }

}
