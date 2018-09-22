using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour {

    protected float speed;
    protected bool isCollided;
    protected float time;
    protected int damage;
    protected bool isDie;
    public bool getHit;
    protected bool isAir;
    protected float item_drop_prob;
    protected Vector3 direction;
    protected Quaternion rotation;

    protected Animator animator;
    protected PlayerController player;
    protected EnemySpawnManager enemySpawnManager;
    protected Rigidbody rb;

    protected Transform target;
    protected GameObject monster_drop;
    protected int waypoint_index = 0;
    
    protected virtual void Start()
    {
        // Self Components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Accessed Scripts
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        target = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>().FirstPoint();
        enemySpawnManager = GameObject.FindGameObjectWithTag("EnemySpawnManager").GetComponent<EnemySpawnManager>();

        isCollided = false;
        isDie = false;
        getHit = false;
        isAir = false;
    }

    protected virtual void Update()
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
            GetComponent<Rigidbody>().detectCollisions = false;

            if (monster_drop != null)
                if (item_drop_prob > 0.8f)
                    Instantiate(monster_drop, transform.position + new Vector3(0, 2.0f, 0), monster_drop.transform.rotation);

            Destroy(gameObject, 0.5f);
        }

        else if (!isCollided)
        {
            direction = target.position - transform.position;
            direction.y = 0;
            rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) <= 3.0f)
                GetNextWaypoint();
        }
    }

    protected virtual void OnTriggerEnter(Collider col)
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
            }
        }
    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        //if (col.gameObject.tag != "Terrain")
        //{
        //    Debug.Log(col.gameObject.name);
        //}
        if (!isDie)
        {
            if (col.gameObject.tag == "Arrow" || col.gameObject.tag == "FireArrow")
            {
                isDie = true;
                isCollided = true;
                player.RestoreHP(damage);
                enemySpawnManager.remainingEnemy--;
                animator.SetBool("GetHit", true);
                Destroy(gameObject, 1f);
            }

            else if (col.gameObject.tag == "Enemy")
                Physics.IgnoreCollision(col.gameObject.GetComponentInChildren<Collider>(), GetComponentInChildren<Collider>());
                

            else if (col.gameObject.tag == "Terrain")
                isAir = false;   
        }
    }

    protected virtual void Damage()
    {
        if (!isDie)
            if (!player.isDie)
                player.TakeDamage(damage);
    }

    protected virtual void GetNextWaypoint()
    {
        if (waypoint_index < Waypoints.points.Length - 1)
        {
            waypoint_index++;
            target = Waypoints.points[waypoint_index];
        }
    }

    protected virtual void StopMovement()
    {
        if (player.isDie)
            animator.SetBool("PlayerDie", true);
    }
}
