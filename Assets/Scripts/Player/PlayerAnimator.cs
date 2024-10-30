using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

	private const string IS_WALKING = "IsWalking";
	private const string PUNCH = "Punch";
	private Vector3 flipVector = new Vector3(0.71f, 0, 0.71f);


	[SerializeField] private PlayerController player;
	[SerializeField] private GameInput gameInput;


	private Animator animator;
	private SpriteRenderer spriteRenderer;

	private void Awake() {
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		gameInput.OnPunch += GameInput_OnPunch;
	}

	private void GameInput_OnPunch(object sender, System.EventArgs e) {
		animator.SetTrigger(PUNCH);
	}

	private void Update() {
		animator.SetBool(IS_WALKING, player.IsWalking());
		spriteRenderer.flipX = Helpers.IsVectorCrossProductPositive(flipVector, player.GetLastMoveDir());
	}
}

