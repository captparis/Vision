using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	class SpecialTanky : SpecialModule
	{
		public override void Init()
		{
			iconPath = "Special-Tanky128";
			base.Init();
			enemy.ModifyMaxHealth(3);
		}
	}
}
