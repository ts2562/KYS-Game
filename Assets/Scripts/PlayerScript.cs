﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour 
{

	public Vector3 startPos;

	//Basic Movement
	public float speed = 40.0f;
	public Vector2 jumpHeight;
	public bool isFalling = false;
	private bool collideWithHazard;
	private bool canMove;	// after dying and before reset, the body cannot move

	//Push Dead Bodies
	private bool canPush;
//	private GameObject pushBodyGO;
	public List<GameObject> pushingList;

	//Life and Death
	public GameObject[] liveList;
	public int death = 0;

	public AudioSource audio;
	//Coroutine
	private IEnumerator waitForRestart;

	//Pushing


	//height correction
	//private Vector3 correction;
	 
	void Start () 
	{
		startPos = new Vector3(transform.position.x,transform.position.y,0);
		collideWithHazard = false;
		canMove = true;
//		canPush = false;
//		pushBodyGO = null;

		pushingList = new List<GameObject>();

		//correction = new Vector3(0f,1.85f,0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		//settings
		if(Input.GetKey(KeyCode.R))
		{
			transform.position = startPos;
		}
		if(Input.GetKey(KeyCode.Escape))
		{
			Application.LoadLevel(Application.loadedLevel);	// Unity now uses SceneManager instead of Application to manage scenes
			SceneManager.LoadScene(SceneManager.sceneCount - 1);
		}

		//Add pushing bodies
		if (canPush) 
		{
			CheckDeadBodyForPushing (pushingList[0]);
		}
		//Movement
		if (canMove)
		{
			
			if(Input.GetKey(KeyCode.A))
			{
				
				if (transform.position.x > -150.0f)
					transform.position += Vector3.left * speed * 0.02f;
				//Pushing
				if (canPush) 
				{
					if (this.transform.position.x >= pushingList [0].transform.position.x) 
					{
						for (int i = 0; i < pushingList.Count; i++) 
						{
						//	pushingList[i].transform.position = new Vector3(this.transform.position.x - (i + 1) * this.transform.GetComponent<SpriteRenderer>().bounds.size.x,
						//		pushingList[i].transform.position.y, 0);
							pushingList[i].GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
						}
					}
				
				}
			}
			if(Input.GetKey(KeyCode.D))
			{
				if (transform.position.x < 150.0f)
					transform.position += Vector3.right * speed * 0.02f;
				
				if (canPush) 
				{
					if (this.transform.position.x <= pushingList [0].transform.position.x) 
					{
						Debug.Log (pushingList.Count);
						for (int i = 0; i < pushingList.Count; i++) 
						{
					//		pushingList[i].transform.position = new Vector3(this.transform.position.x +  (i + 1)  * this.transform.GetComponent<SpriteRenderer>().bounds.size.x,
					//			pushingList[i].transform.position.y, 0);
								pushingList[i].GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
						}
					}
				}
			}
			if (Input.GetKeyDown(KeyCode.Space) && isFalling == false)  //make a limit to how many times player can jump later
			{
				this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (jumpHeight.x, jumpHeight.y);

				isFalling = true;
			}
		}



	}

	private void CheckDeadBodyForPushing(GameObject _pushgo)
	{
//		Debug.Log (_pushgo);
		for (int i = 0; i < death; i++) 
		{
			if (!pushingList.Contains (liveList [i])) 
			{
				if(Mathf.Abs(_pushgo.transform.position.x - liveList[i].transform.position.x) < 
									_pushgo.GetComponent<SpriteRenderer>().bounds.size.x + 0.01f &&
					Mathf.Abs(_pushgo.transform.position.y - liveList[i].transform.position.y) < 
					_pushgo.GetComponent<SpriteRenderer>().bounds.size.y - 0.01f)
				{
					pushingList.Add (liveList[i]);
					CheckDeadBodyForPushing (liveList[i]);
				}
			}
		}
	}

	private IEnumerator WaitForRestart()
	{
		float startRotation = transform.eulerAngles.z;
		float endRotation = startRotation + 360.0f;
		float t = 0.0f;
		while ( t  < 1.0f )
		{
			t += Time.deltaTime;
			float zRotation = Mathf.Lerp(startRotation, endRotation, t / 1.0f) % 360.0f;
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
			yield return null;
		}
		//yield return new WaitForSeconds(1.0f);
//		Debug.Log ("Wait");
		liveList [death % 4].GetComponent<BoxCollider2D> ().isTrigger = false;
		this.transform.position = startPos;
		collideWithHazard = false;
		canMove = true;
		death++;
		yield break;
	}

	private void Death(GameObject _go)			// No matter colliders which kinds of hazards, must call this function to creat a dead body
	{
		//this.GetComponent<SpriteRenderer>().color = col.gameObject.GetComponent<SpriteRenderer>().color;
		if(!collideWithHazard)
		{					
			liveList[death % 4].GetComponent<SpriteRenderer>().color = _go.GetComponent<SpriteRenderer>().color;
			liveList[death % 4].transform.position = this.transform.position;
			liveList [death % 4].GetComponent<BoxCollider2D> ().isTrigger = true;
			waitForRestart = WaitForRestart ();
			StartCoroutine (waitForRestart);
			collideWithHazard = true;
			canMove = false;
			if(death >= liveList.Length){
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	
	}

	void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.transform.tag == "Ground") 
		{
			var normal = collision.contacts[0].normal;
			if (normal.y > 0) 
			{ //if the bottom side hit something 
				//Debug.Log ("You Hit the floor");
				isFalling = false;
				collision.gameObject.GetComponent<BoxCollider2D>().enabled = true;
			}
		}

		if (collision.transform.parent.name == "CrushingRects") 
		{
			var normal =  collision.contacts[0].normal;
			if (normal.y < 0)
			{ //if player's top side hits something 

				audio.Play();
				collision.transform.GetComponent<CrushingRect> ().PauseDoMove ();
				collision.transform.GetComponent<CrushingRect> ().GoBack ();
				Death (collision.gameObject);
				collision.transform.GetComponent<CrushingRect> ().UpdateFallingDistance(collision.transform.position.y);  
			}
		}

		if (collision.transform.name == "IceBallBase") 
		{
			audio.Play();

			collision.transform.GetComponent<IceBall> ().PauseTween (collision.transform);
			Death (collision.gameObject);
		}

		if (collision.transform.parent.name == "Lives") 
		{
			if (Mathf.Abs (collision.transform.position.y - this.transform.position.y) < this.GetComponent<SpriteRenderer> ().bounds.size.y) 
			{
				canPush = true;
				pushingList.Add(collision.gameObject);
				CheckDeadBodyForPushing (pushingList[0]);
			}
			//		pushBodyGO = collision.gameObject.GetComponent<BoxCollider2D>().gameObject;
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.transform.parent.name == "Spikes")
		{
     		//Destroy(col.gameObject);
     		audio.Play();
			//StartCoroutine (waitSeconds (2.0f));

			Death (col.gameObject);
		
     		//Life1.transform.position = playerPos;
     	}

		if (col.transform.tag == "GoalHazard") 
		{
			col.GetComponent<SpriteRenderer> ().color = new Color32 (0, 0, 0, 255);
			Debug.Log ("Die on the goal hazard");
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.transform.parent.name == "Lives") 
		{
			canPush = false;
			for (int i = 0; i < pushingList.Count; i++) 
			{
				pushingList [i].GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			}
//			pushBodyGO = null;
			pushingList.Clear();	
		}

	}
}
