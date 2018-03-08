using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Bourreau_Moves : MonoBehaviour {

    public float moveSpeed = 10f;
    public Transform cameraTransform;

    private CharacterController cC;
	// Use this for initialization
	void Start ()
    {
        cC = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float movementXaxis = Input.GetAxis("Horizontal");
        float movementYaxis = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(movementXaxis, 0, movementYaxis);

        moveDirection *= moveSpeed;

        cC.Move(moveDirection * Time.deltaTime);

    }
}
