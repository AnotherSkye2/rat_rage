using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameStartUI : AnimatedUI, IHasVideo {

	private const string VIDEO_SCREEN = "VideoScreen";
	private const string START_UI_CANVAS = "StartUICanvas";


	public event EventHandler OnPlay;

	
	[SerializeField] private VideoManager videoManager;
	[SerializeField] private Canvas canvas;
	[SerializeField] private GameObject startButtonImageWithStroke;
	[SerializeField] private Button startButton;
	[SerializeField] private Button quitButton;


	private void Start() {
		GetUIAnimator().OnAnimationFinished += UIAnimator_OnAnimationFinished;
		videoManager.OnVideoFinished += VideoManager_OnVideoFinished;

		ButtonUI startButtonUI = startButton.gameObject.GetComponent<ButtonUI>();
		startButtonUI.OnAudioClipFinished += StartButtonUI_OnAudioClipFinished;

		ButtonUI quitButtonUI = quitButton.gameObject.GetComponent<ButtonUI>();
		quitButtonUI.OnAudioClipFinished += QuitButtonUI_OnAudioClipFinished;

		InvokeRepeating("Blink", 1f, 1f);
	}

	private void StartButtonUI_OnAudioClipFinished(object sender, EventArgs e) {
		startButton.interactable = false;
		quitButton.interactable = false;
		AnimateUI(new List<AnimatedUIElement> { GetUIElementByName(START_UI_CANVAS) });
	}
	private void QuitButtonUI_OnAudioClipFinished(object sender, EventArgs e) {
		startButton.interactable = false;
		quitButton.interactable = false;
		Application.Quit();
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

	private void Blink() {
		startButtonImageWithStroke.SetActive(!startButtonImageWithStroke.activeInHierarchy);
	}



}