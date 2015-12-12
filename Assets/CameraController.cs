using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float scrollSpeed = 10f;
	public float moveZoneThreshold = 2.5f;

	private Camera mainCam;

	// Use this for initialization
	void Start () {
		mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

		CheckScrolling();
	}

	private void CheckScrolling()
	{
		Vector3 mousePos = Input.mousePosition;
		float xMin = mainCam.transform.position.x;
		float xMax = xMin + mainCam.pixelWidth;
		float yMin = mainCam.transform.position.y;
		float yMax = yMin + mainCam.pixelHeight;

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
	}
}
