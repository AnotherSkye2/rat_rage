using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager {


	private static float playerMoveDuration = 0.2f;

	public enum Sound {
		Music,
		UIOver,
		UIClick,
		PlayerMove,
		Tool,
		Sahtlid,
		Printer,
		Metallkapp,
		Laud,
		HitButtonChange,
		WrongHitButtonPress,
		Timer,
	}

	private static Dictionary<Sound, float> soundTimerDictionary;
	private static Dictionary<string, Sound> furnitureTagSoundDictionary;
	private static GameObject oneShotGameObject;
	private static AudioSource oneShoutAudioSource;


	public static void Initialize() {
		soundTimerDictionary = new Dictionary<Sound, float>();
		soundTimerDictionary[Sound.PlayerMove] = 0;

		furnitureTagSoundDictionary = new Dictionary<string, Sound>();
		furnitureTagSoundDictionary["Tool"] = Sound.Tool;
		furnitureTagSoundDictionary["Sahtlid"] = Sound.Sahtlid;
		furnitureTagSoundDictionary["Printer"] = Sound.Printer;
		furnitureTagSoundDictionary["Metallkapp"] = Sound.Metallkapp;
		furnitureTagSoundDictionary["Laud"] = Sound.Laud;
	}

	public static void PlaySound(Sound sound, Vector3 position) {
		if (CanPlaySound(sound)) {
			GameObject soundGameObject = new GameObject("Sound");
			soundGameObject.transform.position = position;
			AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
			audioSource.clip = GetAudioClip(sound);
			audioSource.maxDistance = 100f;
			audioSource.spatialBlend = 1f;
			audioSource.rolloffMode = AudioRolloffMode.Linear;
			audioSource.dopplerLevel = 0f;
			audioSource.Play();

			Object.Destroy(soundGameObject, audioSource.clip.length);
		}

	}



	public static void PlaySound(Sound sound) {
		if (CanPlaySound(sound)) {
			if (oneShotGameObject == null) {
				oneShotGameObject = new GameObject("Sound");
				oneShoutAudioSource = oneShotGameObject.AddComponent<AudioSource>();

			}
			AudioClip audioClip = GetAudioClip(sound);
			Debug.Log(audioClip);
			oneShoutAudioSource.PlayOneShot(audioClip);
		}
	}

	public static void PlaySound(string furnitureTag) {
		Sound sound = furnitureTagSoundDictionary[furnitureTag];
		if (CanPlaySound(sound)) {
			if (oneShotGameObject == null) {
				oneShotGameObject = new GameObject("Sound");
				oneShoutAudioSource = oneShotGameObject.AddComponent<AudioSource>();

			}
			AudioClip audioClip = GetAudioClip(sound);
			Debug.Log(audioClip);
			oneShoutAudioSource.PlayOneShot(audioClip);
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
				return soundAudioClip.audioClips[Random.Range(0, soundAudioClip.audioClips.Length) ];
			}
		}
		Debug.LogError("Sound" + sound + "not found!");
		return null;
	}

}
