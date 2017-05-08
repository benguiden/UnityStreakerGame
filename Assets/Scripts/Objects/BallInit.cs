using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInit : MonoBehaviour {

	//This is ran when the ball spawns in so that the ball moves towards the center
	public void Init(float velo){
		//Set the position to be 90 forom the center with some random angle
		this.transform.position = new Vector3 (90f, 30f, 0f);
		this.transform.RotateAround (new Vector3 (), Vector3.up, Random.Range (0f, 360f));

		//Set the balls velocity so that it travels towards the center
		Vector3 newVelo = -this.transform.position.normalized * velo;
		this.GetComponent<Rigidbody> ().velocity = new Vector3(newVelo.x, -newVelo.y/2f, newVelo.z); //Set the y velo to negative so that it moves upwards initially

	}

}
