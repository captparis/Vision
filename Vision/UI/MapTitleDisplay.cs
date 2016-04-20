using UnityEngine;
using System.Collections;

namespace Vision.UI
{
	public class MapTitleDisplay : MonoBehaviour
	{
		[SerializeField]
		float fadeStartTime = 5;
		[SerializeField]
		float fadeEndTime = 3;

		float alpha = 1;
		CanvasGroup CG;

		// Use this for initialization
		void Start()
		{
			CG = GetComponent<CanvasGroup>();
		}

		// Update is called once per frame
		void Update()
		{
			if (Time.timeSinceLevelLoad >= fadeStartTime)
			{
				alpha -= Time.unscaledDeltaTime / fadeEndTime;
				CG.alpha = Mathf.Clamp(alpha, 0, 1);
				if (alpha <= 0)
				{
					Destroy(gameObject);
				}
			}
		}
	}
}
