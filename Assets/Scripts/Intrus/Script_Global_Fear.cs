using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Script_Global_Fear : MonoBehaviour {

    public Image imgFearState;
    private int intruderNumberTot;
    public List<Script_Intruder> intruders = new List<Script_Intruder>();

    private float currentFearState;

    public static Script_Global_Fear Instance { get; private set; } 

    void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
        FearGlobalState();
    }

    void Update()
    {
       
    }

    public void IntruderAmount()
    {
        Debug.Log(intruderNumberTot);
        intruderNumberTot ++;
    }

    public void FearGlobalState ()
    {
        currentFearState = 0;
            foreach(Script_Intruder intruderFear in intruders)
                {
                    currentFearState += intruderFear.CurrentFearState();
                }

        currentFearState /= intruderNumberTot;
        Debug.Log("moyenne:" + currentFearState);
        FearGraphics();
    }

    public void FearGraphics()
    {
        imgFearState.fillAmount = currentFearState / 100;
    }
}
