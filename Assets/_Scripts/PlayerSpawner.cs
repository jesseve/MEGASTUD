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
}
