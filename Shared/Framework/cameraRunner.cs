using UnityEngine;
using System.Collections;

public class cameraRunner : MonoBehaviour {

	public int speed;

	public Transform player;
	
	// Update is called once per frame
	//void Update () {
	//	transform.position = new Vector3(player.position.x + 6, 0, -10);
	//}

	void Update() { 
		transform.Translate(Vector3.right * Time.deltaTime * speed);
	}
}
