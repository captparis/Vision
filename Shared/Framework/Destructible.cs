using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shared.Framework
{
	public class Destructible : MonoBehaviour
	{
		public virtual void TakeDamage(int damage, DamageType damageType)
		{
		}
	}
}
