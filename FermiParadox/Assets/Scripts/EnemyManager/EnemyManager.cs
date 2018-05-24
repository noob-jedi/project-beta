using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    GameObject enemy;
    
	// Use this for initialization
	void Start () {
		
        enemy = GameObject.FindGameObjectWithTag("AITag");
        Vector3 posOffset = new Vector3(0.02f,0.0f , 0.02f);
		/*
        for (int i = 0; i < 2; i++)
        {
            Instantiate(enemy, this.transform.position + posOffset, this.transform.rotation);

        }*/
		Instantiate(enemy, this.transform.position + posOffset, this.transform.rotation);
		Instantiate(enemy, this.transform.position - posOffset, this.transform.rotation);
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
