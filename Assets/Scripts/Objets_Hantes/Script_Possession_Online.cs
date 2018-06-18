using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.Networking;

public class Script_Possession_Online : NetworkBehaviour {

    private GameObject player;
    private Transform objet_hante;

    private bool can_possession; // peut prendre possession de l'objet
    private bool is_possession; // l'obet est possédé

    private float timeLeft; // évite un bug par rapport à l'appuie de la touche
    private bool can_leave = false; // quand on peut quitter la voiture

    void Start ()
    {
        //objet_hante = transform.parent.gameObject; // objet_hante est son parent
        timeLeft = 1f;
        objet_hante = transform;
	}
	
	public void PossessObject()
    {
        if(gameObject.tag ==  "door")
        {
            gameObject.GetComponent<Script_Door>().LockingDoor();
        }
            gameObject.tag = "Possess";
            player.transform.SetParent(transform); // met le spectre en parent de l'objet
            player.GetComponent<Script_Spectre_Moves_Online>().DisableCharacter() ; // désactive le spectre
            
        if (GetComponent<Script_Spectre_Possess_Move>() != null)
        {
            GetComponent<Script_Spectre_Possess_Move>().enabled = true;
        }
            
            is_possession = true;

            timeLeft = 1f;
    }

    public void UnPossessObject()
    {
        //si on sort de l'objet en possession
        if (can_leave)
        {
            if (gameObject.tag == "door")
            {
                gameObject.GetComponent<Script_Door>().LockingDoor();
            }

            gameObject.tag = "Untagged";
            can_leave = false;
            timeLeft = 1f;

            player.transform.parent = null;
            player.GetComponent<Script_Spectre_Moves_Online>().CmdEnablePlayer(); // active le spectre
            CmdGiveAuthority();
            objet_hante.GetComponent<Script_Spectre_Possess_Move>().enabled = false;

            timeLeft = 1f;

            player.GetComponent<Script_Spectre_Moves_Online>().UnSettingPossessTarget();
            player.GetComponent<Script_Spectre_Moves_Online>().ChangeCameraTarget(player.transform);
            is_possession = false;
        }
    }

    private void Update()
    {
        // Délais d'attente entre l'entrée et la sortie de la possession
        // Permet aussi d'utiliser la même touche pour entré et sortir de la possession
        if (timeLeft > 0 && is_possession)
        {
            timeLeft -= Time.deltaTime;
            can_leave = false;
        }
        else if (timeLeft <= 0 && is_possession)
        {
            can_leave = true;
        }

        if (Input.GetKeyDown("e"))
        {
            UnPossessObject();
        }
    }
        

    // détection du spectre dans le trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Script_Spectre_Moves_Online>())
        {
            player = other.gameObject;
            player.GetComponent<Script_Spectre_Moves_Online>().SettingPossessTarget(this);
            //Script_UI_InGame_Manager.Instance.EnableSpectreUI();
            print("Spectre entré" +  can_possession);
            can_possession = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Script_Spectre_Moves_Online>() && !is_possession)
        {
            player.GetComponent<Script_Spectre_Moves_Online>().UnSettingPossessTarget();
           // Script_UI_InGame_Manager.Instance.EnableSpectreUI();
            //player = null;
            print("Spectre sorti");
            can_possession = false;
        }
    }

    public GameObject ReturnPlayer()
    {
        return player;
    }

    [Command]
    public void CmdGiveAuthority()
    {
        var otherOwner = GetComponent<NetworkIdentity>().clientAuthorityOwner;
        GetComponent<NetworkIdentity>().RemoveClientAuthority(otherOwner);
        //player.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
    }
}
