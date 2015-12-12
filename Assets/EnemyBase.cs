using UnityEngine;
using System.Collections;
using System;

public abstract class EnemyBase : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

public abstract class EnemyUnitBase : IDamageable
{
    public string name;
    public int range;
    public int damage;
    public int health;
    public int speed;

    public void TakeDamage(float damage)
    {
        Debug.Log(name + " took " + damage + " damage!");
    }
}
