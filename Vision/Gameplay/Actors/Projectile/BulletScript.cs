using UnityEngine;
using System.Collections;
using Shared.Framework;

public class BulletScript : Destructible {

	public float Speed = 4;
	public float SecondsUntilDestroy = 10;
	public int Damage = 10;
	float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	void FixedUpdate () {
       // Move forward 
		transform.position += Speed * transform.forward;
       
       // If the Bullet has existed as long as SecondsUntilDestroy, destroy it 
       if (Time.time - startTime >= SecondsUntilDestroy) {
		   Destroy(this.gameObject);
       } 
   }
        
   void OnCollisionEnter(Collision collision) {
	   //print("Collision:" + collision.collider.name);
       // Remove the Bullet from the world 
       if (collision.gameObject.tag == "Player")
       {
		   //print("BAM");
		   DamageType dmgType = new DamageType(this.gameObject, "Bullet");
		   collision.gameObject.GetComponent<Destructible>().TakeDamage(Damage, dmgType);
       }
	   Destroy(this.gameObject); 
   }
   public override void TakeDamage(int damage, DamageType dmgType)
   {
	   if (dmgType.source == Camera.main)
	   {
		   Destroy(this.gameObject);
	   }
   }
}
