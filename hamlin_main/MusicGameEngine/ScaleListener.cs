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
		Random random = new Random ();
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
			if (Input.GetKeyDown(KeyCode.Y)) {
				playedNote = 48;
			}
			if (Input.GetKeyDown(KeyCode.S)) {
				playedNote = 49;
			}
			if (Input.GetKeyDown(KeyCode.X)) {
				playedNote = 50;
			}
			if (Input.GetKeyDown(KeyCode.D)) {
				playedNote = 51;
			}
			if (Input.GetKeyDown(KeyCode.C)) {
				playedNote = 52;
			}
			if (Input.GetKeyDown(KeyCode.V)) {
				playedNote = 53;
			}
			if (Input.GetKeyDown(KeyCode.G)) {
				playedNote = 54;
			}
			if (Input.GetKeyDown(KeyCode.B)) {
				playedNote = 55;
			}
			if (Input.GetKeyDown(KeyCode.H)) {
				playedNote = 56;
			}
			if (Input.GetKeyDown(KeyCode.N)) {
				playedNote = 57;
			}
			if (Input.GetKeyDown(KeyCode.J)) {
				playedNote = 58;
			}
			if (Input.GetKeyDown(KeyCode.M)) {
				playedNote = 59;
				print ("M");
			}
			if (Input.GetKeyDown(KeyCode.Comma)) {
				playedNote = 60;
				print ("COMMA");
			}
			if (playedNote == expectedNote) {
				print ("HIT");
				expectedNoteCounter++;

			} 
			else {
				print ("MISS");
				expectedNoteCounter = 0;
				FailSound.Play ();

			}
			if (expectedNoteCounter == fightScale.Length) {
				print ("You WIN");
				expectedNoteCounter = 0;
				expectedNote = fightScale [expectedNoteCounter];
				ApplauseSound.Play ();
			} 
			else {

				expectedNote = fightScale [expectedNoteCounter];
			}
		}
	}
}
