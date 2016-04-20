using UnityEngine;
using System.Collections;

namespace Shared.Debug {
	public class AxisDisplay : MonoBehaviour
	{
		public int length = 1;
		// Use this for initialization
		void Start()
		{
		}

		// Update is called once per frame
		void Update()
		{
			UnityEngine.Debug.DrawRay(transform.position, transform.up * length, Color.green, 0,false);
			UnityEngine.Debug.DrawRay(transform.position, transform.right * length, Color.red, 0, false);
			UnityEngine.Debug.DrawRay(transform.position, transform.forward * length, Color.blue, 0, false);
		}
	}
}

