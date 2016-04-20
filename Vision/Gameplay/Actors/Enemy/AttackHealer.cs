using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	class AttackHealer : AttackModule
	{

		//bool healed;

		public override void Init()
		{
			iconPath = "Attack-Healer128";
			base.Init();
		}
		/*public override bool CanAttack()
		{
			return (!healed);
		}*/

		public override void Attack()
		{
			Shared.Framework.DamageType dmgtype = new Shared.Framework.DamageType(enemy.gameObject, "Healer");
			HeroCharacterController HCC = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCharacterController>();
			if (Vector3.Distance(transform.position, HCC.transform.position) < 3)
			{
				HCC.TakeDamage(-10, dmgtype);
				//healed = true;
			}
		}
		public override void Death(Shared.Framework.DamageType damageType)
		{
			base.Death(damageType);
			if (damageType.name == "Player")
			{
				GameManager.GM.youMonster++;
			}
		}
	}
}
