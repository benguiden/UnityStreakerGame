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
	public AudioClip[] footstepClips;

    public static List<GameObject> _NPCs = new List<GameObject>();

	public static bool playerCaught = false;

	[Header("Repelling Force")]
	public float repellingDistance;

    private static AudioClip[] _fumbleClips;
    private static AudioClip[] _guardDiveClips;
	private static AudioClip[] _footstepClips;

    void Start()
    {
        //Set static arrays to equal the instance's arrays
        //This way we can set up the arrays in the inspector
        _fumbleClips = fumbleClips;
        _guardDiveClips = guardDiveClips;
		_footstepClips = footstepClips;

		//Empty NPC List if scene has been reloaded, due to the list being static,
		//and the gameObjects in that list would of been destroyed
		_NPCs.Clear();

        //Get all NPC Game Objects
        _NPCs.AddRange(GameObject.FindGameObjectsWithTag("NPC"));

    }

	void Update(){
		
		/*/Repel other NPCs
		if (repellingDistance > 0f) {
			int length = _NPCs.Capacity;
			for (int i = 0; i < length; i++) {
				for (int j = length - 1; j > i; j--) {
					Transform objA = _NPCs [i].transform;
					Transform objB = _NPCs [j].transform;
					if (Vector3.Distance (objA.position, objB.position) < repellingDistance) {
						Vector3 force = Repel (objA.position, objB.position, repellingDistance);
						objA.Translate (force * Time.deltaTime);
						objB.Translate (-force * Time.deltaTime);
					}
				}
			}
		}*/


	}

	public static void PlayerCaught(){
		playerCaught = true;

		//Change AI target to the player's mech
		Transform newTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ragdollBody;
		foreach (GameObject guard in _NPCs) {
			GuardAI ai = guard.GetComponent<GuardAI> ();
			ai.target = newTarget;
		}
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
			case "footstepClips":
				return _footstepClips[Random.Range(0, _footstepClips.Length - 1)];
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
	public static Vector3 Repel(Vector3 _a, Vector3 _b, float k){
		Vector2 a = new Vector2 (_a.x, _a.z); //_a as a vector2 using the X & Z axis, because we don't want to repel the NPCs on the Y-axis
		Vector2 b = new Vector2 (_b.x, _b.z);//_b as a vector2 using the X & Z axis
		float mag = Vector2.Distance (a, b);
		Vector2 dir = (a - b).normalized;
		Vector2 force = (k / mag) * dir;
		return new Vector3 (force.x, 0f, force.y); // Return a Vector3 instead of a Vector2 because it's
												   //easier to add to the CharacterController's velocity
	}

}