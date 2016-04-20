using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	[System.Serializable]
	public abstract class AimModule : BaseModule
	{
		[SerializeField]
		protected float turningAngleSpeed = 270;
		protected float wantedFacingAngle;
		public bool turning { get; protected set; }
		protected float maxTargetDist = 8;


		public abstract Vector3 AcquireTarget();
		public abstract float TargetRange(Vector3 target);
		protected bool IsFacingTarget(Vector3 target)
		{
			if (Math.Sign(target.x - enemy.transform.position.x) != Math.Sign(enemy.GetFacingDirection()))
				return true;
			return false;
		}
		public override void Init()
		{
			base.Init();
			wantedFacingAngle = enemy.transform.rotation.eulerAngles.y;
		}
		public override void ProcessUpdate()
		{
			Vector3 euler = transform.rotation.eulerAngles;
			
			if (turningAngleSpeed > 0 && !Mathf.Approximately(euler.y, wantedFacingAngle))
			{
				if (euler.y < wantedFacingAngle)
				{
					euler.y = Mathf.Clamp(euler.y + turningAngleSpeed * Time.deltaTime, euler.y, wantedFacingAngle);
					Quaternion quat = Quaternion.Euler(euler);
					transform.rotation = quat;
				}
				else if (euler.y > wantedFacingAngle)
				{
					euler.y = Mathf.Clamp(euler.y - turningAngleSpeed * Time.deltaTime, wantedFacingAngle, euler.y);
					Quaternion quat = Quaternion.Euler(euler);
					transform.rotation = quat;
				}
				turning = true;
			}
			else
			{
				turning = false;
			}

		}
		protected virtual void TurnTowardsTarget(Vector3 target)
		{
			float direction = transform.position.x - target.x;
			if (Mathf.Abs(direction) > 1) // Only turn if the player is too close i.e. above or below us
			{
				wantedFacingAngle = (direction > 0 ? Enemy.FACE_LEFT : Enemy.FACE_RIGHT);
			}
		}
	}
}
