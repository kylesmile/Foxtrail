using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
	public List<Transform> collisionRayOrigins;
	public float skinWidth = 0.015f;
	public LayerMask obstacleLayers;

	private new Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody2D> ();
	}

	public void Move (Vector2 movement) {
		if (rigidbody) {
			float angle = Mathf.Atan2 (movement.y, movement.x) * Mathf.Rad2Deg;
			rigidbody.MoveRotation (angle);

			Vector2 collidedMovement = CheckCollisions (movement);
			Vector2 targetPosition = rigidbody.position + collidedMovement;
			rigidbody.MovePosition (targetPosition);
		}
	}

	Vector2 CheckCollisions (Vector2 movement) {
		float originalMovementDistance = movement.magnitude + skinWidth;
		float movementDistance = originalMovementDistance;
		Vector2 normalComponent = Vector2.zero;
		foreach (Transform origin in collisionRayOrigins) {
			RaycastHit2D hit = Physics2D.Raycast (new Vector2(origin.position.x, origin.position.y), movement, movementDistance, obstacleLayers);
			if (hit && hit.distance - skinWidth < movementDistance) {
				movementDistance = hit.distance - skinWidth;
				normalComponent = hit.normal * (originalMovementDistance - movementDistance);
			}
		}

		return (movement.normalized * movementDistance) + normalComponent;
	}
}
