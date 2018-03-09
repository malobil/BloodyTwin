using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_Global_Fear : MonoBehaviour {

    public static Script_Global_Fear Instance { get; private set; } // tu peux déclaré une instance comme ça c'est plus simple
    /*private static Script_Global_Fear instance;
    public static Script_Global_Fear Instance()
    {
        return instance;
    }*/

    void Awake()
    {
        Instance = this; // pas oublié de lui dire que l'instance c'est ce script 
        /*if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }*/
    }
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void UpFear(float fearToAdd) // Faut que tu mette ta fonction en public c'est pour ça que ça marché pas
    {

    }
}
