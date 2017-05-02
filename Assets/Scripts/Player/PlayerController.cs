using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (CharacterController))]
public class PlayerController : MonoBehaviour {

	//Variables
	[Tooltip("Maximum running speed of the rigidbody.")]
	public float maxSpeed;
	[Tooltip("The time it takes to accelerate to maximum running speed from 0m/s.")]
	public float accelTime;
	[Tooltip("The time it takes to deaccelerate to 0m/s from maximum running speed.")]
	public float deaccelTime;
	[Range(0f, 1f)]
	[Tooltip("How fast the controller is at changing it's direction when running.")]
	public float steeringSpd;

	private float speed, acceleration, deacceleration;

	//Objects
	[Tooltip("The transform with the direction for the controller to run in.")]
	public Transform cam;
	[Tooltip("The character model to rotate and animate.")]
	public GameObject characterModel;

	private CharacterController controller;
	private Animator anm;

	void Start(){
		//Variables
		acceleration = maxSpeed / accelTime;     //This is similar to calculating speed using distance/time, setting the desired velocity and
		deacceleration = maxSpeed / deaccelTime; //time is a lot easier to understand and predict, than setting the actual acceleration

		//Objects
		controller = this.GetComponent<CharacterController>();
		anm = this.GetComponentInChildren<Animator> ();
	}

	void Update(){

		//Move
		if ((Input.GetAxisRaw ("Vertical") != 0) || (Input.GetAxisRaw ("Horizontal") != 0)) {

			//Set Speed
			if (speed < maxSpeed){
				speed += acceleration * Time.deltaTime; //Accelerate
			}else if (speed > maxSpeed) {
				speed = maxSpeed; //Stop Accelerating
			}
				
			//Takes the input and read it as an angle
			float inDir = Mathf.Atan(Input.GetAxisRaw ("Horizontal") / Input.GetAxisRaw ("Vertical")) * (180f / Mathf.PI);
			if (Input.GetAxisRaw ("Vertical") < 0f){
				inDir += 180f; //Requirement when using the Atan function
			}

			//Steer the character to the desired direction using steeringSpd, inputDirection and the camera direction
			Vector3 modelAngle = characterModel.transform.localEulerAngles;
			float yAngleShift = Mathf.LerpAngle (modelAngle.y, cam.transform.localEulerAngles.y + inDir, steeringSpd);
			characterModel.transform.localEulerAngles = new Vector3 (modelAngle.x, yAngleShift, modelAngle.z);

		} else {
			//Slow down to stop
			if (speed > 0f) {
				speed -= deacceleration * Time.deltaTime;
			} else if (speed < 0f) {
				speed = 0f;
			}
		}

		//Move the character (even when there is no input, so the character can deaccelerate instead of just stopping instantly)
		controller.SimpleMove (speed * characterModel.transform.forward.normalized);


		//Animation
		anm.SetFloat("speed", speed);

		//Set Speed of running animation
		AnimatorStateInfo nextState = anm.GetNextAnimatorStateInfo(0);
		if (nextState.IsName ("Run"))
			anm.speed = 0.5f + (speed / (2f * maxSpeed));

	} 

	void OnTriggerEnter(Collider c){
		if ((c.gameObject.tag == "NPC") || (c.gameObject.tag == "NPCLimb")) {
			//Ragdoll

		}
	}
		
}