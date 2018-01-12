using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class buttons : MonoBehaviour {
	
	public void PlayButton(){
		
		SceneManager.LoadScene("rat_test_lab");
	}
	
	public void ExitButton(){
		
		Application.Quit();
	}
}