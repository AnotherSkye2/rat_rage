using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Diagnostics.Tracing;
using UnityEngine.InputSystem.Utilities;

public class GameInput : MonoBehaviour
{
	public event EventHandler OnPunch;
	public event EventHandler OnAnyKeyPressed;
	public event EventHandler OnWrongKeyPressed;


	[SerializeField] private float minimumRebindTimerRange, maximumRebindTimerRange;
	[SerializeField] private GameController gameController;

	private KeyCode hitButton;
	private PlayerInputActions playerInputActions;
	private IDisposable m_EventListener;

	private void Awake() {
		gameController.OnRebind += GameController_OnRebind ;
		playerInputActions = new PlayerInputActions();
		playerInputActions.Player.Enable();
		hitButton = KeyCode.RightControl;
	}

	private void Start() {
		m_EventListener =
			InputSystem.onAnyButtonPress
				.Call(OnButtonPressed);
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
		Debug.Log(Input.inputString);

	}

	private void OnButtonPressed(InputControl button) {
		//Checking if the current button pressed is WASD, hitbutton and R.Ctrl
		if (button.device.GetType() == typeof(Keyboard)) {
			if (!Keyboard.current.wKey.wasPressedThisFrame && !Keyboard.current.aKey.wasPressedThisFrame && !Keyboard.current.sKey.wasPressedThisFrame && !Keyboard.current.dKey.wasPressedThisFrame && !Input.inputString.Contains(hitButton.ToString().ToLower()) && (!Keyboard.current.rightCtrlKey.wasPressedThisFrame || hitButton != KeyCode.RightControl)) {
				OnWrongKeyPressed?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public Vector2 GetMovementVectorNormalized() {
		Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

		inputVector = inputVector.normalized;
		return inputVector;
	}
}

