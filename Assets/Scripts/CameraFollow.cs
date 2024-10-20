using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FastScriptReload.Examples.FunctionLibrary;

public class CameraFollow : MonoBehaviour {

	[SerializeField] private Transform target;
	[SerializeField] private float distance;

	private void Update() {
		transform.position = new Vector3(target.position.x, target.position.y, target.position.z - distance);

	}
}
