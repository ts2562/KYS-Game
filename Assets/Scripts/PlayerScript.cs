using System.Collections;
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
	private bool collideWithhazard;
	private bool canMove;	// after dying and before reset, the body cannot move

	//Push Dead Bodies
	private bool canPush;
	private GameObject pushBodyGO;

	//Life and Death
	public GameObject[] liveList;
	public int death = 0;

	//Coroutine
	private IEnumerator waitForRestart;

	 
	void Start () 
	{
		startPos = new Vector3(transform.position.x,transform.position.y,0);
		collideWithhazard = false;
		canMove = true;
		canPush = false;
		pushBodyGO = null;
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

		//Movement
		if (canMove)
		{
			if(Input.GetKey(KeyCode.A))
			{
				if (transform.position.x > -150.0f)
					transform.position += Vector3.left * speed * Time.deltaTime;
				//Pushing
				if (canPush && pushBodyGO != null) 
				{
					if (Mathf.Abs (pushBodyGO.transform.position.y - this.transform.position.y) < 1f &&
					   pushBodyGO.transform.position.x < this.transform.position.x) 
					{
						pushBodyGO.transform.position += Vector3.left * speed * Time.deltaTime;
					}
				}
			}
			if(Input.GetKey(KeyCode.D))
			{
				if (transform.position.x < 150.0f)
					transform.position += Vector3.right * speed * Time.deltaTime;
				if (canPush && pushBodyGO != null) 
				{
					if (Mathf.Abs (pushBodyGO.transform.position.y - this.transform.position.y) < 1f &&
						pushBodyGO.transform.position.x > this.transform.position.x) 
					{
						pushBodyGO.transform.position += Vector3.right * speed * Time.deltaTime;
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

	private IEnumerator WaitForRestart()
	{
		yield return new WaitForSeconds(2f);
//		Debug.Log ("Wait");
		liveList [death % 4].GetComponent<BoxCollider2D> ().isTrigger = false;
		this.transform.position = startPos;
		collideWithhazard = false;
		canMove = true;
		death++;
		yield break;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		var normal =  collision.contacts[0].normal;
		if (normal.y > 0) { //if the bottom side hit something 
			//Debug.Log ("You Hit the floor");
			isFalling = false;
			collision.gameObject.GetComponent<BoxCollider2D>().enabled = true;
		}
		if (collision.transform.parent.name == "Lives") 
		{
			canPush = true;
			pushBodyGO = collision.gameObject.GetComponent<BoxCollider2D>().gameObject;
		}

	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.transform.parent.name == "Spikes")
		{
     		//Destroy(col.gameObject);
     		//audio.Play();
			//StartCoroutine (waitSeconds (2.0f));
			if(!collideWithhazard)
			{					
				this.GetComponent<SpriteRenderer>().color = col.gameObject.GetComponent<SpriteRenderer>().color;
				liveList[death % 4].transform.position = this.transform.position;
				liveList [death % 4].GetComponent<BoxCollider2D> ().isTrigger = true;
				waitForRestart = WaitForRestart ();
				StartCoroutine (waitForRestart);
				collideWithhazard = true;
				canMove = false;
			}
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
			pushBodyGO = null;
		}

	}
}
