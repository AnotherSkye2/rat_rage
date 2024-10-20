using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{

	public event EventHandler<OnHitboxHitEventArgs> OnHitboxHit;
	public class OnHitboxHitEventArgs : EventArgs {
		public FurnitureObject  furnitureObject;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "enemy") {
			Debug.Log(other.gameObject);
			Debug.Log("OnTriggerEnter");
			OnHitboxHit?.Invoke(this, new OnHitboxHitEventArgs { 
				furnitureObject = other.gameObject.GetComponent<FurnitureObject>()
			});
		}
	}



}
