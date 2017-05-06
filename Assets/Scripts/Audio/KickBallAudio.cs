using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickBallAudio : MonoBehaviour {

	[Tooltip("The acceleration at which the ball triggers the audio source to play.")]
	public float accelerationTrigger;

	private AudioSource audioSource;
	private Rigidbody rb;
	private float veloLast = 0f;

	void Start(){
		audioSource = this.GetComponent<AudioSource> ();
		rb = this.GetComponent<Rigidbody> ();
	}

	void Update(){

		//Get Acceleration
		float acceleration = rb.velocity.magnitude - veloLast;
		veloLast = rb.velocity.magnitude;

		if (Mathf.Abs(acceleration) >= accelerationTrigger) {
			audioSource.time = 0f;
			audioSource.Play ();
		}

	}

}
