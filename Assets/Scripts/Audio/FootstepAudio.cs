using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour {

	[Tooltip("The time intervals in the animation that triggers the audio source to play.")]
	[Range(0f, 1f)]
	public float[] footStepFrames;
	[Tooltip("That state tag in the animator that the character is walking/running in.")]
	public string runningStateTag;

	private float currentTime = 0f;
	private float lastCurrentTime = 0f; //This is used to check if the float currentTime has changed since the last frame
	private AudioSource audioSource;
	private Animator anm;

	void Start(){
		audioSource = this.GetComponent<AudioSource> ();
		anm = this.GetComponent<Animator> ();
	}

	void Update(){

		//Check if that character is running
		if (anm.GetCurrentAnimatorStateInfo (0).IsTag ("Run")) {
			
			//Get the current time of the animation rounded to the first decimal place
			currentTime = Mathf.Round ((anm.GetCurrentAnimatorStateInfo (0).normalizedTime % 1.0f) * 10f) / 10f;

			if (currentTime != lastCurrentTime) {
				for (int i = 0; i < footStepFrames.Length; i++) {
					if (currentTime == footStepFrames [i]) {
					
						//Pick a random footstep clip
						audioSource.clip = audioSource.clip = NPC.GetClip ("footstepClips");
						audioSource.time = 0f;
						audioSource.Play ();
					}
				}
				lastCurrentTime = currentTime;
			}

		} else {
			lastCurrentTime = 0f;
		}

	}

}
