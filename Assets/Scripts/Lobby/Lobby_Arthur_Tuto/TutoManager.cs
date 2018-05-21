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
	public Transform tutoG;
	public GameObject arrowRight;
	public GameObject arrowLeft;
	public GameObject activeArrows;

	private Transform currentTuto = null;
	private int index = 0;

	private List <Transform> children = new List <Transform>();

	public void ShowElement (GameObject elementToActive)
	{
		elementToActive.SetActive(true);
	}

	public void UnShowElement (GameObject elementToDesactive)
	{
		elementToDesactive.SetActive(false);
	}



	public void ShowTuto (Transform tutoToShow)
	{
		if ( currentTuto != null)
		{
			currentTuto.gameObject.SetActive(false);
			children[index].gameObject.SetActive(false);
			index= 0;
			children[index].gameObject.SetActive(true);
			arrowLeft.SetActive(false);
			arrowRight.SetActive(true);
		}

		index = 0;
		children.Clear();
		currentTuto = tutoToShow;
		tutoToShow.gameObject.SetActive(true);
		activeArrows.gameObject.SetActive(true);

		for (int i = 0; i < currentTuto.childCount; i ++)
		{
			children.Add(currentTuto.transform.GetChild(i));
		}
		Debug.Log("children" + children.Count);
		Debug.Log(index);
	}

	public void UnShowTuto()
	{

		currentTuto.gameObject.SetActive(false);
		activeArrows.gameObject.SetActive(false);
		currentTuto = null;
		index = 0;
	}

	public void NextTuto ()
	{
		if (index < children.Count-1)
		{
			currentTuto.GetChild(index).gameObject.SetActive(false);
			index++;
			currentTuto.GetChild(index).gameObject.SetActive(true);
			arrowLeft.SetActive(true);
			Debug.Log (index);
		}
		
		if(index == children.Count-1)
		{
			arrowRight.SetActive(false);
		}
	}

	public void PreviousTuto()
	{

		if (index > 0)
		{		
			currentTuto.GetChild(index).gameObject.SetActive(false);
		
			index--;
			currentTuto.GetChild(index).gameObject.SetActive(true);
			arrowRight.SetActive(true);
		}

		if(index == 0)
		{
			arrowLeft.SetActive(false);
		}

	}
}
