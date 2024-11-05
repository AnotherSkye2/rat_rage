using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour {

	public event EventHandler<OnFurnitureDestroyedEventArgs> OnFurnitureDestroyed;
	public class OnFurnitureDestroyedEventArgs : EventArgs {
		public int scoreValue;
	}

	[SerializeField] private GameInput gameInput;
	[SerializeField] private float _speed = 5;
	[SerializeField] private PlayerHitBox playerHitBox;

	private bool isWalking;
	private Vector3 moveDir;
	private Vector3 lastMoveDir;
	private Collider coll;
	private float colliderTriggerDuration = 0.1f;


	private void Awake() {
		playerHitBox.OnHitboxHit += PlayerHitBox_OnHitboxHit;
		gameInput.OnPunch += GameInput_OnPunch;
		coll = playerHitBox.GetComponent<Collider>();
	}

	private void PlayerHitBox_OnHitboxHit(object sender, PlayerHitBox.OnHitboxHitEventArgs e) {
		//Debug.Log(e.furnitureObject);
		//Debug.Log("OnHitboxHit");
		if (e.furnitureObject.GetCurrentHp() <= 1) {
			//Debug.Log("0hp!");
			FurnitureObjectSO furnitureObjectSO = e.furnitureObject.GetFurnitureObjectSO();
			OnFurnitureDestroyed?.Invoke(this, new OnFurnitureDestroyedEventArgs {
				scoreValue = furnitureObjectSO.scoreValue
			});
		}
	}

	IEnumerator ColliderTrigger(float time) {
		//Debug.Log("EnableCollider");
		EnableCollider();
		yield return new WaitForSeconds(time);
		//Debug.Log("DisableCollider");
		DisableCollider();
	}

	private void GameInput_OnPunch(object sender, EventArgs e) {
		StartCoroutine(ColliderTrigger(colliderTriggerDuration)); 
	}

	private void FixedUpdate() {
		HandleMovement();
	}

	private void HandleMovement() {

		Vector2 inputVector = gameInput.GetMovementVectorNormalized();
		inputVector = Quaternion.Euler(0, 0, -45) * inputVector;
		moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
		if (moveDir != Vector3.zero) {
			lastMoveDir = moveDir;
		}
		float moveDistance = _speed * Time.deltaTime;
		var playerHeight = 0.1f;
		var playerRadius = 0.24f;

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

		if (canMove) {
		transform.position += moveDir * moveDistance;
		}		

		isWalking = moveDir != Vector3.zero;

		if (isWalking) {
			SoundManager.PlaySound(SoundManager.Sound.PlayerMove, transform.position);
		}
		//Debug.Log(moveDir);

		float rotateSpeed = 10f;
		transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
	}



	private void EnableCollider() {
		coll.enabled = true;
	}

	private void DisableCollider() {
		coll.enabled = false;
	}

	public bool IsWalking() {
		return isWalking;
	}

	public Vector3 GetLastMoveDir() {
		return lastMoveDir;
	}


}


