using UnityEngine;
using System.Collections;
using System;

public abstract class Unit : MonoBehaviour, IDamageable
{

    //public members
    public int range;
    public int damage;
    public int health;
    public float speed;
    public bool isPlayerControlled;

    //protected members
    //Movement
    protected Vector3 targetPoint;
    protected bool isMoving;

    //Attacking
    protected IDamageable unitToAttack;

    //Components
    protected Transform _transform;

    // Use this for initialization
    protected virtual void Start()
    {
        _transform = transform;
        speed *= Time.fixedDeltaTime;
        range = range * range;
    }
    protected virtual void Update() {

    }
    protected virtual void FixedUpdate() {
        if (isMoving == true)
            Move();
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

    public void StartAttack(IDamageable target) {        
        unitToAttack = target;
    }
    public void Attack() {
        if (unitToAttack == null) return;

        //Check if the unit is in range to attack
        Vector3 ePos = unitToAttack.GetPosition();
        if ((ePos - _transform.position).sqrMagnitude < range) {
            unitToAttack.TakeDamage(damage);
        }
    }

    //IDamageable implementation
    public virtual void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage!");
    }
    public Vector3 GetPosition() {
        return _transform.position;
    }
}