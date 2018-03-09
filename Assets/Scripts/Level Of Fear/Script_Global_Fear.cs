using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Script_Global_Fear : MonoBehaviour {

    public Image image_Fear;

    public static Script_Global_Fear Instance { get; private set; } 

    void Awake()
    {
        Instance = this; 
    }
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void UpFear(float fearToAdd) 
    {
        image_Fear.fillAmount += fearToAdd/100;
    }

    public void DownFear(float fearToRemove)
    {
        image_Fear.fillAmount -= fearToRemove/100;
    }
}
