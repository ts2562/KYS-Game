using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal3 : MonoBehaviour {

	public GameObject Player;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collider2D col){
		if(col.gameObject.name == "Player"){
			//Application.LoadLevel("Level2");
			SceneManager.LoadScene("Large_Level_1",  LoadSceneMode.Single);
		}

	}
}
