using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_End_Level_Trigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Intru"))
        {
            Script_UI_InGame_Manager.Instance.IntruderWin();
        }
    }
}
