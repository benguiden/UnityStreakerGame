using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

	//Variables
	[Tooltip("Maximum running velocity of the rigidbody.")]
	public float maxVelocity;
	[Tooltip("The time it takes to accelerate to maximum running velocity from 0m/s.")]
	public float accelTime;
	[Tooltip("The time it takes to deaccelerate to 0m/s from maximum running velocity.")]
	public float deaccelTime;
	[Range(0f, 1f)]
	[Tooltip("How fast the controller is at changing it's direction when running.")]
	public float steeringSpd;

	private float acceleration, deacceleration;

	//Objects
	[Tooltip("The transform with the direction for the controller to run in.")]
	public Transform cam;
	[Tooltip("The character model to rotate and animate.")]
	public GameObject characterModel;
	private Rigidbody rb;

	void Start(){

		//Variables
		acceleration = maxVelocity / accelTime;     //This is similar to calculating speed using distance/time, setting the desired velocity and
		deacceleration = maxVelocity / deaccelTime; //time is a lot easier to understand and predict, than setting the actual acceleration

		//Objects
		rb = this.GetComponent<Rigidbody> ();

	}

	void Update(){

		//Rotate
		if (rb.velocity.magnitude > 0.05f) {
			Vector3 angles = characterModel.transform.localEulerAngles;
			float yDiff = Mathf.LerpAngle (angles.y, cam.transform.localEulerAngles.y, 0.05f);
			characterModel.transform.localEulerAngles = new Vector3 (angles.x, yDiff, angles.z);
		}

	}

	void FixedUpdate(){
		if (Input.GetKey (KeyCode.W)) {
			Vector3 direction = new Vector3 (characterModel.transform.forward.x, 0f, characterModel.transform.forward.z);
			rb.AddForce (direction * acceleration * Time.fixedDeltaTime, ForceMode.VelocityChange);
			if (rb.velocity.magnitude > maxVelocity) {
				rb.velocity /= rb.velocity.magnitude / maxVelocity;
			}
			Debug.Log (rb.velocity.magnitude);
		} else {
			if (rb.velocity.magnitude > 0f) {
				rb.AddForce (-rb.velocity * deacceleration * Time.fixedDeltaTime, ForceMode.VelocityChange);
			}
		}
	}

	float vec2Angle(Vector2 vecIn){
		return Mathf.Atan2 (vecIn.y, vecIn.x);
	}
		
}
