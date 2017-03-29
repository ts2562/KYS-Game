using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Control Actions and Events of the Avatar*/


public class PlayerScript : MonoBehaviour {

	public float speedX = 20.0f;
//	public Vector2 jumpHeight;
//	public GameObject Life1;
	public Vector2 addForce;
	public bool isFalling = false;
	public int deadbodyType;

//	public GameObject SpeedPlat1;
//	public GameObject SpeedPlat2;
//	public GameObject SpeedPlat3;

	private Vector3 startPos; 
	private Object[] deadbodyOB;


	private bool canCreateDeadbody;	// avoid create muti bodies one time;
	private bool canMove;

	IEnumerator updateLevelInfor;


	//Relate scripts
	private LevelManager level;

	private CameraControl camera;

	public void InitData() 
	{
		this.transform.GetComponent<BoxCollider2D> ().enabled = false;
		deadbodyOB = new Object[deadbodyType];
		for (int i = 0; i < deadbodyType; i++) 
		{
			deadbodyOB[i] = Resources.Load ("Prefabs/DeadBody_" + i.ToString());
		}


		camera = Camera.main.GetComponent<CameraControl> ();
		camera.InitData ();
	}

	public void SetGameData()	
	{
		
		canCreateDeadbody = true;
		canMove = false;

		camera.SetData ();

		updateLevelInfor = UpdateLevelInfor ();
		StartCoroutine (updateLevelInfor);
	}

	// Update is called once per frame
	public void UpdateData()
	{
		Movement ();
	}
/*
	void OnCollisionEnter2D(){
		isFalling = false;
	}
	*/


	private void Movement()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			SetGameData ();
		}

		if (canMove) 
		{
			if(Input.GetKey(KeyCode.A)){
				transform.position += Vector3.left * speedX * Time.deltaTime;
			}
			if(Input.GetKey(KeyCode.D)){
				transform.position += Vector3.right * speedX * Time.deltaTime;
			}
			if (Input.GetKeyDown(KeyCode.Space) && isFalling == false)  //make a limit to how many times player can jump later
			{
//				Debug.Log(transform.position.y);

				GetComponent<Rigidbody2D>().AddForce(addForce, ForceMode2D.Impulse);
				isFalling = true;
			}
		}

	}

	IEnumerator UpdateLevelInfor()
	{
		yield return 0.2f;
		Transform root = GameScenesManager.GetLevelSceneRoot ();
		level = root.GetComponent<LevelManager> ();
		startPos = level.playerStartPosition;
		this.transform.position = startPos;
		this.GetComponent<BoxCollider2D> ().enabled = true;
		yield break;
	}

	void OnCollisionEnter2D(Collision2D _other)
	{
		if (_other.contacts[0].normal.y > 0) 
		{ //if the bottom side hit something 
			Debug.Log ("You Hit the floor");
			isFalling = false;
		}

		if (_other.collider.name == "Start" || _other.transform.parent.parent.name == "Deadbody") 
		{
			canMove = true;
			canCreateDeadbody = true;
		}

		/*
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
*/
	}


	void OnTriggerEnter2D(Collider2D _other)
	{
		if (_other.name == "Spike" && canCreateDeadbody) 
		{
			Transform deadbody = GameScenesManager.GetLevelSceneRoot ().FindChild ("Deadbody");

			if (deadbody.FindChild(deadbodyOB[0].name).childCount < level.maxDeadBodyNum[0]) 
			{
				GameObject new_go = Instantiate (deadbodyOB[0]) as GameObject;
				new_go.name = deadbodyOB [0].name;
				new_go.transform.parent = deadbody.FindChild(new_go.name);
				new_go.transform.position = this.transform.position;
				level.createdDeadBodyNum [0]++;
			}
			else
			{
				deadbody.FindChild(deadbodyOB[0].name).GetChild (Random.Range(0, level.createdDeadBodyNum[0])).position = this.transform.position;
			}

			_other.GetComponent<AudioSource> ().Play ();
			this.transform.position = startPos;
			canCreateDeadbody = false;
			canMove = false;
		}

		if (_other.name == "Ice" && canCreateDeadbody) 
		{
			Transform deadbody = GameScenesManager.GetLevelSceneRoot ().FindChild ("Deadbody");

			if (deadbody.FindChild(deadbodyOB[1].name).childCount < level.maxDeadBodyNum[1]) 
			{
				GameObject new_go = Instantiate (deadbodyOB[1]) as GameObject;
				new_go.name = deadbodyOB [1].name;
				new_go.transform.parent = deadbody.FindChild(new_go.name);
				new_go.transform.position = this.transform.position;
				level.createdDeadBodyNum [1]++; 
			}
			else
			{
				deadbody.FindChild(deadbodyOB[1].name).GetChild (Random.Range(0, level.createdDeadBodyNum[1])).position = this.transform.position;
			}

			_other.GetComponent<AudioSource> ().Play ();
			this.transform.position = startPos;
			canCreateDeadbody = false;
			canMove = false;
		}


		if(_other.name == "CrushingREct")
		{
			Transform deadbody = GameScenesManager.GetLevelSceneRoot ().FindChild ("Deadbody");

			if (deadbody.FindChild(deadbodyOB[1].name).childCount < level.maxDeadBodyNum[2]) 
			{
				GameObject new_go = Instantiate (deadbodyOB[2]) as GameObject;
				new_go.name = deadbodyOB [2].name;
				new_go.transform.parent = deadbody.FindChild(new_go.name);
				new_go.transform.position = this.transform.position;
				level.createdDeadBodyNum [2]++; 
			}
			else
			{
				deadbody.FindChild(deadbodyOB[2].name).GetChild (Random.Range(0, level.createdDeadBodyNum[2])).position = this.transform.position;
			}

			_other.GetComponent<AudioSource> ().Play ();
			this.transform.position = startPos;
			canCreateDeadbody = false;
			canMove = false;

		}

		if (_other.name == "Stop") 
		{
			speedX = 15.0f;
		}
		else if(_other.name == "Deadbody_2")
		{
			speedX = 45.0f;
		}

		if (_other.name == "Goal") 
		{
			GameScenesManager.LoadNewScene ();
			SetGameData ();
		}

/*		if(col.gameObject.name == "SpeedPlat1"){
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
			}*/


	}

     
}
