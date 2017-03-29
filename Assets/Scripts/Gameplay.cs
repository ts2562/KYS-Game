using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour 
{
	private PlayerScript player;


	// Use this for initialization
	public void InitData ()  //Init once when the game runs
	{
		player = this.transform.FindChild("Player").GetComponent<PlayerScript> ();


		player.InitData ();

	}

	public void SetData()	// Set or reset Value
	{
		player.SetGameData ();

	}
	
	// Update is called once per frame
	public void UpdateData()	
	{
		player.UpdateData ();
	}
}
