using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Networking;

public class Script_Possession_Online : NetworkBehaviour {

    private GameObject player;
    private Transform objet_hante;
    public Transform cameraPoint;

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
            //player.transform.SetParent(transform); // met le spectre en parent de l'objet
            player.GetComponent<Script_Spectre_Moves_Online>().DisableCharacter() ; // désactive le spectre                      
            player.GetComponent<Script_Spectre_Moves_Online>().DisableCamera();
            player.GetComponent<Script_Spectre_Moves_Online>().EnablePossessCamera(cameraPoint) ;
            GetComponent<Script_Spectre_Possess_Move_Online>().SettingCam(player.GetComponent<Script_Spectre_Moves_Online>().ReturnPossessCamera());
            player.GetComponent<Script_Spectre_Possess_Move_Online>().enabled = true;
            is_possession = true;

            timeLeft = 1f;
    }

    public void UnPossessObject()
    {
        //si on sort de l'objet en possession
        if (can_leave)
        {
            can_leave = false;
            timeLeft = 1f;

            player.transform.parent = null;
            player.SetActive(true); // active le spectre

            player.GetComponent<Script_Spectre_Moves_Offline>().EnableCamera();
            player.GetComponent<Script_Spectre_Moves_Offline>().DisablePossessCamera();
            objet_hante.GetComponent<Script_Spectre_Possess_Move_Online>().enabled = false;

            timeLeft = 1f;
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

        /*if(is_possession)
        {
            player.GetComponent<Script_Spectre_Moves_Online>().CmdUpdatePossessTransform(transform, transform.position);
        }*/
    }
        

    // détection du spectre dans le trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Script_Spectre_Moves_Online>())
        {
            player = other.gameObject;
            player.GetComponent<Script_Spectre_Moves_Online>().SettingPossessTarget(this);
            print("Spectre entré" +  can_possession);
            can_possession = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Script_Spectre_Moves_Online>())
        {
            player = null;
            player.GetComponent<Script_Spectre_Moves_Online>().UnSettingPossessTarget();
            print("Spectre sorti");
            can_possession = false;
        }
    }
}
