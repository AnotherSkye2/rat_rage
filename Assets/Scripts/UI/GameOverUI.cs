using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Video;
using System.Linq;

public class GameOverUI : AnimatedUI, IHasVideo {

	private const string VIDEO_SCREEN = "VideoScreen";
	private const string SCORE_DISPLAY = "ScoreDisplay";
	private const string GAME_OVER_BUTTONS = "GameOverButtons";


	public event EventHandler OnPlay;

	[SerializeField] private VideoManager videoManager;
	[SerializeField] private Button quitButton;
	[SerializeField] private Button againButton;

	private void Awake() {
		videoManager.OnVideoFinished += VideoManager_OnVideoFinished;
		videoManager.OnVideoStarted += VideoManager_OnVideoStarted;

	}


	private void VideoManager_OnVideoStarted(object sender, EventArgs e) {
		AnimateUI(new List<AnimatedUIElement>  {GetUIElementByName(VIDEO_SCREEN)} );
	}

	private void Start() {
		OnPlay?.Invoke(this, EventArgs.Empty);
		//Debug.Log("OnPlay");
		ButtonUI quitButtonUI = quitButton.gameObject.GetComponent<ButtonUI>();
		quitButtonUI.OnAudioClipFinished += QuitButtonUI_OnAudioClipFinished;
		
		ButtonUI againButtonUI = againButton.gameObject.GetComponent<ButtonUI>();
		againButtonUI.OnAudioClipFinished += AgainButtonUI_OnAudioClipFinished;

	}

	private void AgainButtonUI_OnAudioClipFinished(object sender, EventArgs e) {
		SceneLoader.Load(SceneLoader.Scene.Destruction);

	}

	private void QuitButtonUI_OnAudioClipFinished(object sender, EventArgs e) {
		Debug.Log("Quit!");
		Application.Quit();
	}

	private void VideoManager_OnVideoFinished(object sender, EventArgs e) {
		videoManager.OnVideoFinished -= VideoManager_OnVideoFinished;
		videoManager.OnVideoStarted -= VideoManager_OnVideoStarted;
		AnimateUI(new List<AnimatedUIElement> { GetUIElementByName(SCORE_DISPLAY)});
		FunctionTimer.Create(AnimateGameOverButtons, 2.5f);
	}

	private void AnimateGameOverButtons() {
		AnimateUI(new List<AnimatedUIElement> { GetUIElementByName(GAME_OVER_BUTTONS) });
	}

}
