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
    public UnitType unitType;
    public bool isPlayerControlled;

    //protected members
    
    //Movement
    protected Vector3 targetPoint;
    [SerializeField]protected bool isMoving;

    //Attacking    
    protected IDamageable unitToAttack;
    protected List<IDamageable> enemyUnits;
    [SerializeField]
    protected bool isAttacking;

    //Components
    protected Transform _transform;

    // Use this for initialization
    protected virtual void Awake()
    {
        _transform = transform;
        speed *= Time.fixedDeltaTime;
        damage *= Time.fixedDeltaTime;

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
            unitToAttack.TakeDamage(damage);
        }
        else {
            Vector2.MoveTowards(_transform.position, ePos, speed);
        }               
    }
    public void RegisterEnemy(IDamageable enemy)
    {
        enemyUnits.Add(enemy);
    }

    public abstract void Spawn();

    //IDamageable implementation
    public virtual void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage!");
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