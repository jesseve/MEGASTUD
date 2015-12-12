using UnityEngine;
using System.Collections;

public class NormalEnemy : SpawningBuilding {

    protected override void Update() {
        base.Update();
        if (Input.GetMouseButtonDown(0)) {
            SpawnUnits(unitTypesToUse[0], Vector3.zero);
        }
    }
}