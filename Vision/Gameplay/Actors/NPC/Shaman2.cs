using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shaman2 : MonoBehaviour {

	private Vector3 target = Vector3.zero;
	private int speechPosition = 0;
	private bool inTrigger;
	private bool canAdvance;
	private float timer;
	public float timeOut = 5f;
	private Transform speechBubble;
	public Text speechText;
	public string[] speech = {"Hello", "Orcs Smell", "Goodbye"};
	private Animator kingAnim;
	private GameObject hero;
	private HeroCharacterController HeroCC;
	public Transform poof;
	public GameObject speechObject;
	public GameObject TalkBox;
	public GameObject ConvCanvas;

	void Start()
	{
		kingAnim = transform.FindChild("NPCBody/Shaman").GetComponent<Animator>();
		speechBubble = speechObject.transform;
		speechBubble.localScale = target;
		hero = GameObject.FindWithTag("Player");
		HeroCC = hero.GetComponent<HeroCharacterController>();
	}
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject == hero)
		{
			inTrigger = true;
			canAdvance = true;
			ConvCanvas.SetActive(true);
			HeroCC.SpeechFreeze = true;
			if (timer >= timeOut)
				speechPosition = 0;
			if (speechPosition == 0)
				speechText.text = speech[speechPosition];
			if (speechPosition < speech.Length-1)
				target = Vector3.one;
		}
	}
	void OnTriggerExit (Collider col)
	{
		ConvCanvas.SetActive(false);
		if (col.gameObject == hero)
		{
			inTrigger = false;
			target = Vector3.zero;
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
				speechText.text = speech[speechPosition];
				target = Vector3.one;
			}
			else
			{
				Instantiate(poof,hero.transform.position,Quaternion.identity);
				Destroy(hero.GetComponent<Rigidbody>());
				Destroy(hero.GetComponent<HeroCharacterController>());
				Destroy(hero.transform.GetChild(0).gameObject);
				yield return new WaitForSeconds(3);
				LevelManager.LM.LoadNextLevel();
			}
			canAdvance = true;
		}
	}
}
