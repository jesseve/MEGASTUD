using UnityEngine;
using System.Collections;

public abstract class BuildingBase : MonoBehaviour, IDamageable {

	public BuildingType buildingType = BuildingType.None;
    public float health;
    public float moneyCost = 0;
    public float energyCost = 0;
	public bool underConstruction = false;
    public Color primary;
    public Color secondary;

    protected int currentLevel;
    protected Animator _animator;

    protected Transform _transform;
	
    
    // Use this for initialization
	protected virtual void Start () {
        _transform = transform;
        _animator = GetComponent<Animator>();
        currentLevel = 0;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

    public virtual void Upgrade() {
        currentLevel++;
    }

    //IDamageable implementation

    public bool TakeDamage(float damage) {
        health -= damage;
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
        Debug.Log("Died");
        gameObject.SetActive(false);
        DeadHandler.PlayAnimation(_transform.position, primary, secondary);
    }
    public bool IsDead() {
        return health <= 0;
    }
    public Vector3 GetPosition() {
        return _transform.position;
    }
}
