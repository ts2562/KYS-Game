using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CrushingRect : MonoBehaviour 
{
//	public float fallingDistance;		//default
	private Vector3 spawnPos;

	private float curfallingDistance;
	// Use this for initialization
	void Start () 
	{
		spawnPos = this.transform.position;
//		curfallingDistance = fallingDistance;
//		FallDown ();
	}

/*	public void FallDown()
	{
		//DoTween, creates fall down and go back movement loop -- Fall
//		Debug.Log(fallingDistance);
	//	this.transform.DOMove (new Vector3(this.transform.position.x, this.transform.position.y - curfallingDistance, 0), 0.6f).SetEase(Ease.InExpo).OnComplete(GoBack);

	}*/

	public void GoBack()
	{
//		DoTween, creates fall down and go back movement loop -- Back
		this.transform.DOMove (spawnPos, 1f).SetEase(Ease.OutExpo);
	}

	public void PauseDoMove()
	{
		Debug.Log ("Pause");
		DOTween.Pause (this.transform);
	}

/*	public void UpdateFallingDistance(float _y)
	{
		curfallingDistance = spawnPos.y - _y;
	}

	public void ResetFallingDistance ()
	{
		curfallingDistance = fallingDistance;
	} */


	void OnCollisionEnter2D(Collision2D collision)
	{
		var normal =  collision.contacts[0].normal;
		if (normal.y > 0 || normal.y < 0)
		{ 
			GoBack ();
			//if player's top side hits something 

		//	audio.Play();
		//	collision.transform.GetComponent<CrushingRect> ().PauseDoMove ();
		//	collision.transform.GetComponent<CrushingRect> ().GoBack ();

		//	collision.transform.GetComponent<CrushingRect> ().UpdateFallingDistance(collision.transform.position.y);  
		}
	} 	
}
