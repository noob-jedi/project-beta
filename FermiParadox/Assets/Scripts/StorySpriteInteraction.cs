using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StorySpriteInteraction : MonoBehaviour {
    
    public void OnClickStorySprite(string spriteString)
    {

        SceneManager.LoadScene(spriteString);
    }

}
