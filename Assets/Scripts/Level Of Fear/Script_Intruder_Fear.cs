using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Intruder_Fear : MonoBehaviour {

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

    public void Feared()
    {
       Script_Global_Fear.Instance.UpFear(10f); 
    }

    public void Appeased()
    {
        Script_Global_Fear.Instance.DownFear(10f);
    }
}
