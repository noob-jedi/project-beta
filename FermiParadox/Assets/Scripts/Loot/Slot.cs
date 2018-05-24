using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {

    public Item item;
    public bool occupied;
    public Rect position;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void drawSlot()
    {
        GUI.DrawTexture(position, item.image);
    }
}
