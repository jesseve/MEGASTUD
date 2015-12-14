using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ConstructionController : MonoBehaviour {

	[SerializeField] Transform hqTransform = null;
	[SerializeField] BuildingBase[] buildingPrefabs = new BuildingBase[3];
	[SerializeField] Color ghostColor = Color.green;
	[SerializeField] Color cantConstruct = Color.red;
	[SerializeField] string defaultLayer = "Building";
	[SerializeField] string ghostLayer = "GhostImage";
	[SerializeField] Transform buildingParent = null;
	[SerializeField] float constructionRange = 20f;
	public GraphicRaycaster uiRaycaster;

	private SpriteRenderer ghostBuilding;
	private BuildingBase currentBuilding;
	private Rigidbody2D currentRB;
	private Animator currentAnim;
	private CheckSlotAvailability obstacleDetector;
	private AudioSource _audio;

	private Camera mainCam;
	private GameController gameController;
	private UnitController unitController;

	private bool initialized = true;
	void Start()
	{
		mainCam = Camera.main;
		gameController = GetComponent<GameController>();
		unitController = GetComponent<UnitController>();
		_audio = GetComponent<AudioSource>();
	}

	void Update()
	{
		if(currentBuilding != null)
		{
			if (!initialized) 
			{
				PointerEventData pointer = new PointerEventData(EventSystem.current);
				pointer.position = Input.mousePosition;
				List<RaycastResult> objects = new List<RaycastResult>();
				uiRaycaster.Raycast(pointer, objects);
				if (objects.Count < 1) 
				{
					ghostBuilding.enabled = true;
					initialized = true;
					return;
				} 

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

			PointerEventData pointer2 = new PointerEventData(EventSystem.current);
			pointer2.position = Input.mousePosition;
			List<RaycastResult> objects2 = new List<RaycastResult>();
			uiRaycaster.Raycast(pointer2, objects2);

			if(Input.GetMouseButtonUp(0) && canBeConstructed)
			{
				PlaceBuilding();
			}

			if(Input.GetMouseButtonUp(1) || objects2.Count > 0)
			{
				CancelBuilding();
			}
		}
	}

	public void ConstructNewBuilding(int type)
	{
		if(currentBuilding != null)
			return;

		_audio.clip = SoundManager.GetSoundClip(SoundClip.PlaceBuilding);
		_audio.Play();
		unitController.ClearSelection();
		currentBuilding = Instantiate(buildingPrefabs[type]) as BuildingBase;
		currentBuilding.underConstruction = true;
		GameObject go = currentBuilding.gameObject;
		go.layer = LayerMask.NameToLayer(ghostLayer);
		obstacleDetector = go.gameObject.AddComponent<CheckSlotAvailability>();
		currentRB = go.gameObject.AddComponent<Rigidbody2D>();
		currentRB.gravityScale = 0f;
		currentRB.isKinematic = true;
		currentAnim = go.gameObject.GetComponent<Animator>();
		currentAnim.enabled = false;
		ghostBuilding = go.GetComponent<SpriteRenderer>();

		go.transform.GetChild(0).gameObject.SetActive(false);

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

	private void PlaceBuilding()
	{
		GameObject go = currentBuilding.gameObject;
			go.transform.GetChild(0).gameObject.SetActive(true);

		gameController.UpdateResources(currentBuilding.moneyCost, currentBuilding.energyCost);

		ghostBuilding.gameObject.layer = LayerMask.NameToLayer(defaultLayer);
		ghostBuilding.GetComponent<SpriteRenderer>().color = Color.white;
		ghostBuilding.transform.SetParent(buildingParent);
		ghostBuilding = null;

		currentBuilding.underConstruction = false;
		gameController.AddNewBuilding(currentBuilding);
		currentBuilding = null;
		currentAnim.enabled = true;
		currentAnim = null;

		Destroy(currentRB);
		Destroy(obstacleDetector);
	}

	private void CancelBuilding() 
	{
		Destroy(currentBuilding.gameObject);
	}
}

