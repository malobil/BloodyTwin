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
    private float currentPossessTime;
    private bool tryPossessing = false;

    [Header("Communication")]
    public GameObject comCome;
    public GameObject comGotOne;
    public GameObject comRunAway;
    public GameObject comStayHere;

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

        if(Input.GetButtonDown("Communication_Come"))
        {
            CmdCommunication(comCome);
        }
        else if (Input.GetButtonDown("Communication_GotOne"))
        {
            CmdCommunication(comGotOne);
        }
        else if (Input.GetButtonDown("Communication_HeRunAway"))
        {
            CmdCommunication(comRunAway);
        }
        else if (Input.GetButtonDown("Communication_StayHere"))
        {
            CmdCommunication(comStayHere);
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
    void CmdCommunication(GameObject toSpawn)
    {
        GameObject tempCom = Instantiate(toSpawn, transform.position, Quaternion.identity);
        NetworkServer.Spawn(tempCom);
    }

    void SetPauseMenu()
    {
        Script_UI_InGame_Manager.Instance.PauseMenu();
    }
}
