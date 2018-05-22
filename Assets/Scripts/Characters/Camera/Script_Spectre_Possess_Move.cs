using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;
using UnityStandardAssets.Characters;

public class Script_Spectre_Possess_Move : MonoBehaviour
{
    public float moveSpeed;
    public float ejectionSpeedAdd;
    public float maxEjectionSpeed;
    private float ejectionSpeed = 0f;
    private bool isProject;

    private float currentSpeed;
    private Rigidbody rbComponent;
    private Vector3 desiredMoveDirection;
    private Script_Possession_Online associateScriptPossesion; // script gérant la possession / depossession

    private void Start()
    {
        rbComponent = GetComponent<Rigidbody>();
        associateScriptPossesion = GetComponent<Script_Possession_Online>();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            if(ejectionSpeed < maxEjectionSpeed)
            {
                ejectionSpeed += ejectionSpeedAdd * Time.deltaTime ;
            }

            Debug.Log(ejectionSpeed);
        }

        if(Input.GetKeyUp(KeyCode.A))
        {
            Project();
        }
    }

    void FixedUpdate()
    {
        if(isProject)
        {
            return;
        }
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
        rbComponent.AddForce(camera.forward * ejectionSpeed, ForceMode.Impulse);
       // associateScriptPossesion.UnPossessObject();
        StartCoroutine(ProjectionEnd());
        isProject = true;
        ejectionSpeed = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isProject)
        {
            if (collision.gameObject.CompareTag("Intru"))
            {
                Debug.Log("TOUCH" + collision.gameObject.name);
                collision.gameObject.GetComponent<Script_Player_Moves>().Stun();// Call here stun function
                isProject = false;
            }
        }
    }

    IEnumerator ProjectionEnd()
    {
        yield return new WaitForSeconds(3f);
        isProject = false;
        print(isProject);
    }
}
