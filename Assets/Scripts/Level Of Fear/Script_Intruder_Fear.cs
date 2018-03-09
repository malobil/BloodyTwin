using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Intruder_Fear : MonoBehaviour {

    public float fearAdd;
    public float maxFear;
    public float currentFear;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown("f"))
        {
            Feared();
        }

        if (Input.GetKeyDown("g"))
        {
            Appeased();
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
}
