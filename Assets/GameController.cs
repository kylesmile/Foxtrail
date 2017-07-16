using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;

public class GameController : MonoBehaviour {
	public Fox foxPrefab;
	public LevelEndpoint start;
	public LevelEndpoint end;
	public Camera2DFollow cameraFollow;

	[Header("Cinematics")]
	public EndGameScreen endScreen;
	public ExpositionScreen expositionScreen;

	[Header("Cinematics timing")]
	public float startDelay = 0.5f;
	public float showExpositionDelay = 1.0f;
	public float expositionShowDuration = 3.0f;
	public float foxEntranceDelay = 0.5f;
	public float controlsActivationDelay = 0.75f;
	public float endScreenDelay = 1.0f;
	public float winEffectDelay = 2.0f;

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

		Invoke ("ShowExit", startDelay);
	}
	
	void FixedUpdate () {
		if (!active) {
			return;
		}

		if (foxCollider.IsTouching (endCollider)) {
			active = false;
			StopDogs ();
			end.Unblock ();
			fox.Exit (end.OffScreen ());
			Invoke ("WinEffect", winEffectDelay);
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
		Invoke ("ShowExposition", showExpositionDelay);
	}

	void ShowExposition () {
		expositionScreen.Show ();
		Invoke ("FollowFox", expositionShowDuration);
	}

	void FollowFox () {
		expositionScreen.Hide ();
		cameraFollow.SetTarget (fox.transform);
		Invoke ("StartFox", foxEntranceDelay);
	}

	void StartFox () {
		fox.Enter (start.OnScreen ());
		Invoke ("ActivateControls", controlsActivationDelay);
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

	void WinEffect () {
		end.exitEffect.Play ();
		Invoke ("Win", endScreenDelay);
	}

	void Win () {
		endScreen.ShowWon ();
	}
}
