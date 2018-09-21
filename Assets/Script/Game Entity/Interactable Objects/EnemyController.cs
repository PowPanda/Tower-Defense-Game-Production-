using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float speed;
    bool isCollided;
    float time;
    int damage = 200;
    bool isDie;
    public bool getHit;
    bool randomedNumber;
    float randomXBounceDir;

    Rigidbody rb;
    Animator animator;
    PlayerController player;

    private Transform target;
    private int waypoint_index = 0;

    public EnemySpawnManager enemySpawnManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        speed = 2.0f;
        isCollided = false;

        player = GameObject.Find("Player/Main Camera/Bow").GetComponent<PlayerController>();
        target = GameObject.Find("Waypoints").GetComponent<Waypoints>().FirstPoint();
        enemySpawnManager = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawnManager>();

        isDie = false;
        getHit = false;
        randomedNumber = false;
    }

    void FixedUpdate()
    {
        //if (!randomedNumber)
        //{
        //    randomXBounceDir = Random.Range(-15f, 15f);
        //    randomedNumber = true;
        //}

        //if (getHit)
        //{
        //    rb.AddForce(new Vector3(randomXBounceDir, 0f, 30f), ForceMode.Impulse);
        //    //if (transform.position.y >= 5f)
        //    //    rb.AddForce(new Vector3(0f, 0f, 20.5f), ForceMode.Impulse);
        //}
    }

    // Update is called once per frame
    void Update()
    {
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
            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
            //dir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) <= 3.0f)
                GetNextWaypoint();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!isDie)
        {
            if (col.gameObject.tag == "Gate")
            {
                transform.position = transform.position;
                Damage();
                PopUpTextController.CreatePopUpText(damage.ToString(), transform);
                isCollided = true;
                isDie = true;
                gameObject.transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                gameObject.transform.GetComponentInChildren<MeshRenderer>().enabled = false;
                Destroy(gameObject, 2f);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (!isDie)
        {
            if (col.gameObject.tag == "Arrow")
            {
                isDie = true;
                isCollided = true;
                //speed = 0;
                //rb.useGravity = true;
                //getHit = true;
                player.RestoreHP(damage);
                enemySpawnManager.remainingEnemy--;
                //GetComponent<SphereCollider>().enabled = false;
                animator.SetBool("GetHit", true);

                Destroy(gameObject, 1.0f);
            }

            else if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "FlyingEnemy")
            {
                Physics.IgnoreCollision(GetComponent<SphereCollider>(), col.gameObject.GetComponent<SphereCollider>());
            }
        }
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
            animator.SetBool("PlayerDie", true);
        }
    }
}
