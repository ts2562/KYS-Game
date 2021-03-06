﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerScript : MonoBehaviour 
{
	//For sprites
	public Sprite neutral;
	public Sprite maxHappy;
	public Sprite happy;
	public Sprite maxSad;
	public Sprite sad;
	SpriteRenderer sr;

	public Vector3 startPos;
	private Vector3 originalStartPos;
	public int maxLives;

	//Basic Movement
	private float speed = 15.0f;
	public Vector2 curJumpSpeed;
	private Vector2 maxJumpSpeed;
	private float curjumpHeight;
	private float jumpTimer;
	public bool isFalling = false;
	public bool canJump;
	private bool collideWithHazard;
	private bool canMove;	// after dying and before reset, the body cannot move

	//Push Dead Bodies
	public bool canPush;
//	private GameObject pushBodyGO;
	public List<GameObject> pushingList;


	public GameObject fadeImage;

	//Life and Death
	private GameObject[] liveList;
	private Vector3[] startLife;
	public Transform deadBodiesTr;
	public int death = 0;
	private float direction;
	private bool restartLevel;
	private GameObject[] checkpoints;
	public Transform nextLevel;

	// effect on the moving lives
	public Transform lifeTr;
	public bool[] lifeJumping;
	public float jumpingDis;


	private GameObject crush;
	public AudioSource impaledSound;
	public AudioSource freezeSound;
	public AudioSource crushSound;
	public AudioSource victoryCheer;
	//Coroutine
	private IEnumerator waitForRestart;	// no matter it is the first time of entering the level, or a restart of the level, must call this function to RESET data
	private IEnumerator fadeOut;	// Only deal with the black mask
	private IEnumerator waitTimer;

	private bool levelClear;	// when beat a level, pause the screen, show an icon indicates "Press Y to continue"


	public float MaxCamX, MinCamX, MaxCamY, MinCamY;

	private bool cameraFollow;
	//height correction
	//private Vector3 correction;


	void Start () // Init data that only need to init once per level
	{
		//for sprite renderer
		sr = GetComponent<SpriteRenderer>();
		tag = "Alive";
		startPos = new Vector3(transform.position.x, transform.position.y, 0);
		originalStartPos = new Vector3(transform.position.x, transform.position.y, 0);

		crush = GameObject.Find("CrushingRect");

		liveList = new GameObject[maxLives];


		pushingList = new List<GameObject>();
		startLife = new Vector3[maxLives];
		lifeJumping = new bool[maxLives];
		maxJumpSpeed = new Vector2 (0, 40);

		//correction = new Vector3(0f,1.85f,0f);
		death = 0;

		for (int i = 0; i < liveList.Length; i++)
		{
			liveList [i] = lifeTr.transform.GetChild (i).gameObject;
			startLife[i] = new Vector3(liveList[i].transform.position.x, liveList[i].transform.position.y, 0);
		}

		//restartLevel = false;
		ResetData ();
	}

	private void ResetData()	// a list for all data that must be reset every restart
	{
		this.tag = "Alive";
		this.transform.position = startPos;
		this.GetComponent<Collider2D>().isTrigger = false;
		lifeTr.position = Vector3.zero;
		curjumpHeight = 1;
		jumpTimer = 0;
		collideWithHazard = false;
		canMove = true;
		canPush = false;
		isFalling = true;
		canJump = false;
		cameraFollow = true;
		pushingList.Clear ();

		levelClear = false;
		Debug.Log(death);
		for (int i = death; i < liveList.Length; i++) 	// dead boides back to the defual size and color
		{
			liveList [i].transform.position = startLife [i];
			liveList[i].name = "Life";
			liveList [i].transform.parent = lifeTr;
			liveList [i].transform.localScale = new Vector3 (0.5f, 0.5f, 1);
			liveList [i].GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
			liveList [i].GetComponent<BoxCollider2D> ().enabled = false;
			lifeJumping [i] = false;
			liveList [i].transform.position = new Vector3(this.transform.position.x + (i + 2 - death) * 3f * direction, 
				this.transform.position.y - jumpingDis, 0);
			liveList [i].transform.DOMoveY(liveList[i].transform.position.y + 2, 0.3f)
				.SetEase (Ease.InSine)
				.SetLoops (-1, LoopType.Yoyo)
				.SetDelay (Random.Range (0, 1f));
		}
		direction = 1;

		restartLevel = false;
	}

	private void RestartLevel()	//hard reset
	{
		fadeOut = FadeOut();
		StartCoroutine(fadeOut);
		if(maxLives != 0){
		for (int i = 0; i <= deadBodiesTr.childCount; i++)
		{
			liveList[i].transform.parent = lifeTr;
			liveList[i].transform.position = startLife [i];

		}
	}
		startPos = originalStartPos;

		checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
		foreach (GameObject check in checkpoints){
			check.GetComponent<SpriteRenderer>().color = new Color32 (255,255,255,255);
			check.transform.GetChild(0).tag = "Untagged";
		}

		death = 0; 


	}

	// Update is called once per frame
	void Update () 
	{
		//Dev Cheat
		Debug.Log(levelClear);

		if (!levelClear) 
		{
			//Movement
			if (canMove)
			{

				if(Input.GetKey(KeyCode.A))
				{
					direction = Mathf.Lerp(direction, 1, 0.02f);

					//	if (transform.position.x > -150.0f)
					transform.position += Vector3.left * speed * 0.03f;
					//Pushing
					if (canPush && pushingList.Count > 0) 
					{
						if (this.transform.position.x >= pushingList [0].transform.position.x) 
						{
							for (int i = 0; i < pushingList.Count; i++) 
							{
								//	pushingList[i].transform.position = new Vector3(this.transform.position.x -
								//				(i + 1) * this.transform.GetComponent<SpriteRenderer>().bounds.size.x,
								//	pushingList[i].transform.position.y, 0);
								//	pushingList[i].GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0);
								pushingList[i].transform.position += Vector3.left * speed * 0.03f;
								//				
							}
						}

					}
				}
				if(Input.GetKey(KeyCode.D))
				{
					direction = Mathf.Lerp(direction, -1, 0.02f);

					transform.position += Vector3.right * speed * 0.03f;

					if (canPush && pushingList.Count > 0) 
					{
						if (this.transform.position.x <= pushingList [0].transform.position.x) 
						{
							for (int i = 0; i < pushingList.Count; i++) 
							{
								//	pushingList[i].transform.position = new Vector3(this.transform.position.x +  
								//		(i + 1)  * this.transform.GetComponent<SpriteRenderer>().bounds.size.x,
								//		pushingList[i].transform.position.y, 0);
								//	pushingList [i].GetComponent<Rigidbody2D> ().velocity = new Vector2(1, 0);
								pushingList[i].transform.position += Vector3.right * speed * 0.03f;
							}
						}
					}
				}

				if (Input.GetKeyDown (KeyCode.Space)) 
				{
					if (!isFalling) 
					{
						canJump = true;
					}
					else
					{
						jumpTimer = 0;
						curJumpSpeed = Vector2.zero;
						//	this.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
						canJump = false;

					}

				}

				if (Input.GetKey(KeyCode.Space) && canJump)  //make a limit to how many times player can jump later
				{
					if (curJumpSpeed.y < maxJumpSpeed.y) 
					{	
						jumpTimer += Time.deltaTime * 120;
						curJumpSpeed = new Vector2 (0, curjumpHeight * jumpTimer * 2 + 5f);
						this.GetComponent<Rigidbody2D> ().velocity = curJumpSpeed;
						//this.GetComponent<Rigidbody2D> ().AddForce(transform.up*100, ForceMode2D.Impulse);
					}
					else
					{
						canJump = false;
						isFalling = true;
					}
					//	this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (jumpHeight.x, jumpHeight.y);

				}

				if (Input.GetKeyUp (KeyCode.Space)) 
				{
					jumpTimer = 0;
					curJumpSpeed = Vector2.zero;
					//this.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
				}

			}
		}
		else
		{
			this.GetComponent<Animator>().Stop();
			if (Input.GetKeyDown (KeyCode.Space)) 
			{
				if (SceneManager.GetSceneAt (0).buildIndex == SceneManager.sceneCountInBuildSettings - 1) 
				{
					SceneManager.LoadScene (0);
				}
				else
				{
					SceneManager.LoadScene (SceneManager.GetSceneAt(0).buildIndex + 1);
				}

			}
		}


		if(Input.GetKey(KeyCode.O))
		{
            SceneManager.LoadScene (SceneManager.GetSceneAt(0).buildIndex - 1);
        }        
        if(Input.GetKey(KeyCode.P)){
            SceneManager.LoadScene (SceneManager.GetSceneAt(0).buildIndex + 1);
        }
		//For sprite
		if(death == 0){
			sr.sprite = maxSad;
		}
		if(death == 1){
			sr.sprite = sad;
		}
		if(death >= 2 && death <= 4){
			sr.sprite = neutral;
		}
		if(death == 5){
			sr.sprite = happy;
		}
		if(death == 6){
			sr.sprite = maxHappy;
		}

		//settings
		if (!restartLevel) 
		{

			if(Input.GetKey(KeyCode.R))
			{
				tag = "Dead";
				restartLevel = true;
				RestartLevel ();	
			}
			if(Input.GetKey(KeyCode.Escape))
			{
				//		Application.LoadLevel(Application.loadedLevel);	// Unity now uses SceneManager instead of Application to manage scenes
				SceneManager.LoadScene(SceneManager.sceneCount - 1);
			}



			//Add pushing bodies
			if (canPush && pushingList.Count > 0) 
			{
				CheckDeadBodyForPushing (pushingList[0]);
			}

			for (int i = 0; i < pushingList.Count; i++) 
			{
				if (pushingList [i].name != "Life") 
				{
					pushingList.Remove (pushingList[i]);
				}
			}	

			// Moving Lives' Effect
			for (int i = 0; i < liveList.Length; i++) 
			{

	//			Debug.Log (liveList [i].transform.parent );

				if (liveList [i].transform.parent == lifeTr) 
				{
					if (!lifeJumping[i]) 
					{
	//					Debug.Log (isFalling);
						//Debug.Log ("Death" + death);
						if(Mathf.Abs(liveList[i].transform.position.y - this.transform.position.y ) < 5f)
						{
							liveList [i].transform.position = new Vector3 (this.transform.position.x + (i + 2) * 3f * direction, 
								liveList[i].transform.position.y, 0);
						}
						else
						{
							DOTween.Pause (liveList[i].transform);
							lifeJumping[i] = true;

							if(Mathf.Abs(liveList[i].transform.position.y - this.transform.position.y ) < 5f)
							{
								liveList [i].transform.position = new Vector3 (this.transform.position.x + (i + 2) * 3f * direction, 
									liveList[i].transform.position.y, 0);
							}
							else
							{
								DOTween.Pause (liveList[i].transform);
								lifeJumping[i] = true;

							}
						}
					}
					else
					{
						
						if (i - death == 0) 
						{
							liveList [i].transform.position = Vector3.Lerp(liveList[i].transform.position, 
								new Vector3(this.transform.position.x + (i + 2 - death) * 3f * direction, this.transform.position.y - jumpingDis, 0), 0.1f);
						}
						else
						{

							liveList [i].transform.position = Vector3.Lerp(liveList[i].transform.position, 
								new Vector3(liveList[i - 1].transform.position.x + 3f * direction, liveList[i - 1].transform.position.y, 0), 0.1f);
						}


						if(Mathf.Abs(liveList[i].transform.position.y - this.transform.position.y) < jumpingDis + 0.01f &&
							Mathf.Abs(liveList[i].transform.position.x - this.transform.position.x) < (i + 2 - death) * 3 + 0.01f)
						{
							lifeJumping[i] = false;
							if (!DOTween.IsTweening (liveList [i].transform, true)) 
							{
								liveList [i].transform.DOMoveY(liveList[i].transform.position.y + 2, 0.3f)
									.SetEase (Ease.InSine)
									.SetLoops (-1, LoopType.Yoyo)
									.SetDelay (Random.Range (0, 1f));
							}
						}
					}
				}
			}

		}
	
		float camX, camY;
		camX = this.transform.position.x;
		camY = this.transform.position.y;

		if(camY < MinCamY){
			camY = MinCamY;
		}
		if (camY > MaxCamY) {
			camY = MaxCamY;
		}
		if (camX > MaxCamX){
			camX = MaxCamX;
		}
		if(camX < MinCamX){
			camX = MinCamX;
		}
		if(cameraFollow)
			Camera.main.transform.position = new Vector3 (camX, camY, -10);
		//if(!levelClear) 
		PlayAnimation ();

	}


	private void PlayAnimation()
	{
		if (isFalling) 
		{
			this.transform.GetComponent<Animator>().Play("Jump");
		}
		else
		{
//			Debug.Log ("Play");
			this.transform.GetComponent<Animator>().Play("Idle");
		}
	}

	private void CheckDeadBodyForPushing(GameObject _pushgo)
	{
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

		// Character Rotation
		this.GetComponent<Collider2D>().isTrigger = true;

		cameraFollow = false;
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
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
		//Debug.Log(death %maxLives);
		int i = death -1;

		if (i >= 0) {
			liveList [i].GetComponent<BoxCollider2D> ().enabled = true;
			liveList [i].GetComponent<BoxCollider2D> ().isTrigger = false;
		}

		//death++;
		//Reset Data

		ResetData ();

		yield break;
	}

	private IEnumerator FadeOut()
	{

		waitForRestart = WaitForRestart ();
		StartCoroutine (waitForRestart);
		death = 0;
		float t = 0.0f;
		float startAlpha = 0.0f;
		float endAlpha = 1.0f;
		float time = 1.5f;
		while (t < time) {
			t += Time.deltaTime;
			/*
			alpha -= fadeDir * fadeSpeed * Time.deltaTime;
			alpha = Mathf.Clamp01(alpha);
			*/
			float alp = Mathf.Lerp(startAlpha, endAlpha, t);
			Color col = fadeImage.GetComponent<SpriteRenderer>().color;
			col.a = alp;
			fadeImage.GetComponent<SpriteRenderer>().color = col;

			yield return null;
		}
		t = 0.0f;

		while (t < 1.5f) {
			t += Time.deltaTime;
			/*
			alpha -= fadeDir * fadeSpeed * Time.deltaTime;
			alpha = Mathf.Clamp01(alpha);
			*/
			float alp = Mathf.Lerp(endAlpha, startAlpha, t/1.0f);
			Color col = fadeImage.GetComponent<SpriteRenderer>().color;
			col.a = alp;
			fadeImage.GetComponent<SpriteRenderer>().color = col;
			canPush = false;
			isFalling = false;
			yield return null;
		}

		yield break;
	}


	private void LevelClear()
	{
		//Debug.Log ("Die on the goal hazard");
		victoryCheer.Play();
		levelClear = true;
		nextLevel.position = this.transform.position;
		nextLevel.gameObject.SetActive(true);
		nextLevel.DOMove(new Vector3(this.transform.position.x, this.transform.position.y + 5, 0), 0.5f);
		nextLevel.GetComponent<SpriteRenderer> ().DOColor (new Color32(232, 178, 178, 250), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
		//SceneManager.LoadScene (SceneManager.GetSceneAt(0).buildIndex + 1);
	}

	private void Death(GameObject _go)			// No matter colliders which kinds of hazards, must call this function to creat a dead body
	{

		//this.GetComponent<SpriteRenderer>().color = col.gameObject.GetComponent<SpriteRenderer>().color;
		if(maxLives != 0){
		if(!collideWithHazard)
		{					
			liveList[death % maxLives].GetComponent<SpriteRenderer>().color = _go.GetComponent<SpriteRenderer>().color;
			liveList [death % maxLives].transform.localScale =  new Vector3 (0.8f, 0.8f, 1); //this.transform.localScale;
			liveList [death % maxLives].transform.parent = deadBodiesTr;
			DOTween.Pause (liveList [death % maxLives].transform);

			liveList[death % maxLives].transform.position = this.transform.position;
			liveList[death % maxLives].GetComponent<BoxCollider2D> ().isTrigger = true;
			this.tag = "Dead";
			collideWithHazard = true;
			canMove = false;
			if(death >= maxLives)
			{
				restartLevel = true;
				RestartLevel ();
			}
			else {
				waitForRestart = WaitForRestart ();
				StartCoroutine (waitForRestart);
				death++;
			}
		}
		}
		else {
			tag = "Dead";
			this.GetComponent<BoxCollider2D>().isTrigger = true;
			restartLevel = true;
			RestartLevel ();
		} 

	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		//Debug.Log(collision.transform.name);



		if (!levelClear)
		{
			if (collision.transform.parent != null && collision.transform.parent.name == "IceBallBases") 
			{
				freezeSound.Play();
				Debug.Log("hi");
				collision.transform.GetComponent<IceBall> ().PauseTween (collision.transform);
				Death (collision.transform.GetChild(0).gameObject);
			}

			if (collision.transform.tag == "GoalHazard" && this.tag == "Alive") 
			{
				//victoryCheer.Play();
				collision.transform.GetComponent<SpriteRenderer> ().color = new Color32 (0, 0, 0, 255);
				LevelClear ();
			}

			else if (collision.transform.parent != null && collision.transform.parent.name == "CrushingRects") 
			{
				var normal =  collision.contacts[0].normal;
				if (normal.y < 0)
				{ //if player's top side hits something 

					crushSound.Play();
					//	collision.transform.GetComponent<CrushingRect> ().PauseDoMove ();
					//	collision.transform.GetComponent<CrushingRect> ().GoBack ();
					Death (collision.gameObject);
					//	collision.transform.GetComponent<CrushingRect> ().UpdateFallingDistance(collision.transform.position.y);  
				} 
			}

			if (collision.transform.tag == "Ground") 
			{
				var normal = collision.contacts[0].normal;
				if (normal.y > 0) 
				{ //if the bottom side hit something 
					isFalling = false;
					//	collision.gameObject.GetComponent<BoxCollider2D>().enabled = true;
				}
			}


			//Debug.Log (collision.transform.name);
			if (collision.transform.name == "DeadBodies") 
			{
				for (int i = 0; i < collision.transform.childCount; i++) 
				{
					if (collision.transform.GetChild (i).GetComponent<BoxCollider2D> ().IsTouching (this.transform.GetComponent<BoxCollider2D> ())) 
					{
						var normal =collision.contacts[0].normal;
						if (normal.y != 1) {
							if(collision.transform.GetChild(i).name != "BodyGround" &&
								Mathf.Abs (collision.transform.GetChild (i).position.y - this.transform.position.y) < this.GetComponent<SpriteRenderer> ().bounds.size.y - 0.1f) 
							{
								canPush = true;
								//Debug.Log (canPush);
								pushingList.Add(collision.transform.GetChild (i).gameObject);
								CheckDeadBodyForPushing (pushingList[0]);
							}
						}
						//		pushBodyGO = collision.gameObject.GetComponent<BoxCollider2D>().gameObject;
					}

				}


			}


			//Debug.Log(collision.transform.parent.name);


			
		}


	}

	void OnTriggerEnter2D(Collider2D col)
	{
		
		if (col.transform.tag == "GoalHazard" && this.tag == "Alive") 
		{
			victoryCheer.Play();
			col.GetComponent<SpriteRenderer> ().color = new Color32 (0, 0, 0, 255);
			LevelClear ();
		}


		if (!levelClear) 
		{

			if(col.transform.parent != null && col.transform.parent.name == "Spikes")
			{
				//Destroy(col.gameObject);
				impaledSound.Play();
				//StartCoroutine (waitSeconds (2.0f));

				Death (col.gameObject);

				//Life1.transform.position = playerPos;
			}


			if(col.transform.tag == "Checkpoint" && this.tag == "Alive")
			{
				startPos = col.transform.position; 
				col.GetComponent<SpriteRenderer>().color = new Color32 (255, 255, 0, 255);

				if (col.transform.GetChild(0).tag != "Activated"){
					death = 0;
					col.transform.GetChild(0).tag = "Activated";
					for (int i = death; i < liveList.Length; i++) 	// dead boides back to the defual size and color
					{
						liveList [i].transform.position = startLife [i];
						liveList [i].transform.parent = lifeTr;
						liveList [i].transform.localScale = new Vector3 (0.8f, 0.8f, 1);
						liveList [i].GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
						liveList [i].GetComponent<BoxCollider2D> ().enabled = false;
						lifeJumping [i] = false;
						liveList [i].transform.position = new Vector3(this.transform.position.x + (i + 2 - death) * 3f * direction, 
							this.transform.position.y - jumpingDis, 0);
						liveList [i].transform.DOMoveY(liveList[i].transform.position.y + 2, 0.3f)
							.SetEase (Ease.InSine)
							.SetLoops (-1, LoopType.Yoyo)
							.SetDelay (Random.Range (0, 1f));

					}
				}

			}

			// -------------------------- Credit
			if (col.name == "Credit") 
			{
				col.GetComponent<SpriteRenderer> ().DOFade (1, 0.2f);
			}

			// -------------------------- Title

			if (col.name == "A" || col.name == "D" || col.name == "Space") 
			{
				Gudie._instance.guideProgress++;
				Gudie._instance.ShowNewGuide ();
			}
		}

	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if (!levelClear) 
		{
			if (collision.transform.tag == "Ground") 
			{
				var normal = collision.contacts[0].normal;
				if (normal.y > 0) 
				{ //if the bottom side hit something 
					isFalling = false;
					jumpTimer = 0;
					curJumpSpeed = Vector2.zero;
					//	collision.gameObject.GetComponent<BoxCollider2D>().enabled = true;
				}
			}
		}


	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if(!levelClear)
		{
			if (collision.transform.name == "DeadBodies") 
			{
				for (int i = 0; i < collision.transform.childCount; i++) 
				{
					if (!collision.transform.GetChild (i).GetComponent<BoxCollider2D> ().IsTouching (this.transform.GetComponent<BoxCollider2D> ()))
					{
						canPush = false;
						pushingList.Clear ();	
					}	
				}

			}

			if(collision.transform.tag == "Ground")
			{
				isFalling = true;	
			}

		}


	}
}
