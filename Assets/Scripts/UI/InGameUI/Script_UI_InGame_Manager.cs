using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Script_UI_InGame_Manager : NetworkBehaviour {

    [SyncVar]
    public float minutes;
    [SyncVar]
    public float seconds;

    public Text timerText;
    public Image globalFearImage;
    public GameObject gameOverPanel, gameWinPanel;

    public static Script_UI_InGame_Manager Instance { get; private set; }

	// Use this for initialization
	void Start ()
    {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!isServer)
        {
            return;
        }

        Timer();
	}

    void Timer()
    {
        if(seconds > 0)
        {
            seconds -= Time.deltaTime;
            RpcUpdateTimerText();
        }
        else if(seconds <= 0 && minutes > 0)
        {
            seconds = 60;
            minutes--;
        }

        if(seconds <= 0 && minutes <= 0 && Script_Global_Fear_Online.Instance.ReturnNumberOfIntrus() > 0)
        {
            RpcGameOver();
        }
        else if(seconds <= 0 && minutes <= 0 && Script_Global_Fear_Online.Instance.ReturnNumberOfIntrus() <= 0)
        {
            RpcGameWin();
        }
    }

    [ClientRpc]
    void RpcUpdateTimerText()
    {
        timerText.text = minutes.ToString("") + " : " + Mathf.RoundToInt(seconds).ToString("");
    }

    [ClientRpc]
    void RpcGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    [ClientRpc]
    public void RpcGameWin()
    {
        gameWinPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    [ClientRpc]
    public void RpcUpdateGlobalFear(float globalFear)
    {
        globalFearImage.fillAmount = globalFear / 100;
    }

    public void DisconnectPlayer()
    {
        Debug.Log("Disconnect");
        NetworkManager.singleton.StopHost();
    }
}
