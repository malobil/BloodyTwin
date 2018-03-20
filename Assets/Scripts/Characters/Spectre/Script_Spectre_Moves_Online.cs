using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Script_Spectre_Moves_Online : NetworkBehaviour {

    public float speed = 5.0f;
    public GameObject playerCamera;
    public Transform possessCamera;

    private Script_Possession_Online objectToPossess;

    private void Start()
    {
        
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
            CmdGiveAuthority();

            if(isServer)
            {
                RpcDisablePlayer();
            }
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
        playerCamera = camToSet;
    }

    public void DisableCamera()
    {
        playerCamera.SetActive(false);
    }

    public void EnableCamera()
    {
        playerCamera.SetActive(true);
    }

    public void EnablePossessCamera(Transform cameraPoint)
    {
        possessCamera.transform.position = cameraPoint.position;
        possessCamera.transform.rotation = cameraPoint.rotation;
        possessCamera.SetParent(cameraPoint);
        possessCamera.gameObject.SetActive(true);
    }

    public void DisablePossessCamera()
    {
        possessCamera.SetParent(null);
        possessCamera.gameObject.SetActive(false);
    }

    public Camera ReturnPossessCamera()
    {
        return possessCamera.GetComponent<Camera>() ;
    }

    public void SettingPossessTarget(Script_Possession_Online targetToSet)
    {
        objectToPossess = targetToSet;
    }

    public void UnSettingPossessTarget()
    {
        objectToPossess = null;
    }

    void PossessObject()
    {
        objectToPossess.PossessObject();
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

    [ClientRpc]
    void RpcDisablePlayer()
    {
        gameObject.SetActive(false);
    }
}
