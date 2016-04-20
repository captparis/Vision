using UnityEngine;
using System.Collections;

public class GameManager : object {

	private static GameManager _GM;

	public static GameManager GM
	{
		get
		{
			if (_GM == null)
			{
				_GM = new GameManager();
			}
			return _GM;
		}
	}

	public int score = 0;
	public int youMonster = 0;
	public float focus = 0;
	public int enemyKills = 0;
	public int innocentsSpared = 0;
	public enum Difficulty
	{
		Easy, Medium, Hard
	}
	public Difficulty difficulty = Difficulty.Medium;
#if UNITY_EDITOR
	public bool focusAvailable = true;
#else
	public bool focusAvailable = false;
#endif

	public void Reset()
	{
		score = 0;
		youMonster = 0;
		focusAvailable = false;
		focus = 0;
		enemyKills = 0;
		innocentsSpared = 0;
	}

}
