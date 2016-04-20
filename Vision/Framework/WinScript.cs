using UnityEngine;
using System.Collections;
using Vision.Gameplay;

public class WinScript : MonoBehaviour 
{
	//public string nextScene;
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			LevelManager.LM.LoadNextLevel();
			AttackHealer[] healers = FindObjectsOfType(typeof(AttackHealer)) as AttackHealer[];
			foreach (AttackHealer healer in healers)
			{
				if (healer.enabled)
				{
					GameManager.GM.innocentsSpared++;
				}
			}
			//LevelManager.LM.SetState(LevelManager.State.STATE_WIN);
			//return;
		}
	}
}
