using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerManager : MonoBehaviour {

	int counter = 0;

	public Image[] note_container1;
	public Image[] note_container2;
	public Image[] note_container3;
	public Image[] note_container4;
	public Image[] note_container5;
	public Image[] note_container6;
	public Image[] note_container7;
	public Image[] note_container8;
	public Image[] note_container9;
	public Image[] note_container10;
	public Image[] note_container11;


	object[] note_container = new object[11];


	note_container[0]	= note_container1;
	note_container[1]	= note_container2;
	note_container[2]	= note_container3;
	note_container[3]	= note_container4;
	note_container[4]	= note_container5;
	note_container[5]	= note_container6;
	note_container[6]	= note_container7;
	note_container[7]	= note_container8;
	note_container[8]	= note_container9;
	note_container[9]	= note_container10;
	note_container[10]	= note_container11;



	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Period)) {
			note_container[0][counter].enabled = true;
			counter++;
			if (counter == 15) {
				counter = 0;
			}
		}
		if (Input.GetKeyDown(KeyCode.Minus)) {
			resetContainer ();
		}
	}


	void resetContainer () {

		for (int j = 0; j < 16; j++) {

			note_container1[j].enabled = false;

		}
	}

}
