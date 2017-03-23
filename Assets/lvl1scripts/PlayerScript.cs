using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	private float speed = 13.0f;
	public Vector2 jumpHeight;
	//public float bounciness = 2.0f;
	public GameObject Life1;
	//public bool colliding;
	// Use this for initialization
	public bool isFalling = false;
	
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
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

	void OnCollisionEnter2D(){
		isFalling = false;
	}
	
	
     
}
