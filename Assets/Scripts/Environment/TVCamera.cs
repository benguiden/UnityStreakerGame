using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVCamera : MonoBehaviour {

	[Tooltip("The Transform that the camera will follow")]
	public Transform target;

    [Tooltip("The smoothness of the camera rotation as it follows the player. 1 being too smooth that the camera doesn't actually rotate.")]
    [Range(0f, 1f)]
    public float smoothness;

	public float maxZoom = 30f;
	public float minZoom = 10f;

	private Camera cam;

	void Start(){
		cam = this.GetComponent<Camera> ();
	}

	void Update(){
        //Look at the target
		Quaternion currentRot = this.transform.localRotation;
		this.transform.LookAt (target.position);

		//Zoom in and out depending on the speed of the camera
		float newFov = Mathf.Clamp (minZoom + (currentRot.eulerAngles - this.transform.localEulerAngles).magnitude * ((maxZoom-minZoom) / 2f), minZoom, maxZoom);
		cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, newFov, 1f - smoothness);

		this.transform.localRotation = Quaternion.Lerp (currentRot, this.transform.rotation, 1f - smoothness);
    }
}
