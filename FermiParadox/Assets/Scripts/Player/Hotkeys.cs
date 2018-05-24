using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotkeys : MonoBehaviour {

    public GameObject inventoryPanel;
    bool isInventoryOpen = false;
	// Use this for initialization
	void Start () {
        inventoryPanel.SetActive(isInventoryOpen);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("i"))
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryPanel.SetActive(isInventoryOpen);
        }	
	}
}
