using UnityEngine;
using UnityEditor;
using System.Collections;

public abstract class EnemyUnitBase : Unit
{
    public EnemyType type;
    public string enemyName;

    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
}