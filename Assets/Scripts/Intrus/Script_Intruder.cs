using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_Intruder : MonoBehaviour {

    public float fearAdd;
    public float fearRemoved;

    public int intruderNum;
    public Image fearLevel;
    public float currentFear;

	// Use this for initialization
	void Start ()
    {
        Script_Global_Fear.Instance.IntruderAmount();
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
}
