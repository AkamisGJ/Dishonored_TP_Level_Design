using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocket : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}



    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<InteractableObject>() && other.GetComponent<InteractableObject>().isPickup)
        {
            Destroy(other.gameObject);
        }
    }
}
