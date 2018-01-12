using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerManager : MonoBehaviour {

	int counter = 0;

	public int[] fightScale = {48,49,52,54,56,68,60};

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

	public Image[] sign_container1;
	public Image[] sign_container2;
	public Image[] sign_container3;
	public Image[] sign_container4;
	public Image[] sign_container5;
	public Image[] sign_container6;
	public Image[] sign_container7;
	public Image[] sign_container8;
	public Image[] sign_container9;
	public Image[] sign_container10;
	public Image[] sign_container11;





	ArrayList note_container_big = new ArrayList();
	ArrayList sign_container_big = new ArrayList();


	// Use this for initialization
	void Start () {



		note_container_big.Add(note_container1);
		note_container_big.Add(note_container2);
		note_container_big.Add(note_container3);
		note_container_big.Add(note_container4);
		note_container_big.Add(note_container5);
		note_container_big.Add(note_container6);
		note_container_big.Add(note_container7);
		note_container_big.Add(note_container8);
		note_container_big.Add(note_container9);
		note_container_big.Add(note_container10);
		note_container_big.Add(note_container11);

		sign_container_big.Add(sign_container1);
		sign_container_big.Add(sign_container2);
		sign_container_big.Add(sign_container3);
		sign_container_big.Add(sign_container4);
		sign_container_big.Add(sign_container5);
		sign_container_big.Add(sign_container6);
		sign_container_big.Add(sign_container7);
		sign_container_big.Add(sign_container8);
		sign_container_big.Add(sign_container9);
		sign_container_big.Add(sign_container10);
		sign_container_big.Add(sign_container11);

		resetContainers ();
		configContainers (fightScale);

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Period)) {
			note_container1[counter].enabled = true;
			counter++;
			if (counter == 15) {
				counter = 0;
			}
		}
		if (Input.GetKeyDown(KeyCode.Minus)) {
			resetContainers ();
		}
		if (Input.GetKeyDown(KeyCode.Alpha0)) {
			configContainers (fightScale);
			configContainersSigns (fightScale);
		}
	}

	void configContainers ( int[] fightScale ) {

		print ("HALLO");



		int i = 0;

			foreach (Image[] note_container in note_container_big) {


				if (fightScale [i] == 48) { 
					note_container [0].enabled = true;
					print ("Yes 48");
					//sign_container[0].enabled = false;
				}
				else if (fightScale [i] == 49) {
					note_container [0].enabled = true;
					//sign_container[0].enabled = true;
				}
				else if (fightScale [i] == 50) {
					note_container [1].enabled = true;
					print ("Yes 50");
					//sign_container[1].enabled = false;
				}
				else if (fightScale [i] == 51) {
					note_container [1].enabled = true;
					//sign_container[1].enabled = true;
				}
				else if (fightScale [i] == 52) {
					note_container [2].enabled = true;
					//sign_container[2].enabled = false;
				}
				else if (fightScale [i] == 53) {
					note_container [3].enabled = true;
					//sign_container[3].enabled = false;
				}
				else if (fightScale [i] == 54) {
					note_container [3].enabled = true;
					//sign_container[3].enabled = true;
				}
				else if (fightScale [i] == 55) {
					note_container [4].enabled = true;
					//sign_container[4].enabled = false;
				}
				else if (fightScale [i] == 56) {
					note_container [4].enabled = true;
					//sign_container[4].enabled = true;
				}
				else if (fightScale [i] == 57) {
					note_container [5].enabled = true;
					//sign_container[5].enabled = false;
				}
				else if (fightScale [i] == 58) {
					note_container [5].enabled = true;
					//sign_container[5].enabled = true;
				}
				else if (fightScale [i] == 59) {
					note_container [6].enabled = true;
					//sign_container[6].enabled = false;
				}
				else if (fightScale [i] == 60) {
					note_container [7].enabled = true;
					//sign_container[7].enabled = false;
				}
				else if (fightScale [i] == 61) { 
					note_container [8].enabled = true;
					//sign_container[8].enabled = false;
				}
				else if (fightScale [i] == 62) {
					note_container [8].enabled = true;
					//sign_container[8].enabled = true;
				}
				else if (fightScale [i] == 63) {
					note_container [9].enabled = true;
					//sign_container[9].enabled = false;
				}
				else if (fightScale [i] == 64) {
					note_container [9].enabled = true;
					//sign_container[9].enabled = true;
				}
				else if (fightScale [i] == 65) {
					note_container [10].enabled = true;
					//sign_container[10].enabled = false;
				}
				else if (fightScale [i] == 66) {
					note_container [11].enabled = true;
					//sign_container[11].enabled = false;
				}
				else if (fightScale [i] == 67) {
					note_container [11].enabled = true;
					//sign_container[11].enabled = true;
				}
				else if (fightScale [i] == 68) {
					note_container [12].enabled = true;
					//sign_container[12].enabled = false;
				}
				else if (fightScale [i] == 69) {
					note_container [12].enabled = true;
					//sign_container[12].enabled = true;
				}
				else if (fightScale [i] == 70) {
					note_container [13].enabled = true;
					//sign_container[13].enabled = false;
				}
				else if (fightScale [i] == 71) {
					note_container [13].enabled = true;
					//sign_container[13].enabled = true;
				}
				else if (fightScale [i] == 72) {
					note_container [14].enabled = true;
					//sign_container[14].enabled = false;
				}
				else if (fightScale [i] == 73) {
					note_container [14].enabled = true;
					//sign_container[14].enabled = false;
				}
				else if (fightScale [i] == 74) {
					note_container [15].enabled = true;
					//sign_container[15].enabled = false;
				}

			i++;

			}
	}

	void configContainersSigns ( int[] fightScale ) {

		print ("HALLO");



		int i = 0;

		foreach (Image[] sign_container in sign_container_big) {


			if (fightScale [i] == 48) { 				
				sign_container[0].enabled = false;
			}
			else if (fightScale [i] == 49) {
				sign_container[0].enabled = true;
			}
			else if (fightScale [i] == 50) {
				sign_container[1].enabled = false;
			}
			else if (fightScale [i] == 51) {
				sign_container[1].enabled = true;
			}
			else if (fightScale [i] == 52) {
				sign_container[2].enabled = false;
			}
			else if (fightScale [i] == 53) {
				sign_container[3].enabled = false;
			}
			else if (fightScale [i] == 54) {
				sign_container[3].enabled = true;
			}
			else if (fightScale [i] == 55) {
				sign_container[4].enabled = false;
			}
			else if (fightScale [i] == 56) {
				sign_container[4].enabled = true;
			}
			else if (fightScale [i] == 57) {
				sign_container[5].enabled = false;
			}
			else if (fightScale [i] == 58) {
				sign_container[5].enabled = true;
			}
			else if (fightScale [i] == 59) {
				sign_container[6].enabled = false;
			}
			else if (fightScale [i] == 60) {
				sign_container[7].enabled = false;
			}
			else if (fightScale [i] == 61) { 
				sign_container[8].enabled = false;
			}
			else if (fightScale [i] == 62) {
				sign_container[8].enabled = true;
			}
			else if (fightScale [i] == 63) {
				sign_container[9].enabled = false;
			}
			else if (fightScale [i] == 64) {
				sign_container[9].enabled = true;
			}
			else if (fightScale [i] == 65) {
				sign_container[10].enabled = false;
			}
			else if (fightScale [i] == 66) {
				sign_container[11].enabled = false;
			}
			else if (fightScale [i] == 67) {
				sign_container[11].enabled = true;
			}
			else if (fightScale [i] == 68) {
				sign_container[12].enabled = false;
			}
			else if (fightScale [i] == 69) {
				sign_container[12].enabled = true;
			}
			else if (fightScale [i] == 70) {
				sign_container[13].enabled = false;
			}
			else if (fightScale [i] == 71) {
				sign_container[13].enabled = true;
			}
			else if (fightScale [i] == 72) {
				sign_container[14].enabled = false;
			}
			else if (fightScale [i] == 73) {
				sign_container[14].enabled = false;
			}
			else if (fightScale [i] == 74) {
				sign_container[15].enabled = false;
			}

			i++;

		}
	}

	void resetContainer () {

		for (int j = 0; j < 15; j++) {

			note_container1[j].enabled = false;

		}
	}

	void resetContainers () {

		foreach (Image[] note_container in note_container_big) {
			for (int j = 0; j < 15; j++) {

				note_container [j].enabled = false;
			}
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

	for (int i = 0; i < fightScale.Length-1; i++) {

		if (fightScale [i] == 48) { 
			note_container [0].enabled = true;
			sign_container[0].enabled = false;
		}
		if (fightScale [i] == 49) {
			note_container [0].enabled = true;
			sign_container[0].enabled = true;
		}
		if (fightScale [i] == 50) {
			note_container [1].enabled = true;
			sign_container[1].enabled = false;
		}
		if (fightScale [i] == 51) {
			note_container [1].enabled = true;
			sign_container[1].enabled = true;
		}
		if (fightScale [i] == 52) {
			note_container [2].enabled = true;
			sign_container[2].enabled = false;
		}
		if (fightScale [i] == 53) {
			note_container [3].enabled = true;
			sign_container[3].enabled = false;
		}
		if (fightScale [i] == 54) {
			note_container [3].enabled = true;
			sign_container[3].enabled = true;
		}
		if (fightScale [i] == 55) {
			note_container [4].enabled = true;
			sign_container[4].enabled = false;
		}
		if (fightScale [i] == 56) {
			note_container [4].enabled = true;
			sign_container[4].enabled = true;
		}
		if (fightScale [i] == 57) {
			note_container [5].enabled = true;
			sign_container[5].enabled = false;
		}
		if (fightScale [i] == 58) {
			note_container [5].enabled = true;
			sign_container[5].enabled = true;
		}
		if (fightScale [i] == 59) {
			note_container [6].enabled = true;
			sign_container[6].enabled = false;
		}
		if (fightScale [i] == 60) {
			note_container [7].enabled = true;
			sign_container[7].enabled = false;
		}
		if (fightScale [i] == 61) { 
			note_container [8].enabled = true;
			sign_container[8].enabled = false;
		}
		if (fightScale [i] == 62) {
			note_container [8].enabled = true;
			sign_container[8].enabled = true;
		}
		if (fightScale [i] == 63) {
			note_container [9].enabled = true;
			sign_container[9].enabled = false;
		}
		if (fightScale [i] == 64) {
			note_container [9].enabled = true;
			sign_container[9].enabled = true;
		}
		if (fightScale [i] == 65) {
			note_container [10].enabled = true;
			sign_container[10].enabled = false;
		}
		if (fightScale [i] == 66) {
			note_container [11].enabled = true;
			sign_container[11].enabled = false;
		}
		if (fightScale [i] == 67) {
			note_container [11].enabled = true;
			sign_container[11].enabled = true;
		}
		if (fightScale [i] == 68) {
			note_container [12].enabled = true;
			sign_container[12].enabled = false;
		}
		if (fightScale [i] == 69) {
			note_container [12].enabled = true;
			sign_container[12].enabled = true;
		}
		if (fightScale [i] == 70) {
			note_container [13].enabled = true;
			sign_container[13].enabled = false;
		}
		if (fightScale [i] == 71) {
			note_container [13].enabled = true;
			sign_container[13].enabled = true;
		}
		if (fightScale [i] == 72) {
			note_container [14].enabled = true;
			sign_container[14].enabled = false;
		}
		if (fightScale [i] == 73) {
			note_container [14].enabled = true;
			sign_container[14].enabled = false;
		}
		if (fightScale [i] == 74) {
			note_container [15].enabled = true;
			sign_container[15].enabled = false;
		}

*/