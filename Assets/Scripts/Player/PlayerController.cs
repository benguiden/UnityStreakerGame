using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	[Tooltip("The amount of force the controller imposes on other rigidbodies.")]
	public float force = 10.0f;

	private float speed, acceleration, deacceleration;
	private bool isRagdoll = false;

	//Objects
	[Tooltip("The transform with the direction for the controller to run in.")]
	public Transform cam;
	[Tooltip("The character model to rotate and animate.")]
	public GameObject characterModel;
	[Tooltip("The transform that childs all other bones of the model that ragdoll.")]
	public Transform ragdollBody;
	[Tooltip("The TV Camera.")]
	public Camera tVCamera;

	private CharacterController controller;
	private Animator anm;
	private Rigidbody[] ragdollRBs;
	private Collider[] ragdollColliders;

	void Start(){
		//Variables
		acceleration = maxSpeed / accelTime;     //This is similar to calculating speed using distance/time, setting the desired velocity and
		deacceleration = maxSpeed / deaccelTime; //time is a lot easier to understand and predict, than setting the actual acceleration

		//Objects
		controller = this.GetComponent<CharacterController>();
		anm = this.GetComponentInChildren<Animator> ();
		ragdollRBs = this.GetComponentsInChildren<Rigidbody> ();
		ragdollColliders = this.GetComponentsInChildren<Collider> ();

		//Ignore triggering with the limbs of the character
		for (int i=0; i<ragdollRBs.Length; i++){
			Physics.IgnoreCollision (ragdollRBs [i].gameObject.GetComponent<Collider> (), this.GetComponent<CapsuleCollider> ());
		}

	}

	void Update(){

		if (Input.GetKeyDown(KeyCode.Escape)){
			SceneManager.LoadScene (0);
		}

		if (isRagdoll == false) {
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
				anm.SetFloat ("playbackSpeed", 0.5f + (speed / (2f * 9f)));
			
		}

	} 

	void OnTriggerEnter(Collider c){
		if (isRagdoll == false) {
			if (c.gameObject.tag == "NPC") {
				//Ragdoll
				RagdollSetActive (true);
				c.gameObject.GetComponent<GuardAI> ().HitPlayer ();
				NPC.PlayerCaught ();
			} else if (c.gameObject.tag == "NPCLimb") {
				//Ragdoll
				RagdollSetActive (true);
				Transform limb = c.gameObject.transform;

				//Find parent guard object of the limb
				while (limb.parent.tag != "NPC") {
					limb = limb.parent;
				}
				limb.parent.gameObject.GetComponent<GuardAI> ().HitPlayer ();

				NPC.PlayerCaught ();
			}
		}
	}

	private void RagdollSetActive(bool active){
		if (active) {
			//Enable
			isRagdoll = true;

			Transform[] boneTrans = this.GetComponentsInChildren<Transform> (); //Get All transformations from child limbs

			//'Enable' rigidbodies to ragdoll
			foreach (Rigidbody rb in ragdollRBs) {
				rb.isKinematic = false;
				rb.AddForce (controller.velocity, ForceMode.VelocityChange);
			}
			foreach (Collider c in ragdollColliders) {
				c.enabled = true;
			}

			//Disable animator to let bones only be effected by the rigidbodies
			anm.enabled = false;

			for (int i = 0; i < boneTrans.Length; i++) {
				this.GetComponentsInChildren<Transform> () [i] = boneTrans [i]; //Set All transformations from child limbs after disabling the animator
			}

			//Disable controller so there are not collisions will the capsule
			controller.enabled = false;

			//Change camera to TV camera
			cam.gameObject.SetActive (false);
			tVCamera.enabled = true;
			tVCamera.gameObject.GetComponent<AudioListener> ().enabled = true;

			//Crowd intensify
			CrowdControl.intensity += 0.5f;

		} else {
			//Disable
			isRagdoll = false;

			//'Disable' rigidbodies
			foreach (Rigidbody rb in ragdollRBs) {
				rb.isKinematic = true;
			}
			foreach (Collider c in ragdollColliders) {
				c.enabled = false;
			}

			//Enable animator
			anm.enabled = true;

			//Enable collisions will the capsule
			controller.enabled = true;

			//Translate the Player parent of the rigidbodies to the position of the 'main' bone
			Vector3 bonePos = ragdollRBs[0].gameObject.transform.position; //We do this because when the rigidbodies are active (not kinematic), the children objects will translate away from the main parent object,
			this.transform.position = new Vector3 (bonePos.x, this.transform.position.y, bonePos.z); //while the main parent object stays in place. So when we start using the animator again, the rendering mesh will
			//will seem to teleport back to the parent object, instead of where the rigidbodies appeared to stop on the ground.
			//That explanation may sound confusing, but you can comment out these lines to see what I'm on about.

		}
	}
		
	//Collide with other rigidbody objects like balls to add force to them, since the characterController doesn't do that by default
	//A similar example can be found at https://docs.unity3d.com/ScriptReference/CharacterController.OnControllerColliderHit.html
	void OnControllerColliderHit(ControllerColliderHit hit){
		if (hit.gameObject.tag == "Obstacle") {
			//Ragdoll
			RagdollSetActive (true);
			NPC.PlayerCaught ();
			//Play Sound
			AudioSource soundFX = hit.gameObject.GetComponent<AudioSource> ();
			if (soundFX != null) {
				soundFX.time = 0f;
				soundFX.Play ();
			}
		} else {
			Rigidbody otherRB = hit.collider.attachedRigidbody;

			if (otherRB == null || otherRB.isKinematic)
				return;

			if (hit.moveDirection.y < -0.3F)
				return;

			Vector3 pushDir = new Vector3 (hit.moveDirection.x, 0.25f, hit.moveDirection.z);
			otherRB.AddForce ((pushDir * force) / otherRB.mass, ForceMode.VelocityChange);
		}
	}


}