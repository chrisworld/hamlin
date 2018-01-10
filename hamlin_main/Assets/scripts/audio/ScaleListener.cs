using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleListener : MonoBehaviour {

	public enum ScaleNames {	
		CHROMATIC_SCALE = 0,
		MAJOR_SCALE = 1,
		MINOR_SCALE = 2,
		HARMONIC_MINOR_SCALE = 3,
		MELODIC_MINOR_SCALE = 4, // mix of ascend and descend
		NATURAL_MINOR_SCALE = 5,
		DIATONIC_MINOR_SCALE = 6,
		AEOLIAN_SCALE = 7,
		PHRYGIAN_SCALE = 8,
		LOCRIAN_SCALE = 9,
		DORIAN_SCALE = 10,	
		LYDIAN_SCALE = 11,
		MIXOLYDIAN_SCALE = 12,
		PENTATONIC_SCALE = 13,
		BLUES_SCALE = 14,
		TURKISH_SCALE = 15,
		INDIAN_SCALE = 16 
	};  


	public enum NoteNames {
		C ,
		Csh_Dfl,
		D,
		Dsh_Efl,
		E,
		F,
		Fsh_Gfl,
		G,
		Gsh_Afl,
		A,
		Ash_Hfl,
		H
	};


	public AudioSource ApplauseSound;
	public AudioSource FailSound;

	int[] fightScale;

	int expectedNote;
	int expectedNoteCounter = 0;
	int playedNote;


	string GetScaleName(int index){  // Cast Scale Name
		ScaleNames enumValue = (ScaleNames)index;
		string stringName = enumValue.ToString(); 
		return stringName;
	}

	string GetBaseNoteName(int index){  // Check Base Key 

		int modIndex = index % 12;  // Check base Key by Modulo
		NoteNames enumValue = (NoteNames)modIndex;
		string stringName = enumValue.ToString(); 
		return stringName;
	}

	int GetOctave(int index){  // Get Octave Range
		return index / 12;
	}


	int[][] allScales =   // Scales Definition
	{
		new int[] {0, 2, 4, 5, 7, 9, 11, 12},
		new int[] {0, 2, 3, 5, 7, 8, 10, 12},
		new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11},
		new int[] {0, 2, 4, 5, 7, 9, 11},
		new int[] {0, 2, 3, 5, 7, 8, 10},
		new int[] {0, 2, 3, 5, 7, 8, 11},
		new int[] {0, 2, 3, 5, 7, 8, 9, 10, 11}, // mix of ascend and descend
		new int[] {0, 2, 3, 5, 7, 8, 10},
		new int[] {0, 2, 3, 5, 7, 8, 10},
		new int[] {0, 2, 3, 5, 7, 8, 10},
		new int[] {0, 1, 3, 5, 7, 8, 10},
		new int[] {0, 1, 3, 5, 6, 8, 10},
		new int[] {0, 2, 3, 5, 7, 9, 10},	
		new int[] {0, 2, 4, 6, 7, 9, 11},
		new int[] {0, 2, 4, 5, 7, 9, 10},
		new int[] {0, 2, 4, 7, 9},
		new int[] {0, 2, 3, 4, 5, 7, 9, 10, 11},
		new int[] {0, 1, 3, 5, 7, 10, 11},
		new int[] {0, 1, 1, 4, 5, 8, 10},
	};



	int[] ScaleByKey (int key, int[] scale) {

		for(int i = 0; i < scale.Length; i++)
		{
			scale[i] = scale[i] + key;
		}

		return scale;
	}


	int randomKey(int min, int max){
		//Random random = new Random ();
		int randomNumber = Random.Range (min, max);
		return randomNumber;
	}


	// Use this for initialization
	void Start () {


		int fightBaseKey = randomKey (48, 55);  // Picking Random Base Key
		int scale = randomKey (0, 5);			// Picking Random Scale

		//fightScale = scaleByKey (randomKey (), MAJOR_SCALE);  // Generate Fight Scale by Random Keys
			fightScale = ScaleByKey (fightBaseKey, allScales[scale]);  // Generate Fight Scale by Random Keys

		foreach(var item in fightScale)
		{
			print(item.ToString());
		}


		// Debugging

		expectedNote = fightScale [expectedNoteCounter];
		print ("EXPECTED NOTE");
		print(expectedNote.ToString());

		print(GetScaleName (scale));


		// Printing Scale and Base Key Information

		print ("StartNote: " + fightBaseKey);
		print ("BaseKey: " + GetBaseNoteName (fightBaseKey));
		print ("Octave: " + GetOctave (fightBaseKey));

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.anyKeyDown){

            bool musicKeyPressed = false;

            if(Input.GetKeyDown(KeyCode.Y)) {
				playedNote = 48;
                musicKeyPressed = false;
            }
			else if(Input.GetKeyDown(KeyCode.S)) {
				playedNote = 49;
                musicKeyPressed = true;
            }
			else if(Input.GetKeyDown(KeyCode.X)) {
				playedNote = 50;
                musicKeyPressed = true;
            }
			else if(Input.GetKeyDown(KeyCode.D)) {
				playedNote = 51;
                musicKeyPressed = true;
            }
			else if (Input.GetKeyDown(KeyCode.C)) {
				playedNote = 52;
                musicKeyPressed = true;
            }
			else if (Input.GetKeyDown(KeyCode.V)) {
				playedNote = 53;
                musicKeyPressed = true;
            }
			else if (Input.GetKeyDown(KeyCode.G)) {
				playedNote = 54;
                musicKeyPressed = true;
            }
			else if (Input.GetKeyDown(KeyCode.B)) {
				playedNote = 55;
                musicKeyPressed = true;
            }
			else if (Input.GetKeyDown(KeyCode.H)) {
				playedNote = 56;
                musicKeyPressed = true;
            }
			else if (Input.GetKeyDown(KeyCode.N)) {
				playedNote = 57;
                musicKeyPressed = true;
            }
			else if (Input.GetKeyDown(KeyCode.J)) {
				playedNote = 58;
                musicKeyPressed = true;
            }
			else if (Input.GetKeyDown(KeyCode.M)) {
				playedNote = 59;
                musicKeyPressed = true;
            }
			else if (Input.GetKeyDown(KeyCode.Comma)) {
				playedNote = 60;
                musicKeyPressed = true;
            }
			else if(Input.GetKeyDown(KeyCode.E)) {
				playedNote = 61;
				musicKeyPressed = false;
			}
			else if(Input.GetKeyDown(KeyCode.Alpha4)) {
				playedNote = 62;
				musicKeyPressed = true;
			}
			else if(Input.GetKeyDown(KeyCode.R)) {
				playedNote = 63;
				musicKeyPressed = true;
			}
			else if(Input.GetKeyDown(KeyCode.Alpha5)) {
				playedNote = 64;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.T)) {
				playedNote = 65;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.Z)) {
				playedNote = 66;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha7)) {
				playedNote = 67;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.U)) {
				playedNote = 68;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha8)) {
				playedNote = 69;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.I)) {
				playedNote = 70;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha9)) {
				playedNote = 71;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.O)) {
				playedNote = 72;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.P)) {
				playedNote = 73;
				musicKeyPressed = true;
			}





			if (musicKeyPressed && (playedNote == expectedNote)) {
				print ("HIT");
				expectedNoteCounter++;
			} 
			else if(musicKeyPressed){
				print ("MISS");
				expectedNoteCounter = 0;
				FailSound.Play ();
			}
            //do nothing if non-music key pressed, player should still be able to move and non-piano keys should not produce a sound

			if (expectedNoteCounter == fightScale.Length) {
				print ("You WIN");
				ApplauseSound.Play ();
                //reset
                expectedNoteCounter = 0;
                expectedNote = fightScale[expectedNoteCounter];
            } 
			else {
				expectedNote = fightScale [expectedNoteCounter];
			}
		}
	}
}
