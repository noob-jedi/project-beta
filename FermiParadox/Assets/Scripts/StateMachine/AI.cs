using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateProperties;
using UnityEngine.UI;

public class AI : MonoBehaviour {

    public float gameTimer;
    public int timer = 0;
    public GameObject ai,player;
    public GameObject[] ais;
    public GameObject testAI;
    public bool switchState;
    public State<AI> currentState = null;
    public float aggroDistance;
    public float thresholdDistance = 4.5f;
    public float enemyHealth, enemyMaxHealth;
    public Slider sliderHealthEnemy;
	public Animator animator;

    public StateMachine<AI> StateMachine { get; set; }
    public Dictionary<string,StateMachine<AI>> gameObjectDict = new Dictionary<string,StateMachine<AI>>();
    // Use this for initialization
    // Initialize AI with RoamLookAroundState

	void Start () {

        enemyMaxHealth = 100;
        enemyHealth = 100;
        StateMachine = new StateMachine<AI>(this);
        StateMachine.ChangeState(RoamLookAroundState.Instance);
        gameTimer = Time.time;
        //ai = GameObject.FindGameObjectWithTag("AITag");
        //ais = GameObject.FindGameObjectsWithTag("AITag");
        player = GameObject.FindGameObjectWithTag("Player");
        //aggroDistance = Vector3.Distance(ai.transform.position, player.transform.position);
        aggroDistance = Vector3.Distance(this.transform.position, player.transform.position);
        sliderHealthEnemy.value = enemyHealth/enemyMaxHealth;
		animator = GetComponent<Animator> ();
    }

    void Update () {
        //StateMachine = gameObjectDict[this.gameObject.name];
        gameTimer += 0.1f;
        /*
        foreach (KeyValuePair<string, StateMachine<AI>> entry in gameObjectDict)
        {
            // do something with entry.Value or entry.Key
            Debug.Log("gameOBject" + ai.gameObject.name);
            entry.Value.Update();
        }
        */

        if (gameTimer > 25)
        {
            gameTimer = 0;
        }
        StateMachine.Update();
        sliderHealthEnemy.value = enemyHealth / enemyMaxHealth;
    }
//    public bool CheckDistanceToPlayer(AI _owner, float distance, float threshold)
    public bool CheckDistanceToPlayer(AI _owner, float threshold)
    {
        //Debug.Log(_owner.aggroDistance);
        if (Vector3.Distance(_owner.transform.position, player.transform.position) <= threshold)
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
