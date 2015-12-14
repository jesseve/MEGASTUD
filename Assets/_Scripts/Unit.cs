﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public abstract class Unit : MonoBehaviour, IDamageable
{

    //public members    
	public float moneyCost;
	public float energyCost;
    public int range;
    public int visionRange;
    public float damage;
    public float health;
    public float speed;
    public float fireRate;
    public UnitType unitType;
    public bool isPlayerControlled;
    public LayerMask attackLayer;
	public GameObject unitSelector;
    public Color primary;
    public Color secondary;

    [Range(1, 4)]
    public int size = 1;        //determine how many can be spawned by the ai

    //protected members

    //Movement
    protected Vector3 targetPoint;
    protected bool isMoving;

    //Attacking    
    protected IDamageable unitToAttack;    
    protected bool isAttacking;
    protected float attackTimer;
    protected bool movingToTarget;
    protected Vector3 attackPosition;
    protected int sqrRange;

    //Components
    protected Transform _transform;
    protected Animator _animator;
    protected SpriteRenderer _sprite;
    protected HealthBar _health;
    public AudioSource _audio;
    public AudioClip[] unitSounds;

    // Use this for initialization
    protected virtual void Awake()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _health = GetComponentInChildren<HealthBar>();
        if(_health != null)
            _health.Init(health);
               
        speed *= Time.fixedDeltaTime;

        CircleCollider2D circle = GetComponentInChildren<CircleCollider2D>();
        circle.radius = visionRange;

        sqrRange = range * range;
		HandleSelection(false);
    }
    protected virtual void FixedUpdate()
    {
        if (isMoving == true)
		{
            Move();
			SearchForTarget();
		}
        else if (movingToTarget == true) {
            MoveToTarget();
        }
        else if (isAttacking == true)
            Attack();
		else SearchForTarget();
    }

    protected void SetAnimator(string parameter, bool value) {        
        if (_animator != null)
        {
            try
            {
                _animator.SetBool(parameter, value);
            }
            catch (Exception e) {
                Debug.Log("virhe: " + parameter + " value: " + value);
            }
        }
    }    

    public void StartMoving(Vector3 point)
    {
        //Vector3 currentScale = _transform.localScale;
        //currentScale.x = (point.x >= _transform.position.x)?Mathf.Abs(currentScale.x):-(Mathf.Abs(currentScale.x));
        //_transform.localScale = currentScale;
        _sprite.flipX = (point.x < _transform.position.x);
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
    public void HandleSelection(bool selected) { 
		if(unitSelector != null)
			unitSelector.SetActive	(selected);
	}

    public virtual void StartAttack(IDamageable target)
    {
        if (isAttacking == true || movingToTarget == true) return;
        EndMove();

        if (target.Target())
        {
            unitToAttack = target;
            attackPosition = target.GetAttackPosition(_transform.position);
            float dst = (target.GetPosition() - _transform.position).sqrMagnitude;
            if (dst > sqrRange)
            {
                movingToTarget = true;
                SetAnimator("isMoving", true);
            }
            else
            {
				Debug.Log("Echo base we're commencing our attack run!");
                isAttacking = true;
                SetAnimator("isAttacking", isAttacking);

                attackTimer = fireRate;

            }
        }
        else
        {
			MoveToHQ();
        }        
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
				MoveToHQ();
            }
        }
    }

    protected virtual void MoveToTarget() {
        
        _transform.position = Vector3.MoveTowards(_transform.position, attackPosition, speed);

        if (attackPosition == _transform.position) {
            movingToTarget = false;
            isAttacking = true;
            SetAnimator("isMoving", false);
            SetAnimator("isAttacking", isAttacking);

            attackTimer = fireRate;
        }
    }
    protected virtual void SearchForTarget()
    {
		if (unitToAttack != null) return;
		Debug.Log("Searching for targets");
        Collider2D c = Physics2D.OverlapCircle(_transform.position, visionRange, attackLayer);
        if (c != null)
        {
            EndMove();
            IDamageable id = c.GetComponent<IDamageable>();
            if (id == null) { return; }

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
        _health.UpdateHealthBar(health);
        if (health <= 0)
        {
            Die();
            return true;
        }

        return false;
    }
    public Vector3 GetPosition()
    {
        return _transform.position;
    }
    public void Die()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        _animator.Play("Die");
    }
    public bool IsDead()
    {
        return health <= 0;
    }
    public void DieAnim()
    {
        gameObject.SetActive(false);
        DeadHandler.PlayAnimation(_transform.position, primary, secondary, gameObject);
    }
    public bool Target()
    {
        return true;
    }
    public Vector3 GetAttackPosition(Vector3 pos) {
        return transform.position;
    }
	protected abstract void MoveToHQ();
}