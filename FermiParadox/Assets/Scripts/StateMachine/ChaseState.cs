using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateProperties;
using System;

public class ChaseState : State<AI>
{

    private static ChaseState _instance;
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    
    private ChaseState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static ChaseState Instance
    {
        get
        {
            if (_instance == null)
            {
                new ChaseState();
            }
            return _instance;
        }

    }

    public override void EnterState(AI _owner)
    {
        //Debug.Log("Entering RoamState");
    }

    public override void ExitState(AI _owner)
    {
        //Debug.Log("Exiting RoamState");
        _owner.gameTimer = 0;
		_owner.animator.SetFloat ("walk", 0.01f);
    }

    private Quaternion _lookRotation;
    private Vector3 _direction;

    public override void UpdateState(AI _owner)
    {
        /*
        _direction = (player.transform.position - _owner.transform.position).normalized;

        //create the rotation we need to be in to look at the target
        _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, _lookRotation, 0.5f);
        */
        _owner.RotateToPoint(player.transform.position);
        //Debug.Log("Updating ChaseState");
        //Debug.Log(Vector3.Distance(_owner.transform.position, player.transform.position));
        //_owner.transform.
        _owner.transform.position = Vector3.MoveTowards(_owner.transform.position, player.transform.position, 0.02f);

        if (!_owner.CheckDistanceToPlayer(_owner,_owner.thresholdDistance))
        {
            _owner.StateMachine.ChangeState(RoamLookAroundState.Instance);
        }

        if(_owner.CheckDistanceToPlayer(_owner,3.0f))
        {
            _owner.StateMachine.ChangeState(AttackState.Instance);
        }
		_owner.animator.SetFloat ("walk", 1.2f);
    }

}
