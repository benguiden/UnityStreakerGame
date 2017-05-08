using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

	//Variables
	[Tooltip("The change in score per second.")]
	public float scoreSpeed = 0f;
	[Tooltip("The change in scoreSpeed per second.")]
	public float scoreAcceleration = 0f;
	[Tooltip("The change in difficulty per second.")]
	public float difficultySpeed = 0f;
	[Tooltip("The change in difficultySpeed per difficulty increase.")]
	public float difficultyAcceleration = 0f;

	//Objects
	[Tooltip("The player object.")] //We have to find this in the hierarchy because we cannot find it with a taag because it is deactivated
	public GameObject player;
	[Tooltip("The starting camera's game object.")]
	public GameObject startCamera;

	public static int score = 0;
	public static int difficulty = 1;

	private bool gameStarted = false;

	private float _score = 0f;
	private float difficultyTime = 0f;

	void Start(){
		score = 0;
		difficulty = 1;

		difficultyTime = 1f / difficultySpeed;
	}

	void Update(){

		if (gameStarted) {
			
			scoreSpeed += Time.deltaTime * scoreAcceleration;

			_score += Time.deltaTime * scoreSpeed;

			score = (int)Mathf.Floor (_score);

			if (difficultyTime <= 0f) {
				//Increase score
				difficulty++;
				difficultySpeed += difficultyAcceleration;
				difficultyTime += (1f / difficultySpeed);
			}
			
			difficultyTime -= Time.deltaTime;
			//Debug.Log ("Score: " + score.ToString () + " Difficulty: " + difficulty.ToString());
		} else {
			if (Input.GetKeyDown (KeyCode.Space))
				StartGame ();
		}
	}

	private void StartGame(){
		gameStarted = true;

		//Activate player
		player.SetActive (true);

		//Activate NPCs
		foreach (GameObject npc in NPC._NPCs) {
			npc.GetComponent<GuardAI> ().enabled = true;
			npc.GetComponentInChildren<Animator> ().enabled = true;
		}

		//Deactivate the camera
		startCamera.SetActive(false);

	}

}
