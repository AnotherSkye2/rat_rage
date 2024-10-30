using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using TMPro;
public class UIAnimator : MonoBehaviour {

	public event EventHandler<OnAnimationFinishedEventArgs> OnAnimationFinished;

	public class  OnAnimationFinishedEventArgs : EventArgs {
		public AnimatedUIElement animatedUIElement;
	}

	[SerializeField] private GameObject hasAnimatedUIGameObject;

	private const string MOVE = "Move"; 
	//private const string MOVE_SINGLE_AXIS = "MoveSingleAxis"; 
	private const string FADE = "Fade";

	private Sequence sequence;
	private IHasAnimatedUI hasAnimatedUI;
	private RectTransform animatedUIElementTransform;
	private AnimatedUIElementSO animatedUIElementSO;


	private void Start() {
		hasAnimatedUI = hasAnimatedUIGameObject.GetComponent<IHasAnimatedUI>();
		hasAnimatedUI.OnAnimationTrigger += HasAnimatedUI_OnAnimationTrigger;
	}

	private void HasAnimatedUI_OnAnimationTrigger(object sender, IHasAnimatedUI.OnAnimationTriggerEventArgs e) {
		Debug.Log("animate event!");
		Animate(e.uIElements);
	}

	IEnumerator EndOfTweenAnimation(Tween tween, AnimatedUIElement animatedUIElement) {
		yield return tween.WaitForCompletion();
		OnAnimationFinished?.Invoke(this, new OnAnimationFinishedEventArgs {
			animatedUIElement = animatedUIElement
		});
	}

	//IEnumerator EndOfSequenceAnimation(Tween tween) {
	//	yield return tween.WaitForCompletion();
	//	//OnAnimatorFinished?.Invoke(this, )
	//}


	private void Animate(List<AnimatedUIElement> animatedUIElements) {
		Debug.Log("animate!");
		sequence = DOTween.Sequence();
		foreach (AnimatedUIElement animatedUIElement in animatedUIElements) {
			Tween tween = null;
			Tween previousTween = null;

			animatedUIElementSO = animatedUIElement.GetAnimatedUIElementSO();
			animatedUIElementTransform = (RectTransform)animatedUIElement.transform;

			foreach (string uIAnimation in animatedUIElementSO.uIAnimations) { 
				previousTween = tween;
				switch (uIAnimation) {
					case MOVE:
						tween = MoveUI(new Vector2(animatedUIElementSO.toMovePosX,animatedUIElementSO.toMovePosY), animatedUIElementSO.tweenDuration);
						break;
					//case MOVE_SINGLE_AXIS:
					//	MoveUISingleAxis(sequence);
					//	break;
					case FADE:
						tween = FadeUI(animatedUIElement, animatedUIElementSO.fadeAlpha, animatedUIElementSO.tweenDuration);
						break;
					default:
						Debug.LogError("The name of \"UI Animation\" must be formatted correctly. The available animations are: Move,Fade");
						break;
				}
				if (previousTween != null) {
					sequence.Join(previousTween);
				}
			}
			StartCoroutine(EndOfTweenAnimation(tween, animatedUIElement));
		}

	}
	private Tween MoveUI(Vector2 moveVector, float tweenDuration) {
		Tween moveTween = animatedUIElementTransform.DOAnchorPos(moveVector, tweenDuration);
		moveTween.stringId = MOVE;
		return moveTween;
		
	}

	//private Tween MoveUISingleAxis(Sequence sequence) {
	//	if (animatedUIElementTransform.anchoredPosition.y != animatedUIElementSO.toMovePosY) {
	//		return animatedUIElementTransform.DOAnchorPosY(animatedUIElementSO.toMovePosY, animatedUIElementSO.tweenDuration);
	//	}
	//	if (animatedUIElementTransform.anchoredPosition.x != animatedUIElementSO.toMovePosX) {
	//		return animatedUIElementTransform.DOAnchorPosX(animatedUIElementSO.toMovePosX, animatedUIElementSO.tweenDuration);
	//	}
	//}

	private Tween FadeUI(AnimatedUIElement animatedUIElement, float fadealpha, float tweenDuration) {
		CanvasGroup canvasGroup = animatedUIElement.GetComponent<CanvasGroup>();
		Tween fadeTween = canvasGroup.DOFade(fadealpha,tweenDuration);
		fadeTween.stringId = FADE;
		return fadeTween;
	}

}
