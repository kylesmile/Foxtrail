using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;

public class GameController : MonoBehaviour {
	public Fox foxPrefab;
	public LevelEndpoint start;
	public LevelEndpoint end;
	public EndGameScreen endScreen;
	public float endScreenDelay = 1.0f;
	public Camera2DFollow cameraFollow;

	private Collider2D foxCollider;
	private Collider2D endCollider;
	private bool active = false;
	private Fox fox;
	private List<Dog> dogs;

	public void Restart () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	void Start () {
		endCollider = end.GetComponent<Collider2D> ();
		fox = Instantiate<Fox> (foxPrefab, start.offScreenTransform.position, Quaternion.identity);

		foxCollider = fox.GetComponent<Collider2D> ();

		Dog[] dogArray = FindObjectsOfType<Dog> ();
		dogs = new List<Dog> (dogArray);

		Invoke ("ShowExit", 0.5f);
		Invoke ("FollowFox", 2.5f);
		Invoke ("StartFox", 3.0f);
		Invoke ("ActivateControls", 3.75f);
	}
	
	void FixedUpdate () {
		if (!active) {
			return;
		}

		if (foxCollider.IsTouching (endCollider)) {
			active = false;
			StopDogs ();
			fox.Exit (end.OffScreen ());
			Invoke ("Win", endScreenDelay);
			return;
		}

		foreach (Dog dog in dogs) {
			Collider2D dogCollider = dog.GetComponent<Collider2D> ();
			if (dogCollider.IsTouching (foxCollider)) {
				KillFox (dog);
				return;
			}
		}
	}

	void ShowExit () {
		cameraFollow.SetTarget (end.onScreenTransform);
	}

	void FollowFox () {
		cameraFollow.SetTarget (fox.transform);
	}

	void StartFox () {
		fox.Enter (start.OnScreen ());
	}

	void ActivateControls () {
		start.Block ();

		active = true;
		fox.Activate ();
	}

	void KillFox (Dog killer) {
		active = false;

		StopDogs ();

		killer.Kill ();
		fox.Die ();
		Invoke ("Lose", endScreenDelay);
	}

	void StopDogs () {
		foreach (Dog dog in dogs) {
			dog.Stop ();
		}
	}

	void Lose () {
		endScreen.ShowLost ();
	}

	void Win () {
		endScreen.ShowWon ();
	}
}
