using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// BasicSoundPlayer, just plays sounds by pressing Keys
// Only one octave implemented yet

public class SoundPlayer : MonoBehaviour {

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

  //managed by CombatManager
  [HideInInspector]
  public bool inCombat;
  public bool inLearning;

	// Use this for initialization
	void Start () {
    inCombat = false;
    inLearning = false;
	}
	
	// Update is called once per frame
	void Update () {
    if(inCombat || inLearning){ 
      if (Input.GetKeyDown(KeyCode.Y))
      {
        c.Play();
      }
      if (Input.GetKeyDown(KeyCode.S))
      {
        c_sh.Play();
      }
      if (Input.GetKeyDown(KeyCode.Comma))
      {
        c1.Play();
      }
      if (Input.GetKeyDown(KeyCode.X))
      {
        d.Play();
      }
      if (Input.GetKeyDown(KeyCode.D))
      {
        d_sh.Play();
      }
      if (Input.GetKeyDown(KeyCode.C))
      {
        e.Play();
      }
      if (Input.GetKeyDown(KeyCode.V))
      {
        f.Play();
      }
      if (Input.GetKeyDown(KeyCode.G))
      {
        f_sh.Play();
      }
      if (Input.GetKeyDown(KeyCode.B))
      {
        g.Play();
      }
      if (Input.GetKeyDown(KeyCode.H))
      {
        g_sh.Play();
      }
      if (Input.GetKeyDown(KeyCode.M))
      {
        h.Play();
      }
      if (Input.GetKeyDown(KeyCode.N))
      {
        a.Play();
      }
      if (Input.GetKeyDown(KeyCode.J))
      {
        a_sh.Play();
      }
      if (Input.GetKeyDown(KeyCode.Q))
      {
        c1.Play();
      }
      if (Input.GetKeyDown(KeyCode.Alpha2))
      {
        c_sh1.Play();
      }
      if (Input.GetKeyDown(KeyCode.I))
      {
        c2.Play();
      }
      if (Input.GetKeyDown(KeyCode.W))
      {
        d1.Play();
      }
      if (Input.GetKeyDown(KeyCode.Alpha3))
      {
        d_sh1.Play();
      }
      if (Input.GetKeyDown(KeyCode.E))
      {
        e1.Play();
      }
      if (Input.GetKeyDown(KeyCode.R))
      {
        f1.Play();
      }
      if (Input.GetKeyDown(KeyCode.Alpha5))
      {
        f_sh1.Play();
      }
      if (Input.GetKeyDown(KeyCode.T))
      {
        g1.Play();
      }
      if (Input.GetKeyDown(KeyCode.Alpha6))
      {
        g_sh1.Play();
      }
      if (Input.GetKeyDown(KeyCode.U))
      {
        h1.Play();
      }
      if (Input.GetKeyDown(KeyCode.Z))
      {
        a1.Play();
      }
      if (Input.GetKeyDown(KeyCode.Alpha7))
      {
        a_sh1.Play();
      }
    }

	}
}
