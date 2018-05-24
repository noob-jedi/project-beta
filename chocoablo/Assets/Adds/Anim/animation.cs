using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation : MonoBehaviour {

	Animator animator;
	float h;
	float v;
	float sprint;
	bool skill;
	float skillcd;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		skillcd = 0;
	}
	
	// Update is called once per frame
	void Update () {
		v = Input.GetAxisRaw ("Vertical");
		animator.SetFloat ("walk", v);
		if (v > 0) {	
			transform.Translate (Vector3.forward * Time.deltaTime);
		}
		h = Input.GetAxisRaw ("Horizontal");

			animator.SetFloat ("turn", h);
		if (h > 0) {
			transform.Rotate (Vector3.up *45* Time.deltaTime);
		} else if (h < 0) {
			transform.Rotate (Vector3.down *45* Time.deltaTime);
		}

		this.Running();
		animator.SetFloat ("run", sprint);
		/*if (sprint > 0) {			
			transform.Translate (Vector3.up * Time.deltaTime);
		}*/
		this.UseSkill ();
		animator.SetBool ("spell", skill);

		skillcd -= Time.deltaTime;
	}



	void Running(){
		if (Input.GetKey(KeyCode.Space)) {
			sprint = 0.2f;
		} else {
			sprint = 0.0f;
		}
		
	}

	void UseSkill(){
		if (Input.GetKeyDown(KeyCode.V)& skillcd <=0) {
			skill = true;
			skillcd = 3.0f;
		} 
		else {
			skill = false;
		}
		
	}
}
