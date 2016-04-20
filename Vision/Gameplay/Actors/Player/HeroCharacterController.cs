using UnityEngine;
using System.Collections;
using Shared.Framework;

public class HeroCharacterController : Destructible
{

	public int health = 100;
	int maxHealth;


	public float MaxJumpSpeed = 6f;
	public float MinJumpSpeed = 2f;
	public float AttackDamageDelay = 0.5f;
	float JumpTime = 0f;
	float JumpSpeed = 0f;
	float JumpIncrement = 0f;
	public float DashMultiplier = 2.0f;
	bool CanJump = false;
	bool isShield = false;
	bool CanAttack = true;
	public bool SpeechFreeze = false;

	private AudioSource heroAudio;

	public float MoveSpeed = 10f;

	public Vector3 MoveVector;
	public float VerticalVelocity;

	public CharacterController controller;
	public float animScale;
	public float posX;
	public float direction;
	private bool blockInput;
	//private bool dashInput;
	public Transform childMesh;

	private HUDScript HUDScript; 
	private Camera MainCam;
	private Transform Swoosh;

	//Movement
	Vector3 acceleration;
	public float normalGravity = 21f; //gravity during normal movement
	public float shieldGravity = 100f; //gravity increase when shielding
	public float gravity = 21f;	//downward force
	public float terminalVelocity = 20f;	//max downward speed
	float friction;
	Vector3 velocity;

	//Sounds
	public AudioClip[] audioClip;



	Animator anim;

	// Use this for initialization
	void Awake()
	{
		heroAudio = GetComponent<AudioSource>();
		MainCam = Camera.main;
		HUDScript = MainCam.GetComponent<HUDScript>();
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
		Swoosh = transform.FindChild("HeroActive/hero_root/hero_spine_1/hero_spine_2/hero_shoulder_R/hero_elbow_R/hero_wrist_R/hero_item_R/Swoosh");
		Swoosh.gameObject.SetActive(false);
		direction = (transform.rotation.eulerAngles.y == 0) ? 1 : -1;
		JumpIncrement = (MaxJumpSpeed - MinJumpSpeed) / 3;
		//CharacterController = gameObject.GetComponent(“CharacterController”) as CharacterController;
		maxHealth = (health >= 0) ? health : 1;
	}

	// Update is called once per frame
	void Update()
	{

		//Zero off z-axis
		if (transform.localPosition.z != 0)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
		}

		blockInput = Input.GetButton("Block");
		//dashInput = Input.GetButton("Dash");

		//Set animations
		float moveAxis = Input.GetAxis("Horizontal");
		if (blockInput)// && !dashInput)
		{
			moveAxis = 0;
		}
		/*if (dashInput)
		{
			moveAxis = direction;
		}*/
		if (!SpeechFreeze) {
			anim.SetFloat("Speed", Mathf.Abs ( moveAxis));
		}
		else {
			anim.SetFloat ("Speed", 0);
		}

		//Jumping animations !!!!MAKE THIS MORE EFFICIENT!!!!
		if (VerticalVelocity > 0 && !controller.isGrounded)
		{
			anim.SetFloat("Jump", VerticalVelocity);
		}

		else if (!controller.isGrounded)
		{
			anim.SetBool("Falling", true);
		}

		else
		{
			anim.SetBool("Falling", false);
			anim.SetFloat("Jump", 0);
		}

		// Change facing direction
		if (moveAxis > 0 && direction == -1)
		{
			//transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
			transform.FindChild(childMesh.name).Rotate(Vector3.up, 180, Space.World);
			direction = 1;
		}
		else
		{
			if (moveAxis < 0 && direction == 1)
			{
				//transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
				transform.FindChild(childMesh.name).Rotate(Vector3.up, -180, Space.World);
				direction = -1;
			}
		}

		checkMovement();
		if(blockInput && !isShield && !SpeechFreeze)
		{
			
			anim.SetBool ("Shield", true);
			isShield = true;
		}
		
