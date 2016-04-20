using UnityEngine;
using System.Collections;

public class SetColor : MonoBehaviour {

	public Material head;
	public Material body;
	public Material accessories;
	public Color32 skin = new Color(0F, 0, 1F, 1);

	// Use this for initialization
	void Start () {
		//rend = GetComponent<SkinnedMeshRenderer>();
		head.color = skin;
		body.color = skin;
		accessories.color = skin;
	}

}
