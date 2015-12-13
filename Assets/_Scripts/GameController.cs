using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[SerializeField] private float buildingCheckTime = 5f;
	[SerializeField] private float moneyAmount;
	[SerializeField] private float energyAmount;
	[SerializeField] private Text moneyText = null;
	[SerializeField] private Text energyText = null;
	[SerializeField] private BuildingBase playerHeadquarters = null;

	private float checkTimer;
	private List<BuildingBase> activeBuildings;
	private List<BuildingBase> buildingsInQueue;
	private bool checkingBuildings;

	// Use this for initialization
	void Start () {
		UpdateResourceTexts();
		checkTimer = buildingCheckTime;
		activeBuildings = new List<BuildingBase>();
		buildingsInQueue = new List<BuildingBase>();
		activeBuildings.Add(playerHeadquarters);
	}
	
	// Update is called once per frame
	void Update () {
		checkTimer -= Time.deltaTime;
		if(checkTimer <= 0f && !checkingBuildings)
		{
			StartCoroutine(HandleActiveBuildings());
		}
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
					//TODO If enough resources spawn unit and remove from list, else just break
					break;
				}
			}
			yield return null;
		}
		checkTimer = buildingCheckTime;
		foreach(BuildingBase building in buildingsInQueue)
			activeBuildings.Add(building);

		buildingsInQueue.Clear();
		checkingBuildings = false;
	}
}
