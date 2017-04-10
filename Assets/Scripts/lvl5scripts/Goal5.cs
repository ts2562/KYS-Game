using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal5 : MonoBehaviour {
	public GameObject Player;
	// Use this for initialization
	Color[] colors = new Color[2];
	int endColor;
	void Start () {
		endColor = (int)Random.Range(0,2);
		colors[0] = new Color (0.133f, 0.4f, 0.4f);
		colors[1] = new Color (0.667f, 0.224f, 0.224f);
		gameObject.GetComponent<SpriteRenderer>().color = colors[endColor];
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.name == "Player"){
			/*Debug.Log(col.gameObject.GetComponent<SpriteRenderer>().color);
			Debug.Log( colors[endColor]);
			if(col.gameObject.GetComponent<SpriteRenderer>().color.Equals( colors[endColor]))
				Application.LoadLevel("Level6");
			else
				col.gameObject.transform.position = new Vector3(-27.8f, -4.9f, 0.0f);
				*/
			Application.LoadLevel("Level6");
		}

	}
}
