using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;
using cakeslice;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class Script_Bourreau_Moves : NetworkBehaviour
    {
        [Header("Movement")]
        public float speed = 1f;
        public float walkSpeedMultiply;
        public float runSpeed;
        public float runDuration;
        public float runCooldown;

        private float currentRunDuration;
        private float currentRunCooldown;

        [Header("Communication")]
        public GameObject comCome;
        public GameObject comGotOne;
        public GameObject comRunAway;
        public GameObject comStayHere;
        public AudioSource comAudioSource;
        public AudioClip[] comSound;
        public AudioClip[] comSoundSpectre;

        [Header("Attack")]
        public float attackCooldown;
        public float attackDelay = 0.2f;
        public Transform attackSpawnPoint;
        public GameObject attackZonePrefab;
        private float currentAttackCooldown;
        private IEnumerator attack;

        [Header("Sounds")]
        public AudioSource attackASource;
        public AudioClip[] attackSounds;

        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

      
        public Transform detectPoint;
        private GameObject currentObjectHit;
        


        private void Start()
        {
            gameObject.tag = "Bourreau";
            if (!isLocalPlayer)
            {
                return;
            }
            
            Script_UI_InGame_Manager.Instance.LightUp();
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetPauseMenu();
            }

            if (Script_UI_InGame_Manager.Instance.GetIsPause())
            {
                return;
            }

            if (currentRunCooldown > 0)
            {
                currentRunCooldown -= Time.deltaTime;
            }

            if(currentAttackCooldown > 0)
            {
                currentAttackCooldown -= Time.deltaTime;
            }

            
            if (Input.GetButtonDown("Fire1") && currentAttackCooldown <= 0)
            {
                currentAttackCooldown = attackCooldown;
                CmdPlayAttackSound(0);
                StartCoroutine("Attack");                 
            }

            

            if (Input.GetButtonDown("Communication_Come"))
            {
                if(!comAudioSource.isPlaying)
                {
                    PlaySound(0);
                    CmdComSound(0);
                    CmdCommunicationCome();
                }
                
            }
            else if (Input.GetButtonDown("Communication_GotOne"))
            {
                if (!comAudioSource.isPlaying)
                {
                    PlaySound(1);
                    CmdComSound(1);
                    CmdCommunicationGotOne();
                }
            }
            else if (Input.GetButtonDown("Communication_HeRunAway"))
            {
                if (!comAudioSource.isPlaying)
                {
                    PlaySound(2);
                    CmdComSound(2);
                    CmdCommunicationHeRun();
                }
            }
            else if (Input.GetButtonDown("Communication_StayHere"))
            {
                if (!comAudioSource.isPlaying)
                {
                    PlaySound(3);
                    CmdComSound(3);
                    CmdCommunicationStay();
                }
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            if (Script_UI_InGame_Manager.Instance.GetIsPause())
            {
                return;
            }

            if (currentAttackCooldown <= 0)
            {
                // read inputs
                float h = CrossPlatformInputManager.GetAxis("Horizontal") * speed;
                float v = CrossPlatformInputManager.GetAxis("Vertical") * speed ;
                

                // calculate move direction to pass to character
                if (m_Cam != null)
                {
                    // calculate camera relative direction to move:
                    m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                    m_Move = v * m_CamForward + h * m_Cam.right;
                }
                else
                {
                    // we use world-relative directions in the case of no main camera
                    m_Move = v * Vector3.forward + h * Vector3.right;
                }
#if !MOBILE_INPUT
                // walk speed multiplier
                if (Input.GetKey(KeyCode.LeftShift) && currentRunCooldown <= 0 && currentRunDuration <= runDuration)
                {
                    m_Move *= runSpeed;
                    currentRunDuration += Time.deltaTime;
                    //Script_UI_InGame_Manager.Instance.ActiveBourreauUI();
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift) && currentRunCooldown <= 0 || currentRunDuration >= runDuration && currentRunCooldown <= 0)
                {
                    currentRunCooldown = currentRunDuration;
                    currentRunDuration = 0;
                    //Debug.Log("up");
                }
#endif

                
            }
            else
            {
                m_Move = new Vector3(0,0,0) ;
            }
            // pass all parameters to the character control script
            m_Character.Move(m_Move,false,false);
            //m_Jump = false;

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Fire");
                RaycastHit hit;
                if (Physics.Raycast(detectPoint.transform.position,detectPoint.transform.forward, out hit, 4.0f))
                {
                    
                    Debug.Log(hit);
                    if (hit.collider.gameObject.CompareTag("Door"))
                    {
                        CmdDoor(hit.collider.transform.parent.gameObject);
                    }
                }
            }


            RaycastHit hitting;

            if (Physics.Raycast(detectPoint.transform.position, detectPoint.transform.forward, out hitting, 4.0f))
            {
                if (hitting.transform.GetComponent<InteractableObjectBourreau>())
                {
                    currentObjectHit = hitting.transform.gameObject;
                    Debug.Log(currentObjectHit);
                    foreach (GameObject obj in currentObjectHit.GetComponent<InteractableObjectBourreau>().GetRendererList())
                    {
                        if (obj.GetComponent<Outline>() == null)
                        {
                            obj.AddComponent<Outline>();
                        }
                    }
                }
                else
                {
                    if (currentObjectHit != null)
                    {
                        foreach (GameObject obj in currentObjectHit.GetComponent<InteractableObjectBourreau>().GetRendererList())
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
                    foreach (GameObject obj in currentObjectHit.GetComponent<InteractableObjectBourreau>().GetRendererList())
                    {
                        Destroy(obj.GetComponent<Outline>());
                    }

                    currentObjectHit = null;
                }
            }
        }

        IEnumerator Attack()
        {
            yield return new WaitForSecondsRealtime(attackDelay);
            CmdAttack();
        }

        [Command]
        void CmdPlayAttackSound(int idx)
        {
            RpcPlayAttackSound(idx);
        }

        [ClientRpc]
        void RpcPlayAttackSound(int idx)
        {
            attackASource.PlayOneShot(attackSounds[idx]);
        }

        [Command]
        public void CmdDoor(GameObject doorHit)
        {
            doorHit.GetComponent<Script_Door>().ChangeState();
            Debug.Log(doorHit);
        }

        [Command]
        private void CmdAttack()
        {
            GameObject tempAttack = Instantiate(attackZonePrefab, attackSpawnPoint.position, transform.rotation);
            NetworkServer.Spawn(tempAttack);
            RpcPlayAttackSound(1);
            Destroy(tempAttack, attackCooldown);
        }

        void SetPauseMenu()
        {
            if(Script_UI_InGame_Manager.Instance.GetGameState())
            {
                Script_UI_InGame_Manager.Instance.PauseMenu();
            }
        }

        [Command]
        void CmdCommunicationCome()
        {
            GameObject tempCom = Instantiate(comCome, transform.position, Quaternion.identity);
            NetworkServer.Spawn(tempCom);
        }

        [Command]
        void CmdCommunicationStay()
        {
            GameObject tempCom = Instantiate(comStayHere, transform.position, Quaternion.identity);
            NetworkServer.Spawn(tempCom);
        }

        [Command]
        void CmdCommunicationHeRun()
        {
            GameObject tempCom = Instantiate(comRunAway, transform.position, Quaternion.identity);
            NetworkServer.Spawn(tempCom);
        }

        [Command]
        void CmdCommunicationGotOne()
        {
            GameObject tempCom = Instantiate(comGotOne, transform.position, Quaternion.identity);
            NetworkServer.Spawn(tempCom);
        }

        [Command]
        void CmdComSound(int idxToPlay)
        {
            // RpcTargetSound(idxToPlay);
            if(GameObject.FindGameObjectWithTag("Spectre") != null)
            {
                GameObject.FindGameObjectWithTag("Spectre").GetComponent<Script_Spectre_Moves_Online>().TakeSound(idxToPlay);
            }
            else
            {
                Debug.Log("No spectre");
            }
            
        }

        [ClientRpc]
        public void RpcTargetSound(int idxToPlay)
        {
           comAudioSource.PlayOneShot(comSound[idxToPlay]); 
        }

        public void Loose()
        {
            if(isLocalPlayer)
            {
                Script_UI_InGame_Manager.Instance.GameOver();
            }
        }

        void PlaySound(int idx)
        {
            comAudioSource.PlayOneShot(comSound[idx]);
        }

        public void TakeSound(int toPlay)
        {
            TargetRpcTargetBySound(connectionToClient, toPlay);
        }

        [TargetRpc]
        void TargetRpcTargetBySound(NetworkConnection test, int idxToPlay)
        {
            comAudioSource.PlayOneShot(comSoundSpectre[idxToPlay]);
        }
    }
}
