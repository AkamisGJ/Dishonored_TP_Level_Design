using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Air_Assassination : MonoBehaviour {

    public LayerMask layerMaskAssassination;
    public bool canAssassinate;
    public bool isAssassinating;

    GameObject UI_ChokeAndKill;
    RigidbodyFirstPersonController controller;
    AI_Behaviour AI_Behaviour;
    Transform targetAssassination;
    Transform mainCamera;
    Camera cam;
    float timerAssassination;
    MouseLook mouseLook;
    Rigidbody body;
    Blade blade;
    float speedMove = 0;
    Power_Blink power_Blink;




    // Use this for initialization
    void Start ()
    {
        UI_ChokeAndKill = GameObject.Find("UI_ChokeAndKill");
        controller = GameObject.Find("RigidBodyFPSController").GetComponent<RigidbodyFirstPersonController>();
        mainCamera = GameObject.Find("MainCamera").GetComponent<Transform>();
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
        mouseLook = GameObject.Find("RigidBodyFPSController").GetComponent<MouseLook>();
        body = GetComponent<Rigidbody>();
        blade = GameObject.Find("Blade").GetComponent<Blade>();
        power_Blink = GameObject.Find("MainCamera").GetComponent<Power_Blink>();
    }
	


	// Update is called once per frame
	void Update ()
    {
        if (!controller.m_IsGrounded)
        {
            RaycastHit hitAssassination;

            if (!isAssassinating && !power_Blink.isBlinking && Physics.SphereCast(transform.position + Vector3.up * 0.5f, 1.2f, Vector3.down, out hitAssassination, 15, layerMaskAssassination))
            {
                if (hitAssassination.distance >= 2 && hitAssassination.transform.root.gameObject.layer == 9)   //Layer AI
                {
                    AI_Behaviour = hitAssassination.transform.root.gameObject.GetComponent<AI_Behaviour>();

                    if (!AI_Behaviour.isDead && !AI_Behaviour.isUnconscious)
                    {
                        UI_ChokeAndKill.SetActive(true);
                        canAssassinate = true;

                        if (Input.GetButtonDown("Attack") || Input.GetAxis("Attack") > 0.2f)
                        {
                            isAssassinating = true;
                            targetAssassination = hitAssassination.transform.root;
                            targetAssassination.position += Vector3.up * 0.1f;
                            AI_Behaviour = hitAssassination.transform.root.gameObject.GetComponent<AI_Behaviour>();
                            AI_Behaviour.AssassinationByKill();
                            blade.Attack();
                            speedMove = Vector3.Distance(transform.position, targetAssassination.position);
                        }

                        else

                        if (Input.GetButtonDown("Choke"))
                        {
                            isAssassinating = true;
                            targetAssassination = hitAssassination.transform.root;
                            targetAssassination.position += Vector3.up * 0.1f;
                            AI_Behaviour = hitAssassination.transform.root.gameObject.GetComponent<AI_Behaviour>();
                            AI_Behaviour.AssassinationByChoke();
                            speedMove = Vector3.Distance(transform.position, targetAssassination.position);
                        }
                    }
                }

                else

                {
                    UI_ChokeAndKill.SetActive(false);
                    canAssassinate = false;
                }
            }

            else

            {
                UI_ChokeAndKill.SetActive(false);
                canAssassinate = false;
            }
        }

        else

        {
            canAssassinate = false;
        }

        if(isAssassinating)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetAssassination.position + Vector3.up * 0.8f - targetAssassination.forward * 0.2f, Time.deltaTime * speedMove * 1.5f);

            Quaternion orientation;
            orientation = targetAssassination.rotation;
            orientation.eulerAngles = new Vector3(orientation.eulerAngles.x, orientation.eulerAngles.y + 180, orientation.eulerAngles.z);

            Quaternion orientationCam;
            orientationCam = targetAssassination.rotation;
            orientationCam.eulerAngles = new Vector3(75, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, orientation, Time.deltaTime * 10);
            mainCamera.localRotation = Quaternion.Slerp(mainCamera.localRotation, orientationCam, Time.deltaTime * 10);

            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, 35, Time.deltaTime * 30 * 2);

            timerAssassination += Time.deltaTime;
            mouseLook.canRotate = false;
            body.velocity = new Vector3(0, body.velocity.y, 0);

            if (timerAssassination >= 1)
            {
                isAssassinating = false;
                timerAssassination = 0;
                mouseLook.Init(transform, cam.transform);
                mouseLook.canRotate = true;
            }
        }

    }
}
