using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using static GameController;
using UnityEngine.UI;

public class GameUI : AnimatedUI {

	private const string HIT_BUTTON_CHANGE = "HitButtonChange";
	private const string TUTORIAL_OVERLAY = "TutorialOverlay";
	private const string SCORE_NOTIFICATION = "ScoreNotification";
	//private const string CURRENT_HIT_BUTTON = "CurrentHitButton";


	[SerializeField] private GameInput gameInput;
	[SerializeField] private GameObject currentHitButton;
	[SerializeField] private RectTransform scoreNotificationParent;
	[SerializeField] private Sprite currentHitButtonSmallButtonBackgroundSprite;
	[SerializeField] private GameController gameController;
	[SerializeField] private PlayerController player;

	private string previousKey;

	private TMP_Text currentHitButtonText;
	private AnimatedUIElement hitButtonChange;
	private AnimatedUIElement scoreNotification;
	private TMP_Text hitButtonChangeText;
	private float hitButtonChangeVisibleDuration = 1f;

	private void Awake() {
		player.OnFurnitureDestroyed += Player_OnFurnitureDestroyed;
		gameController.OnGameStart += GameController_OnGameStart;
		gameController.OnRebind += GameController_OnRebind;
		GetUIAnimator().OnAnimationFinished += UIAnimator_OnAnimatorFinished;
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