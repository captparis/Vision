using UnityEngine;
using System.Collections;

public class Bomber {
	[SerializeField]
	float shotFrequency = 0.5f;
	private float shotTimer;

	[SerializeField]
	Transform bombPrototype;

	
	// Update is called once per frame
	public void Update () {
		shotTimer += Time.deltaTime;
		if (shotTimer >= shotFrequency)
		{
			shoot();
			shotTimer = 0;
		}
	}
	void shoot()
	{
		//Transform bomb = Instantiate(bombPrototype, transform.position + new Vector3(0, 4, 0), Quaternion.identity) as Transform;
		//bomb.GetComponent<Bomb>().velocity = new Vector3(-10 * Mathf.Sign(transform.rotation.eulerAngles.y), 10, 0);
	}
}
