using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitEffect : MonoBehaviour {

	private ParticleSystem featherParticles;

	void Start () {
		featherParticles = GetComponent<ParticleSystem> ();
	}

	public void Play () {
		featherParticles.Play ();
	}
}
