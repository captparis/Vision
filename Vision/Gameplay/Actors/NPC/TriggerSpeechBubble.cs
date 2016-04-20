using UnityEngine;
using System.Collections;

public class TriggerSpeechBubble : MonoBehaviour {

	private Vector3 target = Vector3.zero;
	private Transform speechBubble;

	void Start()
	{
		speechBubble = transform.FindChild("NPCBody/SpeechBubble");
		speechBubble.localScale = target;
	}
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "Hero")
		{
			target = Vector3.one;
		}
	}
	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.name == "Hero")
		{
			target = Vector3.zero;
		}
	}
	void Update ()
	{
		if (speechBubble.localScale != target) 
		{
			speechBubble.localScale = Vector3.Lerp (speechBubble.localScale, target, Time.deltaTime * 10);
		}
	}
}
