using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class SpawningBuilding : BuildingBase
{

    //public members
    public EnemyUnitBase[] unitTypesToUse;
    public float spawnRate;
    public int spawnCount;

    //protected members
    [SerializeField]protected List<EnemyUnitBase> FreeEnemies;
    [SerializeField]protected List<EnemyUnitBase> ActiveEnemies;

    protected float spawnTimer;

    //Unity methods
    protected override void Start() {
        base.Start();
        CreateEnemyPool();
    }
    protected override void Update()
    {
        base.Update();
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate) {
            SpawnEnemy(unitTypesToUse[UnityEngine.Random.Range(0, unitTypesToUse.Length)], transform.position);
            spawnTimer = 0;
        }
    }

    //public methods
    public void SpawnEnemy(EnemyUnitBase type, Vector3 position)
    {
        for (int i = 0; i < spawnCount; i += type.size)
        {
            EnemyUnitBase enemy = PullFromPool(type.type);
            enemy.Spawn();
            enemy.transform.position = position;
        }
    }

    //protected methods
    protected void CreateEnemyPool() {

        FreeEnemies = new List<EnemyUnitBase>();
        ActiveEnemies = new List<EnemyUnitBase>();

        for (int i = 0; i < unitTypesToUse.Length; i++) {
            for (int j = 0; j < 20; j++) {
                EnemyUnitBase e = CreateEnemy(unitTypesToUse[i].type);
                FreeEnemies.Add(e);
                e.gameObject.SetActive(false);
            }
        }
    }

    //Returns a free enemy of type given. Creates new object if there is no free enemies of the type
    protected EnemyUnitBase PullFromPool(EnemyType type) {

        for (int i = 0; i < FreeEnemies.Count; i++) {
            EnemyUnitBase e = FreeEnemies[i];
            if (e.type == type) {
                ActiveEnemies.Add(e);
                FreeEnemies.Remove(e);
                e.gameObject.SetActive(true);
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
        enemy.gameObject.SetActive(false);
    }

    protected EnemyUnitBase CreateEnemy(EnemyType type) {
        foreach (EnemyUnitBase e in unitTypesToUse)
        {
            if (e.type == type)
            {
                GameObject go = Instantiate(e.gameObject) as GameObject;
                go.tag = tag;
                go.transform.SetParent(transform);
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
