using UnityEngine;
using System.Collections;

public class OpenWallDoor : MonoBehaviour {

	public Transform door;
	// Use this for initialization
	void Start () 
	{
		
	}
	// Update is called once per frame
	void Update () 
	{
	
	}
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "Hero")
		{
			door.GetComponent<Door>().Open();
		}
	}
	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.name == "Hero")
		{
			Destroy(transform.gameObject);
		}
	}
}
