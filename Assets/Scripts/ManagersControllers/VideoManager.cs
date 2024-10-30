using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;

public class VideoManager : MonoBehaviour {

	public event EventHandler OnVideoFinished;
	public event EventHandler OnVideoStarted;

	[SerializeField] private GameObject hasVideoGameObject;
	[SerializeField] private VideoPlayer videoPlayer;

	private IHasVideo hasVideo;
	private bool videoPrepared;
	private bool videoStarted;

	private void Awake() {
		videoPlayer.targetTexture.Release();
		videoPlayer.targetTexture.Create();
		Debug.Log(videoPlayer.frame);
		videoPlayer.Prepare();
		videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
		videoPlayer.started += VideoPlayer_started;
		hasVideo = hasVideoGameObject.GetComponent<IHasVideo>();
		if (hasVideo == null) {
			Debug.LogError("Game Object \"" + hasVideoGameObject + "\" does not have component that implements IHasVideo!");
		}

		hasVideo.OnPlay += HasVideo_OnPlay;
	}

	private void VideoPlayer_started(VideoPlayer source) {
		videoStarted = true;
		OnVideoStarted?.Invoke(this, EventArgs.Empty);
	}

	private void VideoPlayer_prepareCompleted(VideoPlayer source) {
		videoPrepared = true;
	}

	private void Start() {

	}

	private IEnumerator WaitforVideoPreapared() {
		yield return videoPrepared == true;
	}


	private void HasVideo_OnPlay(object sender, System.EventArgs e) {
		if (videoPrepared) {
			Debug.Log("play!");
			videoPlayer.Play();
		} else {
			Debug.Log("can't play!");
			WaitforVideoPreapared();
			videoPlayer.Play();
		}
	}

	private void Update() {
		if (!videoPrepared) {
			Debug.Log("Not prepared!");
		}
		if (videoStarted) {
			if (!videoPlayer.isPlaying) {
				OnVideoFinished?.Invoke(this, EventArgs.Empty);
				videoStarted = false;
			}
		}
	}

}
