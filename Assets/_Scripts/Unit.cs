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

    [Range(1, 4)]
    public int size = 1;        //determine how many can be spawned by the ai

    //protected members

    //Movement
    protected Vector3 targetPoint;
    protected bool isMoving;

    //Attacking    
    protected IDamageable unitToAttack;
    protected List<IDamageable> enemyUnits;
    protected bool isAttacking;
    protected float attackTimer;

    //Components
    protected Transform _transform;

    // Use this for initialization
    protected virtual void Awake()
    {
        _transform = transform;
        speed *= Time.fixedDeltaTime;

        enemyUnits = new List<IDamageable>();

        visionRange = visionRange * visionRange;
        range = range * range;
    }    
    protected virtual void FixedUpdate() {
        if (isMoving == true)
            Move();
        else if (isAttacking == true)
            Attack();
        else
            Patrol();
    }

    public void StartMoving(Vector3 point)
    {
        targetPoint = point;
        isMoving = true;
    }
    public void Move()
    {
        Vector2.MoveTowards(_transform.position, targetPoint, speed);
        if (_transform.position == targetPoint)
            isMoving = false;
    }
    protected virtual void Patrol() {
        if (enemyUnits == null) return;     
        foreach (IDamageable e in enemyUnits) {            
            float dst = (e.GetPosition() - _transform.position).sqrMagnitude;
            if (dst < visionRange) {
                StartAttack(e);
                break;
            }
        }
    }

    public virtual void StartAttack(IDamageable target) {
        isAttacking = true;
        unitToAttack = target;
    }
    public virtual void Attack() {        
        if (unitToAttack == null) return;

        //Check if the unit is in range to attack
        Vector3 ePos = unitToAttack.GetPosition();
        float dst = (ePos - _transform.position).sqrMagnitude;
        if (dst < range)
        {
            attackTimer += Time.fixedDeltaTime;
            if (attackTimer >= fireRate)
            {
                attackTimer = 0;
                if (unitToAttack.TakeDamage(damage) == true) {
                    unitToAttack = null;
                    isAttacking = false;
                }
                
            }
        }
        else {
            attackTimer = fireRate;
            _transform.position = Vector2.MoveTowards(_transform.position, ePos, speed);
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
    public Vector3 GetPosition() {
        return _transform.position;
    }
    public void Die() {
        Debug.Log(gameObject.name + " DIED!");
    }
}

public enum UnitType {
    Unit1,
    Unit2,
    Unit3
}