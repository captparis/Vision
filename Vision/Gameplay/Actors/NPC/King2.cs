using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class King2 : MonoBehaviour {
	
	private Vector3 target = Vector3.zero;
	private int speechPosition = 0;
	private bool inTrigger;
	private bool canAdvance;
	public int crystalSwapPoint = 5;
	private Transform speechBubble;
	public GameObject speechObject;
	public Text speechText;
	public string[] speech = {"Hello", "Orcs Smell", "Goodbye"};
	public Transform door;
	private bool kingHasCrystal = false;
	private Animator kingAnim;
	public Transform crystal;
	private Transform crystalInstance;
	private Transform hero;
	private Transform body;
	private Vector3 crystalInitPos;
	private Vector3 throwMidpoint;
	private Vector3 throwPos;
	private Transform sphere;
	public Transform backdrop;
	private bool endScene;
	private Camera mainCamera;
	public GameObject TalkBox;
	public GameObject ConvCanvas;
	private GameObject Hero;
	private HeroCharacterController HeroCC;
	
	void Start()
	{
		hero = GameObject.FindWithTag("Player").transform;
		speechBubble = speechObject.transform;
		body = transform.FindChild("NPCBody");
		kingAnim = transform.FindChild("NPCBody/King").GetComponent<Animator>();
		sphere = transform.FindChild("NPCBody/Sphere");
		sphere.gameObject.SetActive(false);
		speechBubble.localScale = target;
		mainCamera = Camera.main;
		Hero = GameObject.FindGameObjectWithTag ("Player");
		HeroCC = Hero.GetComponent<HeroCharacterController>();
	}
	void OnTriggerEnter (Collider col)
	{
		ConvCanvas.SetActive(false);
		if (col.transform == hero)
		{
			ConvCanvas.SetActive(true);
			HeroCC.SpeechFreeze = true;
			inTrigger = true;
			canAdvance = true;
			if (speechPosition == 0)
			{
				door.GetComponent<Door>().Close();
				speechText.text = speech[speechPosition];
			}
			if (speechPosition < speech.Length-1)
				target = Vector3.one;
		}
	}
	void OnTriggerExit (Collider col)
	{
		if (col.transform == hero)
		{
			inTrigger = false;
			target = Vector3.zero;
		}
	}
	void Update ()
	{
		if (speechBubble.localScale != target) 
			speechBubble.localScale = Vector3.Lerp (speechBubble.localScale, target, Time.deltaTime * 10);
		if (inTrigger && canAdvance)
		{
			if (Input.GetButton("Fire1"))
			{
				canAdvance = false;
				StartCoroutine(IncrementSpeech());
			}
		}
		if (crystalInstance)
		{
			if (Vector3.Distance (body.position,crystalInstance.position) > 1)
			{
				throwPos = body.position;
				if (Vector3.Distance (body.position,crystalInstance.position) > 3)
					throwPos.y += 6-Mathf.Abs(throwMidpoint.x-crystalInstance.position.x);
				crystalInstance.position = Vector3.Lerp(crystalInstance.position,throwPos,Time.deltaTime);
			}
			else if (!kingHasCrystal)
			{
				StartCoroutine(RecieveCrystal());
			}
		}
		if (sphere)
		{
			if (sphere.gameObject.activeSelf)
			{
				if (sphere.localScale.y < 80)
				{
					sphere.localScale += Vector3.one*Time.deltaTime*25;
				}
				else
				{
					Destroy(sphere.gameObject);
				}
			}
		}
	}
	
	private IEnumerator IncrementSpeech()
	{
		target = Vector3.zero;
		yield return new WaitForSeconds(1);
		if (inTrigger)
		{
			if (speechPosition < speech.Length-1)
			{
				speechPosition++;
				kingAnim.SetTrigger ("Talk");
				speechText.text = speech[speechPosition];
				target = Vector3.one;
			}
			if (speechPosition == speech.Length-1)
			{
				HeroCC.SpeechFreeze = false;
				//LevelManager.LM.LoadNextLevel();
				target = Vector3.zero;
				mainCamera.transform.GetComponent<HUDScript>().FadeBlack(true);
				yield return new WaitForSeconds(2);
				LevelManager.LM.LoadNextLevel();
			}
			if (speechPosition == crystalSwapPoint)
			{
				crystalInstance = Instantiate(crystal,hero.position,Quaternion.identity) as Transform;
				crystalInitPos = crystalInstance.position;
				throwMidpoint = Vector3.Lerp (body.position,crystalInstance.position,0.6f);
			}
			else
			{
				canAdvance = true;
			}
		}
	}
	
	private IEnumerator RecieveCrystal()
	{
		kingHasCrystal = true;
		body.GetComponent<ParticleSystem>().Play();
		sphere.gameObject.SetActive(true);
		sphere.position = crystalInstance.position;
		Destroy (crystalInstance.gameObject);
		yield return new WaitForSeconds(2);
		canAdvance = true;
	}
}
