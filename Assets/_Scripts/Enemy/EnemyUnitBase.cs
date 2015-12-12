using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class EnemyUnitBase : Unit
{
    public EnemyType type;
    public string enemyName;

    protected IDamageable playerHQ;
            
    public override void Spawn()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PlayerUnit");

        enemyUnits = new List<IDamageable>();

        playerHQ = GameObject.FindGameObjectWithTag("PlayerHQ").GetComponent<BuildingBase>();
        StartAttack(playerHQ);

        foreach (GameObject g in gos) {
            Unit u = g.GetComponent<Unit>();
            u.RegisterEnemy(this);
            enemyUnits.Add(u);
        }
    }
    protected override void EndMove()
    {
        if (unitToAttack != null) {
            float dst = (unitToAttack.GetPosition() - _transform.position).sqrMagnitude;
            if (dst < range)
            {
                StartAttack(unitToAttack);
            }
            else
            {
                StartMoving(playerHQ.GetPosition());
            }
        }
    }
}