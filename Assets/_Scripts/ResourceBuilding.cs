using UnityEngine;
using System.Collections;

public class ResourceBuilding : BuildingBase {

	public ResourceType resourceType = ResourceType.Money;
	public float amountProduced = 250f;
	public float productionTime = 5f;

	private float timer;

	private GameController gameController;

	void Awake()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		timer = productionTime;
	}

	protected override void Update()
	{
		base.Update();
		if(underConstruction)
			return;
		
		timer -= Time.deltaTime;

		if(timer <= 0f)
		{
			timer = productionTime;
			gameController.AddResource(resourceType, amountProduced);
		}
	}
}