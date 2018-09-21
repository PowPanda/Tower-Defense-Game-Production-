using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour {

    Rigidbody rb;
    float floatingSpeed;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Invoke("FloatUp", 0.5f);
	}

    void FloatUp()
    {
        rb.AddForce(new Vector3(0,0.5f,0));
        Invoke("Destroy", 5.0f);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
