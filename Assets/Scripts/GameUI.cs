using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text.RegularExpressions;
using static GameInput;
using System.Linq;

public class GameUI : AnimatedUI, IHasAnimatedUI {

	private const string HIT_BUTTON_CHANGE = "HitButtonChange";
	private const string TUTORIAL_CONTROLS_INFO = "TutorialControlsInfo";
	//private const string CURRENT_HIT_BUTTON = "CurrentHitButton";

	public event EventHandler<IHasAnimatedUI.OnAnimationTriggerEventArgs> OnAnimationTrigger;


	[SerializeField] private GameInput gameInput;
	[SerializeField] private GameObject currentHitButton;
	[SerializeField] private GameController gameController;

	private string previousKey;
	private TMP_Text currentHitButtonText;
	private AnimatedUIElement hitButtonChange;
	private TMP_Text hitButtonChangeText;

	private void Awake() {
		gameController.OnGameStart += GameController_OnGameStart;
		gameInput.OnRebind += GameInput_OnRebind;
		GetUIAnimator().OnAnimationFinished += UIAnimator_OnAnimatorFinished;
	}

	private void GameController_OnGameStart(object sender, EventArgs e) {
		OnAnimationTrigger?.Invoke(this, new IHasAnimatedUI.OnAnimationTriggerEventArgs {
			uIElements = new List<AnimatedUIElement> { GetUIElementByName(TUTORIAL_CONTROLS_INFO) }
		});
	}

	private void Start() {
		//Debug.Log(currentHitButton.transform.Find("CurrentHitButtonText").GetComponentInChildren<TMP_Text>());
		currentHitButtonText = currentHitButton.transform.Find("CurrentHitButtonText").GetComponent<TMP_Text>();
		hitButtonChange = GetUIElementByName(HIT_BUTTON_CHANGE);
		hitButtonChangeText = hitButtonChange.transform.Find("HitButtonChangeText").GetComponent<TMP_Text>();
		previousKey = "R.Ctrl";
	}


	private void UIAnimator_OnAnimatorFinished(object sender, UIAnimator.OnAnimationFinishedEventArgs e) {
		if (e.animatedUIElement.name == HIT_BUTTON_CHANGE) {
			e.animatedUIElement.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		}
	}

	private void GameInput_OnRebind(object sender, GameInput.OnRebindEventArgs e) {
		Debug.Log("GameInput_OnRebind");
		hitButtonChange.GetComponent<CanvasGroup>().alpha = 1f;
		string currentKey = Char.ToString(e.key).ToUpper();
		currentHitButtonText.text = currentKey;
		hitButtonChangeText.text = previousKey+" > "+currentKey;
		previousKey = currentKey;
		OnAnimationTrigger?.Invoke(this, new IHasAnimatedUI.OnAnimationTriggerEventArgs {
			uIElements = new List<AnimatedUIElement> {GetUIElementByName(HIT_BUTTON_CHANGE)}
		});
	}

}