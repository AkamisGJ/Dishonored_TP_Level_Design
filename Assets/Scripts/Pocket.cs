using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocket : MonoBehaviour {

	Player_Health Player_Health;
	void Start ()
    {
		Player_Health = GetComponentInParent<Player_Health>();
	}



    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<InteractableObject>() && other.GetComponent<InteractableObject>().isPickup) //Elixir de vie
        {
            InteractableObject incommingObject = other.GetComponent<InteractableObject>();
            if(incommingObject.d == InteractableObject.Description.Elixir && incommingObject.objectType == 0){
                float healAmont = incommingObject.GetComponent<HealPotion>().HealAmmont;
                Player_Health.HealPlayer(healAmont);
            }
            Destroy(other.gameObject);
        }
    }
}
