using UnityEngine;
using System.Collections;

public class LightWiggle : MonoBehaviour {

	private Vector3 InitialPos;
	private float InitialIntensity;
	private float UpdateTime = 0f;
	public float UpdateRate = 6f;
	// Use this for initialization
	void Start () 
	{
		InitialPos = this.transform.position;
		InitialIntensity = this.transform.GetComponent<Light>().intensity;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		UpdateTime += Random.value;
		if (UpdateTime >= UpdateRate)
		{
			UpdateTime = 0f;
			this.transform.position = InitialPos + Random.insideUnitSphere * 0.05f;
			this.transform.GetComponent<Light>().intensity = InitialIntensity + Random.value * 0.5f;
		}
	}
}
