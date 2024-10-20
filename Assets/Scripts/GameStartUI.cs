using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameStartUI : AnimatedUI, IHasVideo, IHasAnimatedUI {

	private const string VIDEO_SCREEN = "VideoScreen";
	private const string START_UI_CANVAS = "StartUICanvas";

	public event EventHandler<IHasAnimatedUI.OnAnimationTriggerEventArgs> OnAnimationTrigger;

	public event EventHandler OnPlay;

	
	[SerializeField] private VideoManager videoManager;
	[SerializeField] private Canvas canvas;
	[SerializeField] private Button startButton;
	[SerializeField] private Button quitButton;

	private AnimatedUIElement videoScreen;

	private void Awake() {
		GetUIAnimator().OnAnimationFinished += UIAnimator_OnAnimationFinished;
		videoManager.OnVideoFinished += VideoManager_OnVideoFinished;
		startButton.onClick.AddListener(() => {
			Debug.Log("start!");
			SoundManager.PlaySound(SoundManager.Sound.UIClick);
			AnimateUI(new List<AnimatedUIElement> { GetUIElementByName(START_UI_CANVAS) });

		});
		quitButton.onClick.AddListener(() => {
			Application.Quit();
		});
	}

	private void Start() {
	}

	private void VideoManager_OnVideoFinished(object sender, EventArgs e) {
		SceneLoader.Load(SceneLoader.Scene.Destruction);
	}

	private void UIAnimator_OnAnimationFinished(object sender, UIAnimator.OnAnimationFinishedEventArgs e) {
		if (e.animatedUIElement.name == START_UI_CANVAS) {
			AnimateUI(new List<AnimatedUIElement> { GetUIElementByName(VIDEO_SCREEN) });
			OnPlay?.Invoke(this, EventArgs.Empty);
		}
	}

	private void AnimateUI(List<AnimatedUIElement> animatedUIElements) {
		Debug.Log("AnimateUI!");
		OnAnimationTrigger?.Invoke(this, new IHasAnimatedUI.OnAnimationTriggerEventArgs {
			uIElements = animatedUIElements
		});
	}

}