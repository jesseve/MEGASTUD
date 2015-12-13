using UnityEngine;
using System.Collections;

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
            //Last wave was sent
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
            Vector3 angle = new Vector3(0, 0, (360 / 8.0f) * i); //z = 360 / maxCount * spawnedCount
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