using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Script_Global_Fear_Online : NetworkBehaviour {

    public Image imgFearState;

    [SyncVar]
    private int intruderNumberTot;

    public List<Script_Intruder> intruders = new List<Script_Intruder>();

    [SyncVar]
    private float currentFearState;

    public static Script_Global_Fear_Online Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //FearGlobalState();
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

    public void FearGlobalState()
    {
        currentFearState = 0;
        foreach (Script_Intruder intruderFear in intruders)
        {
            currentFearState += intruderFear.CurrentFearState();
        }

        currentFearState /= intruderNumberTot;
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
}
