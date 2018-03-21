﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking ;
using UnityEngine.AI;

public class Script_Intruder_Online : NetworkBehaviour {

    public float fearAdd;
    public float fearRemoved;

    public Image fearLevel;

    [SyncVar(hook = "UpdateFearFeedback")]
    public float currentFear = 0f;


    public enum IntruderState { Neutral, Fleeing, Chasing };
    public IntruderState currentState;

    private bool composed;
    private bool armed;
    private bool scared;
    private bool panicked;

    //IA//
    private List<Transform> wayPointsVisited = new List<Transform>();

    private GameObject objectSeen;
    private GameObject objectHeard;

    public float smoothness;
    public Transform body;

    public NavMeshAgent navMeshAI;
    public Transform wayPoint;
    private Vector3 moveTo;
    private float distance = 0f;
    private bool isMoving;

    // Use this for initialization
    void Start()
    {
        if(!isServer)
        {
            return;
        }
        NavPosition(wayPoint.position);
        Script_Global_Fear_Online.Instance.IntruderAmount();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            return;
        }

        if (navMeshAI.remainingDistance <= 0)
        {
            isMoving = false;

            if (currentState == IntruderState.Neutral)
            {
                wayPointsVisited.Add(wayPoint);
                CalculateNearestPath();
            }
        }

        if (Input.GetKeyDown("f") && currentFear < 100)
        {
            FearedImpact(fearAdd);
        }

        if (Input.GetKeyDown("g") && currentFear > 0)
        {
           FearedImpact(fearRemoved);
        }
    }

    public void IntruderDeath()
    {
        if(isServer)
        {
            Script_Global_Fear_Online.Instance.IntruderDead(this, currentFear);
            NetworkServer.UnSpawn(gameObject);
            Destroy(gameObject);  
        }
    }

    public void FearedImpact(float fearState)
    {
            currentFear += fearState;

            if (currentFear >= 100)
            {
                currentFear = 100;
            }
            else if (currentFear <= 0)
            {
                currentFear = 0;
            }
            Script_Global_Fear_Online.Instance.FearGlobalState();
    }

    public float CurrentFearState()
    {
        return currentFear;
    }

    private void UpdateFearFeedback(float fear) 
    {
        fearLevel.fillAmount = fear / 100 ;
        Debug.Log("testinh");
    }

    public void SeeSomething(GameObject target)
    {
        objectSeen = target;
        Debug.Log("VUE");
        CalculateFarestPath();
        wayPointsVisited.Clear();
    }

    public void HearSomething(GameObject target)
    {
        objectHeard = target;
        Debug.Log(objectHeard + "Entendu");

        Vector3 relativePos = objectHeard.transform.position - body.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        body.rotation = rotation;
    }

    private void NavPosition(Vector3 positionToGo)
    {
        isMoving = true;
        navMeshAI.SetDestination(positionToGo);
    }

    private void CalculateFarestPath()
    {
        navMeshAI.isStopped = true;
        distance = 0;

        foreach (Transform wayPointM in Script_WayPoint_Manager.Instance.wayPoints)
        {
            float tempDist = Mathf.Abs(Vector3.Distance(body.position, wayPointM.position));
            Debug.Log(tempDist);
            if (tempDist > distance)
            {

                distance = tempDist;
                moveTo = wayPointM.position;
                wayPoint = wayPointM;
            }
        }

        navMeshAI.isStopped = false;
        NavPosition(moveTo);
    }

    private void CalculateNearestPath()
    {
        Debug.Log("CalculateNearestPath");
        navMeshAI.isStopped = true;
        distance = 9999;

        if (Script_WayPoint_Manager.Instance.wayPoints.Count == wayPointsVisited.Count)
        {
            wayPointsVisited.Clear();
        }

        foreach (Transform wayPointN in Script_WayPoint_Manager.Instance.wayPoints)
        {
            if (!wayPointsVisited.Contains(wayPointN))
            {
                float tempDist = Mathf.Abs(Vector3.Distance(body.position, wayPointN.position));
                Debug.Log(tempDist);
                if (tempDist < distance && tempDist > 1)
                {
                    distance = tempDist;
                    moveTo = wayPointN.position;
                    wayPoint = wayPointN;
                }
            }
            else
            {
                Debug.Log("POINT ALREADY VISITED");
            }
        }

        navMeshAI.isStopped = false;
        NavPosition(moveTo);
    }
}
