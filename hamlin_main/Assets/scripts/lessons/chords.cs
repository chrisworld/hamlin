using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chords : MonoBehaviour {

  // GameObjects
  public Transform player;
  public PlayerController player_controller;
  public Health health;
  public Score score;
  [HideInInspector]
  public Animator anim;

  // settings
  public float distance_activation;
  public ScaleNames scale_name;
  public NoteBaseKey base_key;

  // private vars
  private bool activated = false;
  private int[] box_scale;
  private int[] box_midi;
  private int c_pos;
  private int error_counter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
