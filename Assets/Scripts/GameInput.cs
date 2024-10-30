using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Diagnostics.Tracing;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.LowLevel;

public class GameInput : MonoBehaviour
{
	public event EventHandler OnPunch;
	public event EventHandler OnAnyKeyPressed;
	public event EventHandler OnWrongKeyPressed;


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

	private void Start() {
		InputSystem.onEvent
			.ForDevice<Keyboard>()
			.Where(e => {
				if (e.type != StateEvent.Type && e.type != DeltaStateEvent.Type)
					return false;
				else
					return e.HasButtonPress();
			})
			.Call(ctrl => {
				if (Application.isPlaying) {
					foreach (var button in ctrl.GetAllButtonPresses())
						OnButtonPressed(button);
				}
			});	
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

	private void OnButtonPressed(InputControl button) {
		//Checking if the current button pressed is WASD, hitbutton and R.Ctrl
		Debug.Log(gameController.GetState());
		if (button.name != "w" && button.name != "a" && button.name != "s" && button.name != "d" && button.name != hitButton.ToString().ToLower() && (button.name != "rightCtrl" || hitButton != KeyCode.RightControl)) {
			OnWrongKeyPressed?.Invoke(this, EventArgs.Empty);
		}
	}

	public Vector2 GetMovementVectorNormalized() {
		Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

		inputVector = inputVector.normalized;
		return inputVector;
	}
}

