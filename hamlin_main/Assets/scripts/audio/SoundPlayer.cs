using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// BasicSoundPlayer, just plays sounds by pressing Keys
// Only one octave implemented yet

public class SoundPlayer : MonoBehaviour {

	public AudioSource c;
	public AudioSource c_sh;
	public AudioSource c1;
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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Y)) {
			c.Play ();
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			c_sh.Play ();
		}
		if (Input.GetKeyDown(KeyCode.Comma)) {
			c1.Play ();
		}
		if (Input.GetKeyDown(KeyCode.X)) {
			d.Play ();
		}
		if (Input.GetKeyDown(KeyCode.D)) {
			d_sh.Play ();
		}
		if (Input.GetKeyDown(KeyCode.C)) {
			e.Play ();
		}
		if (Input.GetKeyDown(KeyCode.V)) {
			f.Play ();
		}
		if (Input.GetKeyDown(KeyCode.G)) {
			f_sh.Play ();
		}
		if (Input.GetKeyDown(KeyCode.B)) {
			g.Play ();
		}
		if (Input.GetKeyDown(KeyCode.H)) {
			g_sh.Play ();
		}
		if (Input.GetKeyDown(KeyCode.M)) {
			h.Play ();
		}
		if (Input.GetKeyDown(KeyCode.N)) {
			a.Play ();
		}
		if (Input.GetKeyDown(KeyCode.J)) {
			a_sh.Play ();
		}
		if (Input.GetKeyDown(KeyCode.Q)) {
			c.Play ();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			c_sh.Play ();
		}
		if (Input.GetKeyDown(KeyCode.I)) {
			c1.Play ();
		}
		if (Input.GetKeyDown(KeyCode.W)) {
			d.Play ();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			d_sh.Play ();
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			e.Play ();
		}
		if (Input.GetKeyDown(KeyCode.R)) {
			f.Play ();
		}
		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			f_sh.Play ();
		}
		if (Input.GetKeyDown(KeyCode.T)) {
			g.Play ();
		}
		if (Input.GetKeyDown(KeyCode.Alpha6)) {
			g_sh.Play ();
		}
		if (Input.GetKeyDown(KeyCode.U)) {
			h.Play ();
		}
		if (Input.GetKeyDown(KeyCode.Z)) {
			a.Play ();
		}
		if (Input.GetKeyDown(KeyCode.Alpha7)) {
			a_sh.Play ();
		}
	}
}
