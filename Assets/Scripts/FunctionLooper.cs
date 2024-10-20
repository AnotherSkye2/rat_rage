using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionLooper {

	private Action action;
	private float loopDuration, timer;

	public FunctionLooper(Action action, float timer) {
		this.action = action;
		this.timer = timer;
		this.loopDuration = timer;
	}

	public void Update() {
		timer -= Time.deltaTime;
		if (timer < 0) {
			action();
			timer = loopDuration;
		}
	}

}