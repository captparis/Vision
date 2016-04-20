using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	class MovementFlee : MovementModule
	{

		[SerializeField]
		protected float moveSpeed = 2;
		private Animator anim;

		public override void Init()
		{
			iconPath = "Movement-Flee128";
			base.Init();
			anim = GetComponent<Animator>();
		}

		public override void Move()
		{
			Vector3 move = Vector3.zero;
			Vector3 target = enemy.aimMod.AcquireTarget();
			//Transform transform = enemy.transform;

			if (enemy.aimMod.TargetRange(target) <= maxTargetDist)
			{
				bool blocked = CollisionCheck(-enemy.GetFacingDirection());
				if (Mathf.Approximately(Enemy.FACE_LEFT, enemy.transform.rotation.eulerAngles.y) && !blocked)
				{

					anim.SetFloat("Speed", -5);
					move = Vector3.right * moveSpeed * Time.deltaTime;
				}
				else if (Mathf.Approximately(Enemy.FACE_RIGHT, enemy.transform.rotation.eulerAngles.y) && !blocked)
				{
					anim.SetFloat("Speed", -5);
					move = Vector3.right * -moveSpeed * Time.deltaTime;
				}
				else
				{
					anim.SetFloat("Speed", 0);
				}
			}
			else
			{
				anim.SetFloat("Speed", 0);
			}
			move.y = -21f * Time.deltaTime;
			enemy.controller.Move(move);
		}
		public override void ModifySpeed(float newSpeedModifier)
		{
			if(newSpeedModifier > 0) {
				moveSpeed *= newSpeedModifier;
			}
		}
	}
}
