using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionLooper {

	public static FunctionLooper Create(Action action, float timer) {
		GameObject gameObject = new GameObject("FunctionTimer", typeof(MonoBehaviourHook));
		FunctionLooper functionTimer = new FunctionLooper(action, timer, gameObject);
		gameObject.GetComponent<MonoBehaviourHook>().onUpdate = functionTimer.Update;
		return functionTimer;
	}

	public class MonoBehaviourHook : MonoBehaviour {
		public Action onUpdate;
		private void Update() {
			if (onUpdate != null) onUpdate();
		}
	}

	private Action action;
	private GameObject gameObject;
	private float originalTimerduration;
	private float timer;
	private bool isDestroyed;

	private FunctionLooper(Action action, float timer, GameObject gameObject) {
		this.action = action;
		this.timer = timer;
		originalTimerduration = timer;
		this.gameObject = gameObject;
		isDestroyed = false;
	}

	public void Update() {
		if (!isDestroyed) {
			timer -= Time.deltaTime;
			if (timer < 0 && action != null) {
				action();
				timer = originalTimerduration;
			}
		}
	}

	public float GetTime() {
		return timer;
	}

	public void SetTime(float time) {
		timer = time;
	}

}