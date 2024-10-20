using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public event EventHandler OnGameStart;
	public event EventHandler OnGameEnd;

	private enum State {
		WaitingForStart,
		Playing,
		Paused,
		End,
	}


	[SerializeField] private float gameDuration;
	[SerializeField] private GameInput gameInput;
	[SerializeField] private ScoreManager scoreManager;
	[SerializeField] private PlayerController player;
	[SerializeField] public  Material destroyedMaterial;

	private AudioSource audioSource;
	private FunctionTimer gameTimer;
	private int score;
	private State state;

	private void Awake() {
		state = State.WaitingForStart;
		player.OnFurnitureDestroyed += Player_OnFurnitureDestroyed;
		gameInput.OnAnyKeyPressed += GameInput_OnAnyKeyPressed;
		gameInput.OnRebind += GameInput_OnRebind;
		audioSource = GetComponent<AudioSource>();
	}

	private void Start() {
		SoundManager.Initialize();
	}

	private void Player_OnFurnitureDestroyed(object sender, PlayerController.OnFurnitureDestroyedEventArgs e) {
		score += e.scoreValue;
	}


	private void GameInput_OnAnyKeyPressed(object sender, EventArgs e) {
		if (state == State.WaitingForStart) {
			state = State.Playing;
			OnGameStart?.Invoke(this, EventArgs.Empty);
			gameTimer = FunctionTimer.Create(EndGame, gameDuration);
		}
	}

	private void GameInput_OnRebind(object sender, GameInput.OnRebindEventArgs e) {
		audioSource.Play();
	}


	private void Update() {
		scoreManager.SetScore(score);
	}

	private void EndGame() {
		Debug.Log("Game ended!");
		scoreManager.SetScore(score);
		OnGameEnd?.Invoke(this, EventArgs.Empty);
		SceneLoader.Load(SceneLoader.Scene.End);
	}

	public int GetScore() {
		return score;
	}

	public float GetGameTimeRemaining() {
		if (state != State.WaitingForStart) {
			return gameTimer.GetTime();
		}
		return 60f;
	}

}
