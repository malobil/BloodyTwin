using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_Tuto : MonoBehaviour {

	public List<GameObject> imBourreau = new List<GameObject>();
	public List<GameObject> imSpectre = new List<GameObject>();
	public List<GameObject> imIntrus = new List<GameObject>();
	private int idxBourreau;
	private int idxSpectre;
	private int idxIntrus;

	public GameObject tuto_Personnages;
	public GameObject tuto_Bourreau;
	public GameObject tuto_Spectre;
	public GameObject tuto_Intrus;


	void Start () 
	{
		
	}
	
	void Update () 
	{

	}

	public void TutoBourreau ()
	{
		tuto_Personnages.SetActive(false);
		tuto_Bourreau.SetActive(true);
	}

	public void TutoSpectre ()
	{
		tuto_Personnages.SetActive(false);
		tuto_Spectre.SetActive(true);
	}

	public void TutoIntrus ()
	{
		tuto_Personnages.SetActive(false);
		tuto_Intrus.SetActive(true);
	}

	public void ImageTutoPlusBourreau ()
	{
		imBourreau[idxBourreau].SetActive(false);
		idxBourreau++;
		imBourreau[idxBourreau].SetActive(true);
	}

	public void ImageTutoMoinsBourreau ()
	{
		imBourreau[idxBourreau].SetActive(false);
		idxBourreau--;
		imBourreau[idxBourreau].SetActive(true);
	}


	public void ImageTutoPlusSpectre ()
	{
		imSpectre[idxSpectre].SetActive(false);
		idxSpectre++;
		imSpectre[idxSpectre].SetActive(true);
	}

	public void ImageTutoMoinsSpectre ()
	{
		imSpectre[idxSpectre].SetActive(false);
		idxSpectre--;
		imSpectre[idxSpectre].SetActive(true);
	}


	public void ImageTutoPlusIntrus ()
	{
		imIntrus[idxIntrus].SetActive(false);
		idxIntrus++;
		imIntrus[idxIntrus].SetActive(true);
	}

	public void ImageTutoMoinsIntrus ()
	{
		imIntrus[idxIntrus].SetActive(false);
		idxIntrus--;
		imIntrus[idxIntrus].SetActive(true);
	}
	/*public void ActivateUI (GameObject ToActivate)
	{
		ToActivate.SetActive(true);
	}

	public void DisableUI (GameObject ToDisable)
	{
		ToDisable.SetActive(false);
	}*/
}
