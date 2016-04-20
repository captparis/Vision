using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(DepthOfField))]
[RequireComponent(typeof(ColorCorrectionCurves))]
[RequireComponent(typeof(ParticleSystem))]
public class FocusMode : MonoBehaviour 
{
	public bool focusActive {get; private set;}
	[SerializeField]
	private bool unlimitedFocus = false;
	HeroCharacterController PC;
	Animator anim;
	public float focusTime = 10;
	public float maxFocusTime { get; private set; }
	[SerializeField]
	float focusRegenRate = 0.25f;
	private DepthOfField doField;
	private ColorCorrectionCurves ccCurves;
	private ParticleSystem particles;
	// Use this for initialization
	void Start () {
		focusActive = false;
		PC = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCharacterController>();
		anim = PC.GetComponent<Animator>();
		doField = this.transform.GetComponent<DepthOfField>();
		ccCurves = this.transform.GetComponent<ColorCorrectionCurves>();
		particles = this.transform.GetComponent<ParticleSystem>();
		maxFocusTime = focusTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (!LevelManager.IsPlaying())
		{
			return;
		}
		if (focusActive && !unlimitedFocus)
		{
			focusTime -= Time.unscaledDeltaTime;
			GameManager.GM.focus += Time.unscaledDeltaTime;
			particles.Simulate(Time.unscaledDeltaTime, false, false);
			if (focusTime <= 0)
			{
				DisableFocusEffects();
			}
		}
		else
		{
			focusTime = Mathf.Clamp(focusTime + Time.unscaledDeltaTime * focusRegenRate, 0, maxFocusTime);
		}
        if (!focusActive && (focusTime > 0 || unlimitedFocus) && Input.GetButtonDown("Focus") && GameManager.GM.focusAvailable)
        {
			EnableFocusEffects();
        }
        else if (focusActive && Input.GetButtonUp("Focus"))
        {
			DisableFocusEffects();
        }
	}
	public void DisableFocusEffects() {
		focusActive = false;
		anim.SetBool("Vision", false);
		Time.timeScale = 1;
		doField.enabled = false;
		ccCurves.enabled = false;
		particles.emissionRate = 0;
		particles.Clear();
	}
	public void EnableFocusEffects()
	{
		if (!PC.SpeechFreeze) {
			//print("Focus");
			focusActive = true;
			anim.SetBool("Vision", true);
			Time.timeScale = 0;
			doField.enabled = true;
			ccCurves.enabled = true;
			particles.emissionRate = 3;
			//GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
		}
	}
}
