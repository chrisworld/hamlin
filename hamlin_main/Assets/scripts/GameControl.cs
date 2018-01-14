using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

	public ContainerManager container;

	// Use this for initialization
	void Start () {
		container.resetContainers ();
		container.resetCleffs ();
	}
	
 	void Update() {
		if (Input.GetKey("escape"))
			Application.Quit();
    }
}
