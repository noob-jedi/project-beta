using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateProperties;

public class AI : MonoBehaviour {

    public float gameTimer;
    public int timer = 0;
    public enum States {Roam,Idle,Chase};
    public States callState;
    public GameObject ai,player;
    public bool switchState;
    public State<AI> currentState = null;

    public StateMachine<AI> stateMachine { get; set; }
	// Use this for initialization
    // Initialize AI with RoamLookAroundState
	void Start () {
        /*
        if (ai != null)
        {
            //ai.transform.Rotate
        }*/

        stateMachine = new StateMachine<AI>(this);
        stateMachine.ChangeState(RoamLookAroundState.Instance);
        gameTimer = Time.time;
        ai = GameObject.FindGameObjectWithTag("AITag");
        player = GameObject.FindGameObjectWithTag("SpherePlayerTag");

    }

    // Update is called once per frame
    void Update () {
        gameTimer += 0.1f;
        // Check DistanceToPlayer to switch to Chase State

        //if (CheckDistanceToPlayer() && currentState == null)
        //if (gameTimer > 50)
        //{
        /*    
            if (this.switchState)
            {
                stateMachine.ChangeState(ChaseState.Instance);
                //this.currentState = ChaseState.Instance;
            }
            else
            {
                stateMachine.ChangeState(RoamLookAroundState.Instance);
                //this.currentState = RoamLookAroundState.Instance;
            }
            gameTimer = 0;
            */
        //}
        

        if(gameTimer > 25)
        {
            gameTimer = 0;
        }
        stateMachine.Update();
	}

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
    }
    public Quaternion _lookRotation;
    public Vector3 _direction;
    public void RotateToPoint(Vector3 dest)
    {
        _direction = (dest - this.transform.position).normalized;

        //create the rotation we need to be in to look at the target
        _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _lookRotation, 0.5f);

    }
}
