using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : EnemyController {

	protected override void Start()
    {
        base.Start();
        speed = 1.5f;
        damage = 1;
    }

    protected override void Update()
    {
        base.Update();
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }

    protected override void OnCollisionEnter(Collision col)
    {
        base.OnCollisionEnter(col);
    }

    protected override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
        gameObject.transform.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        gameObject.transform.GetComponentInChildren<MeshRenderer>().enabled = false;
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
