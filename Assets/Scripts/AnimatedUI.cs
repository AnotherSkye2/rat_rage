using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IHasAnimatedUI;

public class AnimatedUI : MonoBehaviour, IHasAnimatedUI {

	public event EventHandler<OnAnimationTriggerEventArgs> OnAnimationTrigger;


	[SerializeField] private List<AnimatedUIElement> uIElements;
	[SerializeField] private UIAnimator uIAnimator;

	public UIAnimator GetUIAnimator() { return uIAnimator; }	
	public List<AnimatedUIElement> GetAllUIElements() { return uIElements; }

	public AnimatedUIElement GetUIElementByName(string name) {
		foreach (var uIElement in uIElements) {
			if (uIElement.name == name) return uIElement ;
		}
		Debug.LogError("No uIElement with name: " + name);
		return null;
	}

	public void AnimateUI(List<AnimatedUIElement> animatedUIElements) {
		OnAnimationTrigger?.Invoke(this, new IHasAnimatedUI.OnAnimationTriggerEventArgs {
			uIElements = animatedUIElements
		});
	}

}
