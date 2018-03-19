using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Script_Global_Fear : MonoBehaviour {

    public Image image_Fear;
    public int intruderNumberTot;

    public static Script_Global_Fear Instance { get; private set; } 

    void Awake()
    {
        Instance = this; 
    }

    void Update()
    {
        Debug.Log(intruderNumberTot);
    }

    public void IntruderAmount(int intruderNum)
    {
        intruderNumberTot += intruderNum;
    }

    public void UpFear(float fearToAdd) 
    {
        image_Fear.fillAmount += (fearToAdd/intruderNumberTot)/100;
    }

    public void DownFear(float fearToRemove)
    {
        image_Fear.fillAmount -= (fearToRemove/intruderNumberTot)/100;
    }
}
