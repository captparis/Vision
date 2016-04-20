using UnityEngine;
using System.Collections;

public class OpenURL : MonoBehaviour {
	

	public void Open(string URL) {
		Application.ExternalEval("window.open('" + URL + "','_blank')");
	}
}
