using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;
using UnityEngine.AI;


namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class Script_Bourreau_Moves : NetworkBehaviour
    {
        [Header("Movement")]
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

        [Header("Attack")]
        public float attackCooldown;
        public Transform attackSpawnPoint;
        public GameObject attackZonePrefab;
        private float currentAttackCooldown;

        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        
        private void Start()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            gameObject.tag = "Bourreau";

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

            if (currentRunCooldown > 0)
            {
                currentRunCooldown -= Time.deltaTime;
                Script_UI_InGame_Manager.Instance.UpdateBourreauStamina(currentRunCooldown, runDuration);
            }

            if(currentAttackCooldown > 0)
            {
                currentAttackCooldown -= Time.deltaTime;
            }

            if(Input.GetButtonDown("Fire1") && currentAttackCooldown <= 0)
            {
                currentAttackCooldown = attackCooldown ;
                CmdAttack();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetPauseMenu();
            }

            if (Input.GetButtonDown("Communication_Come"))
            {
                CmdCommunication(comCome);
                Debug.Log(comCome);
            }
            else if (Input.GetButtonDown("Communication_GotOne"))
            {
                CmdCommunication(comGotOne);
                Debug.Log(comGotOne);
            }
            else if (Input.GetButtonDown("Communication_HeRunAway"))
            {
                CmdCommunication(comRunAway);
                Debug.Log(comRunAway);
            }
            else if (Input.GetButtonDown("Communication_StayHere"))
            {
                CmdCommunication(comStayHere);
                Debug.Log(comStayHere);
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            if (currentAttackCooldown <= 0)
            {
                // read inputs
                float h = CrossPlatformInputManager.GetAxis("Horizontal");
                float v = CrossPlatformInputManager.GetAxis("Vertical");
                

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
                    Script_UI_InGame_Manager.Instance.ActiveBourreauUI();
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
        }

        [Command]
        private void CmdAttack()
        {
            currentAttackCooldown = attackCooldown ;
            GameObject tempAttack = Instantiate(attackZonePrefab, attackSpawnPoint.position, Quaternion.identity);
            NetworkServer.Spawn(tempAttack);
            Destroy(tempAttack, currentAttackCooldown);
            //NetworkServer.UnSpawn(tempAttack);
        }

        void SetPauseMenu()
        {
            Script_UI_InGame_Manager.Instance.PauseMenu();
        }

        [Command]
        void CmdCommunication(GameObject toSpawn)
        {
            GameObject tempCom = Instantiate(toSpawn, transform.position, Quaternion.identity);
            NetworkServer.Spawn(tempCom);
            Debug.Log("COM");
        }
    }
}
