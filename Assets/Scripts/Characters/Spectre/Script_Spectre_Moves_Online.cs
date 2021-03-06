﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.Characters;

public class Script_Spectre_Moves_Online : NetworkBehaviour {

    [Header("Movement")]
    public float speed = 5.0f;
    private Rigidbody rb;
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
    public AudioClip[] comSoundSpectre;
    public AudioClip[] comSoundBourreau;


    [Header("Camera")]
    public GameObject playerCamera;
    public GameObject poupeeCam;
   
    private Script_Possession_Online objectToPossess;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        //Script_UI_InGame_Manager.Instance.CmdRegisterSpectre();
        Script_UI_InGame_Manager.Instance.LightUp();
        rb = GetComponent<Rigidbody>();
        gameObject.tag = "Spectre";
    }

    void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPauseMenu();
        }

        if (Script_UI_InGame_Manager.Instance.GetIsPause())
        {
            return;
        }

        Mouvement(); // déplacement perso

        if(Input.GetButtonDown("Fire2"))
        {
            poupeeCam.SetActive(true);
        }
        
        if(Input.GetButtonUp("Fire2") || !Input.GetButton("Fire2"))
        {
            poupeeCam.SetActive(false);
        }

        if (Input.GetButtonDown("Communication_Come"))
        {
            if(!comAudioSource.isPlaying)
            {
                PlaySound(0);
                CmdCommunicationCome();
                CmdcomSoundSpectre(0);
            }
            
        }
        else if (Input.GetButtonDown("Communication_GotOne"))
        {
            if (!comAudioSource.isPlaying)
            {
                PlaySound(1);
                CmdCommunicationGotOne();
                CmdcomSoundSpectre(1);
            }
        }
        else if (Input.GetButtonDown("Communication_HeRunAway"))
        {
            if (!comAudioSource.isPlaying)
            {
                PlaySound(2);
                CmdCommunicationHeRun();
                CmdcomSoundSpectre(2);
            }
        }
        else if (Input.GetButtonDown("Communication_StayHere"))
        {
            if (!comAudioSource.isPlaying)
            {
                PlaySound(3);
                CmdCommunicationStay();
                CmdcomSoundSpectre(3);
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

        if(Input.GetKeyDown("a") && objectToPossess!= null && !tryPossessing)
        {
            currentPossessTime = possessTime;
            feedbackPossessing.SetActive(true);
            tryPossessing = true;
            sfxAudioSource.Play();
        }

       
	}

    private void Mouvement()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // touche Q & D
        float forwardInput = Input.GetAxis("Vertical"); // touche Z & S

        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up); //déplacement en fonction du regard du spectre && Il faut mettre un VECTOR 3 à la place

        rb.velocity = transform.right * speed * horizontalInput + transform.forward * speed * forwardInput;
    }

    public void SettingCamera(GameObject camToSet, GameObject camP)
    {
        playerCamera = camToSet; // Set la camera du joueur
        poupeeCam = camP;
        poupeeCam.SetActive(false);
        Debug.Log(poupeeCam);

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
        if(objectToPossess != null)
        {
            objectToPossess.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
        }
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
    }

    [Command]
    void CmdCommunicationStay()
    {
        GameObject tempCom = Instantiate(comStayHere, transform.position, Quaternion.identity);
        NetworkServer.Spawn(tempCom);
    }

    [Command]
    void CmdCommunicationHeRun()
    {
        GameObject tempCom = Instantiate(comRunAway, transform.position, Quaternion.identity);
        NetworkServer.Spawn(tempCom);
    }

    [Command]
    void CmdCommunicationGotOne()
    {
        GameObject tempCom = Instantiate(comGotOne, transform.position, Quaternion.identity);
        NetworkServer.Spawn(tempCom);
    }

    [Command]
    void CmdcomSoundSpectre(int idxToPlay)
    {
        //RpcTargetSound(idxToPlay);
        //PlaySound(idxToPlay);
        if (GameObject.FindGameObjectWithTag("Bourreau") != null)
        {
            GameObject.FindGameObjectWithTag("Bourreau").GetComponent<Script_Bourreau_Moves>().TakeSound(idxToPlay);
        }
        else
        {
            Debug.Log("No Bourreau");
        }
    }

    public void StunIntruder(GameObject stun)
    {
        CmdStunIntruder(stun);
    }

    [Command]
    void CmdStunIntruder(GameObject toStun)
    {
        toStun.GetComponent<Script_Player_Moves>().RpcStun();
    }

    [ClientRpc]
    public void RpcTargetSound(int idxToPlay)
    {
        comAudioSource.PlayOneShot(comSoundSpectre[idxToPlay]);
    }

    void SetPauseMenu()
    {
        Script_UI_InGame_Manager.Instance.PauseMenu();
    }

    public void Loose()
    {
        if(isLocalPlayer)
        {
            Script_UI_InGame_Manager.Instance.GameOver();
        } 
    }

    void PlaySound(int idx)
    {
        comAudioSource.PlayOneShot(comSoundSpectre[idx]);
    }

    public void TakeSound(int toPlay)
    {
        TargetRpcTargetBySound(connectionToClient, toPlay);
    }

    [TargetRpc]
    void TargetRpcTargetBySound(NetworkConnection test, int idxToPlay)
    {
        comAudioSource.PlayOneShot(comSoundBourreau[idxToPlay]);
    }

    [Command]
    public void CmdLockDoor(GameObject door)
    {
        if(door != null)
        {
            door.GetComponent<Script_Door>().RpcLockingDoor();
        }
        
    }
}
