using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Health : MonoBehaviour {

    public bool mainHealth;
    public int maxHp = 40;
    public ParticleSystem FX_Blood;
    public int hp;
    AI_Behaviour AI_Behaviour;
    AI_Health AI_HealthMain;
    CarryAndThrow carryAndThrow;
    GameObject myBodyHips;


    // Use this for initialization
    void Start()
    {
        AI_Behaviour = transform.root.gameObject.GetComponent<AI_Behaviour>();
        if(mainHealth) SetKinematic(true, false);
        hp = maxHp;
        AI_HealthMain = transform.root.gameObject.GetComponent<AI_Health>();
        carryAndThrow = GameObject.Find("MainCamera").GetComponent<CarryAndThrow>();
        myBodyHips = transform.root.Find("mixamorig:Hips").gameObject;
    }


    public void SetKinematic(bool newValue, bool unconscious)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            if (rb.gameObject.layer == 9 || rb.gameObject.layer == 10)
            {
                rb.isKinematic = newValue;
                if (newValue == false) rb.gameObject.layer = 10;
                if (newValue == false) rb.gameObject.tag = "Carryable";
                if (rb.GetComponent<InteractableObject>())
                {
                    rb.GetComponent<InteractableObject>().enabled = !newValue;
                    rb.GetComponent<InteractableObject>().currentState = unconscious ? 1 : 0;
                    rb.GetComponent<InteractableObject>().stringDescription = "Elite Guard (" + (rb.GetComponent<InteractableObject>().currentState == 0 ? "dead" : "unconscious") + ")";
                }
            }
        }
    }

    public void Damage(int DamageInfo, bool FXBlood)
    {
        if (mainHealth)
        {
            if (hp <= 0) return;
            hp -= DamageInfo;
            //FX_Blood.Play();
            if (hp <= 0) Die();
            Debug.Log("HP : " + hp);
        }

        else

        {
            AI_HealthMain.Damage(DamageInfo, FXBlood);
            if(FXBlood) FX_Blood.Play();
        }
    }


    public void Die()
    {
        if (mainHealth)
        {
            AI_Behaviour.isUnconscious = false;
            AI_Behaviour.isDead = true;
            SetKinematic(false, false);
            GetComponent<Animator>().enabled = false;
            Debug.Log("Die");
        }
    }


	
	// Update is called once per frame
	void Update ()
    {
		
	}




    private void OnCollisionEnter(Collision collision)
    {
        GameObject objectCol;
        objectCol = collision.gameObject;

        if (carryAndThrow.objectCarrying != myBodyHips && AI_Behaviour.isUnconscious)
        {
            if (GetComponent<Rigidbody>().velocity.magnitude > 4 && (objectCol.layer != 9 && objectCol.layer != 10)) //Layer AI & DeadBody
            {
                Damage(Mathf.CeilToInt(GetComponent<Rigidbody>().velocity.magnitude), false);
            }
        }
    }
}
