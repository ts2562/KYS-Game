using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour 
{

	public float moveSpeed;
	public float minX;
	public float maxX;
	private Transform[] cloudTr;


	// Use this for initialization
	void Start ()
	{
		cloudTr = new Transform[this.transform.childCount];

		for (int i = 0; i < this.transform.childCount; i++) 
		{
			cloudTr [i] = this.transform.GetChild (i);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < cloudTr.Length; i++) 
		{
			cloudTr [i].position = new Vector3 (cloudTr[i].position.x - moveSpeed, cloudTr[i].position.y, 0);
			if(cloudTr[i].position.x < minX)
			{
				cloudTr [i].position = new Vector3 (maxX, cloudTr[i].position.y, 0);
			}
		}
	}
}
