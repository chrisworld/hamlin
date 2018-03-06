using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class buttons : MonoBehaviour {
	
	public void PlayButton(){
    print("play button pressed");
		SceneManager.LoadScene("rat_test_lab");
	}
	
	public void ExitButton(){
		
		Application.Quit();
        //NOTE: this is ignored when testing in the editor. It will work when the game is built.

	}
}