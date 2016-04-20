using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	class SpecialRunner : SpecialModule
	{
		public override void Init()
		{
			iconPath = "Special-Sprinter128";
			base.Init();
			enemy.moveMod.ModifySpeed(2);
			
		}
	}
}

