using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	public Fox fox;
	public Dog dog;
	public CanvasGroup gameOver;
	public float gameOverScreenDelay = 1.0f;

	private Collider2D foxCollider;
	private Collider2D dogCollider;

	public void Restart () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	void Start () {
		foxCollider = fox.GetComponent<Collider2D> ();
		dogCollider = dog.GetComponent<Collider2D> ();
		gameOver.gameObject.SetActive (false);
	}
	
	void FixedUpdate () {
		if (dogCollider.IsTouching (foxCollider)) {
			dog.Kill ();
			fox.Die ();
			Invoke ("GameOver", gameOverScreenDelay);	
		}
	}

	void GameOver () {
		gameOver.gameObject.SetActive (true);
	}
}
