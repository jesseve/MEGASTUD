using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : SpawningBuilding {

    public int maxUnits = 6;
    public float maxRange = 2;
	private GameController gameController;

	void Awake()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	protected override void Update ()
	{}

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
    public bool SpawnPlayerUnit()
	{
		Debug.Log("Energy cost: " + unitToUse.energyCost);
		Debug.Log("Money cost: " + unitToUse.moneyCost);
		if(!gameController.CheckResourceAvailability(unitToUse.moneyCost, unitToUse.energyCost))
			return false;

		gameController.UpdateResources(unitToUse.moneyCost, unitToUse.energyCost);
		SpawnUnits(unitToUse);
		return true;
	}

	public override void SpawnUnits (Unit type)
	{
		Unit unit = PullFromPool(type.unitType);
        if (unit == null) return;
		unit.Spawn();
		Vector3 targetPos = _transform.position + Vector3.right;
		targetPos.z = -0.1f;
		Vector3 angle = new Vector3(0, 0, 360 / maxUnits * ActiveUnits.Count); //z = 360 / maxCount * spawnedCount
		Vector3 dir = targetPos - _transform.position;
		dir = Quaternion.Euler(angle) * dir;
		targetPos = _transform.position + dir;
		unit.transform.position = targetPos;
	
	}
}
