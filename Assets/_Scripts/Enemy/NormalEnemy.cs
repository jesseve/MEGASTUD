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
        if (Input.GetMouseButtonDown(0))
        {
            SpawnUnits(unitTypesToUse[0]);
        }
    }
    public override void Spawn()
    {
        if (currentWave < waves.Length)
        {
            SpawnUnits(unitTypesToUse[waves[currentWave].enemyToSpawn]);
            currentWave++;

        }
        else {
            //Last wave was sent
        }
    }
}

[System.Serializable]
public class EnemyWave {
    public float timeToWait;
    public int enemyCount;
    public int enemyToSpawn;
}