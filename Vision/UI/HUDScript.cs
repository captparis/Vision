using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDScript : MonoBehaviour {

	float playerScore = 0;
	public float levelTime = 60.0f;
	float remainingLevelTime;
	private int levelTimeInt;
	HeroCharacterController PC;
	public GameObject[] armourImages;
	//public int currentArmour = 0;
	public GameObject HUDTime; 
	//private float FocusTime;
	//private float FocusMaxTime;
	private float FocusPercentage;
	private float liquidPosition;
	public GameObject visionMeter;
	private RectTransform visionTransform;
	public GameObject[] visionUI;
	public GameObject[] timeUI;
	bool visionDisabled = true;
	FocusMode FM;
	private RectTransform fadeRect;
	private float fadeValue;
	private float fadeTarget;
	private bool fadeComplete = true;
	public float blackFadeSpeed = 0.6f;
	private float fadeDirection = -1f;

	void Start()
	{
		remainingLevelTime = levelTime;
		if (levelTime == 0)
		{
			for (int i = 0; i < timeUI.Length; i++)
			{
				timeUI[i].SetActive(false);
			}
		}
		if (visionDisabled)
		{
			if (GameManager.GM.focusAvailable)
			{
				visionDisabled = false;
				for (int i = 0; i < visionUI.Length; i++)
				{
					visionUI[i].SetActive(true);

				}
			}
			else
			{
				for (int i = 0; i < visionUI.Length; i++)
				{
					visionUI[i].SetActive(false);
				}
			}
		}
		visionTransform = visionMeter.GetComponent<RectTransform>();
		fadeRect = GameObject.Find("HUD/Black").GetComponent<RectTransform>();
		FM = GetComponent<FocusMode>();
		PC = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCharacterController>();
		updateArmour(PC.health);
		FadeBlack(false);
	}
		
	// Update is called once per frame
	void Update () {
		if (visionDisabled && GameManager.GM.focusAvailable)
		{
			visionDisabled = false;
			for (int i = 0; i < visionUI.Length; i++)
			{
				visionUI[i].SetActive(true);

			}
		}
		playerScore += Time.deltaTime * 100;
		if(LevelManager.IsPlaying())
			remainingLevelTime -= Time.unscaledDeltaTime;
		
		if (levelTime > 0 && remainingLevelTime <= 0.0f)
		{
			timerEnded();
		}
		
		if (!fadeComplete)
		{
			fadeValue += fadeDirection*blackFadeSpeed*Time.deltaTime;
			if (fadeTarget-(fadeValue*fadeDirection)<=0)
			{
				fadeValue = fadeTarget;
				fadeComplete = true;
			}
			Color tempColor = Color.black;
			tempColor.a = fadeValue;
			fadeRect.GetComponent<Image>().color = tempColor;
		}

		FocusPercentage = (FM.focusTime/FM.maxFocusTime) - 1;
		liquidPosition = (FocusPercentage*27) - 10;
		visionTransform.anchoredPosition = new Vector2( 0, liquidPosition);
		HUDTime.GetComponent<Text>().text = (Mathf.Max((int)remainingLevelTime, 0)).ToString();
		
	}
	
	void timerEnded()
	{
		LevelManager.LM.SetState(LevelManager.State.STATE_LOSE);
		return;
	}
	

	public void increaseScore(int amount)
	{
		playerScore += amount;
	}

	void OnDisable()
	{
		GameManager.GM.score = (int)playerScore;
	}

	/* OLD GUI SYSTEM
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 100, 30), "Score: " + (int)(playerScore));

		GUI.Label (new Rect(10, 30, 200, 60), "Time: " + (int)(levelTime));
		GUI.Label(new Rect(10, 50, 100, 30), "HP: " + PC.health);
		GUI.Label(new Rect(10, 70, 100, 30), "Focus Time: " + Mathf.CeilToInt(Camera.main.GetComponent<FocusMode>().FocusTime));
	}*/

	// ARMOUR FUNCTIONS

	
	public void updateArmour(int health)
	{
		
		health /= 10; // 0-50 -> 0-5 Health Range
		
		for (int i = 0; i < armourImages.Length; i++)
		{
			if (i < health)
			{
				armourImages[i].SetActive(true);
			}
			else
			{
				armourImages[i].SetActive(false);
			}
		}
	}
	
	public void FadeBlack (bool fadeOut)
	{
		fadeValue = fadeRect.GetComponent<Image>().color.a;
		if (fadeOut)
		{
			fadeTarget = 1;
			fadeDirection = 1;
			fadeComplete = false;
		}
		else
		{
			fadeTarget = 0;
			fadeDirection = -1;
			fadeComplete = false;
		}
	}
}
