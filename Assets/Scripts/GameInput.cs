using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
	public event EventHandler OnPunch;
	public event EventHandler OnAnyKeyPressed;
	public event EventHandler<OnRebindEventArgs> OnRebind;
	public class OnRebindEventArgs : EventArgs {
		public char key;
	}

	[SerializeField] private float minimumRebindTimerRange, maximumRebindTimerRange;

	private KeyCode hitButton;
	private PlayerInputActions playerInputActions;
	private FunctionLooper rebindLooper;

	private void Awake() {
		rebindLooper = new FunctionLooper(Rebind, UnityEngine.Random.Range(minimumRebindTimerRange, maximumRebindTimerRange));

		playerInputActions = new PlayerInputActions();
		playerInputActions.Player.Enable();

		hitButton = KeyCode.RightControl;
	}

	private void Update() {
		if (Input.GetKeyDown(hitButton)) {
			OnPunch?.Invoke(this, EventArgs.Empty);
		}		
		if (Input.anyKey) {
			OnAnyKeyPressed?.Invoke(this, EventArgs.Empty);
		}
		rebindLooper.Update();
	}

	private void Rebind() {
		char previousKey = (char)hitButton;
		char key = (char)(UnityEngine.Random.Range(0, 25) + 97);
		if (key != 'w' && key != 'a' && key != 's' && key != 'd' && key != previousKey) {
			hitButton = (KeyCode)key;
			OnRebind?.Invoke(this, new OnRebindEventArgs {
				key = key
			});
		} else {
			Rebind();
		}
	}


	public Vector2 GetMovementVectorNormalized() {
		Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

		inputVector = inputVector.normalized;
		return inputVector;
	}
}

