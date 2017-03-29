using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPlatformScript : MonoBehaviour {

	public GameObject Player;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
     

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.name == "Player"){
			//playerScript.speed = 26.0f;
//			GameObject.Find("Player").GetComponent<PlayerScript>().speed= 45.0f;
			Debug.Log ("1");
		}
}
}
