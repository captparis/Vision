using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	public abstract class BaseModule : MonoBehaviour
	{
		protected Enemy enemy;
		public Texture icon {get; private set;}
		protected string iconPath = "";
		
		public virtual void LateInit() { }
		public virtual void Init()
		{
			enemy = GetComponent<Enemy>();
			if(iconPath != "")
				icon = Resources.Load(iconPath) as Texture;

		}
		//public virtual void Init() {}
		public virtual void ProcessUpdate() { }
		public virtual void Death(Shared.Framework.DamageType damageType) { }
	}
}
