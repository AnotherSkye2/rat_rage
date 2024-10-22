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
		quitButton.onClick.AddListener(() => {
			Debug.Log("Quit!");
			Application.Quit();
		});
		againButton.onClick.AddListener(() => {
			SceneLoader.Load(SceneLoader.Scene.Destruction);
		});
	}

	private void VideoManager_OnVideoStarted(object sender, EventArgs e) {
		AnimateUI(new List<AnimatedUIElement>  {GetUIElementByName(VIDEO_SCREEN)} );
	}

	private void Start() {
		OnPlay?.Invoke(this, EventArgs.Empty);
		Debug.Log("OnPlay");
	}

	private void VideoManager_OnVideoFinished(object sender, EventArgs e) {
		AnimateUI(new List<AnimatedUIElement> { GetUIElementByName(SCORE_DISPLAY), GetUIElementByName(GAME_OVER_BUTTONS) });
	}



	//private void UIAnimator_OnAnimatorFinished(object sender, UIAnimator.OnAnimationFinishedEventArgs e) {
	//	throw new NotImplementedException();
	//}



}
