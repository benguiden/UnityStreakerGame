using System.Collections;
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

	private float speed, acceleration;

	//Objects
	private CharacterController controller;
	private Transform target;

	void Start(){
		//Variables
		acceleration = maxSpeed / accelTime; //This is similar to calculating speed using distance/time

		//Objects
		controller = this.GetComponent<CharacterController>();
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update(){

		//Accelerate
		if (speed < maxSpeed) {
			speed += acceleration * Time.deltaTime;
		} else if (speed > maxSpeed) {
			speed = maxSpeed; //Stop Accelerating
		}

		//Chase and Steer towards target
		Vector3 move = NPC.ChaseForce(this.transform.position, target.position, speed, steeringSpd, controller.velocity * Time.deltaTime);

		//Move controller
		controller.SimpleMove (move);

		//Rotate Object towards velocity
		if (controller.velocity.normalized != Vector3.zero) //If the character is no moving, we don't want to set their rotation to snap to (0, 0, 0);
			this.transform.rotation = Quaternion.LookRotation(controller.velocity.normalized);

	}

}
