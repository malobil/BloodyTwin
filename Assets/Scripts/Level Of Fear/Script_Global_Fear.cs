using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_Global_Fear : MonoBehaviour {

    private static Script_Global_Fear instance;
    public static Script_Global_Fear Instance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void UpFear(float fearToAdd)
    {

    }
}
