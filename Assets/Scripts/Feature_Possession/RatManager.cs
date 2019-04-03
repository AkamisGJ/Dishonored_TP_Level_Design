using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class RatManager : MonoBehaviour
{
    public CheckTopCollider m_checkTopCollider = new CheckTopCollider();
    [Serializable] public class CheckTopCollider
    {
        public LayerMask m_checkTopColider;
        public Vector3 m_baseOffset = new Vector3(0, 0.475f, 0);
        public float m_radius = .45f;
        public float m_maxDistance = .25f;
        public bool m_showGizmos = false;
    }

    RigidbodyFirstPersonController m_player;
    Power_Possession m_powerPossession;
    RatControler m_ratControler;
    RatCamera m_ratCamera;
    Camera m_cam;
    bool m_canDieRat = false;

    void Awake(){
        m_ratControler = GetComponent<RatControler>();
        m_ratCamera = GetComponentInChildren<RatCamera>();
        m_cam = GetComponentInChildren<Camera>();
        Enable(false);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Alpha3) && m_canDieRat){
            RaycastHit hit;
            if(!Physics.SphereCast(transform.position + m_checkTopCollider.m_baseOffset, m_checkTopCollider.m_radius, Vector3.up, out hit, m_checkTopCollider.m_maxDistance, m_checkTopCollider.m_checkTopColider)){
                DisableRat();
            }
        }
    }

    void OnDrawGizmos(){
        if(!m_checkTopCollider.m_showGizmos)
            return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + m_checkTopCollider.m_baseOffset, m_checkTopCollider.m_radius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + m_checkTopCollider.m_baseOffset + (Vector3.up * m_checkTopCollider.m_maxDistance), m_checkTopCollider.m_radius);
    }

    void Enable(bool b){
        m_ratControler.enabled = b;
        m_ratCamera.enabled = b;
        m_cam.gameObject.SetActive(b);
    }

    public void SetPowerPossession(Power_Possession pp){
        m_powerPossession = pp;
        if(m_player == null){
            m_player = m_powerPossession.GetComponentInParent<RigidbodyFirstPersonController>();
        }
    }

    public void EnableRat(){
        Enable(true);
        StartCoroutine(TimeToDieRat());
    }

    public void DisableRat(){
        Enable(false);

        m_canDieRat = false;

        m_player.gameObject.transform.position = transform.position + Vector3.up * .1f;
        m_player.gameObject.GetComponentInChildren<Camera>().transform.rotation = transform.rotation;

        m_player.gameObject.SetActive(true);
    }

    IEnumerator TimeToDieRat(){
        yield return new WaitForSeconds(0.5f);
        m_canDieRat = true;
    }

}
