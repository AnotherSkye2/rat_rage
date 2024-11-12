using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FurnitureObjectSO : ScriptableObject {

	public Transform prefab;

	public int hitPoints;
	public int scoreValue;
	public Sprite[] images;

}
