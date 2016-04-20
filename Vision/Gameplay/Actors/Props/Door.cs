using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	private Quaternion rotTarget;
	private Quaternion rot;
	private Transform door;
	public Vector3 openEuler = new Vector3 (0,0,-90);
	public Vector3 closeEuler = new Vector3 (0,0,0);
	public float OpenSpeed = 1;
	public bool AutoOpen = false;
	public bool startOpen = false;
	// Use this for initialization
	void Start () 
	{
		door = this.transform;
		rot = door.rotation;
		if (startOpen)
			rotTarget = Quaternion.Euler(openEuler);
		else
			rotTarget = Quaternion.Euler(closeEuler);
		if (AutoOpen)
			Open ();
	}
	
	// Update is called once per frame
	void Update () {
		rot = door.rotation;
		if (rot != rotTarget)
		{
			if (Quaternion.Angle(rot,rotTarget)<0.002)
			{
				door.rotation = rotTarget;
			}
			else
			{
				door.rotation = Quaternion.Lerp(rot,rotTarget,Time.deltaTime*OpenSpeed);
			}
		}
	}
	
	public void Open () 
	{
		rotTarget = Quaternion.Euler(openEuler);
	}
	public void Close ()
	{
		rotTarget = Quaternion.Euler(closeEuler);
	}
}
