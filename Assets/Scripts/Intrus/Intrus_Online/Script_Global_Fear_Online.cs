using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Script_Global_Fear_Online : NetworkBehaviour {


    public Image imgFearState;
    public float baseFearWhenDead;
    public float fearPurcentWhenDead;

    [SyncVar]
    private int intruderNumberTot;

    public List<Script_Intruder_Online> intruders = new List<Script_Intruder_Online>();

    [SyncVar(hook = ("OnFearChange"))]
    public float currentFearState;

    public static Script_Global_Fear_Online Instance { get; private set; }

    void Awake()
    {
         Instance = this;
    }

    public void IntruderAmount()
    {
       intruderNumberTot++;
       Debug.Log(intruderNumberTot);
    }

    public void IntruderDead(Script_Intruder_Online scriptToRelease, float fearBase)
    {
       /* Call by  Script_Intruder_Online on server*/

        intruderNumberTot--;
        intruders.Remove(scriptToRelease);

        foreach (Script_Intruder_Online intruderFear in intruders)
        {
            intruderFear.FearedImpact(baseFearWhenDead+ (fearBase * (fearPurcentWhenDead / 100f)));
        }

        CheckVictory();
        Debug.Log(intruderNumberTot);
    }


    //command depuis le(s) joueur(s) (only on serveur)
    public void FearGlobalState()
    {

        currentFearState = 0;

        foreach (Script_Intruder_Online intruderFear in intruders)
        {
            currentFearState += intruderFear.CurrentFearState();
        }
        currentFearState /= intruderNumberTot;
        Debug.Log("moyenne:" + currentFearState);
        FearGraphics();        
    }

    
    void OnFearChange(float thisfear)
    {
        if(!isServer)
        { return; }
        Debug.Log(thisfear);
        RpcSendFearToAll(thisfear);
        Script_UI_InGame_Manager.Instance.RpcUpdateGlobalFear(thisfear);
    }

    public void FearGraphics()
    {
        if(imgFearState !=null)
        {
            imgFearState.fillAmount = currentFearState / 100;
        }
    }

    public float ReturnGlobalFear()
    {
        return currentFearState;
    }

    public int ReturnNumberOfIntrus()
    {
        return intruderNumberTot;
    }

    void CheckVictory()
    {
        if(intruderNumberTot <= 0)
        {
            Script_UI_InGame_Manager.Instance.RpcGameWin();
        } 
    }

    [ClientRpc]
    void RpcSendFearToAll(float fearToSet)
    {
        currentFearState = fearToSet;
    }
}
