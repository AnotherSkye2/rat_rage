using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class Helpers {

	public static bool IsVectorCrossProductPositive(Vector3 v1, Vector3 v2) {
		float crossProduct = (v1.x * v2.z) - (v1.z * v2.x);
		return 0 >= crossProduct;
			
	}

}
