using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentVariables : MonoBehaviour {

    public float gameTimer;
	// Use this for initialization
	void Start () {
        gameTimer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        float t = Time.time - gameTimer;
        gameTimer += t;
	}
}
