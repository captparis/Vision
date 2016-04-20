using UnityEngine;
using System.Collections;

public class Lift : MonoBehaviour {

	private Transform lift;
	private Vector3 pos;
	private Vector3 posTarget;
	public Vector3 topPos = new Vector3 (0,0,-90);
	public Vector3 bottomPos = new Vector3 (0,0,0);
	public float liftSpeed = 1;
	public bool autoLift = false;
	// Use this for initialization
	void Start () {
		lift = this.gameObject.transform;
		pos = lift.position;
		posTarget = pos;
		if (autoLift)
			LiftUp();
	}
	
	// Update is called once per frame
	void Update () {
		pos = lift.position;
		if (pos != posTarget)
		{
			if (Vector3.Distance(pos,posTarget)<0.002)
			{
				lift.position = posTarget;
			}
			else
			{
				lift.position = Vector3.Lerp(pos,posTarget,Time.deltaTime*liftSpeed);
			}
		}
	}
	
	public void LiftUp () 
	{
		posTarget = topPos;
	}
	public void LiftDown ()
	{
		posTarget = bottomPos;
	}
}
