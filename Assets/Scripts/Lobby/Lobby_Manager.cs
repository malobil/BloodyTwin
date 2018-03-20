using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class Lobby_Manager : NetworkLobbyManager {

	private GameObject gpti;
	private int playerIdx = -1;
	private int nbSpawn = 0;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        Transform startPos = GetStartPosition();
        
        if (nbSpawn == 0)
        {
        	if (startPos != null)
	        {
	            gpti = Instantiate(spawnPrefabs[playerIdx], startPos.position, startPos.rotation);
	        }
	        else
	        {
	             gpti = Instantiate(spawnPrefabs[playerIdx], Vector3.zero, Quaternion.identity);
	        }
        }
        else if (nbSpawn == 1)
        {
        	if (playerIdx == 0)
        	{
        		if (startPos != null)
		        {
		            gpti = Instantiate(spawnPrefabs[1], startPos.position, startPos.rotation);
		        }
		        else
		        {
		             gpti = Instantiate(spawnPrefabs[1], Vector3.zero, Quaternion.identity);
		        }
        	}
        	else if (playerIdx == 1)
        	{
        		if (startPos != null)
		        {
		            gpti = Instantiate(spawnPrefabs[0], startPos.position, startPos.rotation);
		        }
		        else
		        {
		             gpti = Instantiate(spawnPrefabs[0], Vector3.zero, Quaternion.identity);
		        }
        	}
        }

	    nbSpawn++;    
        return gpti;
    }

    public void SetPlayerIdx (int idx)
    {
    	playerIdx = idx;

        if(UI_Accueil_Manager.s_Instance != null)
        {
            UI_Accueil_Manager.s_Instance.DisplayWaitScreen();
        }
    	
    	StartMatchMaker();
    	ListGames();
        Debug.Log(playerIdx);
    }

    void ListGames ()
    {
    	if (playerIdx == 1)
    	{
    		matchMaker.ListMatches(0,10,"Spectre",false, 0, 0, OnMatchList); // Recherche Spectre
    	}
    	else  if (playerIdx == 0)
    	{
    		matchMaker.ListMatches(0,10,"Bourreau",false, 0, 0, OnMatchList); // Recherche Bourreau
    	}
    }

    public override void OnMatchList (bool sucess, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
    	for (var i = 0; i < matchList.Count; i++)
		{
            if (matchList[i].currentSize < minPlayers){
                matchMaker.JoinMatch(matchList[i].networkId, " "," ", " ", 0, 0, OnMatchJoined);
                return;
            }
        }

        CreateMatch(); 
    }

    void CreateMatch (){
    	if (playerIdx == 0)
    	{
       		matchMaker.CreateMatch("Spectre", 2, true, " ", " ", " ", 0, 0, OnMatchCreate);
    	}
    	else if (playerIdx == 1)
    	{
       		matchMaker.CreateMatch("Bourreau", 2, true, " ", " ", " ", 0, 0, OnMatchCreate);
    	}
    }

    public override void OnMatchCreate (bool success, string extendedInfo, MatchInfo matchInfo){
        if (success) {
           StartHost(matchInfo);
        }
    }

    public override void OnMatchJoined (bool success, string extendedInfo, MatchInfo matchInfo)
    {
        
    	if (success){
            Debug.Log("JOIN");
            Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);
            StartClient(matchInfo);
        }
    }
}
