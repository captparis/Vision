using UnityEngine;
using System.Collections;

public class CameraTracking : MonoBehaviour {

	public bool trackMotion = true;
	public bool allowYMotion = true;
	public float trackSpeed = 10f;
	public Vector3 offset = new Vector3(0f,6.5f,-20f);
	private Transform targetObject;
	private Vector3 motionTarget;
	private Vector3 lookTarget;

	// Use this for initialization
	void Start () 
	{
		targetObject = GameObject.Find("Hero").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (trackMotion)
		{
			motionTarget.x = Mathf.Lerp(this.transform.position.x, targetObject.position.x+offset.x, Time.deltaTime*trackSpeed);
			if (allowYMotion)
				motionTarget.y = Mathf.Lerp(this.transform.position.y, targetObject.position.y+offset.y, Time.deltaTime*trackSpeed);
			else
				motionTarget.y = offset.y;
			motionTarget.z = offset.z;
			if (this.transform.rotation != Quaternion.Euler(Vector3.zero))
			{
				this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(Vector3.zero), Time.deltaTime*trackSpeed);
			}
			this.transform.position = motionTarget;
		}
		else
		{
			lookTarget = targetObject.transform.position;
			lookTarget.y += offset.y;
			this.transform.LookAt(lookTarget);
		}
	}
}