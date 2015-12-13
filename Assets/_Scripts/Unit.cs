using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class Unit : MonoBehaviour, IDamageable
{

    //public members
    public int range;
    public int visionRange;
    public float damage;
    public float health;
    public float speed;
    public float fireRate;
    public UnitType unitType;
    public bool isPlayerControlled;
    public LayerMask attackLayer;

    [Range(1, 4)]
    public int size = 1;        //determine how many can be spawned by the ai

    //protected members

    //Movement
    protected Vector3 targetPoint;
    protected bool isMoving;

    //Attacking    
    protected IDamageable unitToAttack;
    [SerializeField]
    protected bool isAttacking;
    protected float attackTimer;
    protected bool movingToTarget;
    protected int sqrRange;

    //Components
    protected Transform _transform;
    protected Animator _animator;

    // Use this for initialization
    protected virtual void Awake()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
        speed *= Time.fixedDeltaTime;

        sqrRange = range * range;
    }
    protected virtual void FixedUpdate()
    {
        SearchForTarget();
        if (isMoving == true)
            Move();
        else if (isAttacking == true)
            Attack();
    }

    protected void SetAnimator(string parameter, bool value) {
        if (_animator != null)
            _animator.SetBool(parameter, value);
    }

    public void StartMoving(Vector3 point)
    {
		Vector3 currentScale = _transform.localScale;
		currentScale.x = (point.x >= _transform.position.x)?Mathf.Abs(currentScale.x):-(Mathf.Abs(currentScale.x));
		_transform.localScale = currentScale;
        targetPoint = point;
        isMoving = true;
        SetAnimator("isMoving", isMoving);
    }
    public void Move()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, targetPoint, speed);
        if (_transform.position == targetPoint)
        {
            EndMove();
        }
    }
    
    protected virtual void EndMove() {
        isMoving = false;
        SetAnimator("isMoving", isMoving);
    }
    public void HandleSelection(bool selected) { }
    public virtual void StartAttack(IDamageable target)
    {
        isAttacking = true;
        SetAnimator("isAttacking", isAttacking);

        attackTimer = fireRate;
        unitToAttack = target;
    }
    
    public virtual void Attack()
    {
        if (unitToAttack == null) return;

        //Check if the unit is in range to attack
        attackTimer += Time.fixedDeltaTime;

        if (attackTimer >= fireRate)
        {
            attackTimer = 0;
            if (unitToAttack.TakeDamage(damage) == true)
            {
                unitToAttack = null;
                isAttacking = false;
                SetAnimator("isAttacking", isAttacking);
            }
        }
    }
    protected virtual void SearchForTarget()
    {
        if (unitToAttack != null) return;
        Collider2D c = Physics2D.OverlapCircle(_transform.position, visionRange, attackLayer);
        if (c != null)
        {
            EndMove();
            IDamageable id = c.GetComponent<IDamageable>();
            if (id == null) { Debug.Log("IDamageable null"); return; }

            if (id.IsDead() == false && isAttacking == false)
            {
                if ((id.GetPosition() - _transform.position).sqrMagnitude < sqrRange)
                    StartAttack(c.GetComponent<IDamageable>());
                else if(isMoving == false)
                    StartMoving(id.GetPosition());
            }
        }
    }    

    public abstract void Spawn();

    //IDamageable implementation

    //Return true if the object died
    public virtual bool TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
            return true;
        }
        Debug.Log(gameObject.name + " took " + damage + " damage!");
        return false;
    }
    public Vector3 GetPosition()
    {
        return _transform.position;
    }
    public void Die()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        Debug.Log(gameObject.name + " DIED!");
    }
    public bool IsDead()
    {
        return health <= 0;
    }

}