using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

	[SerializeField] private GameInput gameInput;
	[SerializeField] private Rigidbody _rb;
	[SerializeField] private float _speed = 5;

	private bool isWalking;


	private void FixedUpdate() {
		HandleMovement();
	}


	private void HandleMovement() {

		Vector2 inputVector = gameInput.GetMovementVectorNormalized();
		inputVector = Quaternion.Euler(0, 0, -45) * inputVector;
		Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
		float moveDistance = _speed * Time.deltaTime;
		var playerHeight = 1f;
		var playerRadius = 0.25f;

		bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

		if (!canMove) {
			//Can not move towards moveDir

			//Attempt only X-axis movement

			Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
			canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

			if (canMove) {
				moveDir = moveDirX;
			}
			else {

				//Attempt only Z-axis movement
				Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
				canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

				if (canMove) {
					moveDir = moveDirZ;
				}
				else {
					//Cannot move at all	
				}
			}
		}

		transform.position += moveDir * moveDistance;
		Console.WriteLine(moveDir + " | " + moveDistance);

		
		isWalking = moveDir != Vector3.zero;

		//float rotateSpeed = 10f;
		//transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
	}

	public bool IsWalking() {
		return isWalking;
	}
}

