using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//The Gameobject owns this script will be an ice ball spawn base. It will create ice ball by following config data
public class IceBall : MonoBehaviour 
{
	public float creatBallTime;		//time gap between creates two balls

	public Vector2 moveingDistance;	// Default ice ball's move distance, can be changed when it spawn
	public float moveTime;			// Default ice ball's move time, can be changed when it spawn

	private Object iceBallOB;
	private float creatBallTimer;
	// Use this for initialization
	void Start () 
	{
		creatBallTimer = 0;
		iceBallOB = Resources.Load ("Prefabs/IceBall");
	}

	void Update()
	{
		creatBallTimer += Time.deltaTime;
		if (creatBallTimer > creatBallTime) 
		{
			GameObject new_go = Instantiate (iceBallOB) as GameObject;
			new_go.transform.parent = this.transform;
			new_go.name = "IceBall";
			new_go.transform.localPosition = Vector3.zero;
			new_go.transform.DOMove (new Vector3(new_go.transform.position.x + moveingDistance.x,
									new_go.transform.position.y + moveingDistance.y, 0), moveTime)
									.SetEase(Ease.Linear)
									.OnComplete(()=>DestroyGO(new_go));
			creatBallTimer = 0;
		}
	}

	public void DestroyGO(GameObject _go)
	{
		Destroy (_go);
	}

	public void PauseTween(Transform _tr)
	{
		DOTween.Pause (_tr);
	}

	void OnCollisionEnter2D(Collision2D _col)
	{
		if (_col.transform.parent != null) 
		{ 
			// when the ice ball collide with a dead body or a platform, destroy this ice ball
			if (_col.transform.parent.name == "Lives" || _col.transform.parent.name == "Platforms")
			{
				for (int i = 0; i < this.transform.childCount; i++) 
				{
					if (this.transform.GetChild (i) != null && this.transform.GetChild (i).GetComponent<CircleCollider2D> ().IsTouching(_col.collider)) 
					{
						GameObject go = this.transform.GetChild (i).gameObject;
						
						PauseTween (this.transform.GetChild(i));
						go.GetComponent<CircleCollider2D> ().enabled = false;
						go.transform.DOScale (this.transform.GetChild(i).localScale * 1.5f, 0.15f).SetEase(Ease.InQuad);
						go.GetComponent<SpriteRenderer>().DOFade (0, 0.3f).SetEase(Ease.InQuad).OnComplete(()=>DestroyGO(go));
					}
				}
			}

		}
	}
}
