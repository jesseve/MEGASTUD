using UnityEngine;
using System.Collections;

public class NormalEnemy : EnemyBase {

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            SpawnEnemy(enemyTypesToUse[0].type, Vector3.zero);
        }
    }
}