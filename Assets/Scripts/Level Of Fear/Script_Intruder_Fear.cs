using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Intruder_Fear : MonoBehaviour {

    public float fearAdd;
    public float maxFear;
    public float currentFear;

    public bool upFear = false;
    public bool downFear = false;
    public bool limitFearOff = false;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        Adjust_Fear_Level();
        if(Input.GetKeyDown("f") && limitFearOff == false)
        {
            upFear = true;
            Feared();
        }

        if (Input.GetKeyDown("g"))
        {
            Appeased();
            downFear = true;
        }

        if (currentFear >= 100)
        {
            limitFearOff = true;
            LimitFear();
        }
        else 
        {
            limitFearOff = false;
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(700, 10, maxFear, 20), currentFear + "/" + maxFear);
    }

    public void Feared()
    {
       Script_Global_Fear.Instance.UpFear(fearAdd); 
    }

    public void Appeased()
    {
        Script_Global_Fear.Instance.DownFear(1f);
    }

    public void Adjust_Fear_Level()
    {
        if(upFear == true)
        {
            currentFear += fearAdd;
            upFear = false;
        }

        if (downFear == true)
        {
            currentFear -= 1f;
            downFear = false;
        }
    }

    void LimitFear()
    {
        currentFear = maxFear;
    }
}
