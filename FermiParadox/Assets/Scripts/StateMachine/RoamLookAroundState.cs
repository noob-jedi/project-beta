using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateProperties;
using System;

public class RoamLookAroundState : State<AI> {

    private static RoamLookAroundState _instance;
    float lookAroundTime = 8f;
    float roamTime = 3f;
    float initTime;
    double translateX, translateZ, rotY;
    bool rotated = false;
    private System.Random rnd = new System.Random();
	//GameObject player = GameObject.FindGameObjectWithTag("SpherePlayerTag");

    private RoamLookAroundState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }

    public static RoamLookAroundState Instance
    {
        get
        {
            if(_instance == null)
            {
                new RoamLookAroundState();
            }
            return _instance;
        }

    }

    public override void EnterState(AI _owner)
    {
        initTime = _owner.gameTimer;
        Randomize();
		//Debug.Log("Entering RoamState");
        //_owner.transform.Rotate(0, 5, 0);
    }

    public override void ExitState(AI _owner)
    {
		//Debug.Log("Exiting RoamState");

    }

    public override void UpdateState(AI _owner)
    {
        //Debug.Log("Updating");
        float timeDiff = _owner.gameTimer - initTime;
        //Debug.Log(timeDiff);
        //_owner.transform.Translate(0, 0.1f, 0);
        //if(_owner.switchState)
		if (timeDiff <= roamTime) {
			Roam (_owner, timeDiff);
		}else if(timeDiff > roamTime && timeDiff < roamTime + lookAroundTime){
			LookAround(_owner, timeDiff);
		}

        // Static timer still
        if(timeDiff > 12)
        {
            Randomize();
        }

        if (_owner.CheckDistanceToPlayer(_owner, _owner.thresholdDistance))
        {
            _owner.StateMachine.ChangeState(ChaseState.Instance);
		}
		Debug.Log ("walk = " + _owner.animator.GetFloat ("walk"));
		//Debug.Log (_owner.animator.);
    }
    
    /*
    public bool CheckDistanceToPlayer(AI _owner)
    {
        float distance = Vector3.Distance(_owner.ai.transform.position, player.transform.position);
        Debug.Log(distance);
        if (Vector3.Distance(_owner.ai.transform.position, player.transform.position) <= 4.5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }*/

    
    private void Roam(AI _owner, float timeDiff)
    {
        //Vector3 moveVector = new Vector3(0, 0, 0);
        //Debug.Log(_owner.transform.eulerAngles);
        //Debug.Log("x = " + translateX + ", y = " + translateZ + ", theta = " + rotY / Math.PI * 180);
        if (!rotated)
        {
            _owner.transform.eulerAngles = new Vector3(0, (float)(rotY / Math.PI * 180 - 90), 0);
        }
        //if(_owner.transform.rotation[2] )
        //_owner.transform.Rotate(0,(float)(timeDiff/roamTime*rotY),0);
        //_owner.transform.Rotate(new Vector3((float)( translateX * 180),0, (float)(translateZ*180)));
        /*_owner.RotateToPoint(new Vector3(_owner.transform.position[0] + (float)translateX,
            0, _owner.transform.position[2]+ (float)translateZ));
        */
        _owner.transform.Translate((float)translateX, 0,  (float)translateZ);
		_owner.animator.SetFloat("walk", 2f);
    }
    private void Randomize()
    {
        translateX = 0.1 * (rnd.NextDouble() * 2 - 1);
        translateZ = 0.1 * (rnd.NextDouble() * 2 - 1);
        rotY = Math.Atan2(translateZ ,translateX);
        rotated = false;
    }

    private void LookAround(AI _owner,float timeDiff)
    {
        //Debug.Log("timeDiff " + timeDiff);
        Vector3 rotateVector = new Vector3(0, 0, 0);
        if (timeDiff < 2f || timeDiff > 6f)
        {
            rotateVector[1] = 2f;
        }else
        {
            rotateVector[1] = -2f;
        }
        _owner.transform.Rotate(rotateVector);
		_owner.animator.SetFloat ("walk",0.01f);
    }
}
