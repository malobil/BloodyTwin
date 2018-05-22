using System;
using System.Collections;
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
        public GameObject lampGO;

        private float currentflashlightCooldown;

        [Header("Camera")]
        public GameObject cameraPrefab;
        public Transform cameraBasePosition;
        private Transform cameraTransform;

        [Header("Stun")]
        private bool isStun = false;

        private void Start()
        {
            gameObject.tag = "Intru";

            if (!isLocalPlayer)
            {
                return;
            }

            

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

            currentflashlightCooldown = flashlightCooldown; // Setting light CD to base
        }

        public override void OnStartLocalPlayer()
        {
            SetupCamera();
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            /*if(isStun)
            {
                return;
            }*/

            if (currentRunCooldown > 0 )
            {
                currentRunCooldown -= Time.deltaTime;
                Script_UI_InGame_Manager.Instance.UpdatePlayerStamina(currentRunCooldown, runDuration);
            }
            if (currentflashlightCooldown <= 0)
            {
                if (isServer)
                {
                    RpcDisableLamp();
                }
                else
                {
                    CmdDisableLamp();
                }// Stop the flashlight
            }
            else if(lampGO.activeSelf && currentflashlightCooldown > 0)
            {
                currentflashlightCooldown -= Time.deltaTime;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (currentflashlightCooldown > 0)
                {
                    if(isServer)
                    {
                        RpcToggleLamp();
                        Debug.Log("Lamp");
                    }
                    else
                    {
                        CmdToggleLamp() ;
                    }
                   
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

            if (Input.GetKeyDown(KeyCode.S))// debug
            {
                //Die();
                //Stun();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;
                if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 4.0f))
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
                    else if (hit.collider.gameObject.GetComponent<Script_Piles>())
                    {
                        hit.collider.gameObject.GetComponent<Script_Piles>().AddPile(this);
                    }


                    Debug.Log(hit.collider.gameObject.name);
                }
            }
        }

        private void SetPauseMenu()
        {
            Script_UI_InGame_Manager.Instance.PauseMenu();
        }

        public void AddingPile(float toAdd)
        {
            currentflashlightCooldown += toAdd;
        }

        private void LocalToggleLamp()
        {
            lampGO.SetActive(!lampGO.activeSelf);
        }

        private void LocalDisableLamp()
        {
            lampGO.SetActive(false);
        }

        [Command]
        private void CmdGetDoll(GameObject dollToUnspawn)
        {
            Script_UI_InGame_Manager.Instance.GetADoll();
            NetworkServer.UnSpawn(dollToUnspawn);
        }

        [Command]
        private void CmdToggleLamp()
        {
            RpcToggleLamp();
        }

        [ClientRpc]
        private void RpcToggleLamp()
        {
            LocalToggleLamp();
        }

        [Command]
        private void CmdDisableLamp()
        {
            LocalDisableLamp();
            RpcDisableLamp();
        }

        [ClientRpc]
        private void RpcDisableLamp()
        {
            LocalDisableLamp();
        }

        public void SetupCamera()
        {
            GameObject cameraPop = Instantiate(cameraPrefab, cameraBasePosition.position, cameraBasePosition.rotation,this.transform);
            cameraTransform = cameraPop.transform;
            lampGO.transform.parent = cameraPop.transform ;
        }

        [ClientRpc]
        public void RpcStun()
        {
            isStun = true;
            Debug.Log("You are stun");
            GetComponent<FirstPersonController>().enabled = false;
            StartCoroutine(StunTime());
        }

        public void Die()
        {
            if (isLocalPlayer)
            {
                Debug.Log("DIE");
                cameraTransform.parent = null;
                Script_UI_InGame_Manager.Instance.IntruderLoose(); // UI
               
                
                Destroy(gameObject);
            }

            CmdDieServer();
            Destroy(this.gameObject);
            NetworkServer.UnSpawn(this.gameObject);
        }

        [Command]
        void CmdDieServer()
        {
            Script_UI_InGame_Manager.Instance.IntruderDie(); // Count
        }

        public bool ReturnIsStun()
        {
            return isStun;
        }

        IEnumerator StunTime()
        {
            yield return new WaitForSeconds(3f);
            isStun = false;
            GetComponent<FirstPersonController>().enabled = true;
        }
    }
}
