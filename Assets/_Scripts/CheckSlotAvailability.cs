using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckSlotAvailability : MonoBehaviour {

	List<GameObject> obstacles;

	void Awake()
	{
		obstacles = new List<GameObject>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(!obstacles.Contains(other.gameObject))
			obstacles.Add(other.gameObject);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(obstacles.Contains(other.gameObject))
			obstacles.Remove(other.gameObject);
	}

	public bool CanBeConstructed()
	{
		return obstacles.Count < 1;
	}
}
