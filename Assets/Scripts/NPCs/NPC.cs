using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
	//This class will house different static functions that most AIs will use, since each AI will behave
	//differently but still use these basic functions, we can have each AI have their own script and state machine
	//but use static functions from here

	//Seek Function
	public static Vector3 ChaseForce(Vector3 source, Vector3 target, float moveSpd, float steeringSpd, Vector3 currentVelocity){
		Vector3 dirToTarget = Vector3.Normalize (target - source);
		Vector3 velToTarget = moveSpd * dirToTarget; //The steering speed determines how quickly the rigidbody gets to it's desired directional velocity.
		return steeringSpd * (velToTarget - currentVelocity); //If the steering speed is under 1, the rigidbody is most likely to overshoot it's target.
	}
}
