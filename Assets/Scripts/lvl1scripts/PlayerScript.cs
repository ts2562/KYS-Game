using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public float speed = 40.0f;
	public Vector2 jumpHeight;
	public GameObject Life1;
	public bool isFalling = false;
	public Vector3 startPos;
	public int death = 0;

	 
	void Start () {
		startPos = new Vector3(transform.position.x,transform.position.y,0);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.R)){
			transform.position = startPos;
		}
		if(Input.GetKey(KeyCode.Escape)){
			Application.LoadLevel(Application.loadedLevel);
		}

		if(Input.GetKey(KeyCode.A)){
			if (transform.position.x > -150.0f)
				transform.position += Vector3.left * speed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.D)){
			if (transform.position.x < 150.0f)
				transform.position += Vector3.right * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.Space) && isFalling == false)  //make a limit to how many times player can jump later
    	{
        	GetComponent<Rigidbody2D>().AddForce(new Vector2(0,30), ForceMode2D.Impulse);
        	isFalling = true;
   	    }
	}
/*
	void OnCollisionEnter2D(){
		isFalling = false;
	}
	*/
	void OnCollisionEnter2D(Collision2D collide){
		var normal = collide.contacts[0].normal;
		if (normal.y > 0) { //if the bottom side hit something 
			//Debug.Log ("You Hit the floor");
			isFalling = false;
			collide.gameObject.GetComponent<Collider2D>().enabled = true;
		
		}
	}

     
}
