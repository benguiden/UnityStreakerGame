using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
    //This class will house different static functions that most AIs will use, since each AI will behave
    //differently but still use these basic functions, we can have each AI have their own script and state machine
    //but use static functions from here

    //Instead of each instance of the Guard object storing AudioClip information and taking up memory
    //This class with static variables and functions will store that information
    public AudioClip[] fumbleClips;
    public AudioClip[] guardDiveClips;

    public static List<GameObject> _NPCs = new List<GameObject>();

    private static AudioClip[] _fumbleClips;
    private static AudioClip[] _guardDiveClips;

    void Start()
    {
        //Set static arrays to equal the instance's arrays
        //This way we can set up the arrays in the inspector
        _fumbleClips = fumbleClips;
        _guardDiveClips = guardDiveClips;

        //Get all NPC Game Objects
        _NPCs.AddRange(GameObject.FindGameObjectsWithTag("NPC"));

    }

    //Returns a random AudioClip from the specified array
    public static AudioClip GetClip(string arrayName)
    {
        switch (arrayName)
        {
            case "fumbleClips":
                return _fumbleClips[Random.Range(0, _fumbleClips.Length - 1)]; //We don't need 'break;' command because once return is called, the process on this function is destroyed
            case "guardDiveClips":
                return _guardDiveClips[Random.Range(0, _guardDiveClips.Length - 1)];
        }
        return null;
    }

    //Seek Function
    public static Vector3 ChaseForce(Vector3 source, Vector3 target, float moveSpd, float steeringSpd, Vector3 currentVelocity){
		Vector3 dirToTarget = Vector3.Normalize (target - source);
		Vector3 velToTarget = moveSpd * dirToTarget; //The steering speed determines how quickly the rigidbody gets to it's desired directional velocity.
		return steeringSpd * (velToTarget - currentVelocity); //If the steering speed is under 1, the rigidbody is most likely to overshoot it's target.
	}

    //Repel Function
    public static void Repel(GameObject thisObj, Transform repelTrans, float repelSpd){
        Vector3 difference = new Vector3(repelTrans.position.x - thisObj.transform.position.x, 0f, repelTrans.position.z - thisObj.transform.position.z);
        thisObj.GetComponent<CharacterController>().SimpleMove(repelSpd * -difference.normalized);
    }

}