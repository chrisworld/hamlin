using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopBgMusic : MonoBehaviour {

	public Transform MainMenu;
	private AudioSource bgMusic;

	// Use this for initialization
	public void stopMusic() {

		Time.timeScale = 1;
		MainMenu = GameObject.Find("MainMenu").GetComponent<Transform>();
		bgMusic = MainMenu.gameObject.GetComponent<AudioSource>();
		bgMusic.Stop();
	}
}
