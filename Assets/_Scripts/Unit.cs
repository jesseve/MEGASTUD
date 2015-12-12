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
    protected List<IDamageable> enemyUnits;
    [SerializeField]
    protected bool isAttacking;
    protected float attackTimer;
    protected bool movingToTarget;

    //Components
    protected Transform _transform;
    protected Animator _animator;

    // Use this for initialization
    protected virtual void Awake()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
        speed *= Time.fixedDeltaTime;

        enemyUnits = new List<IDamageable>();

        visionRange = visionRange * visionRange;
        range = range * range;
    }
    protected virtual void FixedUpdate()
    {
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
        targetPoint = point;
        isMoving = true;
        SetAnimator("isMoving", isMoving);
    }
    public void Move()
    {
        _transform.position = Vector2.MoveTowards(_transform.position, targetPoint, speed);
        if (_transform.position == targetPoint)
        {
            isMoving = false;
            SetAnimator("isMoving", isMoving);
            EndMove();
        }
    }
    public void HandleSelection(bool selected) { }
    protected virtual void EndMove() {

    }

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
    protected void SearchForTarget()
    {
        Collider2D c = Physics2D.OverlapCircle(_transform.position, range, attackLayer);
        if (c != null)
        {
            unitToAttack = c.GetComponent<IDamageable>();
            StartMoving(unitToAttack.GetPosition());
        }
    }
    public void RegisterEnemy(IDamageable enemy)
    {
        enemyUnits.Add(enemy);
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
        Debug.Log(gameObject.name + " DIED!");
    }
}