using UnityEngine;
using System.Collections;

public class LoadMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LevelManager.LM.LoadLevel(LevelManager.LEVEL_MAINMENU);
	}

}
