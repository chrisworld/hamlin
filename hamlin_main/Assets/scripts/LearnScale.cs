using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnScale : MonoBehaviour {

	public AudioSource activate_sound;
	public Transform player;
	public ScaleListener scaleListener;
	public Health health;
	public ContainerManager container;
	public ScaleNames scale_name;
	public float distance_activation;

	private bool activated = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// check distance
		if(Vector3.Distance(player.position, this.transform.position) < distance_activation)
		{
		  	if(!activated && checkValidKey()){
		  		print("inside");
		    	activate_sound.Play ();
		    	activated = true;
		  	}

		}
	}

	public bool checkValidKey(){
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
