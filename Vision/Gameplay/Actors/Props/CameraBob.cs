using UnityEngine;
using System.Collections;

public class CameraBob : MonoBehaviour 
{
	private float bobDirection = 1f;
	public float bobSpeed = 0.01f;
	private Vector3 defaultPos;
	
	void Start ()
	{
		defaultPos = this.gameObject.transform.position;
	}
	// Update is called once per frame
	void Update () 
	{
		if (Mathf.Abs(defaultPos.y-this.gameObject.transform.position.y) >= 0.5)
		{
			bobDirection *= -1f;
		}
		this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position,(defaultPos+Vector3.up*bobDirection),Time.deltaTime*bobSpeed);
	}
}
