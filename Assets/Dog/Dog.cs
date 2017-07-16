using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof (Animator))]
public class Dog : MonoBehaviour {
	public float movementSpeed = 1.0f;
	public float scentRadius = 3.0f;
	public LayerMask scentLayers;

	private Animator animator;
	private MovementController movementController;
	private Vector2 target = Vector2.zero;
	private bool active = false;
	private bool targeting = false;

	void Start () {
		animator = GetComponent<Animator> ();
		movementController = GetComponent<MovementController> ();
	}

	void FixedUpdate () {
		if (active) {
			Smellable scent = Smell ();

			if (scent) {
				RunTowardsScent (scent);
			} else {
				animator.SetBool ("Moving", false);
			}
		} else if (targeting) {
			MoveToTarget ();
		}

	}

	public void Kill () {
		active = false;
		animator.SetBool ("Kill", true);
	}

	public void Stop () {
		active = false;
		animator.SetBool ("Moving", false);
	}

	public void Enter (Vector2 entranceTarget) {
		targeting = true;
		target = entranceTarget;
	}

	private Smellable Smell () {
		Collider2D[] nearbyScentColliders = Physics2D.OverlapCircleAll (new Vector2(transform.position.x, transform.position.y), scentRadius, scentLayers);
		Smellable strongestScent = null;
		foreach (Collider2D collider in nearbyScentColliders) {
			Smellable scent = collider.gameObject.GetComponent<Smellable> ();
			if (!strongestScent || scent.ScentStrength () > strongestScent.ScentStrength ()) {
				strongestScent = scent;
			}
		}

		return strongestScent;
	}

	private void RunTowardsScent (Smellable scent) {
		animator.SetBool ("Moving", true);
		Vector3 direction = (scent.transform.position - transform.position).normalized * movementSpeed * Time.fixedDeltaTime;
		movementController.Move (new Vector2 (direction.x, direction.y));
	}

	private void MoveToTarget () {
		animator.SetBool ("Moving", true);
		Vector2 currentPosition = new Vector2 (transform.position.x, transform.position.y);
		Vector2 direction = target - currentPosition;

		if (direction.sqrMagnitude < 0.5f) {
			targeting = false;
			active = true;
		} else {
			movementController.Move(direction.normalized * movementSpeed * Time.fixedDeltaTime);
		}
	}

	#if UNITY_EDITOR
	[DrawGizmo(GizmoType.Selected)]
	static void DrawScentCircle (Dog dog, GizmoType _gizmoType) {
		Gizmos.DrawWireSphere (dog.transform.position, dog.scentRadius);
	}
	#endif
}
