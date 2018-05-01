using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovePlayer : MonoBehaviour {

    GameObject player;
    float timerIter= 0.1f;
    Vector3 currPosition;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("SpherePlayerTag");
        if (player != null)
        {
            // For testing, currently not used
            currPosition = player.transform.position;
        }
        else
        {
            Console.WriteLine("not null");
        }
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 translateVector = new Vector3(0,0,0);
        
        if (Input.GetKey("w"))
        {
            translateVector[2] = translateVector[2] + timerIter;
        }else if (Input.GetKey("s"))
        {
            translateVector[2] = translateVector[2] - timerIter;
        }else if (Input.GetKey("a"))
        {
            translateVector[0] = translateVector[0] - timerIter;
        }else if (Input.GetKey("d"))
        {
            translateVector[0] = translateVector[0] + timerIter;
        }
        player.transform.Translate(translateVector);
        
    }
}
