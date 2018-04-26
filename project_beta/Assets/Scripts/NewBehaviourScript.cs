using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class NewBehaviourScript : MonoBehaviour {

    public Image imageBackground;
    public string imageBackgroundString = "";
    Texture2D img;
    

	// Use this for initialization
	void Start () {
        Console.WriteLine("Test if ImageLoaded");
        imageBackground.sprite = (Sprite)Resources.Load<Sprite>("Background/"+imageBackgroundString) as Sprite;
        Console.WriteLine("Test if ImageLoaded 2");
    }
	
	// Update is called once per frame
	void Update () {
        Console.WriteLine("Test if ImageLoaded");
    }
}
