using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	class SpecialHealthy : SpecialModule
	{
		public override void Init()
		{
			iconPath = "Special-Healthy128";
			base.Init();
			enemy.ModifyMaxHealth(2);
		}
	}
}