		//Attacking animation
		if (Input.GetButtonDown("Fire1") && !SpeechFreeze)
		{
			if (CanAttack && !isShield)
			{
				anim.SetTrigger("Attack");
				PlaySound(0);
				Invoke("Attack", AttackDamageDelay);
				StartCoroutine(AttackSwoosh());
			}
		}

		if (blockInput && !SpeechFreeze)
		{
			MoveVector = Vector3.zero;
			
			gravity = shieldGravity;
		}


		else 
		{
			anim.SetBool ("Shield", false);
			isShield = false;
			gravity = normalGravity;
		}

		if (controller.isGrounded && !Input.GetButton("Jump") && !SpeechFreeze)
		{
			CanJump = true;
		}
		if (CanJump)
		{
			if (Input.GetButton("Jump") && !blockInput && !SpeechFreeze)
			{
				JumpTime += Time.deltaTime;
				if (JumpTime >= 0.05)
				{
					if (JumpSpeed == 0)
						JumpSpeed = MinJumpSpeed;
					else if (JumpSpeed == MinJumpSpeed || JumpSpeed == (MinJumpSpeed + JumpIncrement))
						JumpSpeed += JumpIncrement;
					else
						JumpSpeed = MaxJumpSpeed;
					VerticalVelocity = JumpSpeed;
					JumpTime -= 0.05f;
				}
				if (JumpSpeed >= MaxJumpSpeed)
				{
					CanJump = false;
					JumpSpeed = 0f;
					JumpTime = 0f;
				}
			}
			else if (!controller.isGrounded)
			{
				CanJump = false;
			}
		}

		/*if (controller.isGrounded && !Input.GetButton ("Jump"))
		{
			CanJump = true;
		}
		if (CanJump)
		{
			if (Input.GetButton ("Jump"))
			{
				//JumpSpeed += JumpSpeed + Time.deltaTime * 10;
				if (JumpSpeed < MinJumpSpeed)
				{
					JumpSpeed = MinJumpSpeed;
				}
				if (JumpSpeed > MaxJumpSpeed)
				{
					VerticalVelocity = MaxJumpSpeed;
					CanJump = false;
					JumpSpeed = 0f;
				}
				else
				{
					VerticalVelocity = JumpSpeed;
					JumpSpeed += Time.deltaTime * 240;
				}
			}
			else if (!controller.isGrounded)
			{
				CanJump = false;
			}
		}*/

		//HandleActionInput();
		processMovement();

