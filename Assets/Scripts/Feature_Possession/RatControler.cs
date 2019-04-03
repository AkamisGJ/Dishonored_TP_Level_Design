using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof (Rigidbody))]
public class RatControler : MonoBehaviour
{
    [Serializable]
    public class MovementSettings
    {
        public float ForwardSpeed = 8.0f;   // Speed when walking forward
        public float BackwardSpeed = 4.0f;  // Speed when walking backwards
        public float StrafeSpeed = 4.0f;    // Speed when walking sideways
        public float RunMultiplier = 2.0f;   // Speed when sprinting
        public float ChokeMultiplier = 1.0f;    //Speed when the player is choking an AI
        public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
        [HideInInspector] public float CurrentTargetSpeed = 8f;
        public bool isTakingDamages;
        [HideInInspector]
        public bool freezePlayerMovement;

#if !MOBILE_INPUT
        private bool m_Running;
#endif

        public void UpdateDesiredTargetSpeed(Vector2 input)
        {
            //if (input == Vector2.zero) return;

            if (input.y != 0 || input.x != 0)
            {
                CurrentTargetSpeed = (Mathf.Abs(input.y) * (input.y > 0 ? ForwardSpeed : BackwardSpeed) + Mathf.Abs(input.x) * StrafeSpeed) / (Mathf.Abs(input.y) + Mathf.Abs(input.x));
            }

#if !MOBILE_INPUT
            if (Input.GetButton("Run") && ChokeMultiplier == 1)
            {
                CurrentTargetSpeed *= RunMultiplier;
                m_Running = true;
            }
            else
            {
                m_Running = false;
            }

            CurrentTargetSpeed *= ChokeMultiplier;

            CurrentTargetSpeed *= isTakingDamages ? 0.66f : 1;
            if(freezePlayerMovement) CurrentTargetSpeed = 0;
#endif
        }

#if !MOBILE_INPUT
        public bool Running
        {
            get { return m_Running; }
        }
#endif
    }


    [Serializable]
    public class AdvancedSettings
    {
        public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
        public float stickToGroundHelperDistance = 0.5f; // stops the character
        public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
        public bool airControl; // can the user control the direction that is being moved in the air
        [Tooltip("set it to 0.1 or more if you get stuck in wall")]
        public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
    }

    public bool playerKeyholePeek;
    public LayerMask layerMaskGround;
    public LayerMask layerCrouch;
    public bool useGravity = true;
    public bool isLeaning;
    public Camera cam;
    public Transform camPos;
    public MovementSettings movementSettings = new MovementSettings();
    public AdvancedSettings advancedSettings = new AdvancedSettings();

    private RatCamera mouseLook;
    private Rigidbody m_RigidBody;
    private CapsuleCollider m_Capsule;
    private float m_YRotation;
    private Vector3 m_GroundContactNormal;
    public bool m_PreviouslyGrounded, m_IsGrounded;
    
    public Vector3 Velocity
    {
        get { return m_RigidBody.velocity; }
    }

    public bool Grounded
    {
        get { return m_IsGrounded; }
    }

    public bool Running
    {
        get
        {
#if !MOBILE_INPUT
            return movementSettings.Running;
#else
            return false;
#endif
        }
    }


