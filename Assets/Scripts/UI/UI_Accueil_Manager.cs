using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Accueil_Manager : MonoBehaviour {

	public GameObject accueilMenu;
	public GameObject selectionMenu;
	public GameObject is_Waiting;
    public GameObject startMenu;

	public static UI_Accueil_Manager s_Instance;

	void Awake ()
	{
        if (s_Instance != null){
            Destroy (gameObject);
        }
        else {
            s_Instance = this;
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickPlay ()
	{
		accueilMenu.SetActive(false);
		selectionMenu.SetActive(true);
	}

	public void OnClickQuit ()
	{
		Application.Quit();
	}

	public void DisplayWaitScreen ()
	{
		selectionMenu.SetActive(false);
		is_Waiting.SetActive(true);
	}

    public void DisplayPlayScreen()
    {
        startMenu.SetActive(false);
        accueilMenu.SetActive(true);
    }
}
