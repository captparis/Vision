using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Gameplay
{
	public abstract class AttackModule : BaseModule
	{
		//[SerializeField]
		protected AimModule aimMod;
		//protected Transform player;
		[SerializeField]
		//[Tooltip("")]
		protected float attackTimer = 2;
		public bool attacking { get; protected set; }
		[SerializeField]
		public GameObject weapon;
		[SerializeField]
		public GameObject projectile;
		//[SerializeField]
		protected Transform weaponBone;

		protected Transform equip;
		//protected Renderer weaponRenderer;

		protected bool equipped;

		protected Animator anim;

		[SerializeField]
		protected float attackFrequency = 1.5f;
		[SerializeField]
		protected float attackDelayTime = 0.5f;
		protected float attackAnimTime = 1f;

		protected float maxTargetDist = 5;

		public override void Init()
		{
			base.Init();
			aimMod = enemy.aimMod;
			anim = GetComponent<Animator>();
			weaponBone = transform.Find("hero_root/hero_spine_1/hero_spine_2/hero_shoulder_R/hero_elbow_R/hero_wrist_R/hero_hand_1_R");
			if (!equipped && weapon != null)
			{
				equip = (Instantiate(weapon, weaponBone.position, weaponBone.rotation) as GameObject).transform;
				equip.parent = weaponBone;
				equip.localPosition = Vector3.zero;
				equipped = true;
				/*weaponRenderer = equip.GetComponent<Renderer>();
				if (weaponRenderer == null)
				{
					weaponRenderer = equip.GetComponentInChildren<Renderer>();
				}
				weaponRenderer.enabled = false;*/
				equip.gameObject.SetActive(false);
			}
			//player = GameObject.FindGameObjectWithTag("Player").transform;
		}

		public override void ProcessUpdate()
		{
			Vector3 target = aimMod.AcquireTarget();
			//Transform transform = enemy.transform;

			if (aimMod.TargetRange(target) <= maxTargetDist && CanAttack())
			{
				attackTimer += Time.deltaTime;
				if (attackTimer >= attackFrequency)
				{
					//weaponRenderer.enabled = true;
					if(equip)
						equip.gameObject.SetActive(true);
					anim.SetTrigger("Attack");
					Invoke("Attack", attackDelayTime);
					Invoke("AttackEnd", attackAnimTime);
					//StartCoroutine(Attack(target));
					attackTimer = 0;
					//attacking = false;
				}
			}
		}
		public virtual bool CanAttack() { return true; }
		public virtual void Attack()
		{
		}
		public virtual void AttackEnd()
		{
			//weaponRenderer.enabled = false;
			if (equip)
				equip.gameObject.SetActive(false);
		}
	}
}
