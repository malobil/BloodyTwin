using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;
using cakeslice;

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

        [Header("Sound")]
        public AudioSource sourceFx;
        public AudioClip getSmth;

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

        private GameObject currentObjectHit;

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

            if(isStun)
            {
                return;
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;
                if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 4.0f))
                {
                    if(hit.collider.gameObject.CompareTag("Doll"))
                    {
                            CmdGetDoll(hit.collider.gameObject);  
                    }
                    else if(hit.collider.gameObject.CompareTag("Armory"))
                    {
                        if(hit.collider.transform.parent.parent.GetComponent<Script_Armory>())
                        {
                            CmdArmory(hit.collider.transform.parent.parent.gameObject);
                        } 
                    }
                    else if (hit.collider.gameObject.GetComponent<Script_Piles>())
                    {
                        hit.collider.gameObject.GetComponent<Script_Piles>().AddPile(this);
                        PlayFxSound(getSmth);
                        CmdGetAPile(hit.collider.gameObject);
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if(!isLocalPlayer)
            {
                return;
            }

            RaycastHit hitting;
            
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hitting, 2.0f))
            {
                if(hitting.transform.GetComponent<InteractableObject>())
                {
                    currentObjectHit = hitting.transform.gameObject;
                    Debug.Log(currentObjectHit);
                    foreach (GameObject obj in currentObjectHit.GetComponent<InteractableObject>().GetRendererList())
                    {
                        if (obj.GetComponent<Outline>() == null)
                        {
                           obj.AddComponent<Outline>();
                        }
                    }
                }
                else
                {
                    if(currentObjectHit != null)
                    {
                        foreach (GameObject obj in currentObjectHit.GetComponent<InteractableObject>().GetRendererList())
                        {
                            Destroy(obj.GetComponent<Outline>());
                        }
                        
                        currentObjectHit = null;
                    }
                }
            }
            else
            {
                if (currentObjectHit != null)
                {
                    foreach (GameObject obj in currentObjectHit.GetComponent<InteractableObject>().GetRendererList())
                    {
                        Destroy(obj.GetComponent<Outline>());
                    }

                    currentObjectHit = null;
                }
            }

                if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;
                if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 4.0f))
                {
                    if (hit.collider.gameObject.CompareTag("Door"))
                    {
                        CmdDoor(hit.collider.transform.parent.gameObject);
                    }
                }
            }
        }

        void PlayFxSound(AudioClip sound)
        {
            sourceFx.PlayOneShot(sound);
        }

        [Command]
        public void CmdDoor(GameObject doorHit)
        {
            doorHit.GetComponent<Script_Door>().ChangeState();
            Debug.Log(doorHit);
        }

        [Command]
        void CmdArmory(GameObject go)
        {
            go.GetComponent<Script_Armory>().OpenArmory();
        }

        [Command]
        void CmdGetAPile(GameObject go)
        {
            //hit.collider.gameObject.GetComponent<Script_Piles>().AddPile(this);
            go.GetComponent<Script_Piles>().AddPile(this);
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
            Script_UI_InGame_Manager.Instance.PlayASound();
            Script_UI_InGame_Manager.Instance.GetADoll();
            Destroy(dollToUnspawn);
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

        public void IntruderWin()
        {
            if (isLocalPlayer)
            {
                cameraTransform.parent = null;
                Script_UI_InGame_Manager.Instance.IntruderWin();
                CmdIntruderAsWin();
            }
            
           
            Destroy(gameObject);
            NetworkServer.UnSpawn(gameObject);
        }

        [Command]
        void CmdIntruderAsWin()
        {
            Script_UI_InGame_Manager.Instance.IntruderAsWin();
        }

        IEnumerator StunTime()
        {
            yield return new WaitForSeconds(3f);
            isStun = false;
            GetComponent<FirstPersonController>().enabled = true;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            if (body != null && !body.isKinematic)
                body.velocity += hit.controller.velocity * 0.3f;
        }
    }
}
