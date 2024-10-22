using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using static GameController;

public class GameUI : AnimatedUI {

	private const string HIT_BUTTON_CHANGE = "HitButtonChange";
	private const string TUTORIAL_OVERLAY = "TutorialOverlay";
	//private const string CURRENT_HIT_BUTTON = "CurrentHitButton";


	[SerializeField] private GameInput gameInput;
	[SerializeField] private GameObject currentHitButton;
	[SerializeField] private GameController gameController;

	private string previousKey;

	private TMP_Text currentHitButtonText;
	private AnimatedUIElement hitButtonChange;
	private TMP_Text hitButtonChangeText;
	private float hitButtonChangeVisibleDuration = 1f;

	private void Awake() {

		gameController.OnGameStart += GameController_OnGameStart;
		gameController.OnRebind += GameController_OnRebind;
		GetUIAnimator().OnAnimationFinished += UIAnimator_OnAnimatorFinished;
	}
	private void Start() {
		currentHitButtonText = currentHitButton.transform.Find("CurrentHitButtonText").GetComponent<TMP_Text>();
		hitButtonChange = GetUIElementByName(HIT_BUTTON_CHANGE);
		hitButtonChangeText = hitButtonChange.transform.Find("HitButtonChangeText").GetComponent<TMP_Text>();
		previousKey = "R.Ctrl";
	}

	private void GameController_OnRebind(object sender, OnRebindEventArgs e) {
		Debug.Log("GameInput_OnRebind");
		string currentKey = Char.ToString(e.key).ToUpper();
		currentHitButtonText.text = currentKey;
		hitButtonChangeText.text = previousKey + " > " + currentKey;
		previousKey = currentKey;
		hitButtonChange.GetComponent<CanvasGroup>().alpha = 1f;
		StartCoroutine(AnimateUICoroutine(new List<AnimatedUIElement> { GetUIElementByName(HIT_BUTTON_CHANGE) }, hitButtonChangeVisibleDuration));
	}

	private void GameController_OnGameStart(object sender, EventArgs e) {
		AnimateUI(new List<AnimatedUIElement> { GetUIElementByName(TUTORIAL_OVERLAY) });
	}

	private void Update() {

	}

	private void UIAnimator_OnAnimatorFinished(object sender, UIAnimator.OnAnimationFinishedEventArgs e) {
		if (e.animatedUIElement.name == HIT_BUTTON_CHANGE) {
			e.animatedUIElement.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		}
	}



	private IEnumerator AnimateUICoroutine(List<AnimatedUIElement> animatedUIElements,float time) {
		yield return new WaitForSeconds(time);
		AnimateUI(animatedUIElements);
	}

	


}