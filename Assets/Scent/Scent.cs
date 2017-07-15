using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scent : MonoBehaviour {
	[HideInInspector]
	public int creationFrame;

	// Use this for initialization
	void Start () {
		creationFrame = Time.frameCount;
	}
}
