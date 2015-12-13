using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class SpawningBuilding : BuildingBase
{

    //public members
    public Unit[] unitTypesToUse;    
    public float spawnRate;
    public int spawnCount;
	public float spawnRange;

    //protected members
    [SerializeField]protected List<Unit> FreeUnits;
    [SerializeField]protected List<Unit> ActiveUnits;
    protected UnitType type;
    protected Unit unitToUse;

    protected float spawnTimer;

    //Unity methods
    protected override void Start() {
        base.Start();
        CreateUnitPool();
    }
    protected override void Update()
    {
        base.Update();
		if(underConstruction)
			return;
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate) {
            Spawn();
            spawnTimer = 0;
        }
    }
    public virtual void Spawn() {
        SpawnUnits(unitToUse);
    }
    //public methods
    public virtual void SpawnUnits(Unit type)
    {
        for (int i = 0; i < spawnCount; i += type.size)
        {
            Unit unit = PullFromPool(type.unitType);
            unit.Spawn();
            unit.transform.position = transform.position;
        }
    }

    //protected methods
    protected virtual void CreateUnitPool() {

        if (buildingType == BuildingType.OffensiveSpawner)
            type = UnitType.Attack;
        else if (buildingType == BuildingType.DefensiveSpawner)
            type = UnitType.Defence;

        for (int i = 0; i < unitTypesToUse.Length; i++) {
            if (unitTypesToUse[i].unitType == type)
                unitToUse = unitTypesToUse[i];
        }

        FreeUnits = new List<Unit>();
        ActiveUnits = new List<Unit>();

        for (int j = 0; j < 20; j++)
        {
            Unit e = CreateEnemy(unitToUse.unitType);
            FreeUnits.Add(e);
            e.gameObject.SetActive(false);
        }
    }

    //Returns a free enemy of type given. Creates new object if there is no free enemies of the type
    protected virtual Unit PullFromPool(UnitType type) {

        for (int i = 0; i < FreeUnits.Count; i++) {
            Unit e = FreeUnits[i];
            if (e.unitType == type) {
                ActiveUnits.Add(e);
                FreeUnits.Remove(e);
                e.gameObject.SetActive(true);
                return e;
            }
        }
        Unit unit = CreateEnemy(type);
        if(unit != null)
            ActiveUnits.Add(unit);
        return unit;
    }
    protected void PushToPool(Unit unit) {
        ActiveUnits.Remove(unit);
        FreeUnits.Add(unit);
        unit.gameObject.SetActive(false);
    }

    protected Unit CreateEnemy(UnitType type) {
        foreach (Unit e in unitTypesToUse)
        {
            if (e.unitType == type)
            {
                GameObject go = Instantiate(e.gameObject) as GameObject;
                //go.tag = tag;
                go.transform.SetParent(transform);
                return go.GetComponent<Unit>();
            }
        }
        return null;
    }
}