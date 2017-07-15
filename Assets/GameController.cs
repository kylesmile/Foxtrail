using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public Fox fox;
	public Dog dog;
	public CanvasGroup gameOver;

	private Collider2D foxCollider;
	private Collider2D dogCollider;

	void Start () {
		foxCollider = fox.GetComponent<Collider2D> ();
		dogCollider = dog.GetComponent<Collider2D> ();
		gameOver.gameObject.SetActive (false);
	}
	
	void FixedUpdate () {
		if (dogCollider.IsTouching (foxCollider)) {
			gameOver.gameObject.SetActive (true);
		}
	}
}
