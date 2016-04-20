using UnityEngine;
using System.Collections;

public class MysteriousCaveTrap : MonoBehaviour {

	void OnTriggerEnter(Collider col) 
	{
		if (col.gameObject.name == "Hero")
		{
			Destroy(this.gameObject);
		}
	}
}
