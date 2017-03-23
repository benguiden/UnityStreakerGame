using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

	//This script rotates the game object's transform with the 'Mouse X' and the 'Mouse Y' axis inputs.

	public Vector2 rotateSpd;
	public float minAngle, maxAngle;

	void Update(){

		//Rotate transform with respect to 'Mouse X' and 'Mouse Y'
		float deltaY = Input.GetAxis("Mouse X") * rotateSpd.x * Time.deltaTime;
		float deltaX = Input.GetAxis("Mouse Y") * rotateSpd.y * Time.deltaTime;
		this.transform.Rotate (new Vector3 (deltaX, deltaY, 0f));

		//Clamp the local X eular angle from minAngle to maxAngle, so we don't rotate the camera above the head or under the feet
		Vector3 localAngle = this.transform.localEulerAngles;

		localAngle.x = localAngle.x % 360f; //This will put the localAngle.x in the range -180 -> 180, we must do this before clamping the angle
		if (localAngle.x > 180f)			//as transform.localEularAngles can wrap the angles into different ranges we can't work with
			localAngle.x -= 360f;


		localAngle.x = Mathf.Clamp (localAngle.x, minAngle, maxAngle);

		//We have to constrain the Z rotation to 0f due to floating point inaccuracy when we rotate, and overtime will veer off 0f
		this.transform.localEulerAngles = new Vector3 (localAngle.x, localAngle.y, 0f);

	}

}
