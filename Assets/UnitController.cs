using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitController : MonoBehaviour {

	public LayerMask clickLayer;
	public LayerMask playerUnitLayer;
	public LayerMask enemyBuildingLayer;

	private List<Unit> selectedUnits;

	private ConstructionController constructionController;
	private Camera mainCam;

	void Awake()
	{
		constructionController = GetComponent<ConstructionController>();
		mainCam = Camera.main;
	}

	// Use this for initialization
	void Start () {
		selectedUnits = new List<Unit>();
	}
	
	// Update is called once per frame
	void Update () {

		if(!constructionController.CheckMouseAvailability())
			return;

		if(Input.GetMouseButtonUp(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickLayer);

			if(hit.collider != null)
			{
				if(hit.collider.gameObject.layer == playerUnitLayer)
				{
					selectedUnits.Add(hit.collider.GetComponent<Unit>());
				}
				else
				{
					ClearSelection();
				}
			}
		}

		else if(Input.GetMouseButtonUp(1))
		{
			
		}
	}

	public bool CheckMouseAvailability()
	{
		return selectedUnits.Count < 1;
	}

	private void ClearSelection()
	{
		
	}
}
