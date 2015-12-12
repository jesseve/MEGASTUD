using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UnitController : MonoBehaviour {

	public LayerMask clickLayer;
	public LayerMask playerUnitLayer;
	public LayerMask enemyBuildingLayer;
	public LayerMask terrainLayer;
	public float dragThreshold = 1f;
	public float clickLean = 1.3f;
	public GUIStyle dragSkin;

	private List<Unit> selectedUnits;

	private ConstructionController constructionController;
	private Camera mainCam;
	private bool selectingUnits = false;

	private float boxLeft;
	private float boxTop;
	private float boxWidth;
	private float boxHeight;
	private bool isDragging = false;
	private bool finishedDragOnThisFrame = false;
	private float timeToDrag;
	private Vector3 currentMousePos;
	private Vector3 mouseDownPoint;

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

		RaycastHit2D mouseHit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, terrainLayer);
		if(mouseHit.collider != null)
			currentMousePos = mouseHit.point;


		if(Input.GetMouseButtonDown(0) && !isDragging)
		{
			timeToDrag = dragThreshold;

			RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickLayer);
			if(hit.collider != null)
			{
				mouseDownPoint = currentMousePos;
			}
		}

		if(Input.GetMouseButton(0))
		{
			if(isDragging)
				return;
			Debug.Log("TIME TO DRAG: " + timeToDrag);
			timeToDrag -= Time.deltaTime;

			if(timeToDrag <= 0f || IsUserDragging(currentMousePos))
				isDragging = true;
		}

		if(Input.GetMouseButtonUp(0))
		{
			if(isDragging)
			{
				isDragging = false;
				finishedDragOnThisFrame = true;
			}

			else
			{
				RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickLayer);

				if(hit.collider != null)
				{
					if(hit.collider.gameObject.layer == playerUnitLayer)
					{
						if(!Input.GetMouseButton(1))
							ClearSelection();
						selectingUnits = true;
						Unit unit = hit.collider.GetComponent<Unit>();
						selectedUnits.Add(unit);
						unit.HandleSelection(true);
					}
					else
					{
						selectingUnits = false;
						ClearSelection();
					}
				}
			}
		}

		else if(Input.GetMouseButtonUp(1) && !selectingUnits)
		{
			Debug.Log("Action!!");
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

	void OnGUI()
	{
		boxWidth = mainCam.WorldToScreenPoint(mouseDownPoint).x - Input.mousePosition.x;
		boxHeight = mainCam.WorldToScreenPoint(mouseDownPoint).y - Input.mousePosition.y;
		boxLeft = Input.mousePosition.x;
		boxTop = (Screen.height - Input.mousePosition.y) - boxHeight;

		if(isDragging)
		{
			GUI.Box(new Rect(boxLeft, boxTop, boxWidth, boxHeight), "", dragSkin);
		}
	}

	public bool CheckMouseAvailability()
	{
		return selectedUnits.Count < 1;
	}

	private void ClearSelection()
	{
		Debug.Log("CLEAR!");
		foreach(Unit unit in selectedUnits)
			unit.HandleSelection(false);

		selectedUnits.Clear();
	}

	private bool IsUserDragging(Vector3 mousePos)
	{
		bool dragging =  (mousePos.x < mouseDownPoint.x - clickLean || mousePos.x > mouseDownPoint.x + clickLean
			|| mousePos.y < mouseDownPoint.y - clickLean || mousePos.y > mouseDownPoint.y + clickLean);

		Debug.Log("DRAGGING? " + dragging);
		return dragging;
	}
}
