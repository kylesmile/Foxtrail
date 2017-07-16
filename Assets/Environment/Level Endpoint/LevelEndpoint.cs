using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndpoint : MonoBehaviour {
	public Transform offScreenTransform;
	public Transform onScreenTransform;
	public ExitEffect exitEffect;
	public GameObject blocker;

	public Vector2 OffScreen () {
		return new Vector2 (offScreenTransform.position.x, offScreenTransform.position.y);
	}

	public Vector2 OnScreen () {
		return new Vector2 (onScreenTransform.position.x, onScreenTransform.position.y);
	}

	public void Block () {
		blocker.SetActive (true);
	}

	public void Unblock () {
		blocker.SetActive (false);
	}
}
