using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public event EventHandler OnGameStart;
	public event EventHandler OnGameEnd;

	public event EventHandler<OnRebindEventArgs> OnRebind;
	public class OnRebindEventArgs : EventArgs {
		public char key;
	}

	private enum State {
		WaitingForStart,
		Playing,
		Paused,
		End,
	}


	[SerializeField] private float minimumRebindTimerRange, maximumRebindTimerRange;
	[SerializeField] private float gameDuration;
	[SerializeField] private GameInput gameInput;
	[SerializeField] private ScoreManager scoreManager;
	[SerializeField] private PlayerController player;
	[SerializeField] public  Material destroyedMaterial;


	private string previousKey;
	private KeyCode hitButton;
	private FunctionLooper rebindLooper;
	private FunctionTimer gameTimer;
	private int score;
	private State state;

	private void Awake() {
		state = State.WaitingForStart;
		player.OnFurnitureDestroyed += Player_OnFurnitureDestroyed;
		gameInput.OnAnyKeyPressed += GameInput_OnAnyKeyPressed;
	}

	private void Start() {
		rebindLooper = new FunctionLooper(Rebind, UnityEngine.Random.Range(minimumRebindTimerRange, maximumRebindTimerRange));
		SoundManager.Initialize();
		SoundManager.PlaySound(SoundManager.Sound.Music);
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

	private void Update() {
		scoreManager.SetScore(score);
		rebindLooper.Update();
	}


	private void Rebind() {
		char previousKey = (char)hitButton;
		char key = (char)(UnityEngine.Random.Range(0, 25) + 97);
		if (key != 'w' && key != 'a' && key != 's' && key != 'd' && key != previousKey) {
			SoundManager.PlaySound(SoundManager.Sound.HitButtonChange);
			hitButton = (KeyCode)key;
			OnRebind?.Invoke(this, new OnRebindEventArgs {
				key = key
			});
		}
		else {
			Rebind();
		}
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
		return gameDuration;
	}

}
