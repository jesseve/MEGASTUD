using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	[SerializeField] private float moneyAmount;
	[SerializeField] private float energyAmount;
	[SerializeField] private Text moneyText = null;
	[SerializeField] private Text energyText = null;


	// Use this for initialization
	void Start () {
		UpdateResourceTexts();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddResource(ResourceType resource, float amount)
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
}
