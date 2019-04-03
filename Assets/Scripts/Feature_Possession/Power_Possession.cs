using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Power_Possession : MonoBehaviour
{
    [SerializeField] float m_maxDistanceToPossession = 5;
    [SerializeField] LayerMask m_objectToTouch;
    [SerializeField] bool m_showRayCast = true;

    Camera m_camera;
    GameObject m_player;
    
    void Start(){
        m_camera = GetComponent<Camera>();
        m_player = GetComponentInParent<RigidbodyFirstPersonController>().gameObject;
    }

    void Update(){
        RayCast();
    }

    void RayCast(){
        RaycastHit hit;
        Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);

        if(m_showRayCast){
            Debug.DrawRay(ray.origin, ray.direction * m_maxDistanceToPossession, Color.white, 0.025f);
        }

        if(Input.GetKey(KeyCode.Alpha3)){
            if(Physics.Raycast(ray.origin, ray.direction, out hit, m_maxDistanceToPossession, m_objectToTouch)){
                RatManager rm = hit.collider.gameObject.GetComponent<RatManager>();
                if(rm != null){
                    rm.SetPowerPossession(this);
                    rm.EnableRat();
                    StartCoroutine(EnablePlayer(false));
                }
            }
        }
    }

    IEnumerator EnablePlayer(bool b){
        yield return new WaitForSeconds(.1f);
        if(b){
            m_player.SetActive(true);
        }else{
            m_player.SetActive(false);
        }
        
    }

}
