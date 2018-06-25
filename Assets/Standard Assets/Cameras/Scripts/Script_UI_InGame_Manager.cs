using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.Cameras;

public class Script_UI_InGame_Manager : NetworkBehaviour {

    [SyncVar]
    public float minutes;
    [SyncVar]
    public float seconds;

    public Text timerText;
    public Text endText;
    public GameObject gameOverPanel, gameWinPanel, gamePauseMenu, bourreauUI, spectreUI, timerPanel ;

   
    private int intruderAlive = 2;
    private int intruderWin = 0;

    [Header("Poupée")]
    public List<Transform> dollSpawnList = new List<Transform>();
    public GameObject dollPrefab;
    public int dollToSpawn = 4;
    public GameObject doorHall;
    private int dollGet = 0;

    [Header("BourreauUI")]
    public Image bourreauStaminaImage ;

    [Header("End")]
    public List<GameObject> dollIndicator = new List<GameObject>();

    public GameObject lights;

    [Header("Pause")]
    private bool isPause = false;

    [Header("Sound")]
    private AudioSource source;

    public AudioClip dollSound;

    [SyncVar]
    private bool gameIsRunning = false;

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

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {
        Timer(); 
	}

    void Timer()
    {
        if(!isServer || !gameIsRunning)
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
            EndGame();
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
        if(intruderAlive <= 0 && intruderWin <= 0)
        {
            endText.text = "KILLERS win !";
        }
        else
        {
            endText.text = "Intruders win ! ";
        }

        gameWinPanel.SetActive(true);
        gamePauseMenu.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void IntruderWin()
    {
        endText.text = "You win ! ";
        gameWinPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void IntruderAsWin() // execute on server
    {
        intruderWin++;

        RpcIntruderWin();

        if (intruderWin >= intruderAlive)
        {
            RpcGameEnd();
            EndGame();
        }
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
        isPause = !isPause;
        //Debug.Log(isPause + "PAUSE");
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

    public void GetADoll() // execute on server
    {
        dollGet++;

        RpcDestroyIndicator();

        if (dollGet == dollToSpawn)
        {
            Destroy(doorHall);
            NetworkServer.UnSpawn(doorHall);
        }
    }

    [ClientRpc]
    void RpcDestroyIndicator()
    {
        Destroy(dollIndicator[0]);
        dollIndicator.RemoveAt(0);
    }

    public void IntruderDie()
    {
        if(!isServer)
        {
            return;
        }

            intruderAlive--;
            RpcIntruderAlive();
                       
            if (intruderAlive <= 0 || intruderWin > 0)
            {
                RpcGameEnd();
                EndGame();
            }
    }

    [ClientRpc]
    void RpcIntruderAlive()
    {
        intruderAlive--;
    }

    [ClientRpc]
    void RpcIntruderWin()
    {
        intruderWin++;
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

    public void StartGame()
    {
        gameIsRunning = true;
        timerPanel.SetActive(true);
    }

    public void EndGame()
    {
        gameIsRunning = false;
    }

    public bool GetGameState()
    {
        return gameIsRunning;
    }

    public bool GetIsPause ()
    {
        return isPause;
    }

    [ClientRpc]
    public void RpcPlayDollSound()
    {
        source.PlayOneShot(dollSound);
    }
}
