using UnityEngine;
using System.Collections;
using System;

namespace Vision.Framework
{
	public class MusicManager : MonoBehaviour
	{
		private static MusicManager _MM;

		public static MusicManager MM
		{
			get
			{
				if (_MM == null)
				{
					GameObject obj = new GameObject("MusicManager");
					_MM = obj.AddComponent<MusicManager>();
				}
				return _MM;
			}
		}
		AudioClip clip;
		AudioSource source;
		// Use this for initialization
		void Awake()
		{
			source = gameObject.AddComponent<AudioSource>();
			source.priority = 256;
			source.loop = true;
			source.volume = 0.2F;
			DontDestroyOnLoad(gameObject);
		}
		public void PlayMusic(AudioClip newClip)
		{
			if (newClip != null && newClip != clip)
			{
				clip = newClip;
				source.clip = clip;
				source.Play();
			}
		}
		public void StopMusic()
		{
			source.Stop();
		}
	}
}
