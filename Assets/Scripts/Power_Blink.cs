using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Power_Blink : MonoBehaviour {

    public GameObject blinkPosition;
    public Projector blinkProjector;
    public GameObject blinkClimb;
    public GameObject blinkGround;
    public Image blinkSpeedEffect;
    public ParticleSystem blinkParticle;
    public bool isBlinking;


    Transform blinkTransform;
    CapsuleCollider playerCollider;
    Collider camCollider;
    bool climbBlink;
    Vector3 climbBlinkPos;
    
    Camera cam;
    Color blinkSpeedEffectColor;
    float fovByDist = 15;
    RigidbodyFirstPersonController controller;
    Zoom zoom;
    RigidbodyFirstPersonController globalState;
    Air_Assassination air_Assassination;
    bool buttonAxis_UseLeft = true;
    float velocityBlinkSpeedEffectColor;
    Vector3 velocityMovement;
    Vector3 velocityScaleSpeedEffect;
    float velocityBlinkFOV;
    float durationBlink = 0;
    float offsetFloor;
    Vector3 posPlayerClimb;
    float distFromGround;


    // Use this for initialization
    void Start ()
    {
        blinkTransform = blinkPosition.GetComponent<Transform>();
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
        blinkSpeedEffectColor = blinkSpeedEffect.color;
        controller = GameObject.Find("RigidBodyFPSController").GetComponent<RigidbodyFirstPersonController>();
        zoom = GetComponent<Zoom>();
        globalState = GameObject.Find("RigidBodyFPSController").GetComponent<RigidbodyFirstPersonController>();
        air_Assassination = transform.root.GetComponent<Air_Assassination>();
        playerCollider = GameObject.Find("RigidBodyFPSController").GetComponent<CapsuleCollider>();
        camCollider = GameObject.Find("MainCamera").GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxis("UseLeft") > 0.2f) buttonAxis_UseLeft = false;

        if (!globalState.playerKeyholePeek && (Input.GetButton("UseLeft") || Input.GetAxis("UseLeft") > 0.2f) && !isBlinking)
        {
            blinkProjector.enabled = true;
            durationBlink = 0;

            //-----Feedback pos blink on ground-----
            RaycastHit hitBlinkGround;

            if(Physics.SphereCast(blinkTransform.position, 0.3f, Vector3.down, out hitBlinkGround, 50))
            {
                blinkGround.SetActive(true);
                blinkGround.transform.position = hitBlinkGround.point + Vector3.up * 0.25f;
                distFromGround = hitBlinkGround.distance;
            }

            else

            {
                blinkGround.SetActive(false);
            }
            //----------

            if (!blinkParticle.isPlaying)
            {
               blinkParticle.Play();
            }

            RaycastHit hitBlink;
            

            if (Physics.Raycast(transform.position, transform.forward, out hitBlink, 4f) || Physics.SphereCast(transform.position + transform.forward * 2f, 0.35f, transform.forward, out hitBlink, 9.65f))
            {
                offsetFloor = Mathf.Clamp01((playerCollider.height * 0.5f) - hitBlinkGround.distance - 0.25f);

                blinkTransform.position = hitBlink.point + (hitBlink.normal * 0.5f);
                fovByDist = hitBlink.distance + 2;

                //-----Test Blink Climb-----
                RaycastHit hitClimbBlinkGround;

                if (Physics.SphereCast(blinkTransform.position - hitBlink.normal * 0.7f + Vector3.up, 0.1f, Vector3.down, out hitClimbBlinkGround, 5))
                {
                    if (!Physics.CheckSphere(hitClimbBlinkGround.point + Vector3.up * 0.7f, 0.5f) && !Physics.CheckCapsule(blinkTransform.position - Vector3.up * 0.2f, blinkTransform.position + Vector3.up * 0.6f, 0.4f))
                    {
                        climbBlink = true;
                        climbBlinkPos = hitClimbBlinkGround.point + (Vector3.up * playerCollider.height * 0.5f);
                        blinkClimb.SetActive(true);
                        posPlayerClimb = hitClimbBlinkGround.point;
                    }

                    else

                    {
                        climbBlink = false;
                        blinkClimb.SetActive(false);
                    }
                }

                else

                {
                    climbBlink = false;
                    blinkClimb.SetActive(false);
                }
                //----------
            }

            else

            {
                blinkTransform.position = transform.position + transform.forward * 12f;
                climbBlink = false;
                blinkClimb.SetActive(false);
                fovByDist = 15;
                offsetFloor = Mathf.Clamp01((playerCollider.height * 0.5f) - hitBlinkGround.distance - 0.25f);
            }
        }

        else

        {
            blinkParticle.Stop();
            blinkParticle.Clear();
            blinkProjector.enabled = false;
            blinkGround.SetActive(false);
        }

        if(!globalState.playerKeyholePeek && Vector3.Distance(transform.position, blinkTransform.position) > 2)
        {
            var trails = blinkParticle.trails;
            trails.enabled = true;

            if (Input.GetButtonUp("UseLeft") || (Input.GetAxis("UseLeft") <= 0.2f && buttonAxis_UseLeft == false))
            {
                buttonAxis_UseLeft = true;
                RaycastHit RaycastUp;

                if ((climbBlink && Physics.SphereCast(posPlayerClimb + Vector3.up * 0.5f, 0.4f, Vector3.up, out RaycastUp, 1.2f)) || (!climbBlink && Physics.SphereCast(blinkTransform.position, 0.4f, Vector3.up, out RaycastUp, 0.75f)))
                {
                    transform.parent.GetComponent<RigidbodyFirstPersonController>().movementSettings.isCrouched = true;
                    transform.parent.GetComponent<RigidbodyFirstPersonController>().UI_Crouch.SetActive(true);
                    transform.parent.GetComponent<CapsuleCollider>().height = 1.2f;
                    Debug.Log("Crouch after blink");
                    offsetFloor = Mathf.Clamp01((playerCollider.height * 0.5f) - (climbBlink ? 0 : distFromGround) - 0.25f);
                    climbBlinkPos = posPlayerClimb + (Vector3.up * playerCollider.height * 0.5f);
                }

                isBlinking = true;

                blinkClimb.SetActive(false);
                cam.fieldOfView = 105;
                blinkSpeedEffectColor.a = 0.3f;
                blinkSpeedEffect.color = blinkSpeedEffectColor;
                blinkSpeedEffect.rectTransform.localScale = Vector3.one;
                blinkParticle.Stop();
                blinkParticle.Clear();
            }
        }

        else

        {
            var trails = blinkParticle.trails;
            trails.enabled = false;

            blinkClimb.SetActive(false);
            if(Input.GetAxis("UseLeft") <= 0.2f) buttonAxis_UseLeft = true;
        }

        if(isBlinking)
        {
            transform.parent.transform.position = Vector3.SmoothDamp(transform.parent.transform.position, climbBlink ? climbBlinkPos : (blinkTransform.position + Vector3.up * offsetFloor), ref velocityMovement, 0.12f);
            //blinkSpeedEffect.rectTransform.localScale = Vector3.SmoothDamp(blinkSpeedEffect.rectTransform.localScale, new Vector3(12, 12, 1), ref velocityScaleSpeedEffect, 0.1f);

            globalState.useGravity = false;
            globalState.movementSettings.freezePlayerMovement = true;
            durationBlink += Time.deltaTime;
            playerCollider.isTrigger = true;
            camCollider.isTrigger = true;

            if (Vector3.Distance(transform.parent.transform.position, climbBlink ? climbBlinkPos : (blinkTransform.position + Vector3.up * offsetFloor)) < 0.1 || durationBlink > 1f)
            {
                isBlinking = false;
                globalState.useGravity = true;
                globalState.movementSettings.freezePlayerMovement = false;
                playerCollider.isTrigger = false;
                camCollider.isTrigger = false;
                durationBlink = 0;
                playerCollider.GetComponent<Rigidbody>().AddForce(Vector3.down * 1, ForceMode.VelocityChange);
            }
        }

        if(!zoom.isZooming && !air_Assassination.isAssassinating) cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, 65, ref velocityBlinkFOV, 0.1f * fovByDist * 0.1f);
        if (zoom.isZooming) fovByDist = 15;
        blinkSpeedEffectColor.a = Mathf.SmoothDamp(blinkSpeedEffectColor.a, 0, ref velocityBlinkSpeedEffectColor, 0.2f);
        blinkSpeedEffect.color = blinkSpeedEffectColor;
        blinkSpeedEffect.rectTransform.localScale = Vector3.SmoothDamp(blinkSpeedEffect.rectTransform.localScale, new Vector3(12, 12, 1), ref velocityScaleSpeedEffect, 0.1f);
    }
}
