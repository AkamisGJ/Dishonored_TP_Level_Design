using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Zoom : MonoBehaviour {

    public bool isZooming;
    public GameObject zoomImage;
    public Camera cam;
    public Camera camPlayer;
    MouseLook mouseLook;
    RigidbodyFirstPersonController globalState;
    Text text_Exit;

    // Use this for initialization
    void Start ()
    {
        mouseLook = GameObject.Find("RigidBodyFPSController").GetComponent<MouseLook>();
        globalState = GameObject.Find("RigidBodyFPSController").GetComponent<RigidbodyFirstPersonController>();
        text_Exit = GameObject.Find("Text_Exit").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(!globalState.playerKeyholePeek && Input.GetButtonDown("Zoom"))
        {
            isZooming = !isZooming;
            text_Exit.text = isZooming ? "<b>W</b> <i>Exit Spyglass</i>" : "";
        }

        if(globalState.playerKeyholePeek)
        {
            isZooming = false;
        }

        if(isZooming)
        {
            zoomImage.SetActive(true);
            camPlayer.enabled = false;
            cam.fieldOfView = Mathf.MoveTowards(cam.fieldOfView, 15, Time.deltaTime * 50 * 5);
            mouseLook.XSensitivity = 0.8f;
            mouseLook.YSensitivity = 0.8f;
        }

        else

        {
            zoomImage.SetActive(false);
            camPlayer.enabled = true;
            mouseLook.XSensitivity = 3;
            mouseLook.YSensitivity = 3;
        }
	}
}
