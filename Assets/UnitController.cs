﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitController : MonoBehaviour {

	public LayerMask leftClickLayer;
	public LayerMask rightClickLayer;
	public LayerMask playerUnitLayer;
	public LayerMask terrainLayer;
	public float dragThreshold = 1f;
	public float clickLean = 1.3f;
	public GUIStyle dragSkin;
	public GraphicRaycaster uiRaycaster;
	private List<Unit> selectedUnits;

	private ConstructionController constructionController;
	private Camera mainCam;
	private bool selectingUnits = false;

	private float boxLeft;
	private float boxTop;
	private float boxWidth;
	private float boxHeight;
	private bool isDragging = false;
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

		PointerEventData pointer = new PointerEventData(EventSystem.current);
		pointer.position = Input.mousePosition;
		List<RaycastResult> objects = new List<RaycastResult>();
		uiRaycaster.Raycast(pointer, objects);

		if(!constructionController.CheckMouseAvailability() || objects.Count > 0)
		{
			return;
		}


		RaycastHit2D mouseHit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, terrainLayer);
		if(mouseHit.collider != null)
			currentMousePos = mouseHit.point;


		if(Input.GetMouseButtonDown(0) && !isDragging)
		{
			timeToDrag = dragThreshold;

			RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, leftClickLayer);
			if(hit.collider != null)
			{
				mouseDownPoint = currentMousePos;
			}
		}

		if(Input.GetMouseButton(0))
		{
			if(isDragging)
				return;
			
			timeToDrag -= Time.deltaTime;

			if(timeToDrag <= 0f || IsUserDragging(currentMousePos))
				isDragging = true;
		}

		if(Input.GetMouseButtonUp(0))
		{
			if(isDragging)
			{
				isDragging = false;
				Vector2 startPos = new Vector2(mouseDownPoint.x, mouseDownPoint.y);
				Vector2 endPos = new Vector2(currentMousePos.x, currentMousePos.y);

				Collider2D[] units = Physics2D.OverlapAreaAll(startPos, endPos, playerUnitLayer);
				foreach(Collider2D col in units)
				{
					if(!col.CompareTag("PlayerOffensive"))
						return;
					
					Unit unit = col.GetComponent<Unit>();
					if(unit != null)
					{
						unit.HandleSelection(true);
						if(!selectedUnits.Contains(unit))
							selectedUnits.Add(unit);
					}
					else
						Debug.LogError("Why no Unit Script on an object on the player unit layer");
				}
				selectingUnits = false;
			}

			else
			{
				RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, leftClickLayer);

				if(hit.collider != null)
				{
					if(hit.collider.CompareTag("PlayerOffensive"))
					{
						if(!Input.GetMouseButton(1))
						{
							selectingUnits = false;
							ClearSelection();
						}
						else
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
				else
					selectingUnits = false;
			}

		}

		else if(Input.GetMouseButtonUp(1))
		{
			if(!selectingUnits)
			{
				RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, rightClickLayer);

				if(hit.collider != null && selectedUnits.Count > 0)
				{					
//					Vector3 moveTarget = new Vector3(hit.point.x, hit.point.y, -0.1f);
					Vector2 forward = hit.point - new Vector2(selectedUnits[0].transform.position.x, selectedUnits[0].transform.position.y);;
					Vector2 left = new Vector2(-forward.x, forward.y);
					for(int i = 0; i < selectedUnits.Count;i++)
					{
						Unit unit = selectedUnits[i];
						int yOffset = VerticalOffset(i);
						int xOffset = HorizontalOffset(i);
						Vector3 moveTarget = hit.point - forward.normalized * yOffset + left.normalized * xOffset;
						moveTarget.z = -0.1f;
						unit.StartMoving(moveTarget);
					}
				}
			}
			else
				selectingUnits = false;
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

	public void ClearSelection()
	{
		foreach(Unit unit in selectedUnits)
			unit.HandleSelection(false);

		selectedUnits.Clear();
	}

	private bool IsUserDragging(Vector3 mousePos)
	{
		bool dragging =  (mousePos.x < mouseDownPoint.x - clickLean || mousePos.x > mouseDownPoint.x + clickLean
			|| mousePos.y < mouseDownPoint.y - clickLean || mousePos.y > mouseDownPoint.y + clickLean);

		return dragging;
	}

	private int HorizontalOffset(int index)
	{
		float power = index % 5;
		int xFactor = Mathf.CeilToInt(power/2);

		int horizontalOffset = (int)Mathf.Pow (-1, power) * xFactor;
		return horizontalOffset;
	}

	private int VerticalOffset(int index)
	{
		int yFactor = Mathf.FloorToInt(index / 5);
		return yFactor;
	}


}
