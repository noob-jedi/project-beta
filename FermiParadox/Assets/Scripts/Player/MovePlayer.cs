using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

// Change to PlayerBehavior
public class MovePlayer : MonoBehaviour {

    GameObject player;
    float timerIter= 0.1f;
    Vector3 currPosition;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // For testing, currently not used
            currPosition = player.transform.position;
        }
        else
        {
            Console.WriteLine("not null");
        }
        
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 translateVector = new Vector3(0,0,0);
        
        if (Input.GetKey("w"))
        {
            translateVector[2] = translateVector[2] + timerIter;
            //rotateVector[1] = (float) Math.PI/2;
        }else if (Input.GetKey("s"))
        {
            translateVector[2] = translateVector[2] - timerIter;
            //rotateVector[1] = -(float)Math.PI / 2;
        }else if (Input.GetKey("a"))
        {
            translateVector[0] = translateVector[0] - timerIter;
        }else if (Input.GetKey("d"))
        {
            translateVector[0] = translateVector[0] + timerIter;
            //rotateVector[1] = (float)Math.PI;
        }
        
        // hack lock y axis translate
        //translateVector[1] = 0;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        //player.transform.rotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y, 0);
        player.transform.Translate(translateVector);
        getItem();
    }

    // Get Item, currently Clicked item get destroyed
    // TODO put item in inventory
    void getItem()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("tag = " + hit.transform.gameObject.tag + ", distance = " + Vector3.Distance(this.transform.position, hit.transform.gameObject.transform.position));
                if (hit.transform.gameObject.tag == "Item" && Vector3.Distance(this.transform.position, hit.transform.gameObject.transform.position)< 3)
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

}
