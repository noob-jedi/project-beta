using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public Texture2D image;
    public Rect position;

    List<Item> itemList = new List<Item>();
    int slotWidthSize = 10;
    int slotHeightSize = 4;
    int slotWidth = 29;
    int slotHeight = 29;

    public int slotX;
    public int slotY;

    public Slot[,] slots ;
	// Use this for initialization
	void Start () {
        slots = new Slot[slotWidthSize, slotHeightSize];

        for (int i = 0; i < slotWidthSize; i++)
        {
            for (int j = 0; j< slotHeightSize; j++)
            {
                //new Rect(slotX + i * slotWidth, slotY + j * slotHeight)
                slots[i, j] = new Slot();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnGUI()
    {
        GUI.DrawTexture(position, image);
    }
    void drawInventory()
    {
        
    }

    void addItem(Item item)
    {
        bool occupied;

        for(int i = 0; i < item.width; i++)
        {
            for(int j = 0; j< item.height; j++)
            {
                if (slots[i, j].occupied)
                {

                    return;
                }
            }
        }

    }
}
