using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	class MovementStand : MovementModule
	{

		public override void Init()
		{
			iconPath = "Movement-Stand128";
			base.Init();
		}
		public override void Move()
		{
			Vector3 move = Vector3.zero;
			
			move.y = -21f * Time.deltaTime;
			enemy.controller.Move(move);
			return;
		}
	}
}
