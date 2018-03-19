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

    [SyncVar]
    private float currentFearState;

    public static Script_Global_Fear_Online Instance { get; private set; }

    void Awake()
    {
         Instance = this;
    }

    private void Start()
    {
        if(!isServer)
        {
            this.enabled = false;
        }

        Debug.Log(intruderNumberTot);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(intruderNumberTot);
        }
    }

    public void IntruderAmount()
    {
       intruderNumberTot++;
       Debug.Log(intruderNumberTot);
    }

    public void IntruderDead(Script_Intruder_Online scriptToRelease, float fearBase)
    {
        intruderNumberTot--;
        intruders.Remove(scriptToRelease);

        foreach (Script_Intruder_Online intruderFear in intruders)
        {
            intruderFear.FearedImpact(baseFearWhenDead+ (fearBase * (fearPurcentWhenDead / 100f)));
        }

        Debug.Log(intruderNumberTot);
    }

    public void FearGlobalState()
    {
        currentFearState = 0;

        foreach (Script_Intruder_Online intruderFear in intruders)
        {
            currentFearState += intruderFear.CurrentFearState();
        }

        currentFearState /= intruderNumberTot;
        Script_UI_InGame_Manager.Instance.RpcUpdateGlobalFear(currentFearState);
        Debug.Log("moyenne:" + currentFearState);
        FearGraphics();
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
}
