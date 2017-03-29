using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Camera's behaviour*/
public class CameraControl : MonoBehaviour
{
	public Vector3 startPosition;
	public float shakeAmt = 0;

	public void InitData()
	{
	//	Camera.main.transform.position = startPosition;
	
	}

	public void SetData()
	{
		Camera.main.transform.position = startPosition;

	}

	public void ShakeCamera(Collision2D _col)
	{
		shakeAmt = _col.relativeVelocity.magnitude * .0025f;
		InvokeRepeating("CameraShake", 0, .01f);
		Invoke("StopShaking", 0.3f);
	}

	void CameraShake()
	{
		if(shakeAmt>0) 
		{
			float quakeAmt = Random.value*shakeAmt*2 - shakeAmt;
			Vector3 pp = Camera.main.transform.position;
			pp.y+= quakeAmt; // can also add to x and/or z
			Camera.main.transform.position = pp;
		}
	}

	void StopShaking()
	{
		CancelInvoke("CameraShake");
		Camera.main.transform.position = startPosition;
	}

}
