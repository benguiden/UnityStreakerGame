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
		return theta + thisTrans.localEulerAngles.y - 90f;
	}
}
