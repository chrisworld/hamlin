using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enums
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

public enum NoteBaseKey {
	BASE_C = 48,
	BASE_Csh = 49,
	BASE_D = 50,
	BASE_Dsh = 51,
	BASE_E = 52,
	BASE_F = 53,
	BASE_Fsh = 54,
	BASE_G = 55,
	BASE_Gsh = 56,
	BASE_A = 57,
	BASE_Ash = 58,
	BASE_H = 59
};


public class ScaleListener : MonoBehaviour {

  public AudioSource ApplauseSound;
	public AudioSource FailSound;

	int[] fightScale;

	int expectedNote;
	int expectedNoteCounter = 0;
	int playedNote;
  Dictionary<int, string> noteKeys = new Dictionary<int, string>();  

  //used by CombatManager
  private bool playerHasWon;

  //managed by CombatManager
  private bool inCombat;
  private int fightBaseKey;
  private int scale;

  private int[][] allScales =   // Scales Definition
	{
		new int[] {0, 2, 4, 5, 7, 9, 11, 12},
		new int[] {0, 2, 3, 5, 7, 8, 10, 12},
		new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,12},
		new int[] {0, 2, 4, 5, 7, 9, 11, 12},
		new int[] {0, 2, 3, 5, 7, 8, 10, 12},
		new int[] {0, 2, 3, 5, 7, 8, 11, 12},
		new int[] {0, 2, 3, 5, 7, 8, 9, 10, 11, 12}, // mix of ascend and descend
		new int[] {0, 2, 3, 5, 7, 8, 10, 12},
		new int[] {0, 2, 3, 5, 7, 8, 10, 12},
		new int[] {0, 2, 3, 5, 7, 8, 10, 12},
		new int[] {0, 1, 3, 5, 7, 8, 10, 12},
		new int[] {0, 1, 3, 5, 6, 8, 10, 12},
		new int[] {0, 2, 3, 5, 7, 9, 10, 12},	
		new int[] {0, 2, 4, 6, 7, 9, 11, 12},
		new int[] {0, 2, 4, 5, 7, 9, 10, 12},
		new int[] {0, 2, 4, 7, 9, 12},
		new int[] {0, 2, 3, 4, 5, 7, 9, 10, 11, 12},
		new int[] {0, 1, 3, 5, 7, 10, 11, 12},
		new int[] {0, 1, 1, 4, 5, 8, 10 ,12},
	};

	// get a full scale from allScales
	public int[] getFullScale(ScaleNames scale_index){
		return allScales[(int)scale_index];
	}

  // Get Name of the scale
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

	// scales by key
	public int[] ScaleByKey (int key, int[] scale) {
		for(int i = 0; i < scale.Length; i++){
			scale[i] = scale[i] + key;
		}
		return scale;
	}

	// Use this for initialization
	void Start () {

        inCombat = false;
        playerHasWon = false;
        
        //populate dictionary for converting note numbers to the keyboard keys
        noteKeys.Add(48, "Y");
        noteKeys.Add(49, "S");
        noteKeys.Add(50, "X");
        noteKeys.Add(51, "D");
        noteKeys.Add(52, "C");
        noteKeys.Add(53, "V");
        noteKeys.Add(54, "G");
        noteKeys.Add(55, "B");
        noteKeys.Add(56, "H");
        noteKeys.Add(57, "N");
        noteKeys.Add(58, "J");
        noteKeys.Add(59, "M");
        noteKeys.Add(60, "comma or Q");
        noteKeys.Add(61, "2");
        noteKeys.Add(62, "W");
        noteKeys.Add(63, "3");
        noteKeys.Add(64, "E");
        noteKeys.Add(65, "R");
        noteKeys.Add(66, "5");
        noteKeys.Add(67, "T");
        noteKeys.Add(68, "6");
        noteKeys.Add(69, "Z");
        noteKeys.Add(70, "7");
        noteKeys.Add(71, "U");
        noteKeys.Add(72, "i");

    }

  // Update is called once per frame
  void Update () {

    //wait until values set by CombatManager
    if(fightScale == null){
        fightScale = ScaleByKey(fightBaseKey, allScales[scale]);  // Generate Fight Scale by Random Keys
    }

    if (fightScale.Length > 0 && Input.anyKeyDown){

      bool musicKeyPressed = false;

      if(Input.GetKeyDown(KeyCode.Y)) {
					playedNote = 48;
          musicKeyPressed = true;
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
			else if(Input.GetKeyDown(KeyCode.Q)) {
				playedNote = 60;
				musicKeyPressed = true;
			}
			else if(Input.GetKeyDown(KeyCode.Alpha2)) {
				playedNote = 61;
				musicKeyPressed = true;
			}
			else if(Input.GetKeyDown(KeyCode.W)) {
				playedNote = 62;
				musicKeyPressed = true;
			}
			else if(Input.GetKeyDown(KeyCode.Alpha3)) {
				playedNote = 63;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.E)) {
				playedNote = 64;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.R)) {
				playedNote = 65;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha5)) {
				playedNote = 66;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.T)) {
				playedNote = 67;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha6)) {
				playedNote = 68;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.Z)) {
				playedNote = 69;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha7)) {
				playedNote = 70;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.U)) {
				playedNote = 71;
				musicKeyPressed = true;
			}
			else if (Input.GetKeyDown(KeyCode.I)) {
				playedNote = 72;
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

    //Getters and setters for CombatManager

    public bool GetPlayerHasWon()
    {
        return playerHasWon;
    }

    public int[] GetFightScale()
    {
        return fightScale;
    }

    //TODO: modify this so it returns a string with all the info the player needs about the scale to display in GUI
    //used by CombatManager
    public string GetScaleInfo()
    {

        string scaleString = GetScaleName(scale) + ": play ";
        for (int i = 0; i < fightScale.Length; i++)
        {
            scaleString += noteKeys[fightScale[i]];
            if (i != (fightScale.Length - 1))
            {
               scaleString += ", ";
            }
        }

        return scaleString;
    }

    public void SetInCombat(bool value)
    {
        inCombat = value;
    }

    public void SetFightBaseKey(int value)
    {
        fightBaseKey = value;
    }

    public void SetScale(int value)
    {
        scale = value;
    }


}
