using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager {


	private static float playerMoveDuration = 0.2f;

	public enum Sound {
		Music,
		UIClick,
		PlayerMove,
		FurnitureHit,
		HitButtonChange,
		WrongHitButtonPress,
		Timer,
	}

	private static Dictionary<Sound, float> soundTimerDictionary;

	public static void Initialize() {
		soundTimerDictionary = new Dictionary<Sound, float>();
		soundTimerDictionary[Sound.PlayerMove] = 0;
	}

	public static void PlaySound(Sound sound) {
		if (CanPlaySound(sound)) {
			GameObject soundGameObject = new GameObject("Sound");
			AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
			audioSource.PlayOneShot(GetAudioClip(sound));
		}
	}

	private static bool CanPlaySound(Sound sound) {
		switch (sound) {
		default:
			return true;
		case Sound.PlayerMove:
			if (soundTimerDictionary.ContainsKey(sound)) {
				float lastTimePlayed = soundTimerDictionary[sound];
				float playerMoveTimerMax = playerMoveDuration;
				if (lastTimePlayed + playerMoveTimerMax < Time.time) {
						soundTimerDictionary[sound] = Time.time;
					return true;
				} else {
					return false;
				}
			} else {
				return true;
			}
			//break;
		}
	}

	private static AudioClip GetAudioClip(Sound sound) {
		foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClip) {
			if (soundAudioClip.sound == sound) {
				return soundAudioClip.audioClip;
			}
		}
		Debug.LogError("Sound" + sound + "not found!");
		return null;
	}

}
