using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruction : MonoBehaviour {

    public float delayDestruction = 5;

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, delayDestruction);
	}
}
