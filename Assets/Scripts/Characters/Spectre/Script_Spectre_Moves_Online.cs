using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;

public class Script_Spectre_Moves_Online : NetworkBehaviour {

    public float speed = 5.0f;
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

        Mouvement();

        if(Input.GetKeyDown("e") && objectToPossess!= null)
        {
            PossessObject(); 
            ChangeCameraTarget(objectToPossess.transform);
            CmdDisablePlayer(); // Desactive le spectre pour activer l'objet posses
            CmdGiveAuthority(); // Donne l'authorité à l'obj pour pouvoir utiliser les inputs
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
    }

    // fonction de possession obj
    void PossessObject()
    {
        
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
    void CmdEnablePlayer()
    {
        gameObject.SetActive(true);
        RpcEnablePlayer();
    }

    [ClientRpc]
    void RpcEnablePlayer()
    {
        gameObject.SetActive(true);
    }

    void SetPauseMenu()
    {
        Script_UI_InGame_Manager.Instance.PauseMenu();
    }
}
