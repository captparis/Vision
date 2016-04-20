using UnityEngine;
using System.Collections;

public class Dizzy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.LookAt(Vector3.up);
		transform.GetChild(0).transform.Rotate(Vector3.up*Time.deltaTime*-60, Space.World);
	}
}
