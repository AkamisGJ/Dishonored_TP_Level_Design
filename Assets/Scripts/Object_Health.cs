using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Health : MonoBehaviour {

    public int maxHp = 10;
    public GameObject FX_Destroy;
    public AudioClip sound_Destroy;
    public GameObject sound_Source;

    private int hp;
    Rigidbody body;
    float currentVelocity;

    // Use this for initialization
    void Start ()
    {
        if(GetComponent<Rigidbody>()) body = GetComponent<Rigidbody>();
        hp = maxHp;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(body) currentVelocity = body.velocity.magnitude;
    }



    public void Damage(int DamageInfo)
    {
        if (hp <= 0) return;
        hp -= DamageInfo;
        if (hp <= 0) Die();
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject objectCol;
        objectCol = collision.gameObject;

        if (objectCol.GetComponent<Rigidbody>() && objectCol.GetComponent<Rigidbody>().velocity.magnitude > 2 && (objectCol.layer != 9 && objectCol.layer != 11)) //Layer AI or Player
        {
            Damage(10);
        }

        if (body != null && currentVelocity > 6)
        {
            Die();
        }
    }



    void Die()
    {
        GameObject mySoundSource = Instantiate(sound_Source, transform.position, Quaternion.identity);
        mySoundSource.GetComponent<AudioSource>().clip = sound_Destroy;
        mySoundSource.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        mySoundSource.GetComponent<AudioSource>().Play();

        if (FX_Destroy)
        {
            GameObject myFX_Destroy = Instantiate(FX_Destroy, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
