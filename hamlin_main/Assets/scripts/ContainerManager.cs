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

/*

void resetContainer (int index) {

	for (int j = 0; j < 16; j++) {

		note_container[index][j].enabled = false;
		sign_container[index][j].enabled = false;

	}
}


void configContainers ( int[] fightScale ) {

	for (int i = 0; i < fightScale.Length; i++) {

		if (fightScale [i] == 48) { 
			note_container [i] [0].enabled = true;
			sign_container [i] [0].enabled = false;
		}
		if (fightScale [i] == 49) {
			note_container [i] [0].enabled = true;
			sign_container [i] [0].enabled = true;
		}
		if (fightScale [i] == 50) {
			note_container [i] [1].enabled = true;
			sign_container [i] [1].enabled = false;
		}
		if (fightScale [i] == 51) {
			note_container [i] [1].enabled = true;
			sign_container [i] [1].enabled = true;
		}
		if (fightScale [i] == 52) {
			note_container [i] [2].enabled = true;
			sign_container [i] [2].enabled = false;
		}
		if (fightScale [i] == 53) {
			note_container [i] [3].enabled = true;
			sign_container [i] [3].enabled = false;
		}
		if (fightScale [i] == 54) {
			note_container [i] [3].enabled = true;
			sign_container [i] [3].enabled = true;
		}
		if (fightScale [i] == 55) {
			note_container [i] [4].enabled = true;
			sign_container [i] [4].enabled = false;
		}
		if (fightScale [i] == 56) {
			note_container [i] [4].enabled = true;
			sign_container [i] [4].enabled = true;
		}
		if (fightScale [i] == 57) {
			note_container [i] [5].enabled = true;
			sign_container [i] [5].enabled = false;
		}
		if (fightScale [i] == 58) {
			note_container [i] [5].enabled = true;
			sign_container [i] [5].enabled = true;
		}
		if (fightScale [i] == 59) {
			note_container [i] [6].enabled = true;
			sign_container [i] [6].enabled = false;
		}
		if (fightScale [i] == 60) {
			note_container [i] [7].enabled = true;
			sign_container [i] [7].enabled = false;
		}
		if (fightScale [i] == 61) { 
			note_container [i] [8].enabled = true;
			sign_container [i] [8].enabled = false;
		}
		if (fightScale [i] == 62) {
			note_container [i] [8].enabled = true;
			sign_container [i] [8].enabled = true;
		}
		if (fightScale [i] == 63) {
			note_container [i] [9].enabled = true;
			sign_container [i] [9].enabled = false;
		}
		if (fightScale [i] == 64) {
			note_container [i] [9].enabled = true;
			sign_container [i] [9].enabled = true;
		}
		if (fightScale [i] == 65) {
			note_container [i] [10].enabled = true;
			sign_container [i] [10].enabled = false;
		}
		if (fightScale [i] == 66) {
			note_container [i] [11].enabled = true;
			sign_container [i] [11].enabled = false;
		}
		if (fightScale [i] == 67) {
			note_container [i] [11].enabled = true;
			sign_container [i] [11].enabled = true;
		}
		if (fightScale [i] == 68) {
			note_container [i] [12].enabled = true;
			sign_container [i] [12].enabled = false;
		}
		if (fightScale [i] == 69) {
			note_container [i] [12].enabled = true;
			sign_container [i] [12].enabled = true;
		}
		if (fightScale [i] == 70) {
			note_container [i] [13].enabled = true;
			sign_container [i] [13].enabled = false;
		}
		if (fightScale [i] == 71) {
			note_container [i] [13].enabled = true;
			sign_container [i] [13].enabled = true;
		}
		if (fightScale [i] == 72) {
			note_container [i] [14].enabled = true;
			sign_container [i] [14].enabled = false;
		}
		if (fightScale [i] == 73) {
			note_container [i] [14].enabled = true;
			sign_container [i] [14].enabled = false;
		}
		if (fightScale [i] == 74) {
			note_container [i] [15].enabled = true;
			sign_container [i] [15].enabled = false;
		}

*/