using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHint : MonoBehaviour {
	public float fadeTime = 1.0f;
	public float maxOpacity = 0.8f;
	public float minOpacity = 0.0f;

	private int collisionCount = 0;
	private float fadeStart;
	private bool fading = false;
	private new SpriteRenderer renderer;

	void Start () {
		renderer = GetComponent<SpriteRenderer> ();
		renderer.color = new Color (1f, 1f, 1f, 0f);
	}

	void OnTriggerEnter2D (Collider2D _collider) {
		if (collisionCount == 0) {
			fadeStart = Time.time;
			fading = true;
		}
		collisionCount++;
	}

	void OnTriggerExit2D (Collider2D _collider) {
		collisionCount--;
		if (collisionCount == 0) {
			fadeStart = Time.time;
			fading = true;
		}
	}

	void Update () {
		if (fading) {
			float percent = (Time.time - fadeStart) / fadeTime;
			if (collisionCount == 0) {
				percent = 1 - percent;
			}
			renderer.color = new Color (1f, 1f, 1f, Mathf.SmoothStep (minOpacity, maxOpacity, percent));
		}
	}
}
