using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVCamera : MonoBehaviour {

    [Tooltip("The smoothness of the camera rotation as it follows the player. 1 being too smooth that the camera doesn't actually rotate.")]
    [Range(0f, 1f)]
    public float smoothness;

    private Transform target;

    void Start(){
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update(){
        //Look at the target
        this.transform.LookAt(Vector3.Lerp(this.transform.forward, target.position, 1f - smoothness));
    }
}
