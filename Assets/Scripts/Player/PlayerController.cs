using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

	[Tooltip("Maximum running velocity of the rigidbody.")]
	public float maxVelocity;
	[Tooltip("The time it takes to accelerate to maximum running velocity from 0m/s.")]
	public float accelTime;
	[Tooltip("The time it takes to deaccelerate to 0m/s from maximum running velocity.")]
	public float deaccelTime;

	private float acceleration, deacceleration;
	private Rigidbody rb;

	void Start(){

		acceleration = maxVelocity / accelTime;     //This is similar to calculating speed using distance/time, setting the desired velocity and
		deacceleration = maxVelocity / deaccelTime; //time is a lot easier to understand and predict, than setting the actual acceleration

		rb = this.GetComponent<Rigidbody> ();
	}

	void Update(){
		Debug.Log (rb.velocity);

	}

	void FixedUpdate(){
		if (Input.GetKey (KeyCode.W)) {
			if (rb.velocity.magnitude < maxVelocity) {
				rb.AddForce (Vector3.forward * acceleration * Time.fixedDeltaTime, ForceMode.VelocityChange);
			}
		} else {
			if (rb.velocity.magnitude > 0f) {
				rb.AddForce (-rb.velocity * deacceleration * Time.fixedDeltaTime, ForceMode.VelocityChange);
			}
		}
	}
		
}