		//if (moveDirection.magnitude > 0.05f) {
		//	transform.LookAt(transform.position + moveDirection);
		//}

	}

	/*
	 * SafeMove() by Garth Smith @ http://answers.unity3d.com/questions/335720/how-to-make-my-character-stop-stuttering-when-walk.html
	 * Walk down slopes safely. Prevents Player from "hopping" down hills.
	 * Apply gravity before running this. Should only be used if Player
	 * was touching ground on the previous frame.
	 */
	void SafeMove(Vector2 velocity)
	{
		// X and Z first. We don't want the sloped ground to prevent
		// Player from falling enough to touch the ground.
		Vector3 displacement;
		displacement.x = velocity.x * Time.deltaTime;
		displacement.y = 0;
		displacement.z = -controller.transform.position.z;
		controller.Move(displacement);
		// Now Y
		displacement.y = velocity.y * Time.deltaTime;
		// Our steepest down slope is 45 degrees. Force Player to fall at least
		// that much so he stays in contact with the ground.
		if (-Mathf.Abs(displacement.x) < displacement.y && displacement.y < 0)
		{
			displacement.y = -Mathf.Abs(displacement.x) - 0.001f;
		}
		displacement.z = 0;
		displacement.x = 0;
		controller.Move(displacement);
	}

	private IEnumerator AttackSwoosh ()
	{
		CanAttack = false;
		Swoosh.gameObject.SetActive(true);
		yield return new WaitForSeconds(AttackDamageDelay);
		CanAttack = true;
		Swoosh.gameObject.SetActive(false);
	}

	private void Attack()
	{
		RaycastHit[] hits;

		Vector3[] startPos = new Vector3[3]{
			childMesh.position + new Vector3(0, 1.5f, 0),
			childMesh.position + new Vector3(0, 1f, 0),
			childMesh.position + new Vector3(0, 0.5f, 0)
		};
		Color[] colours = new Color[3]{
			Color.red,
			Color.green,
			Color.blue
		};
		for (int i = 0; i < 3; i++)
		{
			Vector3 endPos = startPos[i] + new Vector3(direction * 3, 0, 0);

			hits = Physics.RaycastAll(startPos[i], new Vector3(direction, 0, 0), 3);
			//Debug.DrawRay(startPos[i], new Vector3(direction, 0, 0), colours[i], 3);
			Debug.DrawLine(startPos[i], endPos, colours[i], 3);
			foreach (var hit in hits)
			{
				if (hit.collider != null && hit.collider.GetComponent<Destructible>())
				{
					PlaySound(1);
					DamageType dmgtype = new DamageType(gameObject, "Player");
					hit.collider.GetComponent<Destructible>().TakeDamage(10, dmgtype);
					return;
				}
			}
			
		}
	}
	public override void TakeDamage(int damage, DamageType damageType)
	{
		if (blockInput)
		{
			//print("Blocked");
			PlaySound (2);
			return;
		}
		health = Mathf.Clamp(health - damage, 0, maxHealth);
		HUDScript.updateArmour(health);
		if (health <= 0)
		{
			PlayerDied();
		}
	}

	void PlaySound(int clip)
	{
		if (clip >= audioClip.Length || audioClip[clip] == null)
		{
			Debug.LogError("Tried to play non-existant sound");
			return;
		}
		heroAudio.PlayOneShot(audioClip[clip]);
	}

	public void PlayerDied()
	{
		print("Player Died");
		Rigidbody heroRigid = GetComponent<Rigidbody>();
		anim.SetTrigger ("Death");
		Destroy (heroRigid);
		LevelManager.LM.SetState(LevelManager.State.STATE_LOSE);
	}
	public void KillPlayer(string DamageType = "world")
	{
		health = 0;
		PlayerDied();
	}
	void checkMovement()
	{
		//move l/r
		var deadZone = 0.1f;
		VerticalVelocity = MoveVector.y;
		MoveVector = Vector3.zero;
		if (!SpeechFreeze){
		if (Input.GetAxis("Horizontal") > deadZone || Input.GetAxis("Horizontal") < -deadZone)
		{
			MoveVector += new Vector3(Input.GetAxis("Horizontal"), 0, 0);
		}
		}
		/*if (dashInput)
		{
			MoveVector = new Vector3(direction, 0, 0);
		}*/
		//jump
	}

	void processMovement()
	{

		//print(MoveVector);
		//transform moveVector into world-space relative to character rotation
		//MoveVector = transform.TransformDirection(MoveVector);
		//print(MoveVector);

		//normalize moveVector if magnitude > 1
		MoveVector = Vector3.Normalize(MoveVector);

		//multiply moveVector by moveSpeed
		MoveVector *= MoveSpeed;
		/*if (dashInput)
		{
			anim.SetBool ("Dash", true);
			MoveVector *= DashMultiplier;
		}

		if (!dashInput)
		{
			anim.SetBool ("Dash", false);
		}*/
		//reapply vertical velocity to moveVector.y
		MoveVector = new Vector3(MoveVector.x, VerticalVelocity, MoveVector.z);

		//apply gravity
		applyGravity();

		//move character in world-space
		if(controller.isGrounded)
			SafeMove(MoveVector);
		else
		{
			controller.Move(MoveVector * Time.deltaTime);
		}
	  
	}

	void applyGravity()
	{
		if (MoveVector.y > -terminalVelocity)
		{
			MoveVector = new Vector3(MoveVector.x, (MoveVector.y - gravity * Time.deltaTime), MoveVector.z);
		}
		if (controller.isGrounded && MoveVector.y < -1)
		{
			MoveVector = new Vector3(MoveVector.x, (-1), MoveVector.z);
		}
	}
}
