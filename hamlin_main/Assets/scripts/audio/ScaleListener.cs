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

    //used by CombatManager
    public bool playerHasWon;

    //managed by CombatManager
    public bool inCombat;
    public int fightBaseKey;
    public int scale;

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

    //TODO: modify this so it returns a string with all the info the player needs about the scale to display in GUI
    //used by CombatManager
    public string GetScaleInfo()
    {
        // Debugging
        //expectedNote = fightScale[expectedNoteCounter];
        //print("EXPECTED NOTE");
        //print(expectedNote.ToString());

        return GetScaleName(scale);
        //print("StartNote: " + fightBaseKey);
        //print("BaseKey: " + GetBaseNoteName(fightBaseKey));
        //print("Octave: " + GetOctave(fightBaseKey));
    }

	// Use this for initialization
	void Start () {

        inCombat = false;
        playerHasWon = false;
        fightBaseKey = -1;
        scale = -1;
	}
	
	// Update is called once per frame
	void Update () {

        //wait until values set by CombatManager
        if(fightScale.Length == 0 && fightBaseKey != -1 && scale != -1)
        {
            print("Debug: this should run once at start and only once");
            fightScale = ScaleByKey(fightBaseKey, allScales[scale]);  // Generate Fight Scale by Random Keys
        }

        if (fightScale.Length > 0 && Input.anyKeyDown){

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

            if (inCombat)
            {
                if (musicKeyPressed && (playedNote == expectedNote))
                {
                    print("HIT");
                    expectedNoteCounter++;
                }
                else if (musicKeyPressed)
                {
                    print("MISS");
                    expectedNoteCounter = 0;
                    FailSound.Play();
                }
                //do nothing if non-music key pressed, player should still be able to move and non-piano keys should not produce a sound

                if (expectedNoteCounter == fightScale.Length)
                {
                    print("You WIN");
                    ApplauseSound.Play();
                    playerHasWon = true;
                    //reset
                    expectedNoteCounter = 0;
                    expectedNote = fightScale[expectedNoteCounter];
                }
                else
                {
                    expectedNote = fightScale[expectedNoteCounter];
                }
            }

		}
	}
}
