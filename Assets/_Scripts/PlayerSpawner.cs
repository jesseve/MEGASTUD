﻿using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : SpawningBuilding {

    public int maxUnits = 6;
    public float maxRange = 2;

	protected override void Update ()
	{
		if(dealDamage)
			DealDamageAOE();
	}

    protected override void CreateUnitPool()
    {
        if (buildingType == BuildingType.OffensiveSpawner)
            type = UnitType.Attack;
        else if (buildingType == BuildingType.DefensiveSpawner)
            type = UnitType.Defence;

        for (int i = 0; i < unitTypesToUse.Length; i++)
        {
            if (unitTypesToUse[i].unitType == type)
                unitToUse = unitTypesToUse[i];
        }

        FreeUnits = new List<Unit>();
        ActiveUnits = new List<Unit>();

        for (int j = 0; j < maxUnits; j++)
        {
            Unit e = CreateEnemy(unitToUse.unitType);
            FreeUnits.Add(e);
            e.gameObject.SetActive(false);
        }
    }
    protected override Unit PullFromPool(UnitType type)
    {
        for (int i = 0; i < FreeUnits.Count; i++)
        {
            Unit e = FreeUnits[i];
            if (e.unitType == type)
            {
                ActiveUnits.Add(e);
                FreeUnits.Remove(e);
                e.gameObject.SetActive(true);
                return e;
            }
        }
        
        return null;
    }
    public void SpawnPlayerUnit()
	{
		if(IsDead())
			return;
		gameController.UpdateResources(unitToUse.moneyCost, unitToUse.energyCost);
		SpawnUnits(unitToUse);
		return;
	}

	public override void SpawnUnits (Unit type)
	{
		Unit unit = PullFromPool(type.unitType);
        if (unit == null) return;
		unit.Spawn();
		unit.gameObject.layer = LayerMask.NameToLayer("PlayerUnit");
        Vector3 targetPos = _transform.position + (Vector3.right * maxRange);
        targetPos.z = -0.1f;
        Vector3 angle = new Vector3(0, 0, 360 / maxUnits * ActiveUnits.Count); //z = 360 / maxCount * spawnedCount
        Vector3 dir = targetPos - _transform.position;
        dir = Quaternion.Euler(angle) * dir;
        targetPos = _transform.position + dir;
        unit.transform.position = targetPos;
		if(buildingType == BuildingType.DefensiveSpawner)
		{
			DefensiveUnit defUnit = unit.GetComponent<DefensiveUnit>();
			defUnit.SetInitPos(_transform.position);
		}
    }

	public override void RespawnMe ()
	{
		for(int i = ActiveUnits.Count - 1; i >= 0;i--)
		{
			Unit unit = ActiveUnits[i];
			unit.Die();
			PushToPool(unit);
		}
		base.RespawnMe ();
	}

	public override void Respawn ()
	{
		base.Respawn ();

	}
}
