using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnChord : NoteStateControl {

  // GameObjects
  public Transform player;
  public PlayerController player_controller;
  public Transform hud_chord;
  public Score score;
  
  [HideInInspector]
  public Animator anim;

  // settings
  public float distance_activation;
  public ChordNames chord_name;
  public NoteBaseKey base_key;

  // private vars
  private bool activated = false;

  // start
  void Start () 
  {
    // reference objects for prefab
    if(player == null){
      player = GameObject.Find("Player").GetComponent<Transform>();
      player_controller = player.GetComponent<PlayerController>();
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
    if (hud_chord == null)
    {
      hud_chord = GameObject.Find("Chords").GetComponent<Transform>();
      hud_chord.transform.gameObject.SetActive(false);
    }
    activated = false;
    anim = GetComponent<Animator>();
  }
  
  // update
  void Update () 
  {
    // check distance
    if(Vector3.Distance(player.position, this.transform.position) < distance_activation)
    { 
      // in distance stuff
      inDistance();
      // start the scale
      if(!activated && player_controller.play_mode)
      {
        initLearnChord();
      }
      // stop the scale
      else if(activated && !player_controller.play_mode)
      {
        exitLearnChord();
      }
      // play the scales
      else if(activated)
      {
        int key = 0;
        bool[] key_mask = getChordKeyMask();
        // win condition

        // loose condition

        // check each key
        foreach (bool mask in key_mask){
          if (mask){

            }
            // chord
            else{

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
  private void initLearnChord ()
  {
    hud_chord.transform.gameObject.SetActive(true);
    activated = true;
  } 

  // leave Learn Scale
  private void exitLearnChord ()
  {
    hud_chord.transform.gameObject.SetActive(false);
  }

  // win the Learn Scale
  private void win ()
  {
    exitLearnChord();
    sound_player.activate_sound.Play();
    DestroyClef();
  }

  // in distance condition
  private void inDistance ()
  {
    anim.SetBool ("isWaiting", false);
    anim.SetBool ("isListening", true);
    if (!player_controller.inDistance){ 
      player_controller.inDistance = true;
    }
  }

  // not in distance condition
  private void notInDistance ()
  {
    anim.SetBool ("isWaiting", true);
    anim.SetBool ("isListening", false);
    Vector3 direction = player.position - this.transform.position;
    direction.y = 0;
    this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.05f);
  }
}
