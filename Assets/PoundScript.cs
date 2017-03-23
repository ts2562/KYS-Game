using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoundScript : MonoBehaviour {

public GameObject Player;
float originalY;
Vector3 start = new Vector3(-29.2f, -14.785f, 0);
public float floatStrength = 30;
int flo = 0;
public AudioSource audio;
void Start ()
{
    originalY = transform.position.y;
}

void Update () {
    transform.position = new Vector3(transform.position.x, originalY + (float)(Math.Sin(Time.time) * (30 + flo)), 0);
}

void OnTriggerEnter2D(Collider2D col){
     	if(col.gameObject.name == "Player"){
     		//Destroy(col.gameObject);
     		audio.Play();
     		Player.transform.position = start;
     		flo -= 10;
     		transform.localScale = new Vector3(3.843178f, transform.localScale.y / 1.5f, 0);
     	}
     }
}
