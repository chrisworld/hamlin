using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enums
public enum NoteState {
	DISABLED = 0,
	NORMAL = 1,
	RIGHT = 2,
	WRONG = 3
};

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
	private int[] box_midi;
	private int num_c = 11;
	private int num_n = 15;
	private NoteState[][] note_state = new NoteState[11][];

	// start
	void Start () {
		// get the scale for the scalebox
		box_scale = scale_listener.getFullScale(scale_name);
		box_midi = scale_listener.ScaleByKey((int)base_key, box_scale);
		// init note_state to all disabled
		for (int c = 0; c < num_c; c++){
			note_state[c] = new NoteState[num_n];
			for (int n = 0; n < num_n; n++){
				note_state[c][n] = NoteState.DISABLED;
			}
		}
		int ni = 0;
		foreach (int note in box_scale){
			ni++;
		}
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
	    	container.setScale(box_midi);
	  	}
	  	// stop the scale
	  	else if(activated && player_controller.checkValidJumpKey())
	  	{
	  		print("exit the scalebox");
	  		activated = false;
	  		player_controller.setMoveActivate(true);
	  		container.resetContainers();
	  	}
	  	// play the scales
	  	else if(activated)
	  	{
	  		container.updateNoteContainer(note_state);
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
