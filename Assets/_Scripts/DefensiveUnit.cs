using UnityEngine;
using System.Collections;

public class DefensiveUnit : Unit {

	public float maxRange = 3f;

	private Vector3 initPos;
	private float sqrMaxRange;

	protected override void Awake ()
	{
		base.Awake ();
		sqrMaxRange = maxRange * maxRange;
	}

	public override void Spawn ()
	{
		isAttacking = false;
		isMoving = false;
		movingToTarget = false;
		unitToAttack = null;
		SearchForTarget();
	}

//	protected override void FixedUpdate ()
//	{
//
//		if (isMoving == true)
//		{
//			Move();
//			SearchForTarget();
//		}
//		else if (movingToTarget == true) {
//			MoveToTarget();
//		}
//		else if (isAttacking == true)
//			Attack();
//		else SearchForTarget();
//	}

	protected override void MoveToHQ ()
	{}

	protected override void SearchForTarget ()
	{
		if (unitToAttack != null) return;
		Collider2D c = Physics2D.OverlapCircle(_transform.position, visionRange, attackLayer);
		if (c != null)
		{
			EndMove();
			IDamageable id = c.GetComponent<IDamageable>();
			if (id == null) { return; }

			if (id.IsDead() == false && isAttacking == false)
			{
				if((id.GetPosition() - initPos).sqrMagnitude > sqrMaxRange)
					return;
				
				if ((id.GetPosition() - _transform.position).sqrMagnitude < sqrRange)
					StartAttack(c.GetComponent<IDamageable>());
				else if(isMoving == false)
					StartMoving(id.GetPosition());
			}
		}
	}

	public void SetInitPos(Vector3 pos)
	{
		initPos = pos;
	}
}
