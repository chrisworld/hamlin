using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

// MIDI TEST




// BasicSoundPlayer, just plays sounds by pressing Keys
// Only one octave implemented yet

public class SoundPlayer : MonoBehaviour {

	//Gamesounds

	//character

	public AudioSource hamlin_jump;
	public AudioSource hamlin_hurt;
	public AudioSource hamlin_walk;
	public AudioSource hamlin_run;
	public AudioSource hamlin_flute_out;
	public AudioSource hamlin_flute_in;

	//monster

	public AudioSource monster_spot;
	public AudioSource monster_attack;
	public AudioSource monster_hurt;

	//environment

	public AudioSource ambient_forrest;
	public AudioSource ambient_monklevel;


	//flute

	public AudioSource c;
	public AudioSource c_sh;
	public AudioSource d;
	public AudioSource d_sh;
	public AudioSource e;
	public AudioSource f;
	public AudioSource f_sh;
	public AudioSource g;
	public AudioSource g_sh;
	public AudioSource a;
	public AudioSource a_sh;
	public AudioSource h;
	public AudioSource c1;
	public AudioSource c_sh1;
	public AudioSource d1;
	public AudioSource d_sh1;
	public AudioSource e1;
	public AudioSource f1;
	public AudioSource f_sh1;
	public AudioSource g1;
	public AudioSource g_sh1;
	public AudioSource a1;
	public AudioSource a_sh1;
	public AudioSource h1;
	public AudioSource c2;
	public AudioSource activate_sound;

	public bool MidiKeyPressed = false;
	public int MidiKeyPressedNr;

  public PlayerController player_controller;

  //managed by CombatManager
  [HideInInspector]
  public bool inCombat;
  [HideInInspector]
  public bool inLearning;
  [HideInInspector]
  public bool inPlay;

	void NoteOn(MidiChannel channel, int note, float velocity)
	{
		MidiKeyPressed = true;
		Debug.Log("NoteOn: " + note);
		MidiKeyPressedNr = note;

	}


	void NoteOff(MidiChannel channel, int note)
	{
		MidiKeyPressed = false;
		Debug.Log("NoteOff: " + channel + "," + note);
	}

	void Knob(MidiChannel channel, int knobNumber, float knobValue)
	{
		Debug.Log("Knob: " + knobNumber + "," + knobValue);
	}

	void OnEnable()
	{
		MidiMaster.noteOnDelegate += NoteOn;
		MidiMaster.noteOffDelegate += NoteOff;
		MidiMaster.knobDelegate += Knob;
	}

	void OnDisable()
	{
		MidiMaster.noteOnDelegate -= NoteOn;
		MidiMaster.noteOffDelegate -= NoteOff;
		MidiMaster.knobDelegate -= Knob;
	}



	// Use this for initialization
	void Start () {
    
		ambient_forrest.Play ();
		ambient_monklevel.Play ();

		if(player_controller == null){
      player_controller = GameObject.Find("Player").GetComponent<Transform>().GetComponent<PlayerController>();
    }
    inCombat = false;
    inLearning = false;
    inLearning = false;
	}


	// Update is called once per frame
	void Update () {

    if((inCombat || inLearning || inPlay) && player_controller.hamlinReadyToPlay()){ 
			if (Input.GetKeyDown(KeyCode.Y) || (MidiKeyPressed == true && MidiKeyPressedNr == 48))
      {
		    c.Play();
        player_controller.playNote();
        MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.S) || (MidiKeyPressed == true && MidiKeyPressedNr == 49))
      {
        c_sh.Play();
        player_controller.playNote();
		    MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.Comma) || (MidiKeyPressed == true && MidiKeyPressedNr == 60))
      {
        c1.Play();
        player_controller.playNote();
		    MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.X) || (MidiKeyPressed == true && MidiKeyPressedNr == 50))
      {
        d.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.D) || (MidiKeyPressed == true && MidiKeyPressedNr == 51))
      {
        d_sh.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.C) || (MidiKeyPressed == true && MidiKeyPressedNr == 52))
      {
        e.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.V) || (MidiKeyPressed == true && MidiKeyPressedNr == 53))
      {
        f.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.G)|| (MidiKeyPressed == true && MidiKeyPressedNr == 54))
      {
        f_sh.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.B) || (MidiKeyPressed == true && MidiKeyPressedNr == 55))
      {
        g.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.H) || (MidiKeyPressed == true && MidiKeyPressedNr == 56))
      {
        g_sh.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.M) || (MidiKeyPressed == true && MidiKeyPressedNr == 59))
      {
        h.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.N) || (MidiKeyPressed == true && MidiKeyPressedNr == 57))
      {
        a.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.J) || (MidiKeyPressed == true && MidiKeyPressedNr == 58))
      {
        a_sh.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
      if (Input.GetKeyDown(KeyCode.Q))
      {
        c1.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.Alpha2) || (MidiKeyPressed == true && MidiKeyPressedNr == 61))
      {
        c_sh1.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.I) || (MidiKeyPressed == true && MidiKeyPressedNr == 72))
      {
        c2.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.W) || (MidiKeyPressed == true && MidiKeyPressedNr == 62))
      {
        d1.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.Alpha3) || (MidiKeyPressed == true && MidiKeyPressedNr == 63))
      {
        d_sh1.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.E) || (MidiKeyPressed == true && MidiKeyPressedNr == 64))
      {
        e1.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.R) || (MidiKeyPressed == true && MidiKeyPressedNr == 65))
      {
        f1.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.Alpha5) || (MidiKeyPressed == true && MidiKeyPressedNr == 66))
      {
        f_sh1.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.T) || (MidiKeyPressed == true && MidiKeyPressedNr == 67))
      {
        g1.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.Alpha6) || (MidiKeyPressed == true && MidiKeyPressedNr == 68))
      {
        g_sh1.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.U) || (MidiKeyPressed == true && MidiKeyPressedNr == 71))
      {
        h1.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.Z) || (MidiKeyPressed == true && MidiKeyPressedNr == 69) )
      {
        a1.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
			if (Input.GetKeyDown(KeyCode.Alpha7)|| (MidiKeyPressed == true && MidiKeyPressedNr == 70))
      {
        a_sh1.Play();
        player_controller.playNote();
				MidiKeyPressed = false;
      }
    }
	}
}
