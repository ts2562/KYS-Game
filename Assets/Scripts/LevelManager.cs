using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
	public Vector3 playerStartPosition;
	public int[] maxDeadBodyNum; //key:body's id, value: max allowed number
	public int[] createdDeadBodyNum;

	// Use this for initialization
	void Start()
	{
		createdDeadBodyNum = new int[maxDeadBodyNum.Length];
		for (int i = 0; i < createdDeadBodyNum.Length; i++) 
		{
			createdDeadBodyNum [i] = 0;
		}
	}
}
