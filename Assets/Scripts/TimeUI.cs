using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour {
	//Change the timer on the UI

	private Text textUI;
	private RectTransform trans;

	void Start(){
		textUI = this.GetComponent<Text> ();
		trans = this.GetComponent<RectTransform> ();
		textUI.fontSize = (int)(Screen.height / 6f);
		trans.localPosition = new Vector3 (0f, (-Screen.height/2f)+(textUI.fontSize/2f), 0f);
	}

	void Update () {
		if (Score.gameStarted) {
			string timeStr = "";
			//Check if the seconds is less than 10, then add a 0 before the seconds
			if (Mathf.Floor (Score.time % 60f) >= 10) {
				timeStr = Mathf.Floor (Score.time / 60f).ToString () + ":" + Mathf.Floor (Score.time % 60f).ToString ();
			} else {
				timeStr = Mathf.Floor (Score.time / 60f).ToString () + ":0" + Mathf.Floor (Score.time % 60f).ToString ();
			}
			textUI.text = timeStr;
		}

	}
}
