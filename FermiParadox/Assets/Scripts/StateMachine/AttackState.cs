using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateProperties;
using System;

public class AttackState : State<AI>
{

    private static AttackState _instance;
    private float timer;
    PlayerStats playerStats;

    private AttackState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static AttackState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AttackState();
            }
            return _instance;
        }

    }

    public override void EnterState(AI _owner)
    {
        //_owner.GetComponent<Renderer>().material.color = Color.blue;
        timer = _owner.gameTimer;
        GameObject player = player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        Debug.Log("Entering AttackState");
		_owner.animator.SetFloat ("walk", 0.01f);
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting AttackState");
    }

    public override void UpdateState(AI _owner)
    {
        Debug.Log("Updating AttackState");
        timer += 1f;
        Debug.Log("health = " + playerStats.currentHealth + ", timer = " + timer);
        if (!_owner.CheckDistanceToPlayer(_owner, 3.0f))
        {
            //_owner.GetComponent<Renderer>().material.color = Color.black;
            _owner.StateMachine.ChangeState(RoamLookAroundState.Instance);
            
        }
        if (timer % 50 == 0)
        {
            //Debug.Log("Attacking Player , remainder =" + (timer % 2));
            playerStats.TakeDamage(5);
        }
        else
        {
            //Debug.Log("Modulo not working");
        }

    }

}


/*
public class AttackState : State<AI>
{
    private static AttackState _instance;
    private float timer;
    PlayerStats playerStats;
        
    private AttackState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static AttackState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AttackState();
            }
            return _instance;
        }

    }

    public override void EnterState(AI _owner)
    {
        _owner.GetComponent<Renderer>().material.color = Color.blue;
        timer = _owner.gameTimer;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        throw new System.NotImplementedException();
    }

    public override void ExitState(AI _owner)
    {
        //throw new System.NotImplementedException();
    }

    public override void UpdateState(AI _owner)
    {
        timer += 0.5f;
        Debug.Log("health = " + playerStats.health + ", timer = " + timer);
        if(!_owner.CheckDistanceToPlayer(_owner,_owner.aggroDistance,2.0f))
        {
            _owner.GetComponent<Renderer>().material.color = Color.black;
            _owner.stateMachine.ChangeState(RoamLookAroundState.Instance);
            if(timer % 2 == 0)
            {
                playerStats.TakeDamage(5);
            }
        }
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
*/