using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PounderController : MonoBehaviour
{

	// Use this for initialization
	private Transform[] PounderTr;
	private Vector3[] startPosition;
	private int flo = 0;
	// Use this for initialization
	void Start () 
	{
		PounderTr = new Transform[this.transform.childCount];
		startPosition = new Vector3[this.transform.childCount];
		for (int i = 0; i < PounderTr.Length; i++) 
		{
			PounderTr [i] = this.transform.GetChild (i);
			startPosition[i] = PounderTr [i].position;

		}


	}

	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < PounderTr.Length; i++) 
		{
			PounderTr[i].position = new Vector3(PounderTr[i].position.x, startPosition[i].y + (float)(Mathf.Sin(Time.time) * (30 + flo)), 0);
		}

	}

}
