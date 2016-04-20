using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class King : MonoBehaviour {

	private Vector3 target = Vector3.zero;
	private int speechPosition = 0;
	private bool inTrigger;
	private bool canAdvance;
	private float timer;
	public float timeOut = 5f;
	public GameObject speechObject;
	private Transform speechBubble;
	public Text speechText;
	public string[] speech = {"Hello", "Orcs Smell", "Goodbye"};
	public Transform door;
	public Transform exit;
	private bool doorOpen = false;
	private Animator kingAnim;
	public GameObject TalkBox;
	public GameObject ConvCanvas;
	private GameObject Hero;
	private HeroCharacterController HeroCC;
	public GameObject KingHead;
	public GameObject KingTitle;

	void Start()
	{
		kingAnim = transform.FindChild("NPCBody/King").GetComponent<Animator>();
		speechBubble = speechObject.transform;
		Hero = GameObject.FindGameObjectWithTag ("Player");
		HeroCC = Hero.GetComponent<HeroCharacterController>();
		//speechText = transform.FindChild("NPCBody/SpeechBubble/Canvas/Text").GetComponent<Text>();
		speechBubble.localScale = target;
	}
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "Hero")
		{
			inTrigger = true;
			canAdvance = true;
			ConvCanvas.SetActive(true);
			HeroCC.SpeechFreeze = true;
			if (timer >= timeOut)
				speechPosition = 0;
			if (speechPosition == 0)
			{
				if (!doorOpen)
					door.GetComponent<Door>().Close();
				speechText.text = speech[speechPosition];
			}
			if (speechPosition < speech.Length-1)
				target = Vector3.one;
		}
	}
	void OnTriggerExit (Collider col)
	{
		ConvCanvas.SetActive(false);
		if (col.gameObject.name == "Hero")
		{
			inTrigger = false;
			target = Vector3.zero;
			//KingHead.transform.localScale = Vector3.Lerp (speechBubble.localScale, Vector3.zero, Time.deltaTime * 10);
			//KingTitle.transform.localScale = Vector3.Lerp (speechBubble.localScale, Vector3.zero, Time.deltaTime * 10);
			timer = 0f;
		}
	}
	void Update ()
	{
		if (speechBubble.localScale != target) 
			speechBubble.localScale = Vector3.Lerp (speechBubble.localScale, target, Time.deltaTime * 10);
		if (!inTrigger && timer < timeOut)
		{
			timer += Time.deltaTime;
		}
		if (inTrigger && canAdvance)
		{
			if (Input.GetButton("Fire1"))
			{
				canAdvance = false;
				StartCoroutine(IncrementSpeech());
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
				if (!doorOpen)
				{
					doorOpen = true;
					door.GetComponent<Door>().Open();
					exit.gameObject.SetActive(true);
				}

			}
			canAdvance = true;
		}
	}

}
