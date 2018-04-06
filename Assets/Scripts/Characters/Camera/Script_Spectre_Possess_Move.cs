using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class Script_Spectre_Possess_Move : MonoBehaviour
{
    public float moveSpeed;
    public float maxSpeed;

    private float currentSpeed;
    private Rigidbody rbComponent;

    private void Start()
    {
        rbComponent = GetComponent<Rigidbody>();    
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
        Vector3 desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;
        //Debug.Log(desiredMoveDirection * moveSpeed);

        //now we can apply the movement:
        rbComponent.velocity = desiredMoveDirection * moveSpeed ;
    }
}
