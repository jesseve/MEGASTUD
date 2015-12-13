using UnityEngine;
using System.Collections;

public class PlayerSpawner : SpawningBuilding {

	private GameController gameController;

	void Awake()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}

	protected override void Update ()
	{}

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
		Unit unit = PullFromPool(type);
		unit.Spawn();
		Vector3 targetPos = _transform.position + Vector2.right;
		targetPos.z = -0.1f;
		Vector3 angle = new Vector3(0, 0, 360 / 1 * 1); //z = 360 / maxCount * spawnedCount
		Vector3 dir = targetPos - _transform.position;
		dir = Quaternion.Euler(angle) * dir;
		targetPos = _transform.position + dir;
		unit.transform.position = targetPos;
	
	}
}
