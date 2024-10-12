using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
	[SerializeField] private GameInput gameInput;

	private Collider coll;
	private FunctionTimer colliderTimer;

	private void Awake() {
		colliderTimer = new FunctionTimer(DisableCollider, 0.1f);
		gameInput.OnPunch += GameInput_OnPunch;
		coll = gameObject.GetComponent<Collider>();
	}

	private void Update() {
		if (coll.enabled) {
			colliderTimer.Update();
		}
	}

	private void GameInput_OnPunch(object sender, System.EventArgs e) {
		colliderTimer.SetTime(0.1f);
		EnableCollider();
	}

	//private void OnTriggerEnter(Collider other) {
	//	if (other.gameObject.tag == "enemy") {
	//		Debug.Log("ded Xo");
	//		Destroy(other.gameObject);
	//	}
	//}
	
	private void EnableCollider() {
		Debug.Log("enable!");
		coll.enabled = true;
	}

	private void DisableCollider() {
		Debug.Log("disable!");
		coll.enabled = false;
	}


}
