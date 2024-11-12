using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorFurnitureObject : FurnitureObject {

	private const string BROKEN = "Broken";

	private Animator animator;

	private void Awake() {
		animator = GetComponentInChildren<Animator>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "player") {
			Collider coll = gameObject.GetComponent<Collider>();
			coll.enabled = false;
			animator.SetBool(BROKEN, true);
			SoundManager.PlaySound(gameObject.tag);
		}
	}



}

