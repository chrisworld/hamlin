using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnScale : MonoBehaviour {

	public AudioSource activate_sound;
	public Transform player;
	public PlayerController player_controller;
	public Health health;
	public ScaleListener scale_listener;
	public ContainerManager container;
	public float distance_activation;

	public ScaleNames scale_name;
	public NoteBaseKey base_key;

	private bool activated = false;
	private int[] box_scale;

	// start
	void Start () {
		// get the scale for the scalebox
		box_scale = scale_listener.getFullScale(scale_name);
		box_scale = scale_listener.ScaleByKey((int)base_key, box_scale);
	}
	
	// update
	void Update () {
		// check distance
		if(Vector3.Distance(player.position, this.transform.position) < distance_activation)
		{
	  	// start the scale
	  	if(!activated && checkValidMusicKey()){
	  		print("inside the scalebox");
	    	activate_sound.Play();
	    	activated = true;
	    	player_controller.setMoveActivate(false);
	    	container.setScale(box_scale);
	  	}
	  	// stop the scale
	  	else if(activated && player_controller.checkValidJumpKey())
	  	{
	  		print("exit the scalebox");
	  		activated = false;
	  		player_controller.setMoveActivate(true);
	  		container.resetContainers();
	  	}
		}
	}

	public bool checkValidMusicKey(){
		KeyCode[] valid_keys = {
			KeyCode.Y,
			KeyCode.S,
			KeyCode.X,
			KeyCode.D,
			KeyCode.C,
			KeyCode.V,
			KeyCode.G,
			KeyCode.B,
			KeyCode.H,
			KeyCode.N,
			KeyCode.J,
			KeyCode.M,
			KeyCode.Comma
		};
		// check valid key
		foreach (KeyCode key in valid_keys){
			if(Input.GetKeyDown(key)){ 
				return true;
			}
		}
		return false;
	}
}
