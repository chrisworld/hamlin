using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class NoteStateControl : MonoBehaviour {
	// GameObjects
	public ContainerManager container;
	public SoundPlayer sound_player;
  public bool hideNotes; //hard mode - player must learn scale properly as they don't see the notes to help them, just the scale name

  public NoteBaseKey base_key;


	public NoteState[][] note_state = new NoteState[12][];
	public SignState[][] sign_state = new SignState[12][];
	public int[][] allScales =   // Scales Definition
  {
    new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11},
    new int[] {0, 2, 4, 5, 7, 9, 11, 12},
    new int[] {0, 2, 3, 5, 7, 8, 10, 12},
    new int[] {0, 2, 4, 5, 7, 9, 11, 12},
    new int[] {0, 2, 3, 5, 7, 8, 10, 12},
    new int[] {0, 2, 3, 5, 7, 8, 11, 12},
    new int[] {0, 2, 3, 5, 7, 8, 9, 10, 11, 12}, // mix of ascend and descend
		new int[] {0, 2, 3, 5, 7, 8, 10, 12},
    new int[] {0, 2, 3, 5, 7, 8, 10, 12},
    new int[] {0, 2, 3, 5, 7, 8, 10, 12},
    new int[] {0, 1, 3, 5, 7, 8, 10, 12},
    new int[] {0, 1, 3, 5, 6, 8, 10, 12},
    new int[] {0, 2, 3, 5, 7, 9, 10, 12},
    new int[] {0, 2, 4, 6, 7, 9, 11, 12},
    new int[] {0, 2, 4, 5, 7, 9, 10, 12},
    new int[] {0, 2, 4, 7, 9, 12},
    new int[] {0, 2, 3, 4, 5, 7, 9, 10, 11, 12},
    new int[] {0, 1, 3, 5, 7, 10, 11, 12},
    new int[] {0, 1, 1, 4, 5, 8, 10 ,12},
  };

	// private vars
	private int num_c = 12;
	private int num_n = 15;
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
		print("Please Overwrite start in NoteStateControl");
	}
	
	// update
	void Update () {
		print("Please Overwrite update in NoteStateControl");
	}

	// remove wrong notes played before
	public void cleanWrongNoteState(int[] right_scale){
		for (int c = 0; c < num_c; c++){
			for (int n = 0; n < num_n; n++){
				if (note_state[c][n] == NoteState.WRONG){
					if (scaleToContainerMapping(right_scale[c]) == n)
					{
						note_state[c][n] = NoteState.NORMAL;
						sign_state[c][n] = scaleToSignStateMapping(right_scale[c]);
					}
					else{
						note_state[c][n] = NoteState.DISABLED;
						sign_state[c][n].act = false;
					}
				}
			}
		}
	}

	// set the NoteState to all disabled
	public void initNoteState(){
		for (int c = 0; c < num_c; c++){
			note_state[c] = new NoteState[num_n];
			for (int n = 0; n < num_n; n++){
				note_state[c][n] = NoteState.DISABLED;
			}
		}
	}

	// set the SignState to all disabled
	public void initSignState(){
		for (int c = 0; c < num_c; c++){
			sign_state[c] = new SignState[num_n];
			for (int n = 0; n < num_n; n++){
				sign_state[c][n].act = false;
			}
		}
	}

	// set the NoteState to all disabled
	public void resetNoteState(){
		for (int c = 0; c < num_c; c++){
			note_state[c] = new NoteState[num_n];
			for (int n = 0; n < num_n; n++){
				note_state[c][n] = NoteState.DISABLED;
			}
		}
		container.updateNoteContainer(note_state);
	}

		// set the SignState to all disabled
	public void resetSignState(){
		for (int c = 0; c < num_c; c++){
			sign_state[c] = new SignState[num_n];
			for (int n = 0; n < num_n; n++){
				sign_state[c][n].act = false;
			}
		}
		container.updateSignContainer(sign_state);
	}

	// set the note_state to a scale
	public void setNoteStateToScale(int[] update_scale){
		int ci = 0;
		int ni = 0;
		foreach (int note in update_scale){
			ni = scaleToContainerMapping(note);
			if(!hideNotes) note_state[ci][ni] = NoteState.NORMAL;
			ci++;
		}
		container.updateNoteContainer(note_state);
	}

	// set the sign_state to a scale
	public void setSignStateToScale(int[] update_scale){
		int ci = 0;
		SignState st;
		foreach (int note in update_scale){
			st = scaleToSignStateMapping(note);
			if(!hideNotes) sign_state[ci][st.pos].act = st.act;
			ci++;
		}
		container.updateSignContainer(sign_state);
	}

	// check if valid music key is pressed
	public bool checkValidMusicKey(){
		foreach (KeyCode key in valid_keys){
			if(Input.GetKeyDown(key) ){ 
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

	// map the keys to midi
	public int keyToMidiMapping(int key){
		if (key > 12) {
			key--;
		}
		return key + 48;
	}

	// puts a scale to a midi array
	public int[] scaleToMidi(int[] scale){
		int[] midi = new int[scale.Length];
		for(int i = 0; i < scale.Length; i++){
			midi[i] = scale[i] + (int)base_key;
		}
		return midi;
	}

	// scale to container
	public int scaleToContainerMapping(int scale_note){
		return midiToContainerMapping((int)base_key + scale_note);
	}

	// scale to sign
	public SignState scaleToSignStateMapping(int scale_note){
		return midiToSignState((int)base_key + scale_note);
	}

	// midi to container
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

	// midi to sign state
	public SignState midiToSignState(int midi){
		SignState st;
		st.act = false;
		st.pos = 0;
		switch(midi)
		{
			case 48:	st.pos = 14; st.act = false; return st;	// c
			case 49:	st.pos = 14; st.act = true; return st;	//cis
			case 50:	st.pos = 13; st.act = false; return st;	// d
			case 51:	st.pos = 13; st.act = true; return st;	// dis
			case 52:	st.pos = 12; st.act = false; return st;	// e
			case 53:	st.pos = 11; st.act = false; return st;	//f
			case 54:	st.pos = 11; st.act = true; return st;
			case 55:	st.pos = 10; st.act = false; return st;
			case 56:	st.pos = 10; st.act = true; return st;
			case 57:	st.pos = 9; st.act = false; return st;
			case 58:	st.pos = 9; st.act = true; return st;
			case 59:	st.pos = 8; st.act = false; return st;
			case 60:	st.pos = 7; st.act = false; return st;
			case 61:	st.pos = 7; st.act = true; return st;
			case 62:	st.pos = 6; st.act = false; return st;
			case 63:	st.pos = 6; st.act = true; return st;
			case 64:	st.pos = 5; st.act = false; return st;
			case 65:	st.pos = 4; st.act = false; return st;
			case 66:	st.pos = 4; st.act = true; return st;
			case 67:	st.pos = 3; st.act = false; return st;
			case 68:	st.pos = 3; st.act = true; return st;
			case 69:	st.pos = 2; st.act = false; return st;
			case 70:	st.pos = 2; st.act = true; return st;
			case 71:	st.pos = 1; st.act = false; return st;
			case 72:	st.pos = 0; st.act = false; return st;
			default:	break;
		}
		return st;
	}

	// cast scale
	public string castScale(int scale){

		if (scale == 0) {
			return "CHROMATIC";
		}
		if (scale == 1){
			return "MAJOR";
		}
		if (scale == 2) {
				return "MINOR";
		}
		if (scale == 3) {
				return "HARMONIC MINOR";
		}
		if (scale == 4) {
				return "MELODIC MINOR";
		}
		if (scale == 5) {
				return "NATURAL MINOR";
		}
		if (scale == 6) {
				return "DIATONIC MINOR";
		}
		if (scale == 7) {
				return "AEOLIAN";
		}
		if (scale == 8) {
				return "PHRYGIAN";
		}
		if (scale == 9) {
				return "LOCRIAN";
		}
		if (scale == 10) {
				return "DORIAN";
		}
		if (scale == 11) {
				return "LYDIAN";
		}
		if (scale == 12) {
				return "MIXOLYDIAN";
		}
		if (scale == 13) {
				return "PENTATONIC";
		}
		if (scale == 14) {
				return "BLUES";
		}
		if (scale == 15) {
				return "TURKISH";
		}
		if (scale == 16) {
				return "INDIAN";
		}
			else{
				return "";
			}
	}

	// cast Base Key
	public string castBaseKey(int key){

		if (key == 0) {
			return "C";
		}
		if (key == 1){
			return "C#/Db";
		}
		if (key == 2) {
			return "D";
		}
		if (key == 3) {
			return "D#/Eb";
		}
		if (key == 4) {
			return "E";
		}
		if (key == 5) {
			return "F";
		}
		if (key == 6) {
			return "F#/Gb";
		}
		if (key == 7) {
			return "G";
		}
		if (key == 8) {
			return "G#/Ab";
		}
		if (key == 9) {
			return "A";
		}
		if (key == 10) {
			return "A#/Bb";
		}
		if (key == 11) {
			return "B";
		}
		else{
			return "";
		}
	}
}

