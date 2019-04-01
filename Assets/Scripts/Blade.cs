using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Blade : MonoBehaviour {

    Animation anim;
    int isCombo = 0;
    bool isAttacking;
    RigidbodyFirstPersonController globalState;
    [HideInInspector]
    public bool buttonAxis_Attack;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animation>();
        globalState = GameObject.Find("RigidBodyFPSController").GetComponent<RigidbodyFirstPersonController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(!globalState.playerKeyholePeek && (Input.GetButtonDown("Attack") || (Input.GetAxis("Attack") > 0.2f && buttonAxis_Attack == false)))
        {
            Attack();
            buttonAxis_Attack = true;
        }

        if (Input.GetAxis("Attack") <= 0.2f) buttonAxis_Attack = false;

        if (!anim.isPlaying)
        {
            isCombo = 0;
            isAttacking = false;
        }

        else

        {
            isAttacking = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isAttacking)
        {
            if(other.GetComponent<AI_Health>())
            {
                other.GetComponent<AI_Health>().Damage(10, true);
            }

            if(other.GetComponent<Object_Health>())
            {
                other.GetComponent<Object_Health>().Damage(10);
            }
        }
        
    }



    public void Attack()
    {
        if (anim.IsPlaying("Blade_Attack_Combo") && isCombo % 2 == 0)
        {
            anim.PlayQueued("Blade_Attack");
            isCombo++;
        }

        if (anim.IsPlaying("Blade_Attack") && isCombo % 2 == 1)
        {
            anim.PlayQueued("Blade_Attack_Combo");
            isCombo++;
        }

        if (isCombo == 0)
        {
            anim.Play("Blade_Attack");
            isCombo++;
        }
    }
}


