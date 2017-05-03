using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownScript : MonoBehaviour {

	// Use this for initialization
	public GameObject Player;
	//Vector3 start = new Vector3(-29.2f, -14.785f, 0);
	Vector3 playerPos;
	// Use this for initialization
	void Start () {
		//playerPos = new Vector3(Player.transform.position.x,Player.transform.position.y,0);


	}
	
	// Update is called once per frame
	void Update () {
		//GameObject.Find("Player").GetComponent<PlayerScript>().speed = 20.0f;
	}
     

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.name == "Player"){
			//playerScript.speed = 26.0f;
			//GameObject.Find("Player").GetComponent<PlayerScript>().speed = 15.0f;
			Debug.Log ("2");
		}
}
}

