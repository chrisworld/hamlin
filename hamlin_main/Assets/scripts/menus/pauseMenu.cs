using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour {

	public Transform PauseMenu;
	public Transform SettingsMenu;
	public Transform Player;
  private AudioSource music;

	void Start(){
		PauseMenu.gameObject.SetActive(false);
		SettingsMenu.gameObject.SetActive(false);
		Time.timeScale = 1;
    music = PauseMenu.gameObject.GetComponent<AudioSource>();
	}
	
	public void Update(){
		if(Input.GetKeyDown(KeyCode.Escape) && 
			SettingsMenu.gameObject.activeInHierarchy == false){
			
			Pause();
		}
	}
	
	public void Pause(){
		
		PauseMenu.gameObject.SetActive(!PauseMenu.gameObject.activeInHierarchy);

		if(Time.timeScale == 1){
			//Canvas.gameObject.SetActive(true);
			Time.timeScale = 0;
			Player.GetComponent<CharacterController>().enabled = false;   
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
      music.Play();
    }
    else if(Time.timeScale == 0){
			//Canvas.gameObject.SetActive(false);
			Time.timeScale = 1;
			Player.GetComponent<CharacterController>().enabled = true;
      music.Stop();
    }
	}
	
	public void Menu(string i){
		SceneManager.LoadScene("MainMenu_pablo");
	}
}