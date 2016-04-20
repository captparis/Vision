using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	class AimForward : AimModule
	{
		public override void Init()
		{
			iconPath = "Aiming-FireStraight128";
			base.Init();
		}
		public override Vector3 AcquireTarget()
		{
			return enemy.transform.position + (enemy.GetFacingDirection() * Vector3.right * 2) + (Vector3.up * 1.8f);
		}
		public override float TargetRange(Vector3 target)
		{
			float distance = Vector3.Distance(enemy.transform.position, enemy.transform.position + (enemy.GetFacingDirection() * Vector3.right * 2) + (Vector3.up * 1.8f));

			return distance;
		}
	}
}
