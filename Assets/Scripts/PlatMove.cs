using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlatMove : MonoBehaviour 
{
float originalY;

public float floatStrength = 30;
int flo = 0;


void Start ()
{
    originalY = transform.position.y;
}

void Update () {
    transform.position = new Vector3(transform.position.x, originalY + (float)(Math.Sin(Time.time) * (25)), 0);
}

}