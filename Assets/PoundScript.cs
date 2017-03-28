using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoundScript : MonoBehaviour {

public GameObject Life;
public GameObject Player;

float originalY;
Vector3 start = new Vector3(-29.2f, -14.785f, 0);
public float floatStrength = 30;
public float lifeStart = -18.5f;
int flo = 0;
public AudioSource audio;

Vector3 playerPos;
void Start ()
{
    originalY = transform.position.y;
}

void Update () {
    playerPos = new Vector3(Player.transform.position.x,lifeStart,0);
    transform.position = new Vector3(transform.position.x, originalY + (float)(Math.Sin(Time.time) * (30 + flo)), 0);
}

void OnTriggerEnter2D(Collider2D col){
     	if(col.gameObject.name == "Player"){
     		//Destroy(col.gameObject);
     		audio.Play();
            Life.transform.position = playerPos;
     		Player.transform.position = start;
     		//flo -= 10;
     		//transform.localScale = new Vector3(3.843178f, transform.localScale.y / 1.5f, 0);

     	}
     }
}
