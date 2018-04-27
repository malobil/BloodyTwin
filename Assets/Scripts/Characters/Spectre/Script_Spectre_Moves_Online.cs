using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;

public class Script_Spectre_Moves_Online : NetworkBehaviour {

    [Header("Movement")]
    public float speed = 5.0f;
    [Header("Possession")]
    public float possessTime;
    public GameObject feedbackPossessing ;
    public AudioSource sfxAudioSource;
    public AudioClip possessAudio;
    private float currentPossessTime;
    private bool tryPossessing = false;

    [Header("Communication")]
    public GameObject comCome;
    public GameObject comGotOne;
    public GameObject comRunAway;
    public GameObject comStayHere;
    public AudioSource comAudioSource;
    public AudioClip[] comSound;


    [Header("Camera")]
    public GameObject playerCamera;
   
    private Script_Possession_Online objectToPossess;

    private void Awake()
    {
        gameObject.tag = "Spectre";
    }    

	void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Mouvement(); // déplacement perso

        if (Input.GetButtonDown("Communication_Come"))
        {
            if(!comAudioSource.isPlaying)
            {
                CmdCommunicationCome();
                CmdComSound(0);
            }
            
        }
        else if (Input.GetButtonDown("Communication_GotOne"))
        {
            if (!comAudioSource.isPlaying)
            {
                CmdCommunicationGotOne();
                CmdComSound(1);
            }
        }
        else if (Input.GetButtonDown("Communication_HeRunAway"))
        {
            if (!comAudioSource.isPlaying)
            {
                CmdCommunicationHeRun();
                CmdComSound(2);
            }
        }
        else if (Input.GetButtonDown("Communication_StayHere"))
        {
            if (!comAudioSource.isPlaying)
            {
                CmdCommunicationStay();
                CmdComSound(3);
            }
        }

        if (currentPossessTime > 0 && tryPossessing)
        {
            currentPossessTime -= Time.deltaTime;
        }
        else if(currentPossessTime < 0 && tryPossessing)
        {
            PossessObject();
            currentPossessTime = 0;
        }

        if(Input.GetKeyDown("e") && objectToPossess!= null && !tryPossessing)
        {
            currentPossessTime = possessTime;
            feedbackPossessing.SetActive(true);
            tryPossessing = true;
            sfxAudioSource.Play();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SetPauseMenu();
        }
	}

    private void Mouvement()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // touche Q & D
        float forwardInput = Input.GetAxis("Vertical"); // touche Z & S

        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up); //déplacement en fonction du regard du spectre && Il faut mettre un VECTOR 3 à la place
        
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput); // déplacement Q & D
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput); // déplacement Z & S
    }

    public void SettingCamera(GameObject camToSet)
    {
        playerCamera = camToSet; // Set la camera du joueur
    }

    public void DisableCamera()
    {
        playerCamera.SetActive(false);
    }

    public void EnableCamera()
    {
        playerCamera.SetActive(true);
    }

    public void SettingPossessTarget(Script_Possession_Online targetToSet)
    {
        objectToPossess = targetToSet;
    }

    public void UnSettingPossessTarget()
    {
        objectToPossess = null;
        feedbackPossessing.SetActive(false);
        tryPossessing = false;
    }

    // fonction de possession obj
    void PossessObject()
    {
        sfxAudioSource.Stop();
        ChangeCameraTarget(objectToPossess.transform);
        CmdDisablePlayer(); // Desactive le spectre pour activer l'objet posses
        CmdGiveAuthority(); // Donne l'authorité à l'obj pour pouvoir utiliser les inputs
        objectToPossess.PossessObject();
    }

    public void ChangeCameraTarget(Transform target)
    {
        playerCamera.GetComponent<FreeLookCam>().SetCamera(target);
    }

    public void DisableCharacter()
    {
        gameObject.SetActive(false);
    }

    [Command]
    public void CmdGiveAuthority()
    {
        objectToPossess.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
    }
    
    [Command]
    void CmdDisablePlayer()
    {
        gameObject.SetActive(false);
        RpcDisablePlayer();
    }

    [ClientRpc]
    void RpcDisablePlayer()
    {
        gameObject.SetActive(false);
    }

    [Command]
    public void CmdEnablePlayer()
    {
        gameObject.SetActive(true);
        RpcEnablePlayer();
    }

    [ClientRpc]
    void RpcEnablePlayer()
    {
        gameObject.SetActive(true);
    }

    [Command]
    void CmdCommunicationCome()
    {
        GameObject tempCom = Instantiate(comCome, transform.position, Quaternion.identity);
        NetworkServer.Spawn(tempCom);
        Debug.Log("COM");
    }

    [Command]
    void CmdCommunicationStay()
    {
        GameObject tempCom = Instantiate(comStayHere, transform.position, Quaternion.identity);
        NetworkServer.Spawn(tempCom);
        Debug.Log("COM");
    }

    [Command]
    void CmdCommunicationHeRun()
    {
        GameObject tempCom = Instantiate(comRunAway, transform.position, Quaternion.identity);
        NetworkServer.Spawn(tempCom);
        Debug.Log("COM");
    }

    [Command]
    void CmdCommunicationGotOne()
    {
        GameObject tempCom = Instantiate(comGotOne, transform.position, Quaternion.identity);
        NetworkServer.Spawn(tempCom);
        Debug.Log("COM");
    }

    [Command]
    void CmdComSound(int idxToPlay)
    {
        RpcTargetSound(idxToPlay);
    }

    [ClientRpc]
    public void RpcTargetSound(int idxToPlay)
    {
        comAudioSource.PlayOneShot(comSound[idxToPlay]);
    }

    void SetPauseMenu()
    {
        Script_UI_InGame_Manager.Instance.PauseMenu();
    }
}
