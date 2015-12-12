using UnityEngine;
using System.Collections;

public interface IDamageable
{
	bool TakeDamage(float damage);
    Vector3 GetPosition();
    void Die();
}