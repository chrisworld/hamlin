using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnScale : NoteStateControl {

	// GameObjects
	public Transform player;
	public PlayerController player_controller;
	public Health health;
	public SoundPlayer sound_player;
	public Score score;
	[HideInInspector]
	public Animator anim;

	// settings
	public float distance_activation;
	public ScaleNames scale_name;

	// private vars
	private bool activated = false;
	private int[] box_scale;
	private int[] box_midi;
	private int c_pos;
	private int error_counter;


	// start
	void Start () {
		// get the scale for the scalebox
		box_scale = allScales[(int)scale_name];
		box_midi = scaleToMidi(box_scale);
		// init note_state to disabled
		initNoteState();
		initSignState();
		// container position
		c_pos = 0;
		error_counter = 0;
		// reference objects for prefab
		if(player == null){
      player = GameObject.Find("Player").GetComponent<Transform>();
      player_controller = player.GetComponent<PlayerController>();
      health = player.GetComponent<Health>();
    }
    if(sound_player == null){
      sound_player = GameObject.Find("SoundPlayer").GetComponent<SoundPlayer>();
    }
    if(container == null){
    	 container = GameObject.Find("ContainerManager").GetComponent<ContainerManager>();
    }
    if(score == null){
    	 score = GameObject.Find("GameState").GetComponent<Score>();
    }
    anim = GetComponent<Animator>();
	}
	
	// update
	void Update () {
		// check distance
		if(Vector3.Distance(player.position, this.transform.position) < distance_activation)
		{	
			// animation
			anim.SetBool ("isWaiting", false);
			anim.SetBool ("isListening", true);
	  	// start the scale
	  	if(!activated && checkValidMusicKey() && player_controller.hold_flute){
	  		initLearnScale();
	  	}
	  	// stop the scale
	  	else if(activated && player_controller.checkValidJumpKey())
	  	{
				exitLearnScale();
	  	}
	  	// play the scales
	  	else if(activated)
	  	{
	  		int key = 0;
	  		bool[] key_mask = getKeyMask();
	  		// win condition
	  		if (c_pos >= box_scale.Length){
					winLearnScale();
	  			return;
	  		}
	  		// loose condition
	  		else if (error_counter > 5){
	  			// ToDo: ErrorSound
	  			//activate_sound.Play();
	  			exitLearnScale();
	  			return;
	  		}
	  		// check each key
	  		foreach (bool mask in key_mask){
	  			if (mask){
	  				cleanWrongNoteState(box_scale);
	  				int note_midi = keyToMidiMapping(key);
	  				int note_pos = midiToContainerMapping(note_midi);
	  				// right note
	  				if(note_midi == box_midi[c_pos]){
	  					note_state[c_pos][note_pos] = NoteState.RIGHT;
	  					sign_state[c_pos][note_pos] = midiToSignState(note_midi);
	  					anim.Play("rightNote");
	  					c_pos++;
	  				}
	  				// wrong note
	  				else{
	  					note_state[c_pos][note_pos] = NoteState.WRONG;
	  					sign_state[c_pos][note_pos] = midiToSignState(note_midi);
	  					error_counter++;
	  				}
		  			container.updateNoteContainer(note_state);
		  			container.updateSignContainer(sign_state);
	  			}
	  			key++;
	  		}
	  	}
		}
		// not in distance
		else notInDistance();
	}

	// destroy
	private void DestroyClef (){		
		anim.Play("disappear");
		Destroy(gameObject, 1);
	}

	// init Learn Scale
	private void initLearnScale (){
		c_pos = 0;
		error_counter = 0;
		activated = true;
		sound_player.inLearning = true;
	 	sound_player.activate_sound.Play();
  	player_controller.enterPlayMode();
  	// put scale
  	setNoteStateToScale(box_scale);
  	setSignStateToScale(box_scale);
  	container.updateScaleInd(scale_name, base_key);
	}	

	// leave Learn Scale
	private void exitLearnScale (){
	  activated = false;
		c_pos = 0;
		error_counter = 0;
		sound_player.inLearning = false;
		player_controller.exitPlayMode();
		resetNoteState();
		resetSignState();
		container.resetScaleInd();
	}

	// win the Learn Scale
	private void winLearnScale (){
		exitLearnScale();
		sound_player.activate_sound.Play();
		score.updateScaleScore(scale_name);
		DestroyClef();
	}

	// not in distance condition
	private void notInDistance (){
		anim.SetBool ("isWaiting", true);
		anim.SetBool ("isListening", false);
		Vector3 direction = player.position - this.transform.position;
		direction.y = 0;
		this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.05f);
	}
}
