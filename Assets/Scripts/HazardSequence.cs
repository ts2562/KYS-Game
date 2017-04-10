﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* This script is used for setting current scene's goal hazard sequence*/


public class HazardSequence : MonoBehaviour 
{
	public Transform[] hazardTrList;


	// Use this for initialization
	void Start()
	{
		for (int i = 0; i < hazardTrList.Length; i++) 
		{
			hazardTrList[i].tag = "GoalHazard";	// when player collides something, check the tag
		}
		
	}
}