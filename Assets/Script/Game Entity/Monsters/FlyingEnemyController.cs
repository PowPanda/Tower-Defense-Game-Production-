using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : MonoBehaviour {

    public float speed;
    bool isCollided;
    Vector3 dir;
    int damage;
    bool isDie;
    float drop_prob;
    public bool getHit;

    Rigidbody rb;
    Animator animator;
    PlayerController player;

    public GameObject monster_drop;

    private Transform target;
    private int waypoint_index = 0;

    public EnemySpawnManager enemySpawnManager;

    //int hashWalk = Animator.StringToHash("Walk");

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        player = GameObject.Find("Player/Main Camera/Bow").GetComponent<PlayerController>();
        target = GameObject.Find("Waypoints").GetComponent<Waypoints>().FirstPoint();
        enemySpawnManager = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawnManager>();

        speed = 2.5f;
        isCollided = false;
        damage = 2;
        drop_prob = Random.Range(0.0f, 1.0f);
        
        isDie = false;
        getHit = false;
    }
    
    // Update is called once per frame
	void Update () {
        if (player.isDie)
            StopMovement();

        else if (getHit)
        {
            isCollided = true;
            getHit = false;

            player.RestoreHP(damage);
            enemySpawnManager.remainingEnemy--;
            animator.SetBool("GetHit", true);
            Destroy(gameObject, 1.0f);
        }

        else if (!isCollided)
        {
            transform.LookAt(target);
            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, target.position) <= 0.4f)
            {
                GetNextWaypoint();
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log(col.gameObject.name);
            if (col.gameObject.tag == "Player")
            {
                animator.SetBool("CollidePlayer", true);
                transform.position = transform.position;
                InvokeRepeating("Damage", 1f, 1f);
                isCollided = true;
            }
    }

    void OnCollisionEnter(Collision col)
    {
        if (!isDie)
        {
            if (col.gameObject.tag == "Arrow")
            {
                isDie = true;
                //speed = 0;
                animator.SetBool("GetHit", true);
                player.RestoreHP(damage);
                enemySpawnManager.remainingEnemy--;
                //GetComponent<SphereCollider>().enabled = false;

                if (drop_prob > 0.8f)
                    Instantiate(monster_drop, transform.position + new Vector3(0, 0.5f, 0), monster_drop.transform.rotation);

                rb.useGravity = true;
                isCollided = true;
                Destroy(gameObject, 3f);
            }
        }

        else if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "FlyingEnemy")
            Physics.IgnoreCollision(GetComponent<SphereCollider>(), col.gameObject.GetComponent<SphereCollider>());
    }

    void Damage()
    {
        if (!isDie)
            if (!player.isDie)
                player.TakeDamage(damage);
    }

    void GetNextWaypoint()
    {
        if (waypoint_index < Waypoints.points.Length - 1)
        {
            waypoint_index++;
            target = Waypoints.points[waypoint_index];
        }
    }

    void StopMovement()
    {
        if (player.isDie)
        {
            rb.detectCollisions = false;
            animator.SetBool("PlayerDie", true);
        }
    }
}
