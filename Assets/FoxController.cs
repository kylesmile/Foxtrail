using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour {
	public float movementSpeed = 1.0f;

	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis ("Horizontal") * Time.deltaTime * movementSpeed;
		float vertical = Input.GetAxis ("Vertical") * Time.deltaTime * movementSpeed;

		transform.Translate (horizontal, vertical, 0);
	}
}
