using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ConstructionController : MonoBehaviour {

	[SerializeField] Transform hqTransform = null;
	[SerializeField] BuildingBase[] buildingPrefabs = new BuildingBase[3];
	[SerializeField] Color ghostColor = Color.green;
	[SerializeField] Color cantConstruct = Color.red;
	[SerializeField] string defaultLayer = "Building";
	[SerializeField] string ghostLayer = "GhostImage";
	[SerializeField] Transform buildingParent = null;
	[SerializeField] float constructionRange = 20f;

	private SpriteRenderer ghostBuilding;
	private BuildingBase currentBuilding;
	private Rigidbody2D currentRB;
	private CheckSlotAvailability obstacleDetector;

	private Camera mainCam;
	private GameController gameController;
	private UnitController unitController;

	private bool initialized = true;
	void Start()
	{
		mainCam = Camera.main;
		gameController = GetComponent<GameController>();
		unitController = GetComponent<UnitController>();
	}

	void Update()
	{
		if(currentBuilding != null)
		{
			if (!initialized) 
			{
				if (!EventSystem.current.IsPointerOverGameObject()) 
				{
					ghostBuilding.enabled = true;
					initialized = true;
				} 
				else return;
			}

			RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity);

			float distance = 0f;
			if(hit.collider != null)
			{
				ghostBuilding.transform.position = hit.point;
				distance = Vector3.Distance(hqTransform.position, hit.point);
			}

			bool canBeConstructed = (obstacleDetector.CanBeConstructed() && gameController.CheckResourceAvailability(currentBuilding.moneyCost, currentBuilding.energyCost) && distance <= constructionRange);
			ghostBuilding.color = canBeConstructed?ghostColor:cantConstruct;

			if(Input.GetMouseButtonUp(0) && canBeConstructed)
			{
				gameController.UpdateResources(currentBuilding.moneyCost, currentBuilding.energyCost);

				ghostBuilding.gameObject.layer = LayerMask.NameToLayer(defaultLayer);
				ghostBuilding.GetComponent<SpriteRenderer>().color = Color.white;
				ghostBuilding.transform.SetParent(buildingParent);
				ghostBuilding = null;

				currentBuilding.underConstruction = false;
				currentBuilding = null;

				Destroy(currentRB);
				Destroy(obstacleDetector);
			}

			if(Input.GetMouseButtonUp(1) || EventSystem.current.IsPointerOverGameObject())
			{
				CancelBuilding();
			}
		}
	}

	public void ConstructNewBuilding(int type)
	{
		if(currentBuilding != null || unitController.CheckMouseAvailability())
			return;
		
		currentBuilding = Instantiate(buildingPrefabs[type]) as BuildingBase;
		currentBuilding.underConstruction = true;

		GameObject go = currentBuilding.gameObject;
		go.layer = LayerMask.NameToLayer(ghostLayer);
		obstacleDetector = go.gameObject.AddComponent<CheckSlotAvailability>();
		currentRB = go.gameObject.AddComponent<Rigidbody2D>();
		currentRB.gravityScale = 0f;
		ghostBuilding = go.GetComponent<SpriteRenderer>();
		if(ghostBuilding != null)
		{
			ghostBuilding.color = ghostColor;
		}
		else Debug.LogError("No Sprite Renderer found in building prefab!");
		ghostBuilding.enabled = false;
		initialized = false;

	}

	public bool CheckMouseAvailability()
	{
		return currentBuilding == null;
	}

	private void CancelBuilding() 
	{
		Destroy(currentBuilding.gameObject);
	}
}
