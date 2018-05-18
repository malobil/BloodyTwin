using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TutoManager : MonoBehaviour 
{

	public Transform tutoB;
	public Transform tutoS;
	public Transform tutoI;
	public GameObject arrowRight;
	public GameObject arrowLeft;

	private Transform currentTuto = null;
	private int index = 0;

	public void ShowTuto (Transform tutoToShow)
	{
		if ( currentTuto != null)
		{
			currentTuto.gameObject.SetActive(false);
		}

		currentTuto = tutoToShow;
		tutoToShow.gameObject.SetActive(true);
		Debug.Log("dd") ;
	}

	public void UnShowTuto()
	{
		currentTuto.gameObject.SetActive(false);
		currentTuto = null;
		index = 0;
	}

	void NextTuto ()
	{
		currentTuto.GetChild(index).gameObject.SetActive(false);
		index++;
		currentTuto.GetChild(index).gameObject.SetActive(true);
		arrowLeft.SetActive(true);

		if (currentTuto.GetChild(index+1)== null)
		{
			arrowRight.SetActive(false);
		}
	}

	void PreviousTuto()
	{		
		currentTuto.GetChild(index).gameObject.SetActive(false);
		index--;
		currentTuto.GetChild(index).gameObject.SetActive(true);
		arrowRight.SetActive(true);

		if (currentTuto.GetChild(index-1)== null)
		{
			arrowRight.SetActive(false);
		}
	}
}
