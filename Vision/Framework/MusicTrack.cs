using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vision.Framework
{
	public class MusicTrack : MonoBehaviour
	{
		public AudioClip clip;

		void Awake()
		{
			MusicManager.MM.PlayMusic(clip);
		}
	}
}
