using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour 
{

	public void LoadScene(string level){
		LevelManager.LM.LoadNextLevel();
	}

	public void LoadCredits () {
		Application.LoadLevel ("StartMenu-Credits");
	}
}
