using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KnightThroneCorridor : MonoBehaviour {
	
	private Vector3 target = Vector3.zero;
	private int speechPosition = 0;
	private bool inTrigger;
	private bool canAdvance;
	private float timer;
	public float timeOut = 5f;
	private Transform speechBubble;
	private Text speechText;
	public string[] speech = {"Hello", "Orcs Smell", "Goodbye"};
	public Transform door;
	private bool doorOpen = false;
	
	void Start()
	{
		speechBubble = transform.FindChild("NPCBody/SpeechBubble");
		speechText = transform.FindChild("NPCBody/SpeechBubble/Canvas/Text").GetComponent<Text>();
		speechBubble.localScale = target;
	}
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "Hero")
		{
			inTrigger = true;
			canAdvance = true;
			if (timer >= timeOut)
				speechPosition = 0;
			if (speechPosition == 0)
			{
				speechText.text = speech[speechPosition];
			}
			if (speechPosition < speech.Length-1)
				target = Vector3.one;
		}
	}
	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.name == "Hero")
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
			if (speechPosition == speech.Length-1)
			{
				if (!doorOpen)
				{
					doorOpen = true;
					door.GetComponent<Door>().Open();
				}
			}
			canAdvance = true;
		}
	}
}
