using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{

	public event EventHandler OnPunch;

	private PlayerInputActions playerInputActions;

	private void Awake() {
		playerInputActions = new PlayerInputActions();
		playerInputActions.Player.Enable();
		
		playerInputActions.Player.Punch.performed += Punch_performed;
	}

	private void Punch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
		OnPunch?.Invoke(this, EventArgs.Empty);
	}

	public Vector2 GetMovementVectorNormalized() {
		Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

		inputVector = inputVector.normalized;
		return inputVector;
	}
}

