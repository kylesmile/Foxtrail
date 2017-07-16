using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpositionScreen : MonoBehaviour {

	void Start () {
		Hide ();
	}

	public void Hide () {
		gameObject.SetActive (false);
	}
	
	public void Show () {
		gameObject.SetActive (true);
	}
}
