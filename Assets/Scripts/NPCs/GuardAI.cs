﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (CharacterController))]
public class GuardAI : MonoBehaviour {
	//AI for the guard with state machine, this script will be disabled when the guard jumps
	//for the player and go into ragdoll until they get up off the ground afterwards.

	//Variables
	[Tooltip("Maximum running speed of the rigidbody.")]
	public float maxSpeed;
	[Tooltip("The time it takes to accelerate to maximum running speed from 0m/s.")]
	public float accelTime;
	[Tooltip("How fast the controller steers towards the target.")]
	public float steeringSpd = 1f; //Should generally be close to 1, otherwise the AI is likely to either overshoot the player when chasing them, or steer unrealistically
	[Tooltip("The distance from the target that the controller desides to dive towards the target.")]
	public float diveDistance = 5f;
	[Tooltip("How long it takes for the character to recover from ragdoll mode.")]
	public float recoverTime = 2f;
	[Tooltip("The speed that the controller steps side to side when facing and in the way of the target.")]
	public float sideStepSpeed = 4f;
	[Tooltip("The distance from the target that the controller desides to get in the targets way, instead of moving towards the target.")]
	public float faceDistance = 35f;

	private float speed, acceleration, recoverCount/*The counter to recover time*/;
	private string state = "chase"; //This is the state the AI is in and determines how it will act 

	//Objects
	private CharacterController controller;
	private Transform target, targetModel; //targetModel is the child object of the target that includes the MeshRenderer (Mermaid Man Model), we want to this see where the model is facing for AI purposes
	private Animator anm;
	private Rigidbody[] ragdollRBs;
	private AudioSource audioSource;

	void Start(){
		//Variables
		acceleration = maxSpeed / accelTime; //This is similar to calculating speed using distance/time

		//Objects
		controller = this.GetComponent<CharacterController>();
		target = GameObject.FindGameObjectWithTag("Player").transform;
		targetModel = target.GetComponent<PlayerController> ().characterModel.transform;
		anm = this.GetComponentInChildren<Animator>();
		ragdollRBs = this.GetComponentsInChildren<Rigidbody> ();
		audioSource = this.GetComponent<AudioSource> ();
		audioSource.pitch = 0.9f + Random.Range (0.0f, 0.2f); //So not every guard appears to have the same 'voice'
	}

