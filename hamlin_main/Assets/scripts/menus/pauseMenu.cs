using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour {

	public Transform PauseMenu;
	public Transform SettingsMenu;
	public Transform Player;

	void Start(){
		PauseMenu.gameObject.SetActive(false);
		SettingsMenu.gameObject.SetActive(false);
		Time.timeScale = 1;
	}
	
	public void Update(){
		if(Input.GetKeyDown(KeyCode.Escape) && 
			SettingsMenu.gameObject.activeInHierarchy == false){
			
			Pause();
		}
	}
	
	public void Pause(){
		
		PauseMenu.gameObject.SetActive(!PauseMenu.gameObject.activeInHierarchy);

        //TODO: stop hearts going down while paused

		if(Time.timeScale == 1){
			//Canvas.gameObject.SetActive(true);
			Time.timeScale = 0;
			Player.GetComponent<CharacterController>().enabled = false;   
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}else if(Time.timeScale == 0){
			//Canvas.gameObject.SetActive(false);
			Time.timeScale = 1;
			Player.GetComponent<CharacterController>().enabled = true;   
        }
	}
	
	public void Menu(string i){
		SceneManager.LoadScene("MainMenu");
	}
}