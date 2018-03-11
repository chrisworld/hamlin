using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgMusicMainMenu : MonoBehaviour {

	public Transform MainMenu;
	private AudioSource music;

	// Use this for initialization
	void Start () {

		Time.timeScale = 1;
		music = MainMenu.gameObject.GetComponent<AudioSource>();
		music.Play();
	}

	public void playMainSound(){

		Time.timeScale = 1;
		music = MainMenu.gameObject.GetComponent<AudioSource>();
		music.Play();
	}

}
