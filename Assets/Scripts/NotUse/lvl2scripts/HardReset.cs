using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardReset : MonoBehaviour {


	void Start () {
		
	}

	void Update () {
		if(Input.GetKey(KeyCode.Escape)){
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
