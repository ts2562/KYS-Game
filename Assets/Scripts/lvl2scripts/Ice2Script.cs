using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice2Script : MonoBehaviour {

	//public GameObject ice1;
	public GameObject ice2;
	//public GameObject life1;
	public GameObject life2;
	public GameObject Player;
	//Vector3 ice1Start = new Vector3(-1.5f, -8.7f, 0);
	Vector3 ice2Start = new Vector3(20.7f, 7f, 0);
	Vector3 start = new Vector3(-24.5f, -14.785f, 0);
	Vector3 playerPos;
	public AudioSource audio;
	float speed = 10.0f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		playerPos = new Vector3(Player.transform.position.x,Player.transform.position.y,0);
		transform.position += Vector3.left * speed * Time.deltaTime;
		if(transform.position.x <= -40f){
			transform.position = ice2Start;
		}
		//if(ice2.transform.position.x >= -30f){
		//	ice2.transform.position = ice1Start;
		//}
		}

	void OnTriggerEnter2D(Collider2D col){
     	if(col.gameObject.name == "Player"){
     		//Destroy(col.gameObject);
     		//ice2.transform.position = ice2Start;
     		audio.Play();
     		life2.transform.position = playerPos;
     		Player.transform.position = start;
			transform.position = ice2Start;
     		//ice2.transform.position = ice2Start;
     	}
     	if(col.gameObject.name == "life2"){
     		//Destroy(col.gameObject);
     		ice2.transform.position = ice2Start;
     	}
     }
}