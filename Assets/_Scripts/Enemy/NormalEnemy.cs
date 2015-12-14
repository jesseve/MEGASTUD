using UnityEngine;
using System.Collections.Generic;

public class NormalEnemy : SpawningBuilding {

    public EnemyWave[] waves;
    protected int currentWave = 0;    

    protected override void Start()
    {
        base.Start();
        spawnRate = waves[currentWave].timeToWait;
        spawnCount = waves[currentWave].enemyCount;
    }
    protected override void Update()
    {
        base.Update();        
    }
    public override void Spawn()
    {
        if (currentWave < waves.Length)
        {
            SpawnUnits(unitTypesToUse[waves[currentWave].enemyToSpawn]);            
            spawnRate = waves[currentWave].timeToWait;
            spawnCount = waves[currentWave].enemyCount;
            currentWave++;
        }
        else {
            SpawnUnits(unitTypesToUse[waves[waves.Length - 1].enemyToSpawn]);
        }
    }

    protected override void CreateUnitPool()
    {
        FreeUnits = new List<Unit>();
        ActiveUnits = new List<Unit>();

        for (int i = 0; i < unitTypesToUse.Length; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                Unit e = CreateEnemy(unitTypesToUse[i].unitType);
                FreeUnits.Add(e);
                e.gameObject.SetActive(false);
            }
        }
    }

    public override void SpawnUnits(Unit type)
    {

        for (int i = 0; i < spawnCount; i += type.size)
        {
            Unit unit = PullFromPool(type.unitType);
            unit.Spawn();
            Vector3 targetPos = _transform.position + (Vector3.right * spawnRange);
            targetPos.z = -0.1f;
            Vector3 angle = new Vector3(0, 0, (360 / 12.0f) * i); //z = 360 / maxCount * spawnedCount
            Vector3 dir = targetPos - _transform.position;
            dir = Quaternion.Euler(angle) * dir;
            targetPos = _transform.position + dir;
            unit.transform.position = targetPos;
        }
    }
}

[System.Serializable]
public class EnemyWave {
    public float timeToWait;
    public int enemyCount;
    public int enemyToSpawn;
}