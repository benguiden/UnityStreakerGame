using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

	[Tooltip("The change in score per second.")]
	public float scoreSpeed = 0f;
	[Tooltip("The change in scoreSpeed per second.")]
	public float scoreAcceleration = 0f;
	[Tooltip("The change in difficulty per second.")]
	public float difficultySpeed = 0f;
	[Tooltip("The change in difficultySpeed per difficulty increase.")]
	public float difficultyAcceleration = 0f;

	public static int score = 0;
	public static int difficulty = 1;

	private float _score = 0f;
	private float difficultyTime = 0f;

	void Start(){
		score = 0;
		difficulty = 1;

		difficultyTime = 1f / difficultySpeed;
	}

	void Update(){

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

	}

}
