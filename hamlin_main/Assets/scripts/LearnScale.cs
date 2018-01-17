﻿using System.Collections;
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
	private KeyCode[] valid_keys = {
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
		KeyCode.Comma,
		KeyCode.Q,
		KeyCode.Alpha2,
		KeyCode.W,
		KeyCode.Alpha3,
		KeyCode.E,
		KeyCode.R,
		KeyCode.Alpha5,
		KeyCode.T,
		KeyCode.Alpha6,
		KeyCode.Z,
		KeyCode.Alpha7,
		KeyCode.U,
		KeyCode.I
	};

	// start
	void Start () {
		// get the scale for the scalebox
		box_scale = scale_listener.getFullScale(scale_name);
		//box_midi = scale_listener.ScaleByKey((int)base_key, box_scale);
		// init note_state to all disabled
		for (int c = 0; c < num_c; c++){
			note_state[c] = new NoteState[num_n];
			for (int n = 0; n < num_n; n++){
				note_state[c][n] = NoteState.DISABLED;
			}
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
	    	//container.setScale(box_midi);
	    	// put scale
	    	int ci = 0;
				int ni = 0;
				foreach (int note in box_scale){
					ni = scaleToContainerMapping(note);
					note_state[ci][ni] = NoteState.NORMAL;
					ci++;
				}
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
	  		int k = 0;
	  		bool[] key_mask = getKeyMask();
	  		foreach (bool mask in key_mask){
	  			if (mask){
	  				int midi = keyToMidiMapping(k);
	  				int note_pos = midiToContainerMapping(midi);
	  				Debug.Log("note_pos: "+ note_pos);
	  				note_state[0][note_pos] = NoteState.NORMAL;
	  			}
	  			k++;
	  		}
	  		container.updateNoteContainer(note_state);
	  	}
		}
	}

	public bool checkValidMusicKey(){
		// check valid key
		foreach (KeyCode key in valid_keys){
			if(Input.GetKeyDown(key)){ 
				return true;
			}
		}
		return false;
	}

	// get mask of pressed keys
	public bool[] getKeyMask(){
		int k = 0;
		bool[] key_mask = new bool[valid_keys.Length];
		// set to zero
		for (int c = 0; c < valid_keys.Length; c++){
			key_mask[c] = false;
		}
		// get mask
		foreach (KeyCode key in valid_keys){
			if(Input.GetKeyDown(key)){
				key_mask[k] = true;
			}
			k++;
		}
		return key_mask;
	}

	public int keyToMidiMapping(int key){
		return key + 48;
	}

	public int scaleToContainerMapping(int scale){
		int midi = (int)base_key + scale;
		return midiToContainerMapping(midi);
	}

	public int midiToContainerMapping(int midi){
		switch(midi)
		{
			case 48:
			case 49:	return 14;	// c, cis
			case 50:
			case 51:	return 13;	// d, dis
			case 52:	return 12;	// e
			case 53:	
			case 54:	return 11;	// f, fis
			case 55:	
			case 56:	return 10;
			case 57:
			case 58:	return 9;
			case 59:	return 8;
			case 60:
			case 61:	return 7;
			case 62:
			case 63:	return 6;
			case 64:	return 5;
			case 65:
			case 66:	return 4;
			case 67:
			case 68:	return 3;
			case 69:
			case 70:	return 2;
			case 71:	return 1;
			case 72:	return 0;
			default:	break;
		}
		return 0;
	}
}
