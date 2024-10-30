using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

	public static int score { get; private set; }

	public void SetScore(int score) {
		ScoreManager.score = score;
	}


}
