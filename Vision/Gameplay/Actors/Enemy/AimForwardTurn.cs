using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	class AimForwardTurn : AimModule
	{

		Transform player;

		public override void Init()
		{
			iconPath = "Aiming-FlipFollow128";
			base.Init();
			player = GameObject.FindGameObjectWithTag("Player").transform;
		}

		public override Vector3 AcquireTarget()
		{
			return enemy.transform.position + (enemy.GetFacingDirection() * Vector3.right * 2) + (Vector3.up * 1.5f);
		}
		public override float TargetRange(Vector3 target)
		{
			float distance = Vector3.Distance(enemy.transform.position, player.transform.position);

			return distance;
		}
		public override void ProcessUpdate()
		{
			TurnTowardsTarget(player.position);
			base.ProcessUpdate();
		}
	}
}