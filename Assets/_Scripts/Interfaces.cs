using UnityEngine;
using System.Collections;

public interface IDamageable
{
	bool TakeDamage(float damage);
    Vector3 GetPosition();
    bool IsDead();
    void Die();
    bool Target();
    Vector3 GetAttackPosition(Vector3 position);
}