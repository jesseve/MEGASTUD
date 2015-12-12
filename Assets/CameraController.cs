using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float scrollSpeed = 10f;
	public float moveZoneThreshold = 2.5f;
	public BoxCollider2D terrainCollider;

	private Camera mainCam;
	private float xMinClamp;
	private float xMaxClamp;
	private float yMinClamp;
	private float yMaxClamp;

	private float camVertical;
	private float camHorizontal;

	// Use this for initialization
	void Start () {
		mainCam = Camera.main;
		camVertical = mainCam.orthographicSize;
		camHorizontal = mainCam.orthographicSize * Screen.width / Screen.height;
		xMinClamp = terrainCollider.transform.position.x - terrainCollider.bounds.extents.x + camHorizontal;
		xMaxClamp = terrainCollider.transform.position.x + terrainCollider.bounds.extents.x - camHorizontal;
		yMinClamp = terrainCollider.transform.position.y - terrainCollider.bounds.extents.y + camVertical;
		yMaxClamp = terrainCollider.transform.position.y + terrainCollider.bounds.extents.y - camVertical;
	}
	
	// Update is called once per frame
	void Update () {

		CheckScrolling();
	}

	private void CheckScrolling()
	{
		Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
		Vector3 camPos = mainCam.transform.position;
		Debug.Log("CAM: " + camPos);
		if(Input.GetMouseButtonDown(0))
			Debug.Log("MOUSE: " + mousePos);
		float xMin = camPos.x - camHorizontal;
		float xMax = camPos.x + camHorizontal;
		float yMin = camPos.y - camVertical;
		float yMax = camPos.y + camVertical;

		Debug.Log("xMin: " + xMin+  ", xMax: "+ xMax + ", yMin: " +yMin +", yMax: " + yMax);

		Vector3 moveVector = Vector3.zero;

		if(mousePos.x >= xMin && mousePos.x <= (xMin + moveZoneThreshold))
		{
			Debug.Log("Move left");
			moveVector.x = -1;
		}

		else if(mousePos.x <= xMax && mousePos.x >= (xMax - moveZoneThreshold))
		{
			Debug.Log("Move right");
			moveVector.x = 1;
		}

		if(mousePos.y >= yMin && mousePos.y <= (yMin + moveZoneThreshold))
		{
			Debug.Log("Move Down");
			moveVector.y = -1;
		}

		else if(mousePos.y <= yMax && mousePos.y >= (yMax - moveZoneThreshold))
		{
			Debug.Log("Move Up");
			moveVector.y = 1;
		}

		mainCam.transform.Translate(moveVector * scrollSpeed * Time.deltaTime);
		Vector3 newPos = mainCam.transform.position;
		newPos.x = Mathf.Clamp(newPos.x, xMinClamp, xMaxClamp);
		newPos.y = Mathf.Clamp(newPos.y, yMinClamp, yMaxClamp);
		mainCam.transform.position = newPos;
	}
}
