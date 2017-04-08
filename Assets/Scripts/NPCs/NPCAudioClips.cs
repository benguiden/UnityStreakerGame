using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAudioClips : MonoBehaviour {
	//Instead of each instance of the Guard object storing AudioClip information and taking up memory
	//This class with static variables and functions will store that information
	public AudioClip[] fumbleClips;
	public AudioClip[] guardDiveClips;

	private static AudioClip[] _fumbleClips;
	private static AudioClip[] _guardDiveClips;

	void Start(){
		//Set static arrays to equal the instance's arrays
		//This way we can set up the arrays in the inspector
		_fumbleClips = fumbleClips;
		_guardDiveClips = guardDiveClips;
	}

	//Returns a random AudioClip from the specified array
	public static AudioClip GetClip(string arrayName){
		switch (arrayName) {
		case "fumbleClips":
			return _fumbleClips [Random.Range(0, _fumbleClips.Length-1)]; //We don't need 'break;' command because once return is called, the process on this function is destroyed
		case "guardDiveClips":
			return _guardDiveClips [Random.Range(0, _guardDiveClips.Length-1)];
		}
		return null;
	}
}
