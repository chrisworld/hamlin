using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class buttons : MonoBehaviour {
	
  //legacy
  public void PlayButton(){
    SceneManager.LoadScene("rat_test_lab");
  }

	public void StoryModeButton(){
		SceneManager.LoadScene("rat_test_lab");
    //TODO: this should load monk level instead (when complete)
	}

  public void AdventureModeButton(){
    SceneManager.LoadScene("procedural_terrain");
  }
	
	public void ExitButton(){
		
		Application.Quit();
        //NOTE: this is ignored when testing in the editor. It will work when the game is built.

	}
}