	void Update(){
		float distanceToTarget = Vector3.Distance (this.transform.position, target.position);
		float facingAngle = Geometry.FacingAngle (targetModel.transform, this.transform); //This is the angle between the targets position & FORWARD angle (localEularAngle.y) and this transform

		//State Machine
		switch (state){

		//Chasing the target
		case "chase":
			////////////////////////////////////////////////
			//Accelerate
			if (speed < maxSpeed) {
				speed += acceleration * Time.deltaTime;
			} else if (speed > maxSpeed) {
				speed = maxSpeed; //Stop Accelerating
			}

			//Chase and Steer towards target
			Chase (speed);

			//Change state if close enough to target
			if (distanceToTarget <= diveDistance) {
				state = "dive";
				audioSource.clip = null;
			}else if ((distanceToTarget <= faceDistance) && ((facingAngle <= 45f) && (facingAngle >= -45f)))
				state = "face";


			//Set Animation state
			anm.SetInteger("state", 0);

			break;
			////////////////////////////////////////////////

		//Diving into the target
		case "dive":
			////////////////////////////////////////////////

			//Check if animation is over
			AnimatorStateInfo anmState = anm.GetCurrentAnimatorStateInfo (0);
			if (anmState.IsName ("Dive")) {
				if (anmState.normalizedTime >= 1f) { //NormalizedTime returns the current state time / the animation length, so if this is > 1, the animation is over
					//Change state to ragdoll
					state = "ragdoll";
					recoverCount = recoverTime;

					//Enable Ragdoll
					RagdollSetActive (true);

				} else if (anmState.normalizedTime >= 0.5f) {
					//Move controller forward
					controller.SimpleMove (this.transform.forward * maxSpeed * 1.2f);
					//Play Sound
					if (audioSource.clip == null) {
						audioSource.clip = NPCAudioClips.GetClip ("guardDiveClips");
						audioSource.Play ();
					}
				} else {
					//Move and rotate controller just like the 'chase' state when normalized time < 0.5f
					Chase (maxSpeed);
				}
			} else {
				//The animtor is transitioning to the dice state, but we want the controller to keep moving towards the target
				Chase (maxSpeed);
			}

			//Set Animation state
			anm.SetInteger("state", 1);

			break;
			////////////////////////////////////////////////

			//Ragdolling and falling to the ground, and staying on the ground for a specified ammount of time
		case "ragdoll":
			////////////////////////////////////////////////

			//Set Animation state
			anm.SetInteger ("state", 2);

			//Count down recover time
			recoverCount -= Time.deltaTime;

			//Check if recovered
			if (recoverCount <= 0f) {
				//Deactivee the ragdolls
				RagdollSetActive(false);

				//Change state
				state = "chase";

			}

			break;
			////////////////////////////////////////////////

			//During this state, the AI tries to get in the targets way, instead of running straight towards it
		case "face":
			////////////////////////////////////////////////

			//Face target
			this.transform.LookAt (new Vector3 (target.position.x, this.transform.position.y, target.position.z)); //We use this.transform.position.y so we don't tilt up or down, and stay aligned to the ground
			//Move in the way of the target
			if (facingAngle > 10f) {
				//Move Left
				controller.SimpleMove (this.transform.right * -sideStepSpeed);
				anm.SetFloat ("playbackSpeed", 1f);
			} else if (facingAngle < -10f) {
				//Move Right
				controller.SimpleMove (this.transform.right * sideStepSpeed);
				anm.SetFloat ("playbackSpeed", -1f); //So the animation plays in reverse
			} else {
				anm.SetFloat ("playbackSpeed", 0f);
			}

			//Change state 
			if ((distanceToTarget > faceDistance + 10f) || (facingAngle > 90f) || (facingAngle < -90f)) {
				state = "chase";
				anm.SetFloat ("playbackSpeed", 1f);
			}
			
			//Set Animation state
			anm.SetInteger ("state", 3);

			break;
			////////////////////////////////////////////////

		}
	}

	private void Chase(float speed){
		//Chase and Steer towards target
		Vector3 move = NPC.ChaseForce (this.transform.position, target.position, speed, steeringSpd, controller.velocity * Time.deltaTime);
			
		//Move controller
		controller.SimpleMove (move);

		//Rotate Object towards velocity
		if (controller.velocity.normalized != Vector3.zero) //If the character is no moving, we don't want to set their rotation to snap to (0, 0, 0);
			this.transform.rotation = Quaternion.LookRotation (controller.velocity.normalized);

	}

	private void RagdollSetActive(bool active){
		if (active) {
		//Enable
		
			//'Enable' rigidbodies to ragdoll
			foreach (Rigidbody rb in ragdollRBs) {
				rb.isKinematic = false;
				rb.velocity = controller.velocity * Time.deltaTime * 2f;
			}

			//Disable animator to let bones only be effected by the rigidbodies
			anm.enabled = false;

		} else {
		//Disable

			//'Disable' rigidbodies
			foreach (Rigidbody rb in ragdollRBs) {
				rb.isKinematic = true;
			}

			//Enable animator
			anm.enabled = true;

			//Translate the Guard parent of the rigidbodies to the position of the 'main' bone
			Vector3 bonePos = ragdollRBs[0].gameObject.transform.position; //We do this because when the rigidbodies are active (not kinematic), the children objects will translate away from the main parent object,
			this.transform.position = new Vector3 (bonePos.x, this.transform.position.y, bonePos.z); //while the main parent object stays in place. So when we start using the animator again, the rendering mesh will
			//will seem to teleport back to the parent object, instead of where the rigidbodies appeared to stop on the ground.
			//That explanation may sound confusing, but you can comment out these lines to see what I'm on about.

		}
	}

}
