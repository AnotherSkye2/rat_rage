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
	private int numOfRoundedDecimals = 2;


	private void Awake() {
		Text = GetComponentInChildren<TMP_Text>();
		timerDuration = gameController.GetGameTime();
	}

	private void Update() {
		//Debug.Log(gameController);
		//Debug.Log(timerDuration);
		if (gameController.GetState() == GameController.State.Playing) { 
			timerDuration = gameController.GetGameTimeRemaining();
		}
		string seconds = ((int)(timerDuration % 60)).ToString();
		switch (timerDuration) {
			case < 1f:
				Debug.Log(Mathf.Pow(10, numOfRoundedDecimals));
				Text.text = ((Mathf.Round(timerDuration * (int)Mathf.Pow(10, numOfRoundedDecimals))) / Mathf.Pow(10, numOfRoundedDecimals)).ToString();
				break;
			case < 10f:
				GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1f);
				Text.color = Color.red;
				Text.text = seconds;
				break;
			case < 60f:
				Text.text = seconds;
				break;
			default:
				string minutes = ((int)(timerDuration / 60)).ToString();
				Text.text = minutes + ":" + new string('0', timerTextPaddingZeroesLength - seconds.Length) + seconds;
				break;
		}
	}

}
