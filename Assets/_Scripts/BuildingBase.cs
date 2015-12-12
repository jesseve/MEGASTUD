using UnityEngine;
using System.Collections;

public abstract class BuildingBase : MonoBehaviour, IDamageable {

    public float health;

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

    public void TakeDamage(float damage) {

    }
    public void Die() { }
    public Vector3 GetPosition() {
        return _transform.position;
    }
}
