using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {

	//Objects
	[Tooltip("The velocity at which the transform rotates.")]
	public Vector3 spinVelocity;

	void Update(){
		this.transform.Rotate (spinVelocity * Time.deltaTime);
	}

}
