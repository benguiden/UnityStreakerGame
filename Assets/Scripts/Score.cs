using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour {

	//Variables
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

	public static float time = 0;
	public static int difficulty = 1;
	public static bool gameStarted = false;

	private float difficultyTime = 0f;
	private float restartTime = 1f;

	private GameObject[] spawnPoints;

	void Start(){

		time = 0f;
			
		difficulty = 1;

		gameStarted = false;

		difficultyTime = 1f / difficultySpeed;

		spawnPoints = GameObject.FindGameObjectsWithTag ("Respawn");

	}

	void Update(){

		if ((gameStarted) && (NPC.playerCaught == false)) {
			
			if (difficultyTime <= 0f) {
				IncreaseDifficulty ();
			}
			
			difficultyTime -= Time.deltaTime;

			time += Time.deltaTime;

		} else {
			//Start the game
			if (Input.GetKeyDown (KeyCode.Space))
				StartGame ();
			else if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.Quit ();
			}
		}

		if (NPC.playerCaught) {
			if (restartTime > 0f) {
				restartTime -= Time.deltaTime;
			} else if (Input.anyKeyDown) {
				SceneManager.LoadScene (0);
			}
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
