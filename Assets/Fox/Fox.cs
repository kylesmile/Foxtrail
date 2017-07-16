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
	private Vector2 target = Vector2.zero;
	private bool targeting = false;
	private bool active = false;
	private MovementController movementController;

	void Start () {
		animator = GetComponent<Animator> ();
		movementController = GetComponent<MovementController> ();
	}

	void FixedUpdate () {
		if (dead) {
			return;
		}

		Vector2 direction = GetMovementDirection ();

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

	public void Exit (Vector2 exitTarget) {
		targeting = true;
		target = exitTarget;
	}

	public void Enter (Vector2 entranceTarget) {
		targeting = true;
		target = entranceTarget;
	}

	public void Activate () {
		active = true;
	}

	public override float ScentStrength () {
		return Mathf.Infinity;
	}

	Vector2 GetMovementDirection () {
		if (targeting) {
			Vector2 currentPosition = new Vector2 (transform.position.x, transform.position.y);
			Vector2 direction = target - currentPosition;
			if (direction.sqrMagnitude < 0.5f) {
				targeting = false;
				active = false;
				return Vector2.zero;
			} else {
				return direction.normalized * movementSpeed * Time.fixedDeltaTime;
			}
		} else {
			if (!active) {
				return Vector2.zero;
			}

			float horizontal = Input.GetAxisRaw ("Horizontal");
			float vertical = Input.GetAxisRaw ("Vertical");
			return new Vector2 (horizontal, vertical).normalized * movementSpeed * Time.fixedDeltaTime;
		}
	}
}
