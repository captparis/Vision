using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	class AttackBomber : AttackModule
	{
		public override void Init()
		{
			iconPath = "Attack-Bomb128";
			base.Init();
		}
	}
}
