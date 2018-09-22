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
        FloatUp();
	}

    void FloatUp()
    {
        rb.AddForce(new Vector3(0, 1.0f, 0));
        Invoke("Destroy", 8.0f);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
