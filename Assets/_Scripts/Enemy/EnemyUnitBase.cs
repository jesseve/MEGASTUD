using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class EnemyUnitBase : Unit
{
    public EnemyType type;
    public string enemyName;
        
    public override void Spawn()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerUnit");

        enemyUnits = new List<IDamageable>();

        foreach (GameObject g in gos) {
            Unit u = g.GetComponent<Unit>();
            u.RegisterEnemy(this);
            enemyUnits.Add(u);
        }
    }
}