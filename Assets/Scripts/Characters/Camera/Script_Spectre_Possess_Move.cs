using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class Script_Spectre_Possess_Move : MonoBehaviour
{
    public float moveSpeed;
    public float maxSpeed;
    public float ejectionSpeed;

    private float currentSpeed;
    private Rigidbody rbComponent;
    private Vector3 desiredMoveDirection;
    private Script_Possession_Online associateScriptPossesion; // script gérant la possession / depossession
    private bool canMove = true;

    private void Start()
    {
        rbComponent = GetComponent<Rigidbody>();
        associateScriptPossesion = GetComponent<Script_Possession_Online>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Project();
        }
    }

    void FixedUpdate()
    {

            //reading the input:
            float horizontalAxis = Input.GetAxis("Horizontal");
            float verticalAxis = Input.GetAxis("Vertical");

            //assuming we only using the single camera:
            var camera = Camera.main;
            Debug.Log(camera);

            //camera forward and right vectors:
            var forward = camera.transform.forward;
            var right = camera.transform.right;

            //project forward and right vectors on the horizontal plane (y = 0)
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            //this is the direction in the world space we want to move:
            desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;
            //Debug.Log(desiredMoveDirection * moveSpeed);

            //apply the movement:
            rbComponent.velocity = desiredMoveDirection * moveSpeed;
    }

    private void Project()
    {
        Transform camera = Camera.main.transform;
        rbComponent.AddForce(camera.forward * ejectionSpeed);
        associateScriptPossesion.UnPossessObject();
        Debug.Log("BALANCE");
    }
}
