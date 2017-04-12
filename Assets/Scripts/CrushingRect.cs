using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CrushingRect : MonoBehaviour 
{
	public float fallingDistance;
	private Vector3 spawnPos;
	// Use this for initialization
	void Start () 
	{
		spawnPos = this.transform.position;
		FallDown ();
	}

	public void FallDown()
	{
		//DoTween, creates fall down and go back movement loop -- Fall
//		Debug.Log(fallingDistance);
		this.transform.DOMove (new Vector3(this.transform.position.x, this.transform.position.y - fallingDistance, 0), 0.6f).SetEase(Ease.InExpo).OnComplete(GoBack);

	}

	public void GoBack()
	{
		//DoTween, creates fall down and go back movement loop -- Back
		this.transform.DOMove (spawnPos, 1f).SetEase(Ease.Linear).OnComplete(FallDown);
	}

	public void PauseDoMove()
	{
		Debug.Log ("Pause");
		DOTween.Pause (this.transform);
	}

	public void UpdateFallingDistance(float _y)
	{
		fallingDistance = spawnPos.y - _y;
	}
}
