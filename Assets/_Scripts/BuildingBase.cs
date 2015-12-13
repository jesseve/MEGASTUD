using UnityEngine;
using System.Collections;

public abstract class BuildingBase : MonoBehaviour, IDamageable {

	public BuildingType buildingType = BuildingType.None;
    public float health;
    public float moneyCost = 0;
    public float energyCost = 0;
	public bool underConstruction = false;
    public int maxAttackers;
    public int attackRange;
    public Color primary;
    public Color secondary;

    protected int currentAttackers;
    protected int currentLevel;
    protected Animator _animator;
    protected HealthBar _health;

    protected Transform _transform;
	
    
    // Use this for initialization
	protected virtual void Start () {
        _transform = transform;
        _animator = GetComponent<Animator>();
        _health = GetComponentInChildren<HealthBar>();
        if(_health != null)
            _health.Init(health);

        currentLevel = 0;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

    public virtual void Upgrade() {
        currentLevel++;
    }

    public Vector3 GetAttackPosition(Vector3 pos) {

        Vector3 targetPos = _transform.position + (Vector3.right * attackRange);
        targetPos.z = -0.1f;
        Vector3 angle = new Vector3(0, 0, 360 / maxAttackers * currentAttackers); //z = 360 / maxCount * spawnedCount
        Vector3 dir = targetPos - _transform.position;
        dir = Quaternion.Euler(angle) * dir;
        targetPos = _transform.position + dir;
        
        return targetPos;
    }

    //IDamageable implementation

    public bool TakeDamage(float damage) {
        health -= damage;
		if(_health == null)
		{
			_health = GetComponentInChildren<HealthBar>();
			if(_health != null)
				_health.Init(health);
		}
		
        _health.UpdateHealthBar(health);
        if (health <= 0)
        {
            Die();
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Die() {
        gameObject.layer = LayerMask.NameToLayer("Default");
        _animator.Play("Die");
    }
    public void DieAnim() {  
        DeadHandler.PlayAnimation(_transform.position, primary, secondary, gameObject, 3);
    }
    public bool IsDead() {
        return health <= 0;
    }
    public Vector3 GetPosition() {
        return _transform.position;
    }
    public bool Target() {

        if (currentAttackers < maxAttackers) {

            currentAttackers++;

            return true;
        }
        else {
            return false;
        }
    }
}
