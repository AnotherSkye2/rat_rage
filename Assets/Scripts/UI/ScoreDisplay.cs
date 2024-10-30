using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour {

	private TMP_Text Text;
	private int score;
	private int scoreTextPaddingZeroesLength = 9;
	private string scoreText;


	private void Awake() {
		Text = GetComponentInChildren<TMP_Text>();
	}

	private void Update() {
		score = ScoreManager.score;
		scoreText = (score).ToString();
		Text.text = new string('0', scoreTextPaddingZeroesLength - scoreText.Length) + scoreText;
	}

}