    private void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        mouseLook = GetComponent<RatCamera>();
        mouseLook.Init (transform, cam.transform);
        DisableRat();
    }


    private void Update()
    {
        if (!playerKeyholePeek)
        {
            RotateView();
            Lean();
            PlayerMovement();
        }
    }


    private void FixedUpdate()
    {
        GroundCheck();
        Vector2 input = GetInput();

        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded))
        {
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;
            if (m_IsGrounded)
            {
                desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;
            }

            else

            {
                desiredMove = Vector3.ProjectOnPlane(desiredMove, Vector3.up).normalized;
            }

            desiredMove.x = desiredMove.x*movementSettings.CurrentTargetSpeed;
            desiredMove.z = desiredMove.z*movementSettings.CurrentTargetSpeed;
            desiredMove.y = desiredMove.y*movementSettings.CurrentTargetSpeed;
            //if (m_RigidBody.velocity.sqrMagnitude < (movementSettings.CurrentTargetSpeed*movementSettings.CurrentTargetSpeed))
            //{
                if (!isLeaning)
                {
                    //m_RigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
                    if(m_IsGrounded) desiredMove *= Mathf.Clamp01((Mathf.Abs(input.x) + Mathf.Abs(input.y)));
                    m_RigidBody.MovePosition(m_RigidBody.position + desiredMove /** SlopeMultiplier()*/ * Time.deltaTime);
                }
            //}
        }

        if (m_IsGrounded)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                m_RigidBody.drag = 5f;
            }

            else

            {
                m_RigidBody.drag = 15f;
            }
        }
        else
        {
            m_RigidBody.drag = movementSettings.isTakingDamages ? 15 : 0f;
            /*if (m_PreviouslyGrounded && !m_Jumping)
            {
                StickToGroundHelper();
            }*/
        }

        //m_Capsule.height = Mathf.MoveTowards(m_Capsule.height, 1.9f, 0.7f * Time.deltaTime * 10);
        //camPos.localPosition = Vector3.MoveTowards(camPos.localPosition, new Vector3(isLeaning ? (Input.GetAxis("Lean") * -0.3f) : 0, 0.7f, 0), 0.7f * Time.deltaTime * 5);
        //camPos.localEulerAngles = new Vector3(camPos.localEulerAngles.x, camPos.localEulerAngles.y, isLeaning ? (Input.GetAxis("Lean") * -10) : 0);
    }


    private float SlopeMultiplier()
    {
        float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
        return movementSettings.SlopeCurveModifier.Evaluate(angle);
    }


    private void StickToGroundHelper()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                ((m_Capsule.height/2f) - m_Capsule.radius) +
                                advancedSettings.stickToGroundHelperDistance, layerMaskGround, QueryTriggerInteraction.Ignore))
        {
            if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
            {
                m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
            }
        }
    }


    private Vector2 GetInput()
    {
        
        Vector2 input = new Vector2
            {
                x = CrossPlatformInputManager.GetAxis("Horizontal"),
                y = CrossPlatformInputManager.GetAxis("Vertical")
            };
        movementSettings.UpdateDesiredTargetSpeed(input);
        return input;
    }

    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        mouseLook.LookRotation (transform, cam.transform);

        if (m_IsGrounded || advancedSettings.airControl)
        {
            // Rotate the rigidbody velocity to match the new direction that the character is looking
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
            m_RigidBody.velocity = velRotation*m_RigidBody.velocity;
        }
    }

    void Lean()
    {
        if(m_IsGrounded)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Input.GetAxis("Lean") * 45);
            if(Input.GetAxis("Lean") != 0)
            {
                isLeaning = true;
            }

            else

            {
                isLeaning = false;
            }
        }

        else
        {
            transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y, 0), Time.deltaTime * 30 * 3);
            isLeaning = false;
        }
    }

    /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
    private void GroundCheck()
    {
        m_PreviouslyGrounded = m_IsGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                ((0.25f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, layerMaskGround, QueryTriggerInteraction.Ignore))
        {
            m_IsGrounded = true;
            m_GroundContactNormal = hitInfo.normal;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundContactNormal = Vector3.up;

            if(useGravity)
            {
                m_RigidBody.AddForce(new Vector3(0, -9.8f * 1.5f * (m_RigidBody.drag != 0 ? m_RigidBody.drag * 0.5f : 1), 0), ForceMode.Acceleration);
            }
            
        }
    }

    void PlayerMovement()
    {
        if(movementSettings.freezePlayerMovement)
        {
            m_RigidBody.velocity = Vector3.zero;
        }
    }

    void DisableRat(){
        cam.gameObject.SetActive(false);
        mouseLook.enabled = false;
        this.enabled = false;
    }

}