using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LimbImpact : MonoBehaviour {
    //This script detects if there are any impacts with the
    //rigidbody and plays audio based on the impact force

    [Tooltip("The change in velocity that the Audio Source plays a clip at the quietest volume.")]
    public float deltaVelocityMin = 2f;
    [Tooltip("The change in velocity that the Audio Source plays a clip at the loudest volume.")]
    public float deltaVelocityMax = 10f;

    private Rigidbody rb;
	private AudioSource audioSource;

	void Start(){
		rb = this.GetComponent<Rigidbody> ();
		audioSource = this.GetComponent<AudioSource> ();
	}


	void OnCollisionEnter(Collision c){
		if (rb.isKinematic == false) {
            if (c.relativeVelocity.magnitude >= deltaVelocityMin)
            {
                float force = Mathf.Clamp01(c.relativeVelocity.magnitude / (deltaVelocityMax - deltaVelocityMin)); //Returns a force between 0-1 to set the volume of the Audio Source
                audioSource.clip = NPCAudioClips.GetClip("fumbleClips");
                audioSource.volume = 0.25f + (0.5f * force);
                audioSource.Play();
            }
		}
	}

}
