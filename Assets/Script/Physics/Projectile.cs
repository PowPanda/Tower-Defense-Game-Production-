using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    Rigidbody rb;
    public float shootForce;
    public PlayerController player;
    public GameObject explosionParticle;

    void OnEnable () {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<TrailRenderer>().enabled = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        ApplyForce();
	}
	
	// Update is called once per frame
	void Update () {
        SpinObjectInAir();
	}

    void ApplyForce()
    {
        rb.AddForce(Camera.main.transform.forward * shootForce);
    }

    void SpinObjectInAir()
    {
        float yVelocity = rb.velocity.y;
        float zVelocity = rb.velocity.z;
        float xVelocity = rb.velocity.x;
        float combinedVelocity = Mathf.Sqrt(xVelocity * xVelocity + zVelocity * zVelocity);

        float fallAngle = Mathf.Atan2(yVelocity, combinedVelocity) * 180 / Mathf.PI;    // To convert radian to degree

        transform.eulerAngles = new Vector3(fallAngle, transform.eulerAngles.y, transform.eulerAngles.z);   // We only want to change x so y and z will remain
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name);
        // Add force to push enemy
        if (col.gameObject.tag == "Enemy")
        {
            if (gameObject.tag == "Arrow")
            {
                col.gameObject.GetComponent<EnemyController>().getHit = true;
                float power = 1000.0f;
                Vector3 exploPosition = col.contacts[0].point;
                //col.gameObject.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Acceleration);
                col.gameObject.GetComponent<Rigidbody>().AddExplosionForce(power, exploPosition, 0f, 100.0f);
                Destroy(gameObject);
            }

            else if (gameObject.tag == "FireArrow")
            {
                //Debug.Log("Enemy Hit");
                AreaDamage(col.contacts[0].point, 500.0f);
                Destroy(Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation), 2.0f);
                Destroy(gameObject);
            }
        }

        else if (col.gameObject.tag == "RedBalloon")
        {
            player.fireArrowCount += 1;
            Destroy(col.gameObject);
            Destroy(gameObject);
        }

        else if (col.gameObject.tag == "Terrain")
        {
            //Debug.Log("Terrain Hit");
            if (gameObject.tag == "FireArrow")
            {
                AreaDamage(col.contacts[0].point, 500.0f);
                Destroy(Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation), 2.0f);
                Destroy(gameObject);
            }

            Destroy(gameObject);
        }
    }

    void AreaDamage(Vector3 location, float radius)
    {
        Vector3 exploPosition = location;
        float power = 500.0f;

        Collider[] objectsInRange = Physics.OverlapSphere(location, radius);

        foreach (Collider col in objectsInRange)
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponentInParent<EnemyController>().getHit = true;
                col.gameObject.GetComponentInParent<Rigidbody>().AddExplosionForce(power, exploPosition, 5.0f, 30.0f);
            }
        }
    }
}
