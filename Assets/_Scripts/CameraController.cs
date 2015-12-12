using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	public float scrollSpeed = 10f;
	public float moveZoneThreshold = 2.5f;
	public BoxCollider2D terrainCollider;
	public Camera minimapCam;

	private Camera mainCam;
	private float xMinClamp;
	private float xMaxClamp;
	private float yMinClamp;
	private float yMaxClamp;

	private float camVertical;
	private float camHorizontal;
	private GraphicRaycaster uiRaycaster;

	void Start () {
		mainCam = Camera.main;
		camVertical = mainCam.orthographicSize;
		camHorizontal = mainCam.orthographicSize * Screen.width / Screen.height;
		xMinClamp = terrainCollider.transform.position.x - terrainCollider.bounds.extents.x + camHorizontal;
		xMaxClamp = terrainCollider.transform.position.x + terrainCollider.bounds.extents.x - camHorizontal;
		yMinClamp = terrainCollider.transform.position.y - terrainCollider.bounds.extents.y + camVertical;
		yMaxClamp = terrainCollider.transform.position.y + terrainCollider.bounds.extents.y - camVertical;

		uiRaycaster = (GraphicRaycaster)FindObjectOfType(typeof(GraphicRaycaster));

	}

	void Update () {

		if(!EventSystem.current.IsPointerOverGameObject())
			CheckScrolling();
		if(Input.GetMouseButtonDown(0))
		{
			if(EventSystem.current.IsPointerOverGameObject())
			{
				PointerEventData pointer = new PointerEventData(EventSystem.current);
				pointer.position = Input.mousePosition;
				List<RaycastResult> objects = new List<RaycastResult>();
				uiRaycaster.Raycast(pointer, objects);
				foreach(RaycastResult obj in objects)
				{
					if(obj.gameObject.tag == "Minimap")
					{
						RawImage img = obj.gameObject.GetComponent<RawImage>();
						Texture tex = img.texture;
						Rect rect = img.rectTransform.rect;
						Vector2 point;
						RectTransformUtility.ScreenPointToLocalPointInRectangle(img.rectTransform, pointer.position, null, out point);

						int px = Mathf.Clamp(0, (int)(((point.x-rect.x)*tex.width)/rect.width), tex.width);
						int py = Mathf.Clamp(0, (int)(((point.y-rect.y)*tex.height)/rect.height), tex.height);

						Vector3 normalizedPoint = new Vector3((float)px/tex.width, (float)py/tex.height);
						mainCam.transform.position = minimapCam.ViewportToWorldPoint(normalizedPoint);
						break;
					}
				}
			}
		}
	}

	private void CheckScrolling()
	{
		Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
		Vector3 camPos = mainCam.transform.position;
		if(Input.GetMouseButtonDown(0))
			Debug.Log("MOUSE: " +mousePos);
		float xMin = camPos.x - camHorizontal;
		float xMax = camPos.x + camHorizontal;
		float yMin = camPos.y - camVertical;
		float yMax = camPos.y + camVertical;

		Vector3 moveVector = Vector3.zero;

		if(mousePos.x >= xMin && mousePos.x <= (xMin + moveZoneThreshold))
		{
			moveVector.x = -1;
		}

		else if(mousePos.x <= xMax && mousePos.x >= (xMax - moveZoneThreshold))
		{
			moveVector.x = 1;
		}

		if(mousePos.y >= yMin && mousePos.y <= (yMin + moveZoneThreshold + 1f))
		{
			moveVector.y = -1;
		}

		else if(mousePos.y <= yMax && mousePos.y >= (yMax - moveZoneThreshold))
		{
			moveVector.y = 1;
		}

		mainCam.transform.Translate(moveVector * scrollSpeed * Time.deltaTime);
		Vector3 newPos = mainCam.transform.position;
		newPos.x = Mathf.Clamp(newPos.x, xMinClamp, xMaxClamp);
		newPos.y = Mathf.Clamp(newPos.y, yMinClamp, yMaxClamp);
		mainCam.transform.position = newPos;
	}
}
