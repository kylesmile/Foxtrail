using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitEffect : MonoBehaviour {

	private ParticleSystem featherParticles;
	private AudioSource dyingChicken;

	void Start () {
		featherParticles = GetComponent<ParticleSystem> ();
		dyingChicken = GetComponent<AudioSource> ();
	}

	public void Play () {
		featherParticles.Play ();
		dyingChicken.Play ();
	}
}
