using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal6 : MonoBehaviour {
	public GameObject Player;
	// Use this for initialization
	Color[] colors = new Color[2];
	int endColor;
	void Start () {
		endColor = (int)Random.Range(0,2);
		colors[0] = new Color (0.541f, 0.18f, 0.376f);
		colors[1] = new Color (0.329f, 0.647f, 0.580f);
		gameObject.GetComponent<SpriteRenderer>().color = colors[endColor];
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.name == "Player"){
			/*
			if(col.gameObject.GetComponent<SpriteRenderer>().color == colors[endColor])
				Application.LoadLevel("EndScreen");
			else
				col.gameObject.transform.position = new Vector3(-28.8f, -4.5f, 0.0f);
			*/
			Application.LoadLevel("EndScreen");
		}

	}
}
