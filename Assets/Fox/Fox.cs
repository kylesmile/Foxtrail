using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Animator))]
public class Fox : Smellable {
	public float movementSpeed = 1.0f;
	public float scentDistance = 3.0f;
	public Scent scentPrefab;

	private Animator animator;
	private float accumulatedDistance = 0.0f;
	private bool dead = false;
	private MovementController movementController;

	void Start () {
		animator = GetComponent<Animator> ();
		movementController = GetComponent<MovementController> ();
	}

	void Update () {
		if (dead) {
			return;
		}

		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");
		Vector2 direction = new Vector2 (horizontal, vertical).normalized * movementSpeed * Time.deltaTime;

		if (direction != Vector2.zero) {
			animator.SetBool ("Moving", true);
			movementController.Move (direction);
		} else {
			animator.SetBool ("Moving", false);
		}

		accumulatedDistance += direction.magnitude;

		if (accumulatedDistance > scentDistance) {
			accumulatedDistance = 0.0f;
			Instantiate (scentPrefab, transform.position, transform.rotation);
		}
	}

	public void Die () {
		dead = true;
		animator.SetBool ("Dead", true);
	}

	public override float ScentStrength () {
		return Mathf.Infinity;
	}
}
