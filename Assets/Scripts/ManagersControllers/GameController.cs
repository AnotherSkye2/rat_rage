using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public event EventHandler OnGameStart;
	public event EventHandler<OnGamePausedEventArgs> OnGamePaused;

	public class OnGamePausedEventArgs : EventArgs {
		public bool isPaused;
	}

	public event EventHandler OnGameEnd;

	public event EventHandler<OnRebindEventArgs> OnRebind;
	public class OnRebindEventArgs : EventArgs {
		public char key;
	}

	public enum State {
		WaitingForStart,
		Playing,
		Paused,
		End,
		Default,
	}


	[SerializeField] private float minimumRebindTimerRange, maximumRebindTimerRange;
	[SerializeField] private float gameTime;
	[SerializeField] private ScoreManager scoreManager;
	[SerializeField] private GameUI gameUI;
	[SerializeField] private PlayerController player;
	[SerializeField] public  Material destroyedMaterial;


	private KeyCode hitButton;
	private FunctionLooper rebindLooper;
	private FunctionTimer gameFunctionTimer;
	private int score;
	private State state;
	private State stateBeforePause;

	private void Awake() {
		gameUI.OnPauseMenuButtonPressed += GameUI_OnPauseMenuButtonPressed;
		player.OnFurnitureDestroyed += Player_OnFurnitureDestroyed;
	}


	private void GameUI_OnPauseMenuButtonPressed(object sender, GameUI.OnPauseMenuButtonPressedEventArgs e) {
		Time.timeScale = 1.0f;
		OnGameEnd?.Invoke(this, EventArgs.Empty);
		switch (e.buttonUIString) {
			case "RestartButton":
				SceneLoader.Load(SceneLoader.Scene.Destruction);
				break;
			case "MainMenuButton":
				SceneLoader.Load(SceneLoader.Scene.Start);
				break;
			default:
				break;

		}
	}

	private void Start() {
		GameInput.Instance.OnAnyKeyPressed += GameInput_OnAnyKeyPressed; 
		GameInput.Instance.OnWrongKeyPressed += GameInput_OnWrongKeyPressed;
		GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
		state = State.WaitingForStart;
		stateBeforePause = state;
		rebindLooper = new FunctionLooper(Rebind, UnityEngine.Random.Range(minimumRebindTimerRange, maximumRebindTimerRange));
		SoundManager.Initialize();
		SoundManager.LoopSound(SoundManager.Sound.Music);
	}
	private void Update() {
		scoreManager.SetScore(score);
		rebindLooper.Update();
	}

	private void GameInput_OnPauseAction(object sender, EventArgs e) {
		if (state != State.Paused) {
			stateBeforePause = state;
		}
		PauseGame();
	}

	private void Player_OnFurnitureDestroyed(object sender, PlayerController.OnFurnitureDestroyedEventArgs e) {
		score += e.scoreValue;
	}


	private void GameInput_OnAnyKeyPressed(object sender, EventArgs e) {
		if (state == State.WaitingForStart) {
			state = State.Playing;
			OnGameStart?.Invoke(this, EventArgs.Empty);
			gameFunctionTimer = FunctionTimer.Create(EndGame, gameTime);
		}
	}
	private void GameInput_OnWrongKeyPressed(object sender, EventArgs e) {
		SoundManager.PlaySound(SoundManager.Sound.WrongHitButtonPress);
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
		state = State.End;
		OnGameEnd?.Invoke(this, EventArgs.Empty);
		SceneLoader.Load(SceneLoader.Scene.End);
	}

	private void PauseGame() {
		if (state != State.Paused) {
			state = State.Paused;
			Time.timeScale = 0f;
		} else {
			state = stateBeforePause;
			Time.timeScale = 1f;
		}
		OnGamePaused?.Invoke(this, new OnGamePausedEventArgs { isPaused = state == State.Paused});
	}


	public int GetScore() {
		return score;
	}
	
	public State GetState() {
		return state;
	}
	public float GetGameTime() {		
		return gameTime;
	}

	public float GetGameTimeRemaining() {
		return gameFunctionTimer.GetTime();
	}
}
