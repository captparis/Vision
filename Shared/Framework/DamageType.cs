using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared.Framework
{
	public struct DamageType
	{
		public Object source;
		public string name;

		public DamageType(Object source, string name)
		{
			this.source = source;
			this.name = name;
		}
	}
}