using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using static GameController;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : AnimatedUI {

	public event EventHandler<OnPauseMenuButtonPressedEventArgs> OnPauseMenuButtonPressed;

	public class OnPauseMenuButtonPressedEventArgs : EventArgs {
		public string buttonUIString;
	}

	private const string HIT_BUTTON_CHANGE = "HitButtonChange";
	private const string TUTORIAL_OVERLAY = "TutorialOverlay";
	private const string SCORE_NOTIFICATION = "ScoreNotification";
	private const string GAME_PAUSED_OVERLAY = "GamePausedOverlay";
	//private const string CURRENT_HIT_BUTTON = "CurrentHitButton";


	[SerializeField] private GameInput gameInput;
	[SerializeField] private GameController gameController;
	[SerializeField] private PlayerController player;
	[SerializeField] private GameObject currentHitButton;
	[SerializeField] private ButtonUI restartButtonUI;
	[SerializeField] private ButtonUI mainmMenuButtonUI;
	[SerializeField] private Transform gamePausedOverlay;
	[SerializeField] private RectTransform scoreNotificationParent;
	[SerializeField] private Sprite currentHitButtonSmallButtonBackgroundSprite;

	private string previousKey;
		
	private TMP_Text currentHitButtonText;
	private TMP_Text hitButtonChangeText;
	private AnimatedUIElement hitButtonChange;
	private AnimatedUIElement scoreNotification;
	private float hitButtonChangeVisibleDuration = 1f;

	private void Awake() {
		player.OnFurnitureDestroyed += Player_OnFurnitureDestroyed;
		gameController.OnGameStart += GameController_OnGameStart;
		gameController.OnGamePaused += GameController_OnGamePaused;
		gameController.OnGameEnd += GameController_OnGameEnd;
		gameController.OnRebind += GameController_OnRebind;
		GetUIAnimator().OnAnimationFinished += UIAnimator_OnAnimatorFinished;
		mainmMenuButtonUI.OnAudioClipFinished += MainmenuButton_OnAudioClipFinished;
		restartButtonUI.OnAudioClipFinished += RestartButton_OnAudioClipFinished;
	}

	private void GameController_OnGameEnd(object sender, EventArgs e) {
		player.OnFurnitureDestroyed -= Player_OnFurnitureDestroyed;
		gameController.OnGameStart -= GameController_OnGameStart;
		gameController.OnGamePaused -= GameController_OnGamePaused;
		gameController.OnGameEnd -= GameController_OnGameEnd;
		gameController.OnRebind -= GameController_OnRebind;
		GetUIAnimator().OnAnimationFinished -= UIAnimator_OnAnimatorFinished;
		mainmMenuButtonUI.OnAudioClipFinished -= MainmenuButton_OnAudioClipFinished;
		restartButtonUI.OnAudioClipFinished -= RestartButton_OnAudioClipFinished;
	}

	private void GameController_OnGamePaused(object sender, OnGamePausedEventArgs e) {
		Debug.Log(e.isPaused.ToString());
		if (gamePausedOverlay != null) {
			if (e.isPaused) {
				gamePausedOverlay.gameObject.SetActive(true);
			} else {
				gamePausedOverlay.gameObject.SetActive(false);
			}
		} else {
			Debug.Log("gamePausedOverlay is null!");
		}

	}

	private void Player_OnFurnitureDestroyed(object sender, PlayerController.OnFurnitureDestroyedEventArgs e) {
		AnimatedUIElement newScoreNotification = CreateScoreNotification(scoreNotification, scoreNotificationParent);
		Debug.Log(scoreNotification);
		Debug.Log(newScoreNotification);
		newScoreNotification.gameObject.GetComponentInChildren<TMP_Text>().text = "+" + e.scoreValue.ToString();
		AnimateUI(new List<AnimatedUIElement> { newScoreNotification});
	}

	private void Start() {
		scoreNotification = GetUIElementByName(SCORE_NOTIFICATION);
		currentHitButtonText = currentHitButton.transform.Find("CurrentHitButtonText").GetComponent<TMP_Text>();
		hitButtonChange = GetUIElementByName(HIT_BUTTON_CHANGE);
		hitButtonChangeText = hitButtonChange.transform.Find("HitButtonChangeText").GetComponent<TMP_Text>();
		previousKey = "R.Ctrl";
	}

	private void RestartButton_OnAudioClipFinished(object sender, EventArgs e) {
		OnPauseMenuButtonPressed?.Invoke(this, new OnPauseMenuButtonPressedEventArgs { 
			buttonUIString = "RestartButton"
		});
	}

	private void MainmenuButton_OnAudioClipFinished(object sender, EventArgs e) {
		OnPauseMenuButtonPressed?.Invoke(this, new OnPauseMenuButtonPressedEventArgs {
			buttonUIString = "MainMenuButton"
		});
	}

	private void GameController_OnRebind(object sender, OnRebindEventArgs e) {
		Debug.Log("GameInput_OnRebind");
		string currentKey = Char.ToString(e.key).ToUpper();
		currentHitButtonText.text = currentKey;

		Transform currentHitButtontEXTBackground = currentHitButton.transform.Find("CurrentHitButtonTextBackground");
		currentHitButtontEXTBackground.gameObject.GetComponent<Image>().sprite = currentHitButtonSmallButtonBackgroundSprite;
		currentHitButtontEXTBackground.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

		hitButtonChangeText.text = previousKey + " > " + currentKey;
		previousKey = currentKey;
		hitButtonChange.GetComponent<CanvasGroup>().alpha = 1f;
		StartCoroutine(AnimateUICoroutine(new List<AnimatedUIElement> { GetUIElementByName(HIT_BUTTON_CHANGE) }, hitButtonChangeVisibleDuration));
	}

	private void GameController_OnGameStart(object sender, EventArgs e) {
		SoundManager.PlaySound(SoundManager.Sound.UIClick);
		AnimateUI(new List<AnimatedUIElement> { GetUIElementByName(TUTORIAL_OVERLAY) });
	}


	private void UIAnimator_OnAnimatorFinished(object sender, UIAnimator.OnAnimationFinishedEventArgs e) {
		if (e.animatedUIElement.name == HIT_BUTTON_CHANGE) {
			e.animatedUIElement.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		}
	}

	private AnimatedUIElement CreateScoreNotification(AnimatedUIElement originalScoreNotification, RectTransform parent) {
		AnimatedUIElement newScoreNotification = Instantiate(originalScoreNotification, parent);
		return newScoreNotification;
	}

	private IEnumerator AnimateUICoroutine(List<AnimatedUIElement> animatedUIElements,float time) {
		yield return new WaitForSeconds(time);
		AnimateUI(animatedUIElements);
	}

	


}