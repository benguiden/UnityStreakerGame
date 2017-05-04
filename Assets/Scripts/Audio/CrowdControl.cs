using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : MonoBehaviour {

	//This script will control the volume and pitch of an audio source depending on the intensity
	//With this we can increase the crowds 'excitement' when something happens

	[Tooltip("The maximum volume that the audio source will play at.")]
	[Range(0f, 1f)]
	public float maxVolume = 0.8f;

	[Tooltip("The amount that pitch changes along with intensity.")]
	[Range(0f, 1f)]
	public float pitchChange = 0.2f;

	[Tooltip("The speed at which the audio source looses intensity.")]
	public float calmSpeed = 0.1f;

	public static float intensity = 0.5f;

	private float _apparentIntensity = 0.5f;
	private AudioSource audioSource;

	void Start(){
		audioSource = this.GetComponent<AudioSource> ();
	}

	void Update () {

		intensity -= Time.deltaTime * calmSpeed;

		intensity = Mathf.Clamp01 (intensity);

		if (_apparentIntensity < intensity)
			_apparentIntensity += calmSpeed * Time.deltaTime * 4f;
		else
			_apparentIntensity = intensity;

		audioSource.volume = _apparentIntensity * maxVolume;
		audioSource.pitch = 1 + (_apparentIntensity * pitchChange);

	}

}
