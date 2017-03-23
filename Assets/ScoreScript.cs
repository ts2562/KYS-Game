using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {
	public Text scoreText;
	public int deathCount;
	// Use this for initialization
	void Start () {
		deathCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void InccrementScore(){
		deathCount++;
		scoreText.text = "Death Count: " + deathCount;
	}
}
