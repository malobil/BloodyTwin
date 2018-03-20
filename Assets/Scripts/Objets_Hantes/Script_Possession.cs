using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Script_Possession : MonoBehaviour {

    private GameObject player;
    public Transform objet_hante;
    public Transform cameraPoint;

    private bool can_possession; // peut prendre possession de l'objet
    private bool is_possession; // l'obet est possédé

    private float timeLeft; // évite un bug par rapport à l'appuie de la touche
    private bool can_leave = false; // quand on peut quitter la voiture

    void Start ()
    {
        //objet_hante = transform.parent.gameObject; // objet_hante est son parent
        timeLeft = 1f;
	}
	
	void Update ()
    {
        // Si on peut prendre possession
        if (can_possession && Input.GetKeyDown("e"))
        {
            print("Touche e appuyer");

            is_possession = true;

            player.transform.SetParent(transform); // met le spectre en parent de l'objet
            player.SetActive(false); // désactive le spectre
            player.GetComponent<Script_Spectre_Moves_Offline>().DisableCamera();
            player.GetComponent<Script_Spectre_Moves_Offline>().EnablePossessCamera(cameraPoint) ;
            GetComponent<Script_Spectre_Possess_Move>().SettingCam(player.GetComponent<Script_Spectre_Moves_Offline>().ReturnPossessCamera());
            GetComponent<Script_Spectre_Possess_Move>().enabled = true;

            timeLeft = 1f;
        }

        //si on sort de l'objet en possession
        if (is_possession && can_leave && Input.GetKeyDown("e"))
        {
            print("Touche e appuyer");

            is_possession = false;
            can_leave = false;
            timeLeft = 1f;

            player.transform.parent = null;
            player.SetActive(true); // active le spectre

            player.GetComponent<Script_Spectre_Moves_Offline>().EnableCamera();
            player.GetComponent<Script_Spectre_Moves_Offline>().DisablePossessCamera();
            objet_hante.GetComponent<Script_Spectre_Possess_Move>().enabled = false;

            timeLeft = 1f;
        }

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
    }

    // détection du spectre dans le trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            print("Spectre entré");
            can_possession = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = null;
            print("Spectre sorti");
            can_possession = false;
        }
    }
}
