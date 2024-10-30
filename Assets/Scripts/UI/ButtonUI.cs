using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class ButtonUI : MonoBehaviour {

	public event EventHandler OnAudioClipFinished;

	private void Awake() {
		Button buttonComponent = gameObject.GetComponent<Button>();
		buttonComponent.onClick.AddListener(() => {
			SoundManager.PlaySound(SoundManager.Sound.UIClick);
			StartCoroutine(WaitForAudioClipFinished());
		});
	}

	private IEnumerator WaitForAudioClipFinished() {
		yield return new WaitForSeconds(0.2f);
		OnAudioClipFinished?.Invoke(this, EventArgs.Empty);
	}

}
