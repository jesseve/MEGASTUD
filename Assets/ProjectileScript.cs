using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	public float speed = 5f;
	public Vector3 targetPos;

	void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
		if(transform.position == targetPos)
			Destroy(gameObject);
	}
}
