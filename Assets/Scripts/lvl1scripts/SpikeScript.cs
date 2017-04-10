using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour {

	public GameObject Player;
	public PlayerScript playerScript;
	//public GameObject Life1;
	public GameObject[] spriteList;
	public Color color;
	private bool collide;
	//public AudioSource audio;
	Vector3 start = new Vector3(-83.9f, -16.0f, 0);
	Vector3 playerPos;
	// Use this for initialization
	void Start () {
		color = gameObject.GetComponent<SpriteRenderer>().color;
		collide = false;
	}
	
	// Update is called once per frame
	void Update () {
		playerPos = new Vector3(Player.transform.position.x,Player.transform.position.y,0);
	}
	//Doesn't work for some reason
	IEnumerator waitSeconds(float time){
		yield return new WaitForSecondsRealtime(time);
	}

	void OnTriggerEnter2D(Collider2D col){
    /* 	if(col.gameObject.name == "Player"){
     		//Destroy(col.gameObject);
     		//audio.Play();
			//StartCoroutine (waitSeconds (2.0f));
			if(!collide){															// this cannot check whether player's body has collided something because each script is an istance, they own their own collider. 
			StartCoroutine(waitSeconds(2.0f));
			Player.transform.position = playerScript.startPos;
			Player.GetComponent<SpriteRenderer>().color = color;

				collide = true;
				spriteList[playerScript.death%4].transform.position = playerPos;
				playerScript.death++;

			}
     		//Life1.transform.position = playerPos;
     	}*/
     }
}
