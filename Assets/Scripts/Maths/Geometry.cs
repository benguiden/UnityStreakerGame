using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geometry : MonoBehaviour {
	//This class will house some of the static mathematical functions we need for the AI's

	//This function will return the angle between the target position and the current object's FACING (localEularAngle.y) position
	//This will be useful for FOV detection, or to see if the player is facing any NPCs
	public static float FacingAngle(Transform thisTrans, Transform targetTrans){
		Vector2 thisPos = new Vector2 (thisTrans.position.x, thisTrans.position.z);
		Vector2 targetPos = new Vector2 (targetTrans.position.x, targetTrans.position.z);
		float theta = Mathf.Atan2 (targetPos.y - thisPos.y, targetPos.x - thisPos.x) * Mathf.Rad2Deg;
		theta = theta + thisTrans.localEulerAngles.y - 90f;

		while (theta > 180f) //This is like modulus but it puts it in the range (-180, 180)
			theta -= 360f; //We do this because regular modulus does not put the number in the range of a negative number
		while (theta < -180f)
			theta += 360f;

		//Returns a float between -180f -> 180f
		return theta;
	}
}
