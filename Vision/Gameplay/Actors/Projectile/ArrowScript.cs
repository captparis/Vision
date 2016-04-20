using UnityEngine;
using System.Collections;
using Shared.Framework;

public class ArrowScript : Destructible {

	public float Speed = 1500;
	public float SecondsUntilDestroy = 10;
	public int Damage = 10;
	private Rigidbody rb;
	float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		rb = GetComponent<Rigidbody>();
		rb.AddRelativeForce(Vector3.forward*Speed);
	}
	
	void FixedUpdate () 
	{
       // Move forward 
		//transform.position += Speed * transform.forward;
		if (rb)
		{
			if(rb.velocity != Vector3.zero)
				transform.rotation = Quaternion.LookRotation(rb.velocity);
			transform.Rotate(Vector3.forward*10);
		}
       // If the Bullet has existed as long as SecondsUntilDestroy, destroy it 
       if (Time.time - startTime >= SecondsUntilDestroy) 
       {
		   Destroy(this.gameObject);
       }
   }
        
   void OnTriggerEnter(Collider col) {
		//print(col.gameObject.tag);
	   //print("Collision:" + collision.collider.name);
       // Remove the Bullet from the world 
       if (col.gameObject.tag == "Player")
       {
			//print("BAM");
			DamageType dmgType = new DamageType(this.gameObject, "Bullet");
			col.gameObject.GetComponent<Destructible>().TakeDamage(Damage, dmgType);
			Destroy(this.gameObject);
       }
	   else if (!col.isTrigger)
	   {
	   		Destroy(rb);
	   		Destroy(this.gameObject.GetComponent<BoxCollider>());
			this.transform.parent = col.transform;
	   }
   }
   public override void TakeDamage(int damage, DamageType dmgType)
   {
	   if (dmgType.source == Camera.main)
	   {
		   Destroy(this.gameObject);
	   }
   }
}
