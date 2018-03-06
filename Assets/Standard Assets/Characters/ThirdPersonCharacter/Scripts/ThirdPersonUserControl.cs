using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;


namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : NetworkBehaviour
    {
        public float walkSpeedMultiply;
        public float runSpeed;
        public float runDuration;
        public float runCooldown;

        private float currentRunDuration ;
        private float currentRunCooldown;


        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        
        private void Start()
        {
            if (!isLocalPlayer)
            {
                this.enabled = false ;
            }
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
        
            if(currentRunCooldown > 0)
            {
                currentRunCooldown -= Time.deltaTime;
                Debug.Log(currentRunCooldown);
            }
            else
            {
               // Debug.Log("READY");
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal") * walkSpeedMultiply;
            float v = CrossPlatformInputManager.GetAxis("Vertical") * walkSpeedMultiply;
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v*m_CamForward + h*m_Cam.right ;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right ;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift) && currentRunCooldown <= 0 && currentRunDuration <= runDuration)
            {
                m_Move *= runSpeed;
                currentRunDuration += Time.deltaTime;
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift) && currentRunCooldown <= 0 || currentRunDuration >= runDuration && currentRunCooldown <= 0)
            {
                currentRunCooldown = currentRunDuration ;
                currentRunDuration = 0;
                //Debug.Log("up");
            }
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
