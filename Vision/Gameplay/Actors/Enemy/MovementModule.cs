using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	public abstract class MovementModule : BaseModule
	{

		protected float maxTargetDist = 20;
		//protected Transform player;
		public override void ProcessUpdate()
		{
			Move();
		}
		public abstract void Move();
		public virtual void ModifySpeed(float newSpeedModifier) { }

		public virtual bool CollisionCheck(int direction)
		{
			//Vector3 target = enemy.aimMod.AcquireTarget();
			RaycastHit[] hits;

			Transform transform = enemy.transform;

			Vector3[] startPos = new Vector3[]{
				transform.position + new Vector3(0, 2f, 0),
				transform.position + new Vector3(0, 1, 0),
				//transform.position + new Vector3(0, 0.5f, 0)
			};

			for (int i = 0; i < startPos.Length; i++)
			{

				Vector3 endPos = startPos[i] + (direction * Vector3.right * 1.4f);

				hits = Physics.RaycastAll(startPos[i], (direction * Vector3.right), 1.4f);
				//Debug.DrawRay(startPos[i], new Vector3(direction, 0, 0), colours[i], 3);
				Debug.DrawLine(startPos[i], endPos, Color.yellow);
				int j = 0;
				foreach (var hit in hits)
				{
					j++;
					//Debug.Log("Ray(" + i + ":" + j + ")" + hit.collider.name);
					if (hit.collider != null && !hit.collider.isTrigger)
					{
						return true;
					}
				}

			}
			return false;
		}
	}
}
