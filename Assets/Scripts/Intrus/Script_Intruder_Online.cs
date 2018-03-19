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

    [SyncVar]
    public float currentFear;

    // Use this for initialization
    void Start()
    {
        if(isServer)
        {
            Script_Global_Fear_Online.Instance.IntruderAmount();
        }
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
        if (currentFear >= 100)
        {
            currentFear = 100;
        }
        else if (currentFear <= 0)
        {
            currentFear = 0;
        }
        Script_Global_Fear_Online.Instance.FearGlobalState();
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
}
