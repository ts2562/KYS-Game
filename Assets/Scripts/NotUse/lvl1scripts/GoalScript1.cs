﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript1 : MonoBehaviour {
	public GameObject Player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
     	if(col.gameObject.name == "Player"){
     		//Application.LoadLevel("Level2");
     		SceneManager.LoadScene("Level2",  LoadSceneMode.Single);
     	}

     }
}
