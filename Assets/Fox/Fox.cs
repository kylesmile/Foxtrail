using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Animator))]
public class Fox : Smellable {
	public float movementSpeed = 5.0f;
	public float crawlSpeed = 1.0f;
	public float scentDistance = 3.0f;
	public float runFootstepSeparation = 0.3f;
	public float crawlFootstepSeparation = 0.7f;
	public Scent scentPrefab;
	public LayerMask crawlZoneLayerMask;

	private Animator animator;
	private float accumulatedDistance = 0.0f;
	private bool dead = false;
	private Vector2 target = Vector2.zero;
	private bool targeting = false;
	private bool active = false;
	private int crawlZoneCount = 0;
	private MovementController movementController;
	private AudioSource footsteps;
	private float lastFootstepTime;

	void Start () {
		animator = GetComponent<Animator> ();
		movementController = GetComponent<MovementController> ();
		footsteps = GetComponent<AudioSource> ();
		lastFootstepTime = Time.time;
	}

	void FixedUpdate () {
		if (dead) {
			return;
		}

		Vector2 direction = GetMovementDirection ();

		if (direction != Vector2.zero) {
			float footstepSeparation = crawlZoneCount > 0 ? crawlFootstepSeparation : runFootstepSeparation;
			if (Time.time - lastFootstepTime > footstepSeparation) {
				footsteps.Play ();
				lastFootstepTime = Time.time;
			}
			animator.SetBool ("Moving", true);
			if (crawlZoneCount > 0) {
				animator.SetBool ("Crawling", false);
			}
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

	void OnTriggerEnter2D (Collider2D otherCollider) {
		if (otherCollider.gameObject.CompareTag("Fence")) {
			crawlZoneCount++;
		}
	}

	void OnTriggerExit2D (Collider2D otherCollider) {
		if (otherCollider.gameObject.CompareTag("Fence")) {
			crawlZoneCount--;
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
			Vector2 direction = new Vector2 (horizontal, vertical).normalized;
			if (crawlZoneCount > 0) {
				direction *= crawlSpeed;
			} else {
				direction *= movementSpeed;
			}
			return direction * Time.fixedDeltaTime;
		}
	}
}
