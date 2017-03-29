using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceController : MonoBehaviour 
{

	private Transform[] iceTr;
	public Vector3[] startPosition;
	// Use this for initialization
	void Start () 
	{
		iceTr = new Transform[this.transform.childCount];
		for (int i = 0; i < iceTr.Length; i++) 
		{
			iceTr [i] = this.transform.GetChild (i);
		}


	}
	
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < iceTr.Length; i++) 
		{
			iceTr [i].position += Vector3.left * 8.0f * Time.deltaTime;
			if(iceTr [i].position.x <= -45f)
			{
				iceTr [i].position = startPosition [i];
			}
		}
		
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.name == "DeadBody_1") 
		{
			for (int i = 0; i < iceTr.Length; i++) 
			{
				if(iceTr[i].GetComponent<BoxCollider2D>().IsTouching(col))
				{
					iceTr [i].position = startPosition [i];
				}
			}

		}
	}
}
