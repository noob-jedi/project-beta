using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item {

    public Texture2D image;
    public int x;
    public int y;
    public int width;
    public int height;

    public abstract void performAction();
}
