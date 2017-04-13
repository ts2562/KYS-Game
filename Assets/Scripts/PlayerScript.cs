using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

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


	public GameObject fadeImage;

	//Life and Death
	public GameObject[] liveList;
	private Vector3[] startLife;
	public Transform lifeTr;
	public Transform deadBodiesTr;
	public int death = 0;
	private float direction;


	private GameObject crush;
	public AudioSource audio;
	//Coroutine
	private IEnumerator waitForRestart;
	private IEnumerator fadeOut;

	//Pushing

	private bool cameraFollow;
	//height correction
	//private Vector3 correction;
	 
	void Start () 
	{
		startPos = new Vector3(transform.position.x, transform.position.y, 0);
		cameraFollow = true;
		crush = GameObject.Find("CrushingRect");
		collideWithHazard = false;
		canMove = true;
//		canPush = false;
//		pushBodyGO = null;
		pushingList = new List<GameObject>();
		startLife = new Vector3[liveList.Length];
		//correction = new Vector3(0f,1.85f,0f);
		for (int i = 0; i < liveList.Length; i++)
		{
		//	liveList [i].transform.localScale = new Vector3 (0.8f, 0.8f, 0);
			liveList [i].transform.position = new Vector3(this.transform.position.x + (i + 2) * 3f, this.transform.position.y, 0);
//			Debug.Log (	liveList [i].transform.localPosition.y);

			//	liveList [i].transform.localPosition = new Vector3(Mathf.Sin(i * 60 * Mathf.Deg2Rad) * 1.2f, Mathf.Cos(i * 60 * Mathf.Deg2Rad) * 1.2f, 0);
			liveList [i].transform.DOMoveY (this.transform.position.y + 2, 0.3f)
				.SetEase (Ease.InSine)
				.SetLoops (-1, LoopType.Yoyo)
				.SetDelay (Random.Range (0, 1f));
		}

		for (int i = 0; i < liveList.Length; i++)
		{
			startLife[i] = new Vector3(liveList[i].transform.position.x, liveList[i].transform.position.y, 0);

		}
		direction = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		//settings
		if(Input.GetKey(KeyCode.R))
		{
			fadeOut = FadeOut();
			StartCoroutine(fadeOut);
		}
		if(Input.GetKey(KeyCode.Escape))
		{
	//		Application.LoadLevel(Application.loadedLevel);	// Unity now uses SceneManager instead of Application to manage scenes
			SceneManager.LoadScene(SceneManager.sceneCount - 1);
		}

		//Add pushing bodies
		if (canPush) 
		{
			CheckDeadBodyForPushing (pushingList[0]);
		}

		// Lives
		for(int i = death; i < liveList.Length; i ++)
		{

			liveList [i].transform.position = new Vector3 (this.transform.position.x + (i + 2 - death) * 3f * direction, liveList[i].transform.position.y, 0);
			if (!collideWithHazard && !isFalling) 
			{
				if (!DOTween.IsTweening (liveList [i].transform, true)) 
				{
					DOTween.Play (liveList[i].transform);
				}

				if (i - death == 0) 
				{
					if(Mathf.Abs (liveList [i].transform.position.x - this.transform.position.x) > 6.5f ||
						Mathf.Abs(liveList[i].transform.position.y - this.transform.position.y) > 3) 
					{
						DOTween.Pause (liveList[i].transform);
						liveList [i].transform.position = new Vector3(this.transform.position.x + (i + 2) * 3f, 
							this.transform.position.y, 0);
							
						liveList [i].transform.DOMoveY (this.transform.position.y + 2, 0.2f)
							.SetEase (Ease.InSine)
							.SetLoops (-1, LoopType.Yoyo)
							.SetDelay(Random.Range(0, 1f)).SetDelay(Random.Range(0, 1));
					}
				}
				else
				{
					if(Mathf.Abs (liveList [i].transform.position.x - liveList [i - 1].transform.position.x) > 4f ||
						Mathf.Abs(liveList[i].transform.position.y -liveList [i - 1].transform.position.y) > 3) 
					{
						DOTween.Pause (liveList[i].transform);
						liveList [i].transform.position = new Vector3(this.transform.position.x + (i + 2) * 3f, 
							this.transform.position.y, 0);
						
						liveList [i].transform.DOMoveY (this.transform.position.y + 2, 0.2f).SetEase (Ease.InSine).SetLoops (-1, LoopType.Yoyo).SetDelay(Random.Range(0, 1f));
					}
					
				}


					
			}
			else
			{
				DOTween.Pause (liveList[i].transform);
			}
		}




		//Movement
		if (canMove)
		{

			if(Input.GetKey(KeyCode.A))
			{
				direction = Mathf.Lerp(direction, 1, 0.02f);
				
			//	if (transform.position.x > -150.0f)
					transform.position += Vector3.left * speed * 0.03f;
				//Pushing
				if (canPush) 
				{
					if (this.transform.position.x >= pushingList [0].transform.position.x) 
					{
						for (int i = 0; i < pushingList.Count; i++) 
						{
							pushingList[i].transform.position = new Vector3(this.transform.position.x -
									(i + 1) * this.transform.GetComponent<SpriteRenderer>().bounds.size.x,
							pushingList[i].transform.position.y, 0);
						//	pushingList[i].GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0);
						}
					}
				
				}
			}
			if(Input.GetKey(KeyCode.D))
			{
				direction = Mathf.Lerp(direction, -1, 0.02f);
			//	if (transform.position.x < 150.0f)
					transform.position += Vector3.right * speed * 0.03f;
				
				if (canPush) 
				{
					if (this.transform.position.x <= pushingList [0].transform.position.x) 
					{
						//Debug.Log (pushingList.Count);
						for (int i = 0; i < pushingList.Count; i++) 
						{
							pushingList[i].transform.position = new Vector3(this.transform.position.x +  (i + 1)  * this.transform.GetComponent<SpriteRenderer>().bounds.size.x,
								pushingList[i].transform.position.y, 0);
						//	pushingList [i].GetComponent<Rigidbody2D> ().velocity = new Vector2(1, 0);
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
		float camX, camY;
		camX = this.transform.position.x;
		camY = this.transform.position.y;
	
		if(camY < -15f){
			camY = -15f;
		}
		if (camY > 8.0f) {
			camY = 8.0f;
		}
		if (camX > 2){
			camX = 2;
		}
		if(camX < 2){
			camX = 2;
		}
//		Debug.Log(camY);
		if(cameraFollow)
			Camera.main.transform.position = new Vector3 (camX, camY, -10);

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
		//yield return new WaitForSeconds(1.0f);
//		Debug.Log ("Wait");
		liveList [death % 6].GetComponent<BoxCollider2D> ().enabled = true;
		liveList [death % 6].GetComponent<BoxCollider2D> ().isTrigger = false;
	
		this.transform.position = startPos;
		collideWithHazard = false;
		canMove = true;
		death++;
		this.GetComponent<Collider2D>().isTrigger = false;
		pushingList.Clear();
		cameraFollow = true;
		direction = 1;
		yield break;
	}

	private IEnumerator FadeOut() {
		float t = 0.0f;
		float startAlpha = 0.0f;
		float endAlpha = 1.0f;
		while (t < 1.0f) {
			t += Time.deltaTime;
			/*
			alpha -= fadeDir * fadeSpeed * Time.deltaTime;
			alpha = Mathf.Clamp01(alpha);
			*/
			float alp = Mathf.Lerp(startAlpha, endAlpha, t/1.0f);
			Color col = fadeImage.GetComponent<SpriteRenderer>().color;
			col.a = alp;
			fadeImage.GetComponent<SpriteRenderer>().color = col;

			yield return null;
		}
		t = 0.0f;
		death = 0;
		this.transform.position = startPos;
//		crush.GetComponent<CrushingRect>().ResetFallingDistance();
		//Debug.Log(crush.GetComponentInChildren<CrushingRect>().fallingDistance);
		direction = 1;
		for (int i = 0; i < liveList.Length; i++){
			liveList[i].transform.position = startLife[i];
			liveList[i].transform.parent = lifeTr;
			liveList [i].transform.localScale = new Vector3 (1, 1, 1);
			liveList [i].GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);

			//Debug.Log(liveList[i].transform.position.x);
		}
		while (t < 1.0f) {
			t += Time.deltaTime;
			/*
			alpha -= fadeDir * fadeSpeed * Time.deltaTime;
			alpha = Mathf.Clamp01(alpha);
			*/
			float alp = Mathf.Lerp(endAlpha, startAlpha, t/1.0f);
			Color col = fadeImage.GetComponent<SpriteRenderer>().color;
			col.a = alp;
			fadeImage.GetComponent<SpriteRenderer>().color = col;

			yield return null;
		}

		yield break;
	}

	private void Death(GameObject _go)			// No matter colliders which kinds of hazards, must call this function to creat a dead body
	{
		//this.GetComponent<SpriteRenderer>().color = col.gameObject.GetComponent<SpriteRenderer>().color;
		if(!collideWithHazard)
		{					
			liveList[death % 6].GetComponent<SpriteRenderer>().color = _go.GetComponent<SpriteRenderer>().color;
			liveList [death % 6].transform.localScale = this.transform.localScale;
			liveList [death % 6].transform.parent = deadBodiesTr;
			DOTween.Pause (liveList [death % 6].transform);
			
			liveList[death % 6].transform.position = this.transform.position;
			liveList[death % 6].GetComponent<BoxCollider2D> ().isTrigger = true;
			waitForRestart = WaitForRestart ();
			StartCoroutine (waitForRestart);
			collideWithHazard = true;
			canMove = false;
			if(death >= liveList.Length)
			{
				fadeOut = FadeOut();
				StartCoroutine(fadeOut);
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
			//	collision.transform.GetComponent<CrushingRect> ().PauseDoMove ();
			//	collision.transform.GetComponent<CrushingRect> ().GoBack ();
				Death (collision.gameObject);
			//	collision.transform.GetComponent<CrushingRect> ().UpdateFallingDistance(collision.transform.position.y);  
			} 
		}

		if (collision.transform.parent.name == "IceBallBases") 
		{
			audio.Play();

			collision.transform.GetComponent<IceBall> ().PauseTween (collision.transform);
			Death (collision.transform.GetChild(0).gameObject);
		}

		if (collision.transform.parent.name == "DeadBodies") 
		{
			if (Mathf.Abs (collision.transform.position.y - this.transform.position.y) < this.GetComponent<SpriteRenderer> ().bounds.size.y - 0.1f) 
			{
				
				canPush = true;
				//Debug.Log (canPush);
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
		if (collision.transform.parent.name == "DeadBodies") 
		{
			canPush = false;
/*			for (int i = 0; i < pushingList.Count; i++) 
			{
				pushingList [i].GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			}*/
//			pushBodyGO = null;
			pushingList.Clear();	
		}

		if(collision.transform.tag == "Ground")
		{
			isFalling = true;	
		}

	}
}
