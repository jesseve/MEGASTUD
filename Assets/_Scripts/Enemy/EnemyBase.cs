using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class EnemyBase : MonoBehaviour
{

    //public members
    public EnemyUnitBase[] enemyTypesToUse;    

    //protected members
    protected List<EnemyUnitBase> FreeEnemies;
    protected List<EnemyUnitBase> ActiveEnemies;

    //Unity methods
    protected void Start() {
        CreateEnemyPool();
    }

    //public methods
    public void SpawnEnemy() {

    }

    //protected methods
    protected void CreateEnemyPool() {

        FreeEnemies = new List<EnemyUnitBase>();
        ActiveEnemies = new List<EnemyUnitBase>();

        for (int i = 0; i < enemyTypesToUse.Length; i++) {
            for (int j = 0; j < 20; j++) {
                GameObject go = Instantiate(enemyTypesToUse[i].gameObject) as GameObject;
                FreeEnemies.Add(go.GetComponent<EnemyUnitBase>());
            }
        }
    }

    //Returns a free enemy of type given. Creates new object if there is no free enemies of the type
    protected EnemyUnitBase PullFromPool(EnemyType type) {

        for (int i = 0; i < FreeEnemies.Count; i++) {
            EnemyUnitBase e = FreeEnemies[0];
            if (e.type == type) {
                ActiveEnemies.Add(e);
                FreeEnemies.Remove(e);
                return e;
            }
        }
        EnemyUnitBase enemy = CreateEnemy(type);
        if(enemy != null)
            ActiveEnemies.Add(enemy);
        return enemy;
    }
    protected void PushToPool(EnemyUnitBase enemy) {
        ActiveEnemies.Remove(enemy);
        FreeEnemies.Add(enemy);
    }

    protected EnemyUnitBase CreateEnemy(EnemyType type) {
        foreach (EnemyUnitBase e in enemyTypesToUse)
        {
            if (e.type == type)
            {
                GameObject go = Instantiate(e.gameObject) as GameObject;
                return go.GetComponent<EnemyUnitBase>();
            }
        }
        return null;
    }
}

public enum EnemyType {
    Enemy1,
    Enemy2,
    Enemy3
}
