using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class buttons : MonoBehaviour {
	
	public void PlayButton(){
		
		SceneManager.LoadScene("GameScene");
	}
	
	public void ExitButton(){
		
		Application.Quit();
	}
}