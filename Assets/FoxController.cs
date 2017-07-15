using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour {
	public float movementSpeed = 1.0f;

	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxisRaw ("Horizontal") * Time.deltaTime * movementSpeed;
		float vertical = Input.GetAxisRaw ("Vertical") * Time.deltaTime * movementSpeed;
		Vector3 direction = new Vector3 (horizontal, vertical, 0);


		if (direction != Vector3.zero) {
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			transform.Translate (transform.InverseTransformDirection(direction));
		}
	}
}
