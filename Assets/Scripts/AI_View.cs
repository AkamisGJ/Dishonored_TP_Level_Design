using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_View : MonoBehaviour {

    public bool mainView;
    public LayerMask layerViewPlayer;

    Collider playerCol;
    AI_Behaviour AI_Behaviour;
    Transform camPlayer;
    Power_Blink power_Blink;

	// Use this for initialization
	void Start ()
    {
        playerCol = GameObject.Find("RigidBodyFPSController").GetComponent<Collider>();
        AI_Behaviour = transform.root.GetComponent<AI_Behaviour>();
        camPlayer = GameObject.Find("MainCamera").GetComponent<Transform>();
        power_Blink = GameObject.Find("MainCamera").GetComponent<Power_Blink>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(mainView)
        {
            if (AI_Behaviour.viewPlayer >= 1 && !AI_Behaviour.isDead && !AI_Behaviour.isUnconscious && !AI_Behaviour.isChoking && !AI_Behaviour.isKilling && !AI_Behaviour.knockoutByChoke)
            {
                RaycastHit hitPlayer;

                if (Physics.Linecast(transform.position, camPlayer.position, out hitPlayer, layerViewPlayer))
                {
                    //Layer Player
                    if (hitPlayer.transform.gameObject.layer == 11 && !power_Blink.isBlinking)
                    {
                        AI_Behaviour.distanceToPlayer = hitPlayer.distance;
                        AI_Behaviour.angleToPlayer = Vector3.Angle(-transform.up, (camPlayer.position - transform.position));
                        AI_Behaviour.isSeeingPlayer = true;
                        AI_Behaviour.targetLookAt = camPlayer;
                    }

                    else

                    {
                        AI_Behaviour.isSeeingPlayer = false;
                    }
                }

                else

                {
                    AI_Behaviour.isSeeingPlayer = false;
                }
            }

            else

            {
                AI_Behaviour.isSeeingPlayer = false;
            }
        }
	}


    private void OnTriggerEnter(Collider other)
    {
        if(other == playerCol)
        {
            AI_Behaviour.ViewPlayer(1);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other == playerCol)
        {
            AI_Behaviour.ViewPlayer(-1);
        }
    }
}
