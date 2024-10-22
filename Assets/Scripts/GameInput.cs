using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
	public event EventHandler OnPunch;
	public event EventHandler OnAnyKeyPressed;


	[SerializeField] private float minimumRebindTimerRange, maximumRebindTimerRange;
	[SerializeField] private GameController gameController;

	private KeyCode hitButton;
	private PlayerInputActions playerInputActions;

	private void Awake() {
		gameController.OnRebind += GameController_OnRebind ;
		playerInputActions = new PlayerInputActions();
		playerInputActions.Player.Enable();
		hitButton = KeyCode.RightControl;
	}

	private void GameController_OnRebind(object sender, GameController.OnRebindEventArgs e) {
		hitButton = (KeyCode)e.key;
	}


	private void Update() {
		if (Input.GetKeyDown(hitButton)) {
			OnPunch?.Invoke(this, EventArgs.Empty);
		}		
		if (Input.anyKey) {
			OnAnyKeyPressed?.Invoke(this, EventArgs.Empty);
		}
	}



	public Vector2 GetMovementVectorNormalized() {
		Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

		inputVector = inputVector.normalized;
		return inputVector;
	}
}

