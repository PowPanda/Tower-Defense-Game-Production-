using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour {

    Rigidbody rigidB;
    public float shootForce;
    public PlayerController player;

    void OnEnable()
    {
        GetComponent<BoxCollider>().enabled = true;
        player = GameObject.Find("Player/Main Camera/Bow").GetComponent<PlayerController>();
        rigidB = GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.zero;
        ApplyForce();
    }

    // Update is called once per frame
    void Update()
    {
        SpinObjectInAir();
    }

    void ApplyForce()
    {
        rigidB.AddForce(Camera.main.transform.forward * shootForce);
    }

    void SpinObjectInAir()
    {
        float yVelocity = rigidB.velocity.y;
        float zVelocity = rigidB.velocity.z;
        float xVelocity = rigidB.velocity.x;
        float combinedVelocity = Mathf.Sqrt(xVelocity * xVelocity + zVelocity * zVelocity);

        float fallAngle = Mathf.Atan2(yVelocity, combinedVelocity) * 180 / Mathf.PI;    // To convert radian to degree

        transform.eulerAngles = new Vector3(fallAngle, transform.eulerAngles.y, transform.eulerAngles.z);   // We only want to change x so y and z will remain
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            AreaDamage(transform.position, 1.0f);
            Destroy(gameObject);
        }

        if (col.gameObject.tag == "RedBalloon")
        {
            player.fireArrowCount += 1;
            Destroy(col.gameObject);
            Destroy(gameObject);
        }

        if (col.gameObject.tag == "Terrain")
            Destroy(gameObject);
    }

    void AreaDamage(Vector3 location, float radius)
    {
        Collider[] objectsInRange = Physics.OverlapSphere(location, radius);

        foreach (Collider col in objectsInRange)
        {
            if (col.gameObject.tag == "Enemy")
                Destroy(col.gameObject);
        }
    }
}