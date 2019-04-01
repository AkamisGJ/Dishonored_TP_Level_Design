using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class CarryAndThrow : MonoBehaviour {

    public bool isCarrying;
    public Transform carryPos;
    public Transform carryBodyPos;
    public GameObject blade;
    public GameObject objectCarrying;

    Transform previousParent;
    float powerThrow = 1;
    Text text_Drop;
    Text text_Throw;
    Interaction interaction;
    Transform rootBody;
    RigidbodyFirstPersonController globalState;
    Blade bladeScript;

    // Use this for initialization
    void Start ()
    {
        text_Drop = GameObject.Find("Text_Drop").GetComponent<Text>();
        text_Throw = GameObject.Find("Text_Throw").GetComponent<Text>();
        interaction = GetComponent<Interaction>();
        globalState = GameObject.Find("RigidBodyFPSController").GetComponent<RigidbodyFirstPersonController>();
        bladeScript = GameObject.Find("Blade").GetComponent<Blade>();
    }
	


	// Update is called once per frame
	void Update ()
    {
        if (!globalState.playerKeyholePeek)
        {
            Throw();


            if (!isCarrying || !interaction.isInteracted)
            {
                Carry();
            }
        }
	}



    void Throw()
    {
        if (isCarrying && (Input.GetButtonDown("Throw") || Input.GetAxis("Throw") > 0.2f))
        {
            powerThrow = 12;
        }

        if (isCarrying && Input.GetButtonDown("Drop"))
        {
            powerThrow = 0;
        }


        if (isCarrying && ((Input.GetButtonDown("Throw") || Input.GetAxis("Throw") > 0.2f) || (Input.GetButtonDown("Drop") && !interaction.isInteracted)))
        {
            objectCarrying.GetComponent<Rigidbody>().isKinematic = false;
            objectCarrying.GetComponent<Rigidbody>().useGravity = true;
            objectCarrying.GetComponent<Collider>().isTrigger = false;
            objectCarrying.transform.parent = previousParent;
            previousParent = null;
            bladeScript.buttonAxis_Attack = true;
            blade.SetActive(true);
            objectCarrying.GetComponent<Rigidbody>().AddForce(transform.forward * (objectCarrying.layer == 10 ? 3 : 1) * powerThrow + Vector3.up * (powerThrow == 0 ? 0 : 2), ForceMode.VelocityChange);
            isCarrying = false;
            objectCarrying = null;

            text_Drop.text = "";
            text_Throw.text = "";
        }
    }


    void Carry()
    {
        RaycastHit hitCarry;

        if (!isCarrying && Physics.Raycast(transform.position, transform.forward, out hitCarry, 3))
        {
            if (hitCarry.transform.tag == "Carryable")
            {
                if (hitCarry.transform.gameObject.layer == 10) // DeadBody Layer
                {
                    //if (Input.GetButtonDown("Interaction"))
                    //{
                        //Transform rootBody;
                        rootBody = hitCarry.transform.root.Find("mixamorig:Hips");
                        /*previousParent = rootBody.parent;
                        rootBody.parent = carryBodyPos;

                        rootBody.localPosition = new Vector3(0, 0, 0);
                        rootBody.localRotation = Quaternion.identity;

                        Rigidbody body;
                        body = rootBody.GetComponent<Rigidbody>();
                        body.useGravity = false;
                        body.isKinematic = true;

                        rootBody.GetComponent<Collider>().isTrigger = true;

                        objectCarrying = rootBody.gameObject;
                        isCarrying = true;
                        blade.SetActive(false);

                        text_Drop.text = "<i>Drop</i> <b>F</b>";
                        text_Throw.text = "<i>Throw</i> <b>(left mouse button)</b>";*/
                    //}
                }

                else

                {
                    if (Input.GetButtonDown("Interaction"))
                    {
                        hitCarry.transform.parent = carryPos;
                        hitCarry.transform.localPosition = new Vector3(0, 0, 0);
                        hitCarry.transform.localRotation = Quaternion.identity;

                        Rigidbody body;
                        body = hitCarry.transform.GetComponent<Rigidbody>();
                        body.useGravity = false;
                        body.isKinematic = true;

                        hitCarry.collider.isTrigger = true;

                        objectCarrying = hitCarry.transform.gameObject;
                        isCarrying = true;
                        blade.SetActive(false);

                        text_Drop.text = "<i>Drop</i> <b>F</b>";
                        text_Throw.text = "<i>Throw</i> <b>(left mouse button)</b>";
                    }
                }
            }
        }

        if (isCarrying)
        {
            objectCarrying.GetComponent<Rigidbody>().Sleep();
        }
    }

    public void CarryBody()
    {
        previousParent = rootBody.parent;
        rootBody.parent = carryBodyPos;

        rootBody.localPosition = new Vector3(0, 0, 0);
        rootBody.localRotation = Quaternion.identity;

        Rigidbody body;
        body = rootBody.GetComponent<Rigidbody>();
        body.useGravity = false;
        body.isKinematic = true;

        rootBody.GetComponent<Collider>().isTrigger = true;

        objectCarrying = rootBody.gameObject;
        isCarrying = true;
        blade.SetActive(false);

        text_Drop.text = "<i>Drop</i> <b>F</b>";
        text_Throw.text = "<i>Throw</i> <b>(left mouse button)</b>";
    }
}
