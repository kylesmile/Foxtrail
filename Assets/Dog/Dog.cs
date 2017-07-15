using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Collider2D[] nearbyScentColliders = Physics2D.OverlapCircleAll (new Vector2(transform.position.x, transform.position.y), scentRadius, scentLayers);
		Smellable nearestScent = null;
		foreach (Collider2D collider in nearbyScentColliders) {
			Smellable scent = collider.gameObject.GetComponent<Smellable> ();
			if (!nearestScent || scent.ScentStrength () > nearestScent.ScentStrength ()) {
				nearestScent = scent;
			}
		}

		if (nearestScent) {
			animator.SetBool ("Moving", true);
			Vector3 direction = (nearestScent.transform.position - transform.position).normalized;

			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			transform.Translate (transform.InverseTransformDirection (direction * movementSpeed * Time.fixedDeltaTime));
		} else {
			animator.SetBool ("Moving", false);
		}
	}

	#if UNITY_EDITOR
	[DrawGizmo(GizmoType.Selected)]
	static void DrawScentCircle (Dog dog, GizmoType _gizmoType) {
		Gizmos.DrawWireSphere (dog.transform.position, dog.scentRadius);
	}
	#endif
}
