using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Script_Intruder : MonoBehaviour {

    public float fearAdd;
    public float fearRemoved;

    public int intruderNum;
    public Image fearLevel;
    public float currentFear;

    // AI //

    [SerializeField]

    public Transform destination;
    public GameObject intruderGO;
    public NavMeshAgent intruderAI;

    private int currentWayPoint;

    public List<GameObject> railPoints = new List<GameObject>();

    public float switchWay = 0.2f;

    private bool travelling;
    private bool waiting;
    private bool wayForward;
    public bool patrolWayting;

    public enum IntruderState {Armed, Composed, Scared, Panic};
    public IntruderState currentState;

    public float waitTimer;

	// Use this for initialization
	public void Start ()
    {
        intruderAI = intruderGO.GetComponent<NavMeshAgent>();
        Script_Global_Fear.Instance.IntruderAmount();

        if(railPoints != null && railPoints.Count >= 2)
        {
            currentWayPoint = 0;
            SetDestination();
        }
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown("f"))
        {
            FearedImpact(fearAdd);
        }

        if (Input.GetKeyDown("g"))
        {
            FearedImpact(fearRemoved);
        }


        if(travelling = true && intruderAI.remainingDistance <= 1.0f)
        {
            travelling = false;
            ChangeWayPoint();
            SetDestination();
        }
    }

    public void IntruderDeath()
    {
        Destroy(this.gameObject);
    }

    public void FearedImpact (float fearState)
    {
        
        currentFear += fearState;
        if (currentFear >= 100)
        {
            currentFear = 100;
        }
        else if(currentFear <= 0)
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

    private void SetDestination()
    {
        if(railPoints != null)
        {
            Vector3 targetVector = railPoints[currentWayPoint].transform.position;
            intruderAI.SetDestination(targetVector);
            travelling = true;
        }
    }

    private void ChangeWayPoint()
    {
        if(currentState == IntruderState.Composed)
        {
            currentWayPoint = (currentWayPoint + 1) % railPoints.Count;
        }
        else
        {
            if (--currentWayPoint < 0)
            {
                currentWayPoint = railPoints.Count - 1;
            }
        }
    }
}
