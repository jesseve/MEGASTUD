using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public abstract class BuildingBase : MonoBehaviour, IDamageable {

	public BuildingType buildingType = BuildingType.None;
	public GameObject minimapIcon;
	public bool dealDamage = false;
	public LayerMask targetLayer;
	public float aoeInterval = 2f;
	public int aoeRange;
	public float aoeDamage;
    public float health;
    public float moneyCost = 0;
    public float energyCost = 0;
	public bool underConstruction = false;
    public int maxAttackers;
    public int attackRange;
    public Color primary;
    public Color secondary;
	public float respawnTime = 0f;
	public GameObject demolishedBuilding;

    protected int currentAttackers;
    protected int currentLevel;
    protected Animator _animator;
    protected HealthBar _health;
	protected float _aoeTimer;

    protected Transform _transform;
	protected GameController gameController;

	private float respawnTimer;
	private float currentHealth;
	private SpriteRenderer buildingVisual;
	private Sprite defaultSprite;
	private LayerMask defaultLayer;
	private bool respawning = false;

	void Awake()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		currentHealth = health;
		buildingVisual = GetComponent<SpriteRenderer>();
		defaultSprite = buildingVisual.sprite;
		defaultLayer = gameObject.layer;
		if(demolishedBuilding != null)
			demolishedBuilding.SetActive(false);
		_aoeTimer = aoeInterval;
	}

	protected virtual void OnEnable()
	{
		minimapIcon.SetActive(true);
	}

	protected virtual void OnDisable()
	{
		minimapIcon.SetActive(false);
	}
    
    // Use this for initialization
	protected virtual void Start () {
        _transform = transform;
        _animator = GetComponent<Animator>();
        _health = GetComponentInChildren<HealthBar>();

        if(_health != null)
			_health.Init(currentHealth);

        currentLevel = 0;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if(dealDamage)
			DealDamageAOE();
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
		currentHealth -= damage;
		if(_health == null)
		{
			_health = GetComponentInChildren<HealthBar>();
			if(_health != null)
				_health.Init(currentHealth);
		}
		
		_health.UpdateHealthBar(currentHealth);
		if (currentHealth <= 0)
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
		Debug.Log("DIE DIE DIE MY DARLING!");
        gameController.HQDown(this);
        gameObject.layer = LayerMask.NameToLayer("Default");
        _animator.Play("Die");
    }
    public void DieAnim() {  
        DeadHandler.PlayAnimation(_transform.position, primary, secondary, gameObject, 3);
		//TODO
    }

    public void Stop()
    {
        _animator.Play("Idle");
        this.enabled = false;
    }

    public bool IsDead() {
		return currentHealth <= 0;
    }
    public Vector3 GetPosition() {
        return _transform.position;
    }
	public bool Target(ref BuildingBase building) {

        if (currentAttackers < maxAttackers) {

            currentAttackers++;
			building = this;
            return true;
        }
        else {
            return false;
        }
    }

	public void RemoveAttacker()
	{
		currentAttackers--;
	}

	public virtual void RespawnMe()
	{
		if(respawnTime <= 0 || respawning)
			return;

		respawning = true;
		buildingVisual.enabled = false;
		if(demolishedBuilding != null)
		{
			demolishedBuilding.SetActive(true);
			demolishedBuilding.transform.gameObject.SetActive(true);
		}

		Collider2D col = GetComponent<Collider2D>();
		if(col != null)
			col.enabled = false;
		gameController.DeactivateBuilding(this);
		respawnTimer = respawnTime;
		_health.gameObject.SetActive(false);
		GameController.RespawnEvent += Respawn;
	}

	public virtual void Respawn()
	{
		respawnTimer -= Time.deltaTime;

		if(respawnTimer <= 0)
		{
			GameController.RespawnEvent -= Respawn;
			gameController.ReactivateBuilding(this);
			_health.gameObject.SetActive(true);
			currentHealth = health; 
			_health.UpdateHealthBar(currentHealth);
			buildingVisual.sprite = defaultSprite;
			buildingVisual.color = Color.white;
			if(demolishedBuilding != null)
				demolishedBuilding.SetActive(false);
			buildingVisual.enabled = true;
			gameObject.layer = defaultLayer;
			currentAttackers = 0;
			Collider2D col = GetComponent<Collider2D>();
			if(col != null)
				col.enabled = true;
			respawning = false;
		}
	}

	protected virtual void DealDamageAOE()
	{

		_aoeTimer -= Time.deltaTime;
		if(_aoeTimer <= 0f)
		{
			_aoeTimer = aoeInterval;
			Collider2D[] targets = Physics2D.OverlapCircleAll(_transform.position, aoeRange, targetLayer);
			if(targets.Length > 0)
				_animator.Play("Attack");
			
			foreach(Collider2D col in targets)
			{
				IDamageable target = col.GetComponent<IDamageable>();
				if(target != null)
					target.TakeDamage(aoeDamage);
			}
		}
	}

}
