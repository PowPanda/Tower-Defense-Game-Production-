using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : EnemyController {
    
    protected override void Start()
    {
        base.Start();
        speed = 2.0f;
        damage = 2;
        item_drop_prob = Random.Range(0.0f, 1.0f);
        monster_drop = Resources.Load<GameObject>("Prefabs/Objects/RedBalloon");
    }

    protected override void Update()
    {
        base.Update();
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }

    protected override void OnCollisionEnter(Collision col)
    {
        base.OnCollisionEnter(col);

        rb.useGravity = true;
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
