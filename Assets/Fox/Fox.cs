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

	void Start () {
		animator = GetComponent<Animator> ();
	}

	void FixedUpdate () {
		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");
		Vector3 direction = new Vector3 (horizontal, vertical, 0).normalized * movementSpeed * Time.fixedDeltaTime;

		if (direction != Vector3.zero) {
			animator.SetBool ("Moving", true);
			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			transform.Translate (transform.InverseTransformDirection (direction));
		} else {
			animator.SetBool ("Moving", false);
		}

		accumulatedDistance += direction.magnitude;

		if (accumulatedDistance > scentDistance) {
			accumulatedDistance = 0.0f;
			Instantiate (scentPrefab, transform.position, transform.rotation);
		}
	}

	public override float ScentStrength () {
		return Mathf.Infinity;
	}
}
