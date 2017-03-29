using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public float speed = 20.0f;
	public Vector2 jumpHeight;
	public GameObject Life1;
	public bool isFalling = false;

	public GameObject SpeedPlat1;
	public GameObject SpeedPlat2;
	public GameObject SpeedPlat3;
	
	Vector3 startPos; 
	void Start () {
		startPos = new Vector3(transform.position.x,transform.position.y,0);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.R)){
			transform.position = startPos;
		}

		if(Input.GetKey(KeyCode.A)){
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.D)){
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.Space) && isFalling == false)  //make a limit to how many times player can jump later
    	{
    		Debug.Log(transform.position.y);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0,25), ForceMode2D.Impulse);
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
			Debug.Log ("You Hit the floor");
			isFalling = false;

		}
		Debug.Log(collide.gameObject.name);
		if(collide.gameObject.name == "SpeedPlat1"){
			speed = 26.0f;
			Debug.Log ("1");
		}
		if(collide.gameObject.name == "SpeedPlat2"){
			speed = 26.0f;
			Debug.Log ("2");
		}
		if(collide.gameObject.name == "SpeedPlat3"){
			speed = 26.0f;
			Debug.Log ("3");
		}
		else {
			speed = 13.0f;
		}
	}

/*
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.name == "SpeedPlat1"){
			speed = 26.0f;
			Debug.Log ("1");
		}
		if(col.gameObject.name == "SpeedPlat2"){
			speed = 26.0f;
			Debug.Log ("2");
		}
		if(col.gameObject.name == "SpeedPlat3"){
			speed = 26.0f;
			Debug.Log ("3");
		}
		else {
			speed = 13.0f;
		}

	}
	*/
     
}
