using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	public class AttackBow : AttackModule
	{
		//[SerializeField]
		//private Transform arrow;
		[SerializeField]
		Vector3 aimOffset = new Vector3(1, 1.5f, 0);

		float aimRot;
		public Transform spineBone;

		public override void Init()
		{
			iconPath = "Attack-Bow128";

			base.Init();
			spineBone = transform.Find("hero_root/hero_spine_1/hero_spine_2");
		}
		public override void ProcessUpdate()
		{
			Vector3 target = aimMod.AcquireTarget();
			Transform transform = enemy.transform;

			//Vector3 euler = transform.rotation.eulerAngles;

			Vector3 aimStart = transform.position;
			aimStart.y += aimOffset.y;
			aimStart.x += aimOffset.x * enemy.GetFacingDirection();
			Vector3 direction = target - aimStart;

			//Debug.DrawRay(aimStart, direction, Color.black, 0.2f);
			aimRot = Mathf.Rad2Deg * Mathf.Atan(direction.y / -Mathf.Abs(direction.x));
			
			base.ProcessUpdate();
		}

		public override void Attack()
		{
			Vector3 target = aimMod.AcquireTarget();
			
			//Vector3 euler = transform.rotation.eulerAngles;

			Vector3 aimStart = transform.position;
			aimStart.y += aimOffset.y;
			aimStart.x += aimOffset.x * enemy.GetFacingDirection();
			
			Vector3 direction = target - aimStart;

			Debug.DrawRay(aimStart, direction, Color.black, 0.2f);
			//aimRot = Mathf.Rad2Deg * Mathf.Atan(direction.y / -Mathf.Abs(direction.x));
			direction.Normalize();
			direction *= 1.5f;
			Vector3 bulletOffset = direction;
			//direction.z = Mathf.Abs(direction.x);
			//direction.x = 0;

			Instantiate(projectile, aimStart + bulletOffset * 0.5f, Quaternion.Euler(new Vector3(aimRot, transform.rotation.eulerAngles.y, 0)));
		}

		void LateUpdate()
		{
			float clampedRot = Mathf.Clamp(270 + aimRot, 225, 315);
			spineBone.localRotation = Quaternion.Euler(new Vector3(0, 70, clampedRot));
		}
	}
}
