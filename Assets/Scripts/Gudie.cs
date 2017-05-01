using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gudie : MonoBehaviour 
{

	public static Gudie _instance;
	public Transform guideWall;
	public int guideProgress;


	// Use this for initialization
	void Start ()
	{
		_instance = this;
		guideProgress = 0;
		for(int i = 0; i < this.transform.childCount; i ++)
		{
			this.transform.GetChild (i).gameObject.SetActive (false);
		}
		ShowNewGuide ();
	}
	
	// Update is called once per frame

	public void ShowNewGuide()
	{
		for(int i = 0; i < this.transform.childCount; i ++)
		{
			if (i == guideProgress) 
			{
				this.transform.GetChild (i).gameObject.SetActive (true);
			}
			else
			{
				this.transform.GetChild (i).gameObject.SetActive (false);
			}
		}

		if (guideProgress == 3) 
		{
			guideWall.gameObject.SetActive (false);	
		}
	}
}
