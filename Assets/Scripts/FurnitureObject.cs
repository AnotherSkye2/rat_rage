using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureObject : MonoBehaviour {

	[SerializeField] private FurnitureObjectSO furnitureObjectSO;
	[SerializeField] private FurnitureDestruction furnitureDestruction;

	private int hp;
	private int currentHp;
	private AudioSource audioSource;

	private void Awake() {
		hp = furnitureObjectSO.hitPoints;
		currentHp = hp;
	}


	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "player") {
			currentHp--;
			if (currentHp <= 0) {
				Collider coll = gameObject.GetComponent<Collider>();
				coll.enabled = false;
			}
			furnitureDestruction.ChangeImage(hp-currentHp);
			PlaySound();
		}
	}

	public virtual void PlaySound() {
		AudioClip[] sfxS = furnitureObjectSO.sounds;
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = sfxS[UnityEngine.Random.Range(0, 3)];
		//Debug.Log(audioSource.clip);
		audioSource.Play();
	}



	public FurnitureObjectSO GetFurnitureObjectSO() {
		return furnitureObjectSO;
	}

	public int GetCurrentHp() {
		return currentHp;
	}

}
