using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasAnimatedUI {

	public event EventHandler<OnAnimationTriggerEventArgs> OnAnimationTrigger;

	public class OnAnimationTriggerEventArgs : EventArgs {
		public List<AnimatedUIElement> uIElements;
	}

	public void AnimateUI(List<AnimatedUIElement> animatedUIElements) {
		
	}
}
