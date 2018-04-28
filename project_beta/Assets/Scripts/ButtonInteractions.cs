using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonInteractions : MonoBehaviour {
    
    // Reusable, switch to generic one
    public void OnClickStoryButton(string buttonString){
        SceneManager.LoadScene(buttonString);
    }
	
	
}
