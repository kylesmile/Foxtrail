using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class Dog : MonoBehaviour {
	public float movementSpeed = 1.0f;
	public float scentRadius = 3.0f;
	public LayerMask scentLayers;
	
	// Update is called once per frame
	void FixedUpdate () {
		Collider2D[] nearbyScentColliders = Physics2D.OverlapCircleAll (new Vector2(transform.position.x, transform.position.y), scentRadius, scentLayers);
		Scent nearestScent = null;
		foreach (Collider2D collider in nearbyScentColliders) {
			Scent scent = collider.gameObject.GetComponent<Scent> ();
			if (!nearestScent || scent.creationFrame > nearestScent.creationFrame) {
				nearestScent = scent;
			}
		}

		if (nearestScent) {
			Vector3 direction = (nearestScent.transform.position - transform.position).normalized;

			float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			transform.Translate (transform.InverseTransformDirection (direction * movementSpeed * Time.fixedDeltaTime));
		}
	}

	[DrawGizmo(GizmoType.Selected)]
	static void DrawScentCircle (Dog dog, GizmoType _gizmoType) {
		Gizmos.DrawWireSphere (dog.transform.position, dog.scentRadius);
	}
}
