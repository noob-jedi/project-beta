using StateProperties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAI : MonoBehaviour {

    public AI ai;
    public List<AI> aiList;
	// Use this for initialization
	void Start () {
        ai = new AI
        {
            StateMachine = new StateMachine<AI>(ai)
        };
    }
	
	// Update is called once per frame
	void Update () {
        ai.StateMachine.Update();
	}
}
