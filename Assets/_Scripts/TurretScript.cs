using UnityEngine;
using System.Collections;

public class TurretScript : BuildingBase {

	public ProjectileScript projectilePrefab;

	private IDamageable targetUnit = null;
	private SpriteRenderer spriteRend;

	protected override void Update ()
	{}

	void OnEnable()
	{
		targetUnit = null;
		if(spriteRend == null)
			spriteRend = GetComponent<SpriteRenderer>();
	}

	void FixedUpdate ()
	{
		if(IsDead())
			return;
		
		if(targetUnit == null)
			CheckForTargets();
		else
			AttackTarget();
		
	}

	private void CheckForTargets()
	{
		Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(_transform.position, aoeRange, targetLayer);
		foreach(Collider2D col in targetsInRange)
		{
			IDamageable target = col.GetComponent<IDamageable>();
			if(target != null)
			{
				targetUnit = target;
				_aoeTimer = aoeInterval;
				break;
			}
		}
	}

	private void AttackTarget()
	{
		_aoeTimer -= Time.fixedDeltaTime;
		Vector3 targetPos = targetUnit.GetPosition();
		spriteRend.flipX = !(targetPos.x < _transform.position.x);
		if(_aoeTimer <= 0)
		{
			_aoeTimer = aoeInterval;
			ProjectileScript projectile = Instantiate(projectilePrefab, _transform.position, Quaternion.identity) as ProjectileScript;
			float angle = Mathf.Atan2( _transform.position.y -targetPos.y, _transform.position.x - targetPos.x) * Mathf.Rad2Deg;
			projectile.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0,0,1));
			projectile.targetPos = targetPos;

			if(targetUnit.TakeDamage(aoeDamage))
			{
				targetUnit = null;
			}
		}
	}

}
