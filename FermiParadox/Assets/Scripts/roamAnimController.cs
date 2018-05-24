using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class roamAnimController : MonoBehaviour {
    
    float lookAroundTime = 8f;
    float roamTime = 1f;
    float initTime;
    double translateX, translateZ, rotY;
    bool rotated = false;
    public float gameTimer;
    public float aggroDistance, thresholdDistance = 4.5f;
    public bool playerNear;
    float timeDiff;
    GameObject player;
    EnvironmentVariables envVar;
    private System.Random rnd = new System.Random();
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        gameTimer = Time.time;
        Randomize();
    }
	
	// Update is called once per frame
	void Update () {
        timeDiff = Time.time - gameTimer;
        // Static timer still
        if (timeDiff > 4)
        {
            Randomize();
        }
        if (timeDiff <= roamTime)
        {
            Roam(timeDiff);
        }
        Debug.Log("--------------");
        Debug.Log(timeDiff);
        Debug.Log("--------------");

        if (CheckDistanceToPlayer(thresholdDistance))
        {
            playerNear = true;
        }
        /*
        if (this.CheckDistanceToPlayer(aggroDistance, thresholdDistance))
        {
            _owner.StateMachine.ChangeState(ChaseState.Instance);
        }
        */

    }
    private void Roam(float timeDiff)
    {
        //Vector3 moveVector = new Vector3(0, 0, 0);
        Debug.Log(this.transform.eulerAngles);
        Debug.Log("x = " + translateX + ", y = " + translateZ + ", theta = " + rotY / Math.PI * 180);
        /*
        if (!rotated)
        {
            _owner.transform.eulerAngles = new Vector3(0, (float)(rotY / Math.PI * 180 - 90), 0);
        }
        */
        this.transform.Translate((float)translateX, 0, (float)translateZ);

    }
    private void Randomize()
    {
        translateX = 0.1 * (rnd.NextDouble() * 2 - 1);
        translateZ = 0.1 * (rnd.NextDouble() * 2 - 1);
        rotY = Math.Atan2(translateZ, translateX);
        rotated = false;
        gameTimer = Time.time;
    }
    public bool CheckDistanceToPlayer(float threshold)
    {
        //distance = Vector3.Distance(_owner.ai.transform.position, player.transform.position);
        if (Vector3.Distance(this.transform.position, player.transform.position) <= threshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
