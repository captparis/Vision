using UnityEngine;
using System.Collections;

public class TriggerToggleCameraMotion : MonoBehaviour {

	public bool ToggleMotionOffOnEnter = true;
	private GameObject MainCamera;
	private GameObject Hero;
	
	// Use this for initialization
	void Start () 
	{
		Hero = GameObject.Find("Hero");
		MainCamera = Camera.main.gameObject;
	}
	
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject == Hero)
		{
			MainCamera.GetComponent<CameraTracking>().trackMotion = !ToggleMotionOffOnEnter;
		}
	}
	void OnTriggerExit (Collider col)
	{
		if (col.gameObject == Hero)
		{
			MainCamera.GetComponent<CameraTracking>().trackMotion = ToggleMotionOffOnEnter;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
