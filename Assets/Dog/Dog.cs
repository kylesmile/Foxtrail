using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof (Animator))]
public class Dog : MonoBehaviour {
	public enum Direction { Forward = 1, Backward = -1 };

	public float runSpeed = 3.0f;
	public float scentRadius = 3.0f;
	public float patrolSpeed = 1.0f;
	public float patrolTurnFactor = 0.02f;
	public Direction patrolDirection = Direction.Forward;
	public LayerMask scentLayers;

	private Animator animator;
	private MovementController movementController;
	private bool active = true;

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
				Patrol ();
			}
		}
	}

	public void Kill () {
		active = false;
		animator.SetBool ("Kill", true);
	}

	public void Stop () {
		active = false;
		animator.SetBool ("Chasing", false);
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
		animator.SetBool ("Chasing", true);
		Vector3 direction = (scent.transform.position - transform.position).normalized * runSpeed * Time.fixedDeltaTime;
		movementController.Move (new Vector2 (direction.x, direction.y));
	}

	private void Patrol () {
		animator.SetBool ("Chasing", false);
		Vector3 movement = (Vector3.right + Vector3.up * patrolTurnFactor * (int)patrolDirection).normalized * patrolSpeed * Time.fixedDeltaTime;
		Vector3 direction = transform.TransformDirection (movement);
		movementController.Move (new Vector2 (direction.x, direction.y));
	}

	#if UNITY_EDITOR
	[DrawGizmo(GizmoType.Selected)]
	static void DrawScentCircle (Dog dog, GizmoType _gizmoType) {
		Gizmos.DrawWireSphere (dog.transform.position, dog.scentRadius);
	}
	#endif
}
