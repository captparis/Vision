using UnityEngine;
using System.Collections;
using Shared.Framework;

public class DestroyerScript : MonoBehaviour {

	public GameObject EntryEffect;
	
	void OnTriggerEnter(Collider other)
	{
		if(EntryEffect)
		{
			Instantiate(EntryEffect, other.transform.position, Quaternion.Euler(Vector3.zero));
		}
		
		if(other.tag == "Player")
		{
			other.GetComponent<HeroCharacterController>().KillPlayer();
			other.gameObject.SetActive(false);
			return;
		}

		if(other.GetComponent<Destructible>())
		{
			DamageType dmgtype = new DamageType(Camera.main, "World");
			other.GetComponent<Destructible>().TakeDamage(1000, dmgtype);
		}
	}
}
