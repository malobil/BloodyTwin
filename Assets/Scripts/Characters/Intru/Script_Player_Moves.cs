using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

namespace UnityStandardAssets.Characters
{
    public class Script_Player_Moves : NetworkBehaviour
    {
        [Header("Movement")]
        public float walkSpeedMultiply;
        public float runSpeed;
        public float runDuration;
        public float runCooldown;

        private float currentRunDuration;
        private float currentRunCooldown;

        private FirstPersonController m_Character; // A reference to the main first person character
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        [Header("Flashlight")]
        public float flashlightCooldown;

        private float currentflashlightCooldown;

        private void Start()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            gameObject.tag = "Intru";

            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. First person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<FirstPersonController>();
        }


        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            if (currentRunCooldown > 0)
            {
                currentRunCooldown -= Time.deltaTime;
                Script_UI_InGame_Manager.Instance.UpdatePlayerStamina(currentRunCooldown, runDuration);
            }
            if (currentflashlightCooldown <= 0)
            {
                // Stop the flashlight
            }
            else
            {
                currentflashlightCooldown -= Time.deltaTime;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (currentflashlightCooldown > 0)
                {

                }
                else
                {
                    // Play a sound like a "tic" which show to te player he can't use his flashlight
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetPauseMenu();
            }

            if(Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.forward, out hit, 4.0f))
                {
                    if(hit.collider.gameObject.CompareTag("Doll"))
                    {
                        Debug.Log("Doll");
                        Destroy(hit.collider.gameObject);
                        CmdGetDoll(hit.collider.gameObject);
                    }
                    else if(hit.collider.gameObject.GetComponent<Script_Armory>())
                    {
                        hit.collider.gameObject.GetComponent<Script_Armory>().OpenArmory();
                    }
                }
            }
        }

        private void SetPauseMenu()
        {
            Script_UI_InGame_Manager.Instance.PauseMenu();
        }

        [Command]
        private void CmdGetDoll(GameObject dollToUnspawn)
        {
            Script_UI_InGame_Manager.Instance.GetADoll();
            NetworkServer.UnSpawn(dollToUnspawn);
        }
    }
}
