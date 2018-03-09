using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Moves : MonoBehaviour {

    // public float speed = 5.0f;
    public Rigidbody rb;
    public float thrust = 5.0f;
    Vector3 m_NewForce;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        
	}
	

	void Update ()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, transform.up); // déplacement en fonction du regard du spectre
        Mouvement();
	}

    private void Mouvement()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            rb.AddForce(transform.forward * thrust, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            rb.AddForce(-transform.forward * thrust, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.AddForce(transform.right * thrust, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rb.AddForce(-transform.right * thrust, ForceMode.Impulse);
        }
        /*
        float horizontalInput = Input.GetAxis("Horizontal"); // touche Q & D
        float forwardInput = Input.GetAxis("Vertical"); // touche Z & S
        */

        /*        
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput); // déplacement Q & D
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput); // déplacement Z & S
        */

    }
}
