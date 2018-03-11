using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class buttons : MonoBehaviour {
	
  //legacy
  public void PlayButton(){
    SceneManager.LoadScene("rat_test_lab");
  }

	public void StoryModeButton(){
		SceneManager.LoadScene("monk_cat");
	}

  public void AdventureModeButton(){
    SceneManager.LoadScene("procedural_terrain");
  }
	
	public void ExitButton(){
		
		Application.Quit();
        //NOTE: this is ignored when testing in the editor. It will work when the game is built.

	}
}