using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public abstract class Unit : MonoBehaviour, IDamageable
{

    //public members
	public GameObject minimapIcon;
	public SoundClip attackSound = SoundClip.Attack;
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
	protected BuildingBase targetBuilding;

    //Components
    protected Transform _transform;
    protected Animator _animator;
    protected SpriteRenderer _sprite;
    protected HealthBar _health;
	protected AudioSource _audio;

	private LayerMask defaultLayer = -1;
	private LineRenderer _moveLine;


	protected virtual void OnEnable()
	{
		minimapIcon.SetActive(true);
		if(defaultLayer != -1)
			gameObject.layer = defaultLayer;
	}

	protected virtual void OnDisable()
	{
		minimapIcon.SetActive(false);
	}
    // Use this for initialization
    protected virtual void Awake()
    {
        _transform = transform;
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _health = GetComponentInChildren<HealthBar>();
		_audio = GetComponent<AudioSource>();
		_moveLine = GetComponent<LineRenderer>();
		if(_moveLine != null)
		{
			_moveLine.SetVertexCount(2);
			_moveLine.SetPosition(0, _transform.position + new Vector3(0, -0.2f, 0));
			_moveLine.SetPosition(1, _transform.position + new Vector3(0, -0.2f, 0));
		}
        if(_health != null)
            _health.Init(health);
               
        speed *= Time.fixedDeltaTime;

        CircleCollider2D circle = GetComponentInChildren<CircleCollider2D>();
        circle.radius = visionRange;

        sqrRange = range * range;
		HandleSelection(false);

		defaultLayer = gameObject.layer;
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

    protected void SetAnimator(string parameter) {        
        if (_animator != null)
        {
            try
            {
                _animator.Play(parameter);
            }
            catch (Exception e) {
                Debug.Log("virhe: " + parameter);
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
		if(_moveLine != null)
		{
			_moveLine.SetVertexCount(2);
			_moveLine.SetPosition(1, point);
			_moveLine.enabled = true;
		}
        SetAnimator("Walking");
    }
    public virtual void Move()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, targetPoint, speed);
		if(_moveLine != null)
			_moveLine.SetPosition(0, _transform.position + new Vector3(0, -0.2f, 0));
        if (_transform.position == targetPoint)
        {
            EndMove();
        }
    }
    
    protected virtual void EndMove() {
        isMoving = false;
		if(_moveLine != null)
			_moveLine.enabled = false;
        //SetAnimator("Idle");
    }
    public void HandleSelection(bool selected) { 
		if(unitSelector != null)
			unitSelector.SetActive	(selected);
		if(_moveLine != null)
			_moveLine.enabled = isMoving;
	}

    public virtual void StartAttack(IDamageable target)
    {
        if (isAttacking == true || movingToTarget == true) return;
        EndMove();

		if (target.Target(ref targetBuilding))
        {
            unitToAttack = target;
            attackPosition = target.GetAttackPosition(_transform.position);
			float dst = (target.GetPosition() - _transform.position).sqrMagnitude;
            if (dst > sqrRange)
            {

				if(_moveLine != null)
				{
					_moveLine.SetVertexCount(2);
					_moveLine.SetPosition(1, attackPosition);
					_moveLine.enabled = true;
				}
				movingToTarget = true;
                SetAnimator("Walking");
            }
            else
            {
                isAttacking = true;

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
		_sprite.flipX = (unitToAttack.GetPosition().x < _transform.position.x);
        if (attackTimer >= fireRate)
        {
			_audio.clip = SoundManager.GetSoundClip(attackSound);
			_audio.Play();
            attackTimer = 0;
            SetAnimator("Attack");
            if (unitToAttack.TakeDamage(damage) == true)
            {
                unitToAttack = null;
                isAttacking = false;
				MoveToHQ();
            }
        }
    }

    protected virtual void MoveToTarget() {
        
		_transform.position = Vector3.MoveTowards(_transform.position, attackPosition, speed);
		if(_moveLine != null)
			_moveLine.SetPosition(0, _transform.position + new Vector3(0, -0.2f, 0));

        if (attackPosition == _transform.position) {
            movingToTarget = false;
            isAttacking = true;

            attackTimer = fireRate;
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
		if(targetBuilding != null)
			targetBuilding.RemoveAttacker();
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
	public bool Target(ref BuildingBase building)
    {
		building = null;
        return true;
    }
    public Vector3 GetAttackPosition(Vector3 pos) {
        return transform.position;
    }
	protected abstract void MoveToHQ();
}