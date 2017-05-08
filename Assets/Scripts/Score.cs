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
	[Tooltip("The guard prefab.")]
	public Object guardPrefab;
	[Tooltip("The guard parent.")]
	public Transform guardParent;
	[Tooltip("The don prefab.")]
	public Object donPrefab;
	[Tooltip("The don parent.")]
	public Transform donParent;
	[Tooltip("The ball prefab.")]
	public Object ballPrefab;
	[Tooltip("The ball parent.")]
	public Transform ballParent;

	public static int score = 0;
	public static int difficulty = 1;

	private bool gameStarted = false;
	private float _score = 0f;
	private float difficultyTime = 0f;

	private GameObject[] spawnPoints;

	void Start(){
		score = 0;
		difficulty = 1;

		difficultyTime = 1f / difficultySpeed;

		spawnPoints = GameObject.FindGameObjectsWithTag ("Respawn");

	}

	void Update(){

		if ((gameStarted) && (NPC.playerCaught == false)) {
			
			scoreSpeed += Time.deltaTime * scoreAcceleration;

			_score += Time.deltaTime * scoreSpeed;

			score = (int)Mathf.Floor (_score);

			if (difficultyTime <= 0f) {
				IncreaseDifficulty ();
			}
			
			difficultyTime -= Time.deltaTime;
			Debug.Log ("Score: " + score.ToString () + " Difficulty: " + difficulty.ToString());
		} else {
			if (Input.GetKeyDown (KeyCode.Space))
				StartGame ();
		}
	}

	private void IncreaseDifficulty(){
		//Increase Difficulty
		difficulty++;
		difficultySpeed += difficultyAcceleration;
		difficultyTime += (1f / difficultySpeed);

		//Spawn NPC
		Transform newSpawn = spawnPoints[Random.Range(0, spawnPoints.Length-1)].transform;

		int ran = Random.Range(0, 2);

		if (ran == 0) {
			//Spawn Don with a 1 in 3 chance
			GameObject newNPC = (GameObject)Instantiate (donPrefab, newSpawn);
			newNPC.transform.parent = donParent;

			GuardAI ai = newNPC.GetComponent<GuardAI> ();
			ai.maxSpeed += 6f * ((float)difficulty / 20f);
			ai.sideStepSpeed += 1f * ((float)difficulty / 20f);
			ai.recoverTime -= 1f * ((float)difficulty / 20f);
			ai.steeringSpd = Mathf.Clamp01 (ai.steeringSpd + (1.5f * ((float)difficulty / 20f)));
		}else{
			//Spawn Gaurd with a 2 in 3 chance
			GameObject newNPC = (GameObject)Instantiate (guardPrefab, newSpawn);
			newNPC.transform.parent = guardParent;

			GuardAI ai = newNPC.GetComponent<GuardAI> ();
			ai.maxSpeed += 4f * ((float)difficulty / 20f);
			ai.sideStepSpeed += 2f * ((float)difficulty / 20f);
			ai.recoverTime -= 1.5f * ((float)difficulty / 20f);
			ai.steeringSpd = Mathf.Clamp01 (ai.steeringSpd + (2f * ((float)difficulty / 20f)));
		}

		//Spawn Ball
		GameObject newball = (GameObject)Instantiate (ballPrefab, newSpawn);
		newball.transform.parent = ballParent;
		newball.GetComponent<BallInit> ().Init (40f);

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

		//Crowd Cheering
		CrowdControl.intensity += 0.75f;

	}

}
