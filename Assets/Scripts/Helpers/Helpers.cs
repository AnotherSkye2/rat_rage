using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class Helpers {
	private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
	public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);

	public static bool IsVectorCrossProductPositive(Vector3 v1, Vector3 v2) {
		float crossProduct = (v1.x * v2.z) - (v1.z * v2.x);
		return 0 >= crossProduct;
			
	}

}
