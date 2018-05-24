using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour {

    public GameObject EnemyHealth;
    public GameObject Enemy;

    //GameObject FixedHealthBar;
	// Use this for initialization
	void Start () {
        //EnemyHealth.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = this.transform.position;
        Vector3 vp = Camera.main.WorldToViewportPoint(pos);
        vp.x = vp.x * Screen.width;
        vp.y = vp.y * Screen.height;
        vp.x -= 0;
        vp.y += 25;
        EnemyHealth.transform.position = vp;
        //EnemyHealth.transform.position = Camera.main.WorldToScreenPoint(target.position);
    }
}
