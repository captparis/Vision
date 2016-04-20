using UnityEngine;
using System.Collections;
using Shared.Framework;

namespace Vision.Gameplay
{
	[RequireComponent(typeof(AimModule))]
	[RequireComponent(typeof(AttackModule))]
	[RequireComponent(typeof(MovementModule))]
	[RequireComponent(typeof(SpecialModule))]
	public class Enemy : Destructible
	{
		public const float FACE_LEFT = 270;
		public const float FACE_RIGHT = 90;
		//protected Transform player;
		[SerializeField]
		private int health = 10;
		private int maxHealth;
		public AttackModule attackMod;// { get; private set; }
		public MovementModule moveMod;// { get; private set; }
		public AimModule aimMod;// { get; private set; }
		public SpecialModule specialMod;
		public CharacterController controller;
		private Animator anim;
		bool rampingDown;
		const float rampSeconds = 3;
		float rampValue = 0;
		Renderer[] renderers;
		public GameObject deathEffect;

		FocusMode FM;

		public virtual void Awake()
		{
			anim = GetComponent<Animator>();
			attackMod = GetComponent<AttackModule>();
			moveMod = GetComponent<MovementModule>();
			aimMod = GetComponent<AimModule>();
			specialMod = GetComponent<SpecialModule>();

			renderers = GetComponentsInChildren<Renderer>();
			maxHealth = (health > 0) ? health : 1;

			aimMod.Init();
			attackMod.Init();
			moveMod.Init();
			specialMod.Init();

			//HCC = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCharacterController>();
			FM = Camera.main.GetComponent<FocusMode>();
		}
		public void Start()
		{
			//attackMod.Start();
			//moveMod.Start();
			//aimMod.Start();
			controller = GetComponent<CharacterController>();
		}
		public void Update()
		{
			if (rampingDown)
			{
				rampValue = Mathf.Clamp(rampValue - (Time.deltaTime / rampSeconds), 0, 1);// Mathf.Lerp
				for(int i = 0; i < renderers.Length; i++) {
					print("Ramping down" + rampValue);
					DynamicGI.SetEmissive(renderers[i], Color.red * rampValue);
				}
				if (rampValue == 0)
				{
					rampingDown = false;
				}
			}
			aimMod.ProcessUpdate();
			if (!aimMod.turning)
				attackMod.ProcessUpdate();
			moveMod.ProcessUpdate();
		}
		public override void TakeDamage(int damage, DamageType damageType)
		{
			if (!enabled)
				return;
			health = Mathf.Clamp(health - damage, 0, maxHealth);
			if (damage > 0)
			{
				rampingDown = true;
				rampValue = 1;
			}
			if (health <= 0)
			{
				GameManager.GM.enemyKills++;
				Transform deathBone = transform.Find("hero_root/hero_spine_1/hero_spine_2/hero_head");
				if (deathEffect != null)
				{
					Transform deathT = (Instantiate(deathEffect, deathBone.position, deathBone.rotation) as GameObject).transform;
					deathT.parent = deathBone;
					deathT.localPosition = Vector3.zero;
				}
				attackMod.Death(damageType);
				moveMod.Death(damageType);
				aimMod.Death(damageType);
				specialMod.Death(damageType);
				
				attackMod.enabled = false;
				moveMod.enabled = false;
				aimMod.enabled = false;
				specialMod.enabled = false;

				attackMod = null;
				moveMod = null;
				aimMod = null;
				specialMod = null;
				anim.SetTrigger("Death");
				CharacterController enemyController = GetComponent<CharacterController>();
				Rigidbody enemyRigid = GetComponent<Rigidbody>();
				Destroy(enemyController);
				Destroy(enemyRigid);
				enabled = false;

				//Destroy(gameObject);
			} 
		}
		public void ModifyMaxHealth(float multiplier)
		{
			if (multiplier > 0)
			{
				maxHealth = Mathf.RoundToInt(maxHealth * multiplier);
				health = maxHealth;
			}
		}
		public int GetFacingDirection()
		{
			float rot = transform.rotation.eulerAngles.y % 360;
			if (rot > 180)
			{
				return -1;
			}
			else
			{
				return 1;
			}
		}
		void OnGUI()
		{
			Vector3 pos = transform.position;
			pos.y += 5;
			Vector3 ScreenPos = Camera.main.WorldToScreenPoint(pos);
			if (FM.focusActive)
			{
				GUI.Label(new Rect(ScreenPos.x, Camera.main.pixelHeight - ScreenPos.y, 50, 50), attackMod.icon);
				GUI.Label(new Rect(ScreenPos.x + 50, Camera.main.pixelHeight - ScreenPos.y, 50, 50), aimMod.icon);
				GUI.Label(new Rect(ScreenPos.x, (Camera.main.pixelHeight - ScreenPos.y) + 50, 50, 50), moveMod.icon);
				GUI.Label(new Rect(ScreenPos.x + 50, (Camera.main.pixelHeight - ScreenPos.y) + 50, 50, 50), specialMod.icon);
			}
		}
	}
}
