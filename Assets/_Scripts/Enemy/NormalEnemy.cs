using UnityEngine;
using System.Collections;

public class NormalEnemy : EnemyBase {

    protected override void Update() {
        base.Update();
        if (Input.GetMouseButtonDown(0)) {
            SpawnEnemy(unitTypesToUse[0], Vector3.zero);
        }
    }
}