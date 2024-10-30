using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour {

	[SerializeField] private GameController gameController;

	private TMP_Text Text;
	private float timerDuration;
	private int timerTextPaddingZeroesLength = 2;


	private void Awake() {
		Text = GetComponentInChildren<TMP_Text>();
	}

	private void Update() {
		timerDuration = gameController.GetGameTimeRemaining();
		//Debug.Log(timerDuration);
		string seconds = ((int)(timerDuration % 60)).ToString();
		if (timerDuration >= 60) {
			string minutes = ((int)(timerDuration / 60)).ToString();
			Text.text = minutes + ":" + new string('0', timerTextPaddingZeroesLength - seconds.Length) + seconds;
		} else {
			Text.text = ((int)(timerDuration)).ToString();
		}
	}

}
