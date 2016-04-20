using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared.Framework
{
	interface IDamageable
	{
		void TakeDamage(int damage, DamageType damageType);
	}
}