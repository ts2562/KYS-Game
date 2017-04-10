using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeScript5 : MonoBehaviour {
	public Color color;
	public GameObject Player;
	// Use this for initialization
	void Start () {
		color = gameObject.GetComponent<SpriteRenderer>().color;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionEnter2D(Collision2D collide){
		if(collide.gameObject.name == "Player"){
			Debug.Log("HI");
			Player.GetComponent<SpriteRenderer>().color = color;
		}
	}
}
