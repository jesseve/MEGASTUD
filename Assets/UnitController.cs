using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitController : MonoBehaviour {

	public LayerMask clickLayer;
	public LayerMask playerUnitLayer;
	public LayerMask enemyBuildingLayer;
	public LayerMask terrainLayer;

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
		{
			Debug.Log("Unit can't be selected");
			return;
		}
		Debug.Log("Unit can be selected");
		if(Input.GetMouseButtonUp(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickLayer);

			if(hit.collider != null)
			{
				if(hit.collider.gameObject.layer == playerUnitLayer)
				{
					if(Input.GetMouseButton(0))
						ClearSelection();
					
					Unit unit = hit.collider.GetComponent<Unit>();
					selectedUnits.Add(unit);
					unit.HandleSelection(true);
				}
				else
				{
					ClearSelection();
				}
			}
		}

		else if(Input.GetMouseButtonUp(1))
		{
			RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickLayer);

			if(hit.collider != null)
			{
				if(hit.collider.gameObject.layer == terrainLayer)
				{
					foreach(Unit unit in selectedUnits)
						unit.StartMoving(hit.point);
				}
				else if(hit.collider.gameObject.layer == enemyBuildingLayer)
				{
					IDamageable target = hit.collider.GetComponent<IDamageable>();
					if(target != null)
					{
						foreach(Unit unit in selectedUnits)
							unit.StartAttack(target);
					}
					else Debug.LogError("Why no IDamageable in enemy building?!?!");
				}
			}
		}
	}

	public bool CheckMouseAvailability()
	{
		return selectedUnits.Count < 1;
	}

	private void ClearSelection()
	{
		foreach(Unit unit in selectedUnits)
			unit.HandleSelection(false);

		selectedUnits.Clear();
	}
}
