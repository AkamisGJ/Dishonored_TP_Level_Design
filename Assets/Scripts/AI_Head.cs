using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Head : MonoBehaviour {


    AI_Behaviour AI_Behaviour;
    AI_Health AI_Health;



	// Use this for initialization
	void Start ()
    {
        AI_Behaviour = transform.root.gameObject.GetComponent<AI_Behaviour>();
        AI_Health = transform.root.gameObject.GetComponent<AI_Health>();
    }
	


	// Update is called once per frame
	void Update () {
		
	}



    private void OnCollisionEnter(Collision collision)
    {
        GameObject objectCol;
        objectCol = collision.gameObject;

        if (objectCol.GetComponent<Rigidbody>() && objectCol.GetComponent<Rigidbody>().velocity.magnitude > 6 && (objectCol.layer != 9 && objectCol.layer != 11)) //Layer AI & Player
        {
            Knockout();
        }
    }



    public void Knockout()
    {
        if (!AI_Behaviour.isDead)
        {
            AI_Behaviour.agent.enabled = false;
            AI_Behaviour.isUnconscious = true;
            AI_Health.SetKinematic(false, true);
            AI_Behaviour.gameObject.GetComponent<Animator>().enabled = false;
        }
    }
}
