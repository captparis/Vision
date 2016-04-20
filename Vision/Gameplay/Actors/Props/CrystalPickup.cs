using UnityEngine;
using System.Collections;

public class CrystalPickup : MonoBehaviour 
{
	private Transform crystalObject;
	private float bobDirection = 1f;
	public float bobSpeed = 0.01f;
	public GameObject crystalAudioObject;
	private AudioSource crystalAudio;
	public AudioClip crystalGet;
	
	// Use this for initialization
	void Start () 
	{
		crystalAudio = crystalAudioObject.GetComponent<AudioSource>();
		crystalObject = this.transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Mathf.Abs(crystalObject.transform.localPosition.y) >= 0.5)
		{
			bobDirection *= -1f;
		}
		crystalObject.transform.localPosition = Vector3.Lerp(crystalObject.transform.localPosition,Vector3.up*bobDirection,Time.deltaTime*bobSpeed);
		crystalObject.Rotate(Vector3.up*Time.deltaTime*10, Space.Self);
	}
	
	void OnTriggerEnter(Collider col) 
	{
		if (col.gameObject.name == "Hero")
		{
			crystalAudio.PlayOneShot(crystalGet);
			GameManager.GM.focusAvailable = true;
			Destroy(this.gameObject);
		}
	}
}
