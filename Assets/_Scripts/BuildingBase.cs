using UnityEngine;
using System.Collections;

public abstract class BuildingBase : MonoBehaviour, IDamageable {

    public float health;
    public float moneyCost = 0;
    public float energyCost = 0;

    protected int currentLevel;
    
    protected Transform _transform;
	
    
    // Use this for initialization
	protected virtual void Start () {
        _transform = transform;
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
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Die() { }
    public Vector3 GetPosition() {
        return _transform.position;
    }
}
