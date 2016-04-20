using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Guard : MonoBehaviour {

	private Vector3 target = Vector3.zero;
	private Transform speechBubble;
	public Transform door;
	
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
			door.GetComponent<Door>().Open();
		}
	}
	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.name == "Hero")
		{
			target = Vector3.zero;
			Destroy(transform.GetComponent<BoxCollider>());
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
