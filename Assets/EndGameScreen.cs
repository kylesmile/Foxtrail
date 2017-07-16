using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour {
	public Text textElement;

	void Start () {
		gameObject.SetActive (false);
	}

	public void ShowLost () {
		textElement.text = "You lose!";
		gameObject.SetActive (true);
	}

	public void ShowWon () {
		textElement.text = "You win!";
		gameObject.SetActive (true);
	}
}
