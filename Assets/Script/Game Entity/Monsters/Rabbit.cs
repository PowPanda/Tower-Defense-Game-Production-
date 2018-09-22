using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : EnemyController {

    private float timer;
    private float jumpHeight;

    protected override void Start()
    {
        base.Start();
        speed = 3.5f;
        damage = 3;
        jumpHeight = 2.5f;
    }

    protected override void Update()
    {
        base.Update();
        switch(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name)
        {
            case "rabbit_idle":
                timer += Time.time;
                if (timer > animator.GetCurrentAnimatorClipInfo(0)[0].clip.length)
                {
                    animator.SetBool("IsMove", true);
                    timer = 0;
                }
                break;

            case "rabbit_move":
                timer += Time.time;
                transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
                if (!isAir)
                {
                    rb.velocity = new Vector3(0,jumpHeight,0);
                    isAir = true;
                }
                    
                if (timer > animator.GetCurrentAnimatorClipInfo(0)[0].clip.length + 1)
                {
                    animator.SetBool("PlayerDie", true);
                    timer = 0;
                }
                break;
        }
    }

    protected override void OnCollisionEnter(Collision col)
    {
        base.OnCollisionEnter(col);
    }

    protected override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
        gameObject.transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        Destroy(gameObject, 2f);
    }

    protected override void Damage()
    {
        base.Damage();
    }

    protected override void GetNextWaypoint()
    {
        base.GetNextWaypoint();
    }

    protected override void StopMovement()
    {
        base.StopMovement();
    }
}
