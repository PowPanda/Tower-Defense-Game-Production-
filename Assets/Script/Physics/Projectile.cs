using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    Rigidbody rigidB;
    public float shootForce;
    public PlayerController player;
    public GameObject explosionParticle;

    void OnEnable () {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<TrailRenderer>().enabled = true;
        player = GameObject.Find("Player/Main Camera/Bow").GetComponent<PlayerController>();
        rigidB = GetComponent<Rigidbody>();
        rigidB.velocity = Vector3.zero;
        ApplyForce();
	}
	
	// Update is called once per frame
	void Update () {
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
        // Act force to push enemy
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "FlyingEnemy")
        {
            if (gameObject.tag == "Arrow")
            {
                float power = 1000.0f;
                Vector3 exploPosition = col.contacts[0].point;
                //col.gameObject.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Acceleration);

                col.gameObject.GetComponent<Rigidbody>().AddExplosionForce(power, exploPosition, 0f, 100.0f);
                Destroy(gameObject);
            }

            else if (gameObject.tag == "FireArrow")
            {
                AreaDamage(col.contacts[0].point, 5.0f);
                Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
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
            if (gameObject.tag == "FireArrow")
            {
                AreaDamage(col.contacts[0].point, 5.0f);
                Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
                Destroy(gameObject);
            }

            Destroy(gameObject);
        }
    }

    void AreaDamage(Vector3 location, float radius)
    {
        Vector3 exploPosition = location;
        float power = 1000.0f;

        Collider[] objectsInRange = Physics.OverlapSphere(location, radius);

        foreach (Collider col in objectsInRange)
        {
            if (col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponent<EnemyController>().getHit = true;
                col.gameObject.GetComponent<Rigidbody>().AddExplosionForce(power, exploPosition, radius, 100.0f);
            }

            if (col.gameObject.tag == "FlyingEnemy")
            {
                col.gameObject.GetComponent<FlyingEnemyController>().getHit = true;
                col.gameObject.GetComponent<Rigidbody>().AddExplosionForce(power, exploPosition, radius, 100.0f);
            }
        }
    }
}
