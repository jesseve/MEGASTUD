using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class EnemyUnitBase : Unit
{
    public EnemyType type;
    public string enemyName;

    protected IDamageable playerHQ;
            
    public override void Spawn()
    {
        GameObject hq = GameObject.FindGameObjectWithTag("PlayerHQ");
        if(hq != null)
            playerHQ = hq.GetComponent<BuildingBase>();
        if(playerHQ != null)
            StartMoving(playerHQ.GetPosition());
    }
    protected override void EndMove()
    {
        base.EndMove();
        //if (unitToAttack != null) {
        //    float dst = (unitToAttack.GetPosition() - _transform.position).sqrMagnitude;
        //    if (dst < sqrRange)
        //    {
        //        StartAttack(unitToAttack);
        //    }
        //    else
        //    {
        //        StartMoving(playerHQ.GetPosition());
        //    }
        //}
    }
    protected override void SearchForTarget()
    {
        if (unitToAttack != null) return;
        Collider2D c = Physics2D.OverlapCircle(_transform.position, visionRange, attackLayer);
        if (c != null)
        {
            base.EndMove();
            IDamageable id = c.GetComponent<IDamageable>();
            if (id == null) { return; }

            if (id.IsDead() == false && isAttacking == false)
            {
                if ((id.GetPosition() - _transform.position).sqrMagnitude < sqrRange)
                    StartAttack(c.GetComponent<IDamageable>());
                else if (isMoving == false)
                    StartMoving(id.GetPosition());
                return;
            }
        }
		MoveToHQ();
    }

	protected override void MoveToHQ ()
	{
		if(playerHQ != null)
			StartMoving(playerHQ.GetPosition());
	}
}