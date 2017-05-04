using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollBreak : MonoBehaviour {

	//This script functions to check if the ragdoll breaks and the limbs streatch too much,
	//if so it deletes the game object, as this bug can ruin the gameplay

	[Tooltip("If the limb exceeds this limit, the game object will be destroyed.")]
	public float stretchLimit = 0.1f;

	void Update(){
		if (this.transform.localPosition.magnitude >= stretchLimit) {
			//Find limb parent
			Transform limb = this.transform;
			//Find parent guard object of the limb
			while (limb.parent.tag != "NPC") {
				limb = limb.parent;
			}
			Debug.Log ("Destroyed " + limb.parent.gameObject.name + " game object due to ragdoll stretching.");
			Destroy (limb.parent.gameObject);
		}
	}

}
