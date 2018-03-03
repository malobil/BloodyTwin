using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Possession : MonoBehaviour {

    public GameObject player;
    public GameObject spectre_camera;
    public GameObject objet_hante;
    public GameObject objet_camera;

    private bool can_possession; // peut prendre possession de l'objet
    private bool is_possession; // l'obet est possédé

    private float timeLeft; // évite un bug par rapport à l'appuie de la touche
    private bool can_leave = false; // quand on peut quitter la voiture

    void Start ()
    {
        objet_hante = transform.parent.gameObject; // objet_hante est son parent
        timeLeft = 1f;
	}
	
	void Update ()
    {
        // Si on peut prendre possession
        if (can_possession && Input.GetKeyDown("e"))
        {
            print("Touche e appuyer");

            is_possession = true;

            player.transform.parent = gameObject.transform.parent;
            player.SetActive(false); // désactive le spectre
            spectre_camera.SetActive(false); // désactive la camera spectre

            objet_camera.SetActive(true);

            objet_hante.GetComponent<Script_Moves>().enabled = true;

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
            spectre_camera.SetActive(true); // actice la camera spectre

            objet_camera.SetActive(false);

            objet_hante.GetComponent<Script_Moves>().enabled = false;

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
            print("Spectre entré");
            can_possession = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Spectre sorti");
            can_possession = false;
        }
    }
}
