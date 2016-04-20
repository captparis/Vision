using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	public class AttackSword : AttackModule
	{
		[SerializeField]
		Vector3 aimOffset = new Vector3(0, 1.5f, 0);

		float aimRot;
		public Transform spineBone;

		public override void Init()
		{
			iconPath = "Attack-Sword128";
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
			RaycastHit[] hits;

			Transform transform = enemy.transform;
			
			Vector3[] startPos = new Vector3[3]{
				transform.position + new Vector3(0, 0, 0),
				transform.position + new Vector3(0, -0.5f, 0),
				transform.position + new Vector3(0, -1, 0)
			};
			Color[] colours = new Color[3]{
				Color.red,
				Color.green,
				Color.blue
			};
			for (int i = 0; i < 3; i++)
			{
				Vector3 aimStart = Vector3.zero;
				aimStart.y = aimOffset.y;
				aimStart.x = aimOffset.x * enemy.GetFacingDirection();
				startPos[i] += aimStart;
			}
			Vector3 direction = target - startPos[0];

			direction.Normalize();
			direction *= 3f;

			for (int i = 0; i < 3; i++)
			{

				Vector3 endPos = startPos[i] + direction;

				hits = Physics.RaycastAll(startPos[i], direction, 3f);
				//Debug.DrawRay(startPos[i], new Vector3(direction, 0, 0), colours[i], 3);
				Debug.DrawLine(startPos[i], endPos, colours[i], 1);
				foreach (var hit in hits)
				{
					//print(hit.collider.name);
					if (hit.collider != null && hit.collider.GetComponent<HeroCharacterController>())
					{
						Shared.Framework.DamageType dmgtype = new Shared.Framework.DamageType(enemy.gameObject, "Attacker");
						hit.collider.GetComponent<HeroCharacterController>().TakeDamage(10, dmgtype);
						return;
					}
				}

			}
		}
		void LateUpdate()
		{
			float clampedRot = Mathf.Clamp(270 + aimRot, 225, 315);
			spineBone.localRotation = Quaternion.Euler(new Vector3(0, 70, clampedRot));
		}
	}
}
