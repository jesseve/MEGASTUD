using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[SerializeField] private float checkTime = 5f;
	[SerializeField] private float moneyAmount;
	[SerializeField] private float energyAmount;
	[SerializeField] private Text moneyText = null;
	[SerializeField] private Text energyText = null;
	[SerializeField] private BuildingBase playerHeadquarters = null;

	private float checkTimer;
	private float resourceTimer;
	private List<BuildingBase> activeBuildings;
	private List<BuildingBase> buildingsInQueue;
	private bool checkingBuildings;

	// Use this for initialization
	void Awake () {
		UpdateResourceTexts();
		checkTimer = checkTime;
		resourceTimer = checkTime;
		activeBuildings = new List<BuildingBase>();
		buildingsInQueue = new List<BuildingBase>();
		activeBuildings.Add(playerHeadquarters);
	}
	
	// Update is called once per frame
	void Update () {
		checkTimer -= Time.deltaTime;
		if(checkTimer <= 0f)
		{
			if(!checkingBuildings)
				StartCoroutine(HandleActiveBuildings());
			else
			{
				resourceTimer -= Time.deltaTime;
				if(resourceTimer <= 0f)
				{
					resourceTimer = checkTime;
					GetResources();
				}
			}
		} else resourceTimer = checkTime;
	}

	public void AddResources(ResourceType resource, float amount)
	{
		switch(resource)
		{
		case ResourceType.Money:
			moneyAmount += amount;
			break;

		case ResourceType.Energy:
			energyAmount += amount;
			break;
		}

		UpdateResourceTexts();
	}

	public bool CheckResourceAvailability(float money, float energy)
	{
		if(money > moneyAmount || energy > energyAmount)
			return false;

		return true;
	}

	public void UpdateResources(float money, float energy)
	{
		moneyAmount -= money;
		energyAmount -= energy;
		UpdateResourceTexts();
	}

	private void UpdateResourceTexts()
	{
		moneyText.text = "Money: " + moneyAmount;
		energyText.text = "Energy: " +energyAmount;
	}

	public void AddNewBuilding(BuildingBase building)
	{
		buildingsInQueue.Add(building);
	}

	private void GetResources()
	{
		foreach(BuildingBase building in activeBuildings)
		{
			if(building.buildingType == BuildingType.ResourceBuilding)
			{
				ResourceBuilding resourceBuilding = (ResourceBuilding)building;
				AddResources(resourceBuilding.resourceType, resourceBuilding.amountProduced);
			}
		}

		for(int i = buildingsInQueue.Count - 1; i >= 0; i--)
		{
			if(buildingsInQueue[i].buildingType == BuildingType.ResourceBuilding)
			{
				activeBuildings.Add(buildingsInQueue[i]);
				buildingsInQueue.RemoveAt(i);
			}
		}
	}

	private IEnumerator HandleActiveBuildings()
	{
		checkingBuildings = true;
		List<BuildingBase> remainingBuildings = new List<BuildingBase>();
		for(int i = activeBuildings.Count - 1; i >= 0;i--)
			remainingBuildings.Add(activeBuildings[i]);

		while(remainingBuildings.Count > 0)
		{
			for(int i = remainingBuildings.Count - 1; i >= 0;i--)
			{
				BuildingBase building = remainingBuildings[i];

				switch(building.buildingType)
				{
				case BuildingType.ResourceBuilding:
					ResourceBuilding resourceBuilding = (ResourceBuilding)building;
					AddResources(resourceBuilding.resourceType, resourceBuilding.amountProduced);
					remainingBuildings.RemoveAt(i);
					break;
				case BuildingType.OffensiveSpawner: case BuildingType.DefensiveSpawner:
					PlayerSpawner spawner = (PlayerSpawner)building;
					spawner.SpawnPlayerUnit();
					remainingBuildings.RemoveAt(i);
					break;
				}
			}
			yield return null;
		}
		checkTimer = checkTime;
		foreach(BuildingBase building in buildingsInQueue)
			activeBuildings.Add(building);

		buildingsInQueue.Clear();
		checkingBuildings = false;
	}
}
