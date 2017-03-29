using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeScript : MonoBehaviour {

	public GameObject Player;
	//public float bounciness = 2.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D col){
     	if(col.gameObject.name == "Player"){
     		//Destroy(col.gameObject);
     		Debug.Log("fdsfdsf");
     		//Player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounciness, ForceMode2D.Impulse);
     	}
     }
}
