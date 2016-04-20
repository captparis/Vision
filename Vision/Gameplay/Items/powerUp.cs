using UnityEngine;
using System.Collections;

public class powerUp : MonoBehaviour {

	HUDScript hud;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			hud = GameObject.Find("Main Camera").GetComponent<HUDScript>();
			hud.increaseScore (10);
			Destroy (this.gameObject);
		}
	}
}
