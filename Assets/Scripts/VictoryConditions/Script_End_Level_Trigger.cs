using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters;

public class Script_End_Level_Trigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Intru"))
        {
            other.GetComponent<Script_Player_Moves>().IntruderWin();
            Debug.Log("FIN");
        }
    }
}
