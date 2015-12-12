using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	[SerializeField] private float money;
	[SerializeField] private float energy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddResource(ResourceType resource, float amount)
	{
		switch(resource)
		{
		case ResourceType.Money:
			money += amount;
			break;

		case ResourceType.Energy:
			energy += amount;
			break;
		}
	}
}
