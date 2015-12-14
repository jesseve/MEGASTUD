using UnityEngine;
using System.Collections;

public class MenuUnit : Unit {

	public Transform[] wayPoints;
	private int index;

	protected override void Awake ()
	{
		_transform = transform;
		_animator = GetComponent<Animator>();
		_sprite = GetComponent<SpriteRenderer>();
		speed *= Time.fixedDeltaTime;
		minimapIcon = new GameObject ();
		index = 0;
		Vector3 vec = wayPoints[index].position;  
		StartMoving (vec);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Spawn ()
	{
		
	}

	protected override void MoveToHQ ()
	{
		
	}

	protected override void EndMove ()
	{
		base.EndMove ();
		StartCoroutine (Timer ());
	}

	private IEnumerator Timer () {
        float f = Random.value;
        if (f < 0.1f)
            SetAnimator("Attack");
        else
            SetAnimator ("Idle");
		yield return new WaitForSeconds (Random.Range(1,4));
		index = index < wayPoints.Length - 1 ? index + 1 : 0;
		StartMoving (wayPoints [index].position);
	}

	protected override void OnEnable ()
	{
		
	}

	protected override void OnDisable ()
	{
		
	}
}
