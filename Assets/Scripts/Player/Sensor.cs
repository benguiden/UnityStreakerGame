using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour {

	Transform cam; //The transfrom the sensor will point at to block the view

	void Start(){
		cam = Camera.main.transform;
	}

	void Update(){
		this.transform.LookAt (cam);
	}

}
