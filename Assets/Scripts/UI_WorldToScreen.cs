using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WorldToScreen : MonoBehaviour {

    public bool billboardWorld = true;
    public Transform target;
    public bool fixedOnScreen;
    public bool applyScale = true;


    Transform camPos;
    Camera cam;
    Vector3 screenView;


    // Use this for initialization
    void Start ()
    {
        camPos = GameObject.Find("Camera_UI").GetComponent<Transform>();
        cam = GameObject.Find("Camera_UI").GetComponent<Camera>();
        if (!billboardWorld)
        {
            transform.parent.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            transform.parent.GetComponent<Canvas>().worldCamera = cam;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (billboardWorld)
        {
            transform.rotation = camPos.rotation;
        }

        else

        {
            if (fixedOnScreen)
            {
                screenView = cam.WorldToViewportPoint(target.position);
                if (screenView.x < 0.05) screenView.x = 0.05f;
                if (screenView.x > 0.95) screenView.x = 0.95f;
                if (screenView.y < 0.05) screenView.y = 0.05f;
                if (screenView.y > 0.95) screenView.y = 0.95f;
            }

            Vector3 screenPos;
            if(!fixedOnScreen) screenPos = cam.ViewportToScreenPoint(screenView);

            screenPos = cam.WorldToScreenPoint(target.position);
            screenPos = new Vector3(screenPos.x - Screen.width * 0.5f, screenPos.y - Screen.height * 0.5f, 0);
            transform.localPosition = screenPos;

            if(applyScale) transform.localScale = Vector3.one / Mathf.Max(1.0f, (1 + Vector3.Distance(target.position, camPos.position) * 0.2f));
        }
    }
}
