using UnityEngine;
using System.Collections;
using System;

public class TestPlayerUnit : Unit
{
    public override void Spawn()
    {
        //throw new NotImplementedException();
        health = maxHealth;
		isAttacking = false;
		isMoving = false;
		movingToTarget = false;
		unitToAttack = null;
		SearchForTarget();
    }

//    public override void StartAttack(IDamageable target)
//    {
//        
//    }

	protected override void MoveToHQ ()
	{
	}
}