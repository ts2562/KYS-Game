using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Control the gaming status*/

public class GameManager : MonoBehaviour 
{
	public static GAMEPROGRESS gameProgress;
	public static bool haveStartedGame;
	public enum GAMEPROGRESS {start = 0, guide, prepare, gaming, reset, wait, other};

	private Gameplay gamePlay;

	// Use this for initialization
	void Start () 
	{
		gameProgress = GAMEPROGRESS.prepare;
		gamePlay = this.transform.GetComponent<Gameplay> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(gameProgress == GAMEPROGRESS.prepare)
		{
			GameScenesManager.InitData ();
			gamePlay.InitData ();
			gamePlay.SetData ();
			gameProgress = GAMEPROGRESS.gaming;
		}
		else if(gameProgress == GAMEPROGRESS.gaming)
		{
			gamePlay.UpdateData ();
		}
		else if(gameProgress == GAMEPROGRESS.reset)
		{
			gamePlay.SetData ();
	//		GameScenesManager.ResetData ();
			gameProgress = GAMEPROGRESS.gaming;
		}
		else if(gameProgress == GAMEPROGRESS.wait)
		{
			
		}
		else if(gameProgress == GAMEPROGRESS.other)
		{
			if (Input.GetKeyDown (KeyCode.Escape))
			{
				Application.Quit ();
			}
		}
		
	}
}
