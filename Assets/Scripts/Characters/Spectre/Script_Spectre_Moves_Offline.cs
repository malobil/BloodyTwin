using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Script_Spectre_Moves_Offline : MonoBehaviour {

    public float speed = 5.0f;
	
    private void Start()
    {
        
    }    

	void Update ()
    {
        /*if (!isLocalPlayer)
        {
            return;
        }*/
        Mouvement();
	}

    private void Mouvement()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // touche Q & D
        float forwardInput = Input.GetAxis("Vertical"); // touche Z & S

        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, transform.up); //déplacement en fonction du regard du spectre
        
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput); // déplacement Q & D
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput); // déplacement Z & S

        /*if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(Vector3.down * Time.deltaTime * speed);
        }*/
    }
}
