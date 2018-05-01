using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateProperties;
using System;

public class IdleState : State<AI>
{

    private static IdleState _instance;

    private IdleState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static IdleState Instance
    {
        get
        {
            if (_instance == null)
            {
                new IdleState();
            }
            return _instance;
        }

    }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Entering RoamState");
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting RoamState");
    }

    public override void UpdateState(AI _owner)
    {
        Debug.Log("Updating");
    }

}
