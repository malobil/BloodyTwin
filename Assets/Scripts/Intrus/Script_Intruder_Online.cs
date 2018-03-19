using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking ;

public class Script_Intruder_Online : NetworkBehaviour {

    public float fearAdd;
    public float fearRemoved;

    public int intruderNum;
    public Image fearLevel;

    [SyncVar(hook = "UpdateFearFeedback")]
    public float currentFear = 0f;

    // Use this for initialization
    void Start()
    {
        if(!isServer)
        {
            return;
        }

        Script_Global_Fear_Online.Instance.IntruderAmount();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            return;
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
            //UpdateFearFeedback();
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
}
