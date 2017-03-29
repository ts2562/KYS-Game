using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour 
{


	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.R) || Input.GetKeyDown (KeyCode.Space))
		{
			GameManager.gameProgress = GameManager.GAMEPROGRESS.reset;
		}
	}
}
