using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureObject : MonoBehaviour {

	[SerializeField] private FurnitureObjectSO furnitureObjectSO;

	private int hp;

	private void Awake() {
		hp = GetHitpoints(furnitureObjectSO);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "player") {
			hp--;
			Debug.Log(hp);
			if (hp <= 0 ) {
				//Event for points
				Destroy(gameObject);
			}
		}
	}

	private int GetHitpoints(FurnitureObjectSO furnitureObjectSO) {
		return furnitureObjectSO.hitPoints;
	}
}
