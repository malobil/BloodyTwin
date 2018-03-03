using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Moves : MonoBehaviour {

    public float speed = 5.0f;

    void Start ()
    {
		
	}
	

	void Update ()
    {
        Mouvement();
	}

    private void Mouvement()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // touche Q & D
        float forwardInput = Input.GetAxis("Vertical"); // touche Z & S

        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput); // déplacement Q & D
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput); // déplacement Z & S

    }
}
