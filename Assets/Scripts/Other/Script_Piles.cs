using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters;

public class Script_Piles : MonoBehaviour {

    public float valor = 5f;

	public void AddPile(Script_Player_Moves player)
    {
        player.AddingPile(valor);
        UnSpawnObj();
    }

    private void UnSpawnObj()
    {
        NetworkServer.UnSpawn(gameObject);
        Destroy(gameObject);
    }
}
