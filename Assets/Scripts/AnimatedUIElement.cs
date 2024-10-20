using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimatedUIElement : MonoBehaviour {

	[SerializeField] private AnimatedUIElementSO animatedUIElementSO;

	public AnimatedUIElementSO GetAnimatedUIElementSO() {
		return animatedUIElementSO;
	}


}
