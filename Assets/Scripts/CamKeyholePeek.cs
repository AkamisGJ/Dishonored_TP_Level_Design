using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class CamKeyholePeek : MonoBehaviour {

    public float XSensitivity = 3;
    public float YSensitivity = 3;
    public float MinimumX = -45;
    public float MaximumX = 45;
    public float MinimumY = -45;
    public float MaximumY = 45;
    public float smoothTime = 20;

    Quaternion m_CameraTargetRot;
    RectTransform keyholePeek;
    RectTransform keyholePeekWhite;
    float multi = 0.4f;
    Text text_Exit;
    RigidbodyFirstPersonController globalState;

    // Use this for initialization
    void Start ()
    {
        m_CameraTargetRot = transform.localRotation;
        keyholePeek = GameObject.Find("KeyholePeek").GetComponent<RectTransform>();
        keyholePeekWhite = GameObject.Find("KeyholePeek_White").GetComponent<RectTransform>();
        text_Exit = GameObject.Find("Text_Exit").GetComponent<Text>();
    }



    private void OnEnable()
    {
        text_Exit = GameObject.Find("Text_Exit").GetComponent<Text>();
        text_Exit.text = "<b>F</b> <i>Exit Keyhole</i>";

        keyholePeek = GameObject.Find("KeyholePeek").GetComponent<RectTransform>();
        keyholePeekWhite = GameObject.Find("KeyholePeek_White").GetComponent<RectTransform>();
        keyholePeek.GetComponent<Image>().enabled = true;
        keyholePeekWhite.GetComponent<Image>().enabled = true;
        globalState = GameObject.Find("RigidBodyFPSController").GetComponent<RigidbodyFirstPersonController>();
        globalState.playerKeyholePeek = true;
    }



    // Update is called once per frame
    void Update ()
    {
        float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
        float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

        m_CameraTargetRot *= Quaternion.Euler(-xRot, yRot, 0f);

        m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, m_CameraTargetRot, smoothTime * Time.deltaTime);
        keyholePeek.localRotation = transform.localRotation;
        keyholePeekWhite.localRotation = transform.localRotation;
        keyholePeek.localRotation = new Quaternion(keyholePeek.localRotation.x * -multi, keyholePeek.localRotation.y * -multi, keyholePeek.localRotation.z, keyholePeek.localRotation.w);
        keyholePeekWhite.localRotation = new Quaternion(keyholePeekWhite.localRotation.x * -multi /** 2.5f*/, keyholePeekWhite.localRotation.y * -multi /** 2.5f*/, -keyholePeekWhite.localRotation.z, keyholePeekWhite.localRotation.w);
        keyholePeekWhite.localPosition = new Vector3(-transform.localRotation.y * 180, -109 + transform.localRotation.x * 180, 120);

        ExitKeyhole();
    }



    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
        angleY = Mathf.Clamp(angleY, MinimumY, MaximumY);
        q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

        q.z = 0;

        return q;
    }



    void ExitKeyhole()
    {
        if(Input.GetButtonDown("Interaction"))
        {
            transform.localRotation = Quaternion.identity;
            m_CameraTargetRot = Quaternion.identity;
            text_Exit.text = "";
            gameObject.SetActive(false);
            keyholePeek.GetComponent<Image>().enabled = false;
            keyholePeekWhite.GetComponent<Image>().enabled = false;
            globalState.playerKeyholePeek = false;
        }
    }
}
