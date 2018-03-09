using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Lobby_Player_Accueil : NetworkLobbyPlayer 
{

	// Use this for initialization
	void Start () {
		
	}

	public override void OnStartLocalPlayer()
	{
		// SendReadyToBeginMessage();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
