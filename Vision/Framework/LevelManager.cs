using UnityEngine;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour
{
	private Font GUIFont;
	private static LevelManager _LM;

	public static LevelManager LM
	{
		get
		{
			if (_LM == null)
			{
				GameObject obj = new GameObject("LevelManager");
				_LM = obj.AddComponent<LevelManager>();
			}
			return _LM;
		}
	}

	public const int LEVEL_LOCKED = 1;
	public const int LEVEL_COMPLETED = 2;
	public const string LEVEL_MAINMENU = "StartMenu";
	public enum State
	{
		STATE_MENU,
		STATE_PLAYING,
		STATE_WIN,
		STATE_LOSE
	}
	State currentState = State.STATE_MENU;

	public class LevelData
	{
		public string name;
		public bool locked;
		public bool completed;

		public LevelData(string name, bool locked, bool completed)
		{
			this.name = name;
			this.locked = locked;
			this.completed = completed;
		}
		public LevelData(string name)
		{
			this.name = name;
			this.locked = false;
			this.completed = false;
		}
	};
	public LevelData[] LevelDataTable =
	{
		new LevelData("ThroneCorridor"),
		new LevelData("ThroneRoom"),
		new LevelData("ThroneCorridor - Exit"),
		new LevelData("EmeraldFields"),
		new LevelData("MysteriousCave"),
		new LevelData("AmberWoods"),
		new LevelData("WarTent"),
		new LevelData("EmeraldWarzone"),
		new LevelData("ThroneCorridor - EndGame"),
		new LevelData("ThroneRoom - EndGame")
	};

	int CurrentLevel = -1;

	// Use this for initialization
	void Awake()
	{
		GUIFont = (Font)Resources.Load ("EightyPercent");
		LoadLevelInfo();
		DontDestroyOnLoad(gameObject);
		CurrentLevel = GetLevelIndex(Application.loadedLevelName);
		if (CurrentLevel == -1)
		{
			SetState(State.STATE_MENU);
		}
		else
		{
			SetState(State.STATE_PLAYING);
		}
	}

	public void ResetLevelInfo()
	{
		for (int i = 0; i < LevelDataTable.Length; i++)
		{
			LevelDataTable[i].completed = false;
			LevelDataTable[i].locked = true;
		}
		LevelDataTable[0].locked = false;
		SaveLevelInfo();
	}
	public void SetLevelStatusLock(string name, bool locked)
	{
		for (int i = 0; i < LevelDataTable.Length; i++)
		{
			if (name == LevelDataTable[i].name)
			{
				LevelDataTable[i].locked = locked;
				SaveLevelInfo();
				return;
			}
		}
		print("Level " + name + " couldn't be found!");
	}
	public void CompleteLevel()
	{
		LevelDataTable[CurrentLevel].completed = true;
		if (CurrentLevel + 1 < LevelDataTable.Length)
		{
			LevelDataTable[CurrentLevel + 1].locked = false;
		}
		SaveLevelInfo();
		return;
	}
	public void LoadLevelInfo()
	{
		int[] lvlArray = PlayerPrefsX.GetIntArray("LevelInfo");
		if (lvlArray.Length < LevelDataTable.Length)
		{
			int oldArraySize = lvlArray.Length;
			Array.Resize<int>(ref lvlArray, LevelDataTable.Length);
			for (int i = oldArraySize; i < LevelDataTable.Length; i++)
			{
				if (i == 0)
				{
					lvlArray[0] = 0;
				}
				else
				{
					lvlArray[i] = LEVEL_LOCKED;
				}
			}
		}
		else if (lvlArray.Length > LevelDataTable.Length)
		{
			Array.Resize<int>(ref lvlArray, LevelDataTable.Length);
		}
		for (int i = 0; i < LevelDataTable.Length; i++)
		{
			LevelDataTable[i].locked = (lvlArray[i] & LEVEL_LOCKED) > 0; //Bitwise check if level is locked i.e. 1 or 3
			LevelDataTable[i].completed = (lvlArray[i] & LEVEL_COMPLETED) > 0; // ditto for completed i.e. 2 or 3
		}
	}
	public void SaveLevelInfo()
	{
		int[] lvlArray = new int[LevelDataTable.Length];
		for (int i = 0; i < LevelDataTable.Length; i++)
		{
			lvlArray[i] = 0;
			if (LevelDataTable[i].locked)
				lvlArray[i] += LEVEL_LOCKED;
			if (LevelDataTable[i].completed)
				lvlArray[i] += LEVEL_COMPLETED;
		}
		PlayerPrefsX.SetIntArray("LevelInfo", lvlArray);
	}
	public int GetLevelIndex(string levelName)
	{
		//print(levelName);
		if (levelName == LEVEL_MAINMENU)
		{
			return -1;
		
		}
		else
		{
			for (int i = 0; i < LevelDataTable.Length; i++)
			{
				if (levelName == LevelDataTable[i].name)
				{
					return i;
				}
			}
		}
		return -1;
	}
	public void LoadLevel(int levelNum)
	{
		if (levelNum == -1)
		{
			CurrentLevel = -1;
			SetState(State.STATE_MENU);
			Application.LoadLevel(LEVEL_MAINMENU);
		}
		else if(levelNum < LevelDataTable.Length)
		{
			CurrentLevel = levelNum;
			SetState(State.STATE_PLAYING);
			Application.LoadLevel(LevelDataTable[levelNum].name);
		}
	}
	public void LoadNextLevel()
	{
		//print("Current Level:" + CurrentLevel);
		CurrentLevel++;
		if (CurrentLevel >= LevelDataTable.Length)
		{
			LoadLevel(-1);

		}
		else
		{
			LoadLevel(CurrentLevel);
		}
	}
	public void LoadLevel(string levelName)
	{
		if (levelName == LEVEL_MAINMENU)
		{
			CurrentLevel = -1;
			SetState(State.STATE_MENU);
		}
		else
		{
			for (int i = 0; i < LevelDataTable.Length; i++)
			{
				if (levelName == LevelDataTable[i].name)
				{
					CurrentLevel = i;
					break;
				}
			}
			SetState(State.STATE_PLAYING);
		}
		Application.LoadLevel(levelName);
	}
	public void SetCurrentLevel(int levelNum)
	{
		CurrentLevel = levelNum;
	}
	public bool IsInState(State state)
	{
		if (currentState == state)
		{
			return true;
		}
		return false;
	}
	void OnGUI()
	{
		switch (currentState)
		{
			case State.STATE_LOSE:
			GUIStyle overStyle = new GUIStyle (GUI.skin.label);
			GUIStyle buttonStyle = new GUIStyle (GUI.skin.button);
			overStyle.fontSize = 48;
			overStyle.font = GUIFont;
			buttonStyle.normal.background = (Texture2D)Resources.Load ("retryButton");
			buttonStyle.hover.background = (Texture2D)Resources.Load ("retryButtonHover");
			buttonStyle.active.background = (Texture2D)Resources.Load ("retryButtonActive");
			//buttonStyle.colors.normalColor = Color.green;
				GUI.Label(new Rect(Screen.width / 2 - 140, 150, 300, 100), "GAME OVER", overStyle);

				//GUI.Label(new Rect(Screen.width / 2 - 40, 300, 80, 30), "Score: " + GameManager.GM.score);
				if (GUI.Button(new Rect(Screen.width / 2 - 50, 250, 100, 50), "Retry?", buttonStyle))
				{
					LoadLevel(LevelDataTable[CurrentLevel].name);
				}
				break;
			case State.STATE_WIN:
				GUI.Label(new Rect(Screen.width / 2 - 40, 100, 80, 30), "LEVEL COMPLETED");

				//GUI.Label(new Rect(Screen.width / 2 - 40, 300, 80, 30), "Score: " + GameManager.GM.score);
				if (GUI.Button(new Rect(Screen.width / 2 - 30, 350, 60, 30), "NEXT LEVEL"))
				{
					LoadLevel(LEVEL_MAINMENU);
				}
				break;
		}

	}

	public void SetState(State newState)
	{
		EndState(currentState);
		currentState = newState;
		StartState(newState);
	}

	private void StartState(State newState)
	{
		switch (newState)
		{
			case State.STATE_LOSE:
				Time.timeScale = 0;
				break;
			case State.STATE_WIN:
				Time.timeScale = 0;
				CompleteLevel();
				break;
			case State.STATE_MENU:
				GameManager.GM.Reset();
				break;
		}
	}

	private void EndState(State previousState)
	{
		switch (previousState)
		{
			case State.STATE_LOSE:
				Time.timeScale = 1;
				break;
			case State.STATE_WIN:
				Time.timeScale = 1;
				break;
		}
	}
	void Update()
	{

	}
	public static bool IsPlaying()
	{
		if (LM.currentState == State.STATE_PLAYING)
			return true;

		return false;
	}
	//~LevelManager()
	//{
	//	SaveLevelInfo();
	//}
}