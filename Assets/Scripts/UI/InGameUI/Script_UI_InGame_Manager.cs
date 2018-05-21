using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.ThirdPerson;

public class Script_UI_InGame_Manager : NetworkBehaviour {

    [SyncVar]
    public float minutes;
    [SyncVar]
    public float seconds;

    public Text timerText;
    public Text endText;
    public GameObject gameOverPanel, gameWinPanel, gamePauseMenu, bourreauUI, spectreUI ;

    
    [SyncVar]
    private int intruderAlive = 1;
    private List<GameObject> intruders = new List<GameObject>();

    [Header("Poupée")]
    public List<Transform> dollSpawnList = new List<Transform>();
    public GameObject dollPrefab;
    public int dollToSpawn = 4;
    public GameObject doorHall;
    private int dollGet = 0;

    [Header("BourreauUI")]
    public Image bourreauStaminaImage ;

    public GameObject lights;

    public static Script_UI_InGame_Manager Instance { get; private set; }

	// Use this for initialization
	void Awake ()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
	}

    private void Start()
    {
        if (isServer)
        {
            SpawnDolls();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        Timer(); 
	}

    void Timer()
    {
        if(!isServer)
        {
            return;
        }

        if(seconds > 0)
        {
            seconds -= Time.deltaTime;
            RpcUpdateTimerText();
        }
        else if(seconds <= 0 && minutes > 0)
        {
            seconds = 59;
            minutes--;
        }

        if(seconds <= 0 && minutes <= 0)
        {
            RpcGameEnd();
        }
    }

    [ClientRpc]
    void RpcUpdateTimerText()
    {
        timerText.text = minutes.ToString("0") + " : " + Mathf.RoundToInt(seconds).ToString("00");
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    [ClientRpc]
    public void RpcGameEnd()
    {
        if(intruderAlive <= 0)
        {
            endText.text = "KILLERS win !";
        }
        else
        {
            endText.text = "Intruders win ! ";
        }

        gameWinPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void IntruderWin()
    {
        gameWinPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void IntruderLoose()
    {
        gameOverPanel.SetActive(true);
    }

    public void PauseMenu()
    {
        Cursor.visible = !gamePauseMenu.activeSelf;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
        gamePauseMenu.SetActive(!gamePauseMenu.activeSelf);
    }

    public void DisconnectPlayer()
    {
        Debug.Log("Disconnect");
        if(isServer)
            NetworkManager.singleton.StopHost();
        else
            NetworkManager.singleton.StopClient();
    }

    public void ActiveBourreauUI()
    {
        if (bourreauUI.activeSelf == false)
        {
            bourreauUI.SetActive(true);
        }
    }

    public void EnableSpectreUI()
    {
        spectreUI.SetActive(!spectreUI.activeSelf);
    }

    public void UpdateBourreauStamina(float stamina, float maxStamina)
    {
        bourreauStaminaImage.fillAmount = stamina / maxStamina ;
    }

    public void UpdatePlayerStamina(float stamina, float maxStamina)
    {
        bourreauStaminaImage.fillAmount = stamina / maxStamina;
    }

    private void SpawnDolls()
    {
        for(int i = 0; i < dollToSpawn; i++)
        {
            int tempRandom = Random.Range(0, dollSpawnList.Count);
            GameObject tempDoll = Instantiate(dollPrefab, dollSpawnList[tempRandom].position, Quaternion.identity);
            NetworkServer.Spawn(tempDoll);
            dollSpawnList.RemoveAt(tempRandom);
        }
    }

    public void GetADoll()
    {
        dollGet++;

        if(dollGet == dollToSpawn)
        {
            Destroy(doorHall);
            NetworkServer.UnSpawn(doorHall);
        }
    }

    public void CmdIntruderDie()
    {
        if(isServer)
        {
            intruderAlive--;
            
            if (intruderAlive <= 0)
            {
                RpcGameEnd();
            }
        } 
    }

    public void LightUp()
    {
        if(lights != null)
        {
            lights.SetActive(true);
        }
        else
        {
            Debug.Log("NO LIGHT");
        }
    }
}
