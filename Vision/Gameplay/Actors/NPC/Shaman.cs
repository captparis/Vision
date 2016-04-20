using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shaman : MonoBehaviour {

	private Vector3 target = Vector3.zero;
	private int actionPosition = 1;
	private int speechPosition = 1;
	private bool inTrigger;
	private float timer;
	public float timeOut = 1;
	public Transform speechBubble;
	public Text speechText;
	private ParticleSystem spawnEffect;
	private GameObject forceField;
	public string[] speech = {"Hello", "Orcs Smell", "Goodbye"};
	public Transform lift;
	private Transform npcBody;
	private bool actionReady = true;
	public GameObject TalkBox;
	public GameObject ConvCanvas;
	private GameObject hero;
	private HeroCharacterController HeroCC;

	void Start()
	{
		npcBody = transform.FindChild("NPCBody");
		spawnEffect = transform.FindChild("Poof").GetComponent<ParticleSystem>();
		forceField = transform.FindChild("ForceField").gameObject;
		speechBubble.localScale = target;
		hero = GameObject.FindWithTag("Player");
		HeroCC = hero.GetComponent<HeroCharacterController>();
	}
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "Hero")
		{
			inTrigger = true;
			//HeroCC.SpeechFreeze = true;
			if (actionPosition == 1 && actionReady)
			{
				StartCoroutine(SpawnShaman(true));
			}
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.name == "Hero")
		{
			ConvCanvas.SetActive(false);
			inTrigger = false;
		}
	}

	void Update ()
	{
		if (speechBubble.localScale != target) 
			speechBubble.localScale = Vector3.Lerp (speechBubble.localScale, target, Time.deltaTime * 10);
		if (actionPosition == 2 && actionReady)
		{
			if (Input.GetButton("Fire1"))
			{
				StartCoroutine(IncrementSpeech());
			}
		}
	}
	
	private IEnumerator SpawnShaman (bool spawn)
	{
		actionReady = false;
		yield return new WaitForSeconds(1);
		spawnEffect.Emit(50);
		npcBody.gameObject.SetActive(spawn);
		if (spawn)
		{
			HeroCC.SpeechFreeze = true;
		}
		else
		{
			HeroCC.SpeechFreeze = false;
		}
		if (spawn)
			speechText.text = speech[speechPosition];
			yield return new WaitForSeconds(1);
		    ConvCanvas.SetActive(true);
			target = Vector3.one;
		actionPosition++;
		actionReady = true;
	}
	private IEnumerator CallLift ()
	{
		actionReady = false;
		yield return new WaitForSeconds(3);
		lift.GetComponent<Lift>().LiftUp();
		actionReady = true;
	}

	private IEnumerator IncrementSpeech()
	{
		actionReady = false;
		target = Vector3.zero;
		yield return new WaitForSeconds(1);
		if (speechPosition < speech.Length-1)
		{
			speechPosition++;
			speechText.text = speech[speechPosition];
			target = Vector3.one;
		}
		if (speechPosition == speech.Length-1)
		{
			//HeroCC.SpeechFreeze = false;
			StartCoroutine(SpawnShaman(false));
			StartCoroutine(CallLift());
			Destroy(forceField);
		}
		else
		{
			actionReady = true;
		}
	}
}
