using UnityEngine;
using System.Collections;
using Shared.Framework;

public class Bomb : MonoBehaviour {
	
	[SerializeField]
	private float gravity = -20f;
	public Vector3 velocity;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		velocity += (gravity * Vector3.up * Time.deltaTime);
		transform.Translate(velocity * Time.deltaTime);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player")
		{
			//print("BAM");
			DamageType dmgType = new DamageType(this.gameObject, "Bullet");
			collision.gameObject.GetComponent<Destructible>().TakeDamage(10, dmgType);
		}
		Destroy(this.gameObject); 
        
    }
}
