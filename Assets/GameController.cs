using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;

public class GameController : MonoBehaviour {
	public Fox foxPrefab;
	public Dog dogPrefab;
	public LevelEndpoint start;
	public LevelEndpoint end;
	public EndGameScreen endScreen;
	public float endScreenDelay = 1.0f;
	public Camera2DFollow cameraFollow;

	private Collider2D foxCollider;
	private Collider2D dogCollider;
	private Collider2D endCollider;
	private bool active = false;
	private Fox fox;
	private Dog dog;

	public void Restart () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	void Start () {
		endCollider = end.GetComponent<Collider2D> ();
		fox = Instantiate<Fox> (foxPrefab, start.offScreenTransform.position, Quaternion.identity);

		foxCollider = fox.GetComponent<Collider2D> ();


		dog = Instantiate<Dog> (dogPrefab, start.offScreenTransform.position, Quaternion.identity);
		dogCollider = dog.GetComponent<Collider2D> ();

		Invoke ("StartFox", 0.5f);
		Invoke ("ActivateControls", 1.5f);
		Invoke ("StartDog", 2.5f);
	}
	
	void FixedUpdate () {
		if (!active) {
			return;
		}

		if (dogCollider.IsTouching (foxCollider)) {
			active = false;
			dog.Kill ();
			fox.Die ();
			Invoke ("Lose", endScreenDelay);	
		} else if (foxCollider.IsTouching (endCollider)) {
			active = false;
			dog.Stop ();
			fox.Exit (end.OffScreen ());
			Invoke ("Win", endScreenDelay);
		}
	}

	void StartFox () {
		fox.Enter (start.OnScreen ());
	}

	void ActivateControls () {
		start.Block ();
		cameraFollow.SetTarget (fox.transform);
		active = true;
		fox.Activate ();
	}

	void StartDog () {
		dog.Enter (start.OnScreen ());
	}

	void Lose () {
		endScreen.ShowLost ();
	}

	void Win () {
		endScreen.ShowWon ();
	}
}
