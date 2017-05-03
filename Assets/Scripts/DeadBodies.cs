using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodies : MonoBehaviour 
{
	private PlayerScript player;
	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find ("Player").GetComponent<PlayerScript> ();
	}


	void OnCollisionEnter2D(Collision2D _col)
	{
		if (_col.transform.tag == "Ground") 
		{
 
			for (int i = 0; i < this.transform.childCount; i++) 
			{
				if (this.transform.GetChild (i).GetComponent<BoxCollider2D> ().IsTouching (_col.collider) && _col.collider.name != "Life") 

				{
					Debug.Log (i);
					Vector2 normal = _col.contacts [0].normal;
					if (normal.x != 0) 
					{
						this.transform.GetChild (i).transform.name = "BodyGround";
						Debug.Log (i + "  " + this.transform.GetChild(i).name);
						player.pushingList.Remove (this.transform.GetChild(i).gameObject);
					}
				}
			}
		}
	}

	void OnCollisionStay2D(Collision2D _col)
	{
		if (_col.transform.tag == "Ground" ) 
		{

			for (int i = 0; i < this.transform.childCount; i++) 
			{
				if (this.transform.GetChild (i).GetComponent<BoxCollider2D> ().IsTouching (_col.collider) && _col.collider.name != "Life") 
				{
					Vector2 normal = _col.contacts [0].normal;
					//					Debug.Log (normal);
					if (normal.x != 0) 
					{
						this.transform.GetChild (i).transform.name = "BodyGround";

						player.pushingList.Remove (this.transform.GetChild(i).gameObject);
					}
				}


			}
		}
	}

	void OnCollisionExit2D(Collision2D _col)
	{
		if (_col.transform.tag == "Ground") 
		{
			for (int i = 0; i < this.transform.childCount; i++) 
			{

				if (!this.transform.GetChild (i).GetComponent<BoxCollider2D> ().IsTouching (_col.collider)) 
				{
					Debug.Log (this.transform.GetChild(i).transform.name);
					this.transform.GetChild (i).transform.name = "Life";
				}

			}
		}
	}
}
