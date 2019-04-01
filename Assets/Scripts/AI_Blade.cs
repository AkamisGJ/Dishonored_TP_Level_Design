using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Blade : MonoBehaviour
{

    Animator anim;



	// Use this for initialization
	void Start ()
    {
        anim = transform.root.GetComponent<Animator>();
	}
	


	// Update is called once per frame
	void Update ()
    {
		
	}


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player_Health>())
        {
            AnimatorStateInfo animStateInfo;
            animStateInfo = anim.GetCurrentAnimatorStateInfo(0);

            if (animStateInfo.IsName("Base Layer.AttackSword"))
            {
                Player_Health player_Health;
                player_Health = collision.gameObject.GetComponent<Player_Health>();

                player_Health.DamagePlayer(20);
            }
        }
    }
}
