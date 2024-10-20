using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour {

	[SerializeField] private GameController gameController;

	private TMP_Text Text;
	private float timerDuration;


	private void Awake() {
		Text = GetComponentInChildren<TMP_Text>();
	}

	private void Update() {
		timerDuration = gameController.GetGameTimeRemaining();
		Text.text = ((int)timerDuration).ToString();
	}

}
