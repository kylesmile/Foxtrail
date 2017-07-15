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
	private bool caughtFox = false;
	private MovementController movementController;

	void Start () {
		animator = GetComponent<Animator> ();
		movementController = GetComponent<MovementController> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (caughtFox) {
			return;
		}

		Smellable scent = Smell ();

		if (scent) {
			RunTowardsScent (scent);
		} else {
			animator.SetBool ("Moving", false);
		}
	}

	public void Kill () {
		caughtFox = true;
		animator.SetBool ("Kill", true);
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

	#if UNITY_EDITOR
	[DrawGizmo(GizmoType.Selected)]
	static void DrawScentCircle (Dog dog, GizmoType _gizmoType) {
		Gizmos.DrawWireSphere (dog.transform.position, dog.scentRadius);
	}
	#endif
}
