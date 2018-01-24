using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sign State
/* public struct SignStateSL
{
  public bool act;
  public int pos;
}

// Enums
public enum NoteStateSL
{
  DISABLED = 0,
  NORMAL = 1,
  RIGHT = 2,
  WRONG = 3
}; */   //if we ever delete LearnScale.cs, these need to be uncommented back in!!!


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
  public ContainerManager container;

  [HideInInspector]
	public int[] fightScale;

  int fightBaseKey;
  int scale;

  int expectedNote;
	int expectedNoteCounter = 0;
	int playedNote;
  Dictionary<int, string> noteKeys = new Dictionary<int, string>();
  
  // the two states as 2D
  private NoteState[][] note_state = new NoteState[11][];
  private SignState[][] sign_state = new SignState[11][];
  
  private int num_c = 11;
  private int num_n = 15;
  private int c_pos;
  private int error_counter;
  private KeyCode[] valid_keys = {
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
    KeyCode.Comma,
    KeyCode.Q,
    KeyCode.Alpha2,
    KeyCode.W,
    KeyCode.Alpha3,
    KeyCode.E,
    KeyCode.R,
    KeyCode.Alpha5,
    KeyCode.T,
    KeyCode.Alpha6,
    KeyCode.Z,
    KeyCode.Alpha7,
    KeyCode.U,
    KeyCode.I
  };

  //used by CombatManager
  [HideInInspector]
  public bool playerHasWon;

  //managed by CombatManager
  [HideInInspector]
  public bool inCombat;
  [HideInInspector]
  public bool correctNotePlayed;
  [HideInInspector]
  public bool wrongNotePlayed;

  private int[][] allScales =   // Scales Definition
	{
		new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11},
		new int[] {0, 2, 4, 5, 7, 9, 11, 12},
		new int[] {0, 2, 3, 5, 7, 8, 10, 12},
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

  public void InitFightScale(int baseKeyIndex, int scaleIndex){
    fightBaseKey = baseKeyIndex;
    scale = scaleIndex;
    fightScale = ScaleByKey(fightBaseKey, allScales[scale]);
    resetNoteState();
    resetSignState();
    setNoteStateToScale(fightScale);
    setSignStateToScale(fightScale);
    c_pos = 0;
    error_counter = 0;
  }




  //---merged from learn scale


  // remove wrong notes played before
  void cleanWrongNoteState(int[] right_scale)
  {
    for (int c = 0; c < num_c; c++)
    {
      for (int n = 0; n < num_n; n++)
      {
        if (note_state[c][n] == NoteState.WRONG)
        {
          if (scaleToContainerMapping(right_scale[c]) == n)
          {
            note_state[c][n] = NoteState.NORMAL;
            sign_state[c][n] = scaleToSignStateMapping(right_scale[c]);
          }
          else
          {
            note_state[c][n] = NoteState.DISABLED;
            sign_state[c][n].act = false;
          }
        }
      }
    }
    container.updateNoteContainer(note_state);
    container.updateSignContainer(sign_state);
  }

  // set the NoteState to all disabled
  void resetNoteState()
  {
    for (int c = 0; c < num_c; c++)
    {
      note_state[c] = new NoteState[num_n];
      for (int n = 0; n < num_n; n++)
      {
        note_state[c][n] = NoteState.DISABLED;
      }
    }
    container.updateNoteContainer(note_state);
  }

  // set the SignState to all disabled
  void resetSignState()
  {
    for (int c = 0; c < num_c; c++)
    {
      sign_state[c] = new SignState[num_n];
      for (int n = 0; n < num_n; n++)
      {
        sign_state[c][n].act = false;
      }
    }
    container.updateSignContainer(sign_state);
  }

  // set the note_state to a scale
  void setNoteStateToScale(int[] update_scale)
  {
    int ci = 0;
    int ni = 0;
    foreach (int note in update_scale)
    {
      ni = scaleToContainerMapping(note);
      note_state[ci][ni] = NoteState.NORMAL;
      ci++;
    }
    container.updateNoteContainer(note_state);
  }

  // set the sign_state to a scale
  void setSignStateToScale(int[] update_scale)
  {
    int ci = 0;
    SignState st;
    foreach (int note in update_scale)
    {
      st = scaleToSignStateMapping(note);
      sign_state[ci][st.pos].act = st.act;
      ci++;
    }
    container.updateSignContainer(sign_state);
  }

  // check if valid music key is pressed
  public bool checkValidMusicKey()
  {
    // check valid key
    foreach (KeyCode key in valid_keys)
    {
      if (Input.GetKeyDown(key))
      {
        return true;
      }
    }
    return false;
  }

  // get mask of pressed keys
  public bool[] getKeyMask()
  {
    int k = 0;
    bool[] key_mask = new bool[valid_keys.Length];
    // set to zero
    for (int c = 0; c < valid_keys.Length; c++)
    {
      key_mask[c] = false;
    }
    // get mask
    foreach (KeyCode key in valid_keys)
    {
      if (Input.GetKeyDown(key))
      {
        key_mask[k] = true;
      }
      k++;
    }
    return key_mask;
  }

  // map the keys to midi
  public int keyToMidiMapping(int key)
  {
    return key + 48;
  }

  // puts a scale to a midi array
  public int[] scaleToMidi(int[] scale)
  {
    int[] midi = new int[scale.Length];
    for (int i = 0; i < scale.Length; i++)
    {
      midi[i] = scale[i] + (int)fightBaseKey;
    }
    return midi;
  }

  public int scaleToContainerMapping(int scale_note)
  {
    return midiToContainerMapping((int)fightBaseKey + scale_note);
  }

  public SignState scaleToSignStateMapping(int scale_note)
  {
    return midiToSignState((int)fightBaseKey + scale_note);
  }

  public int midiToContainerMapping(int midi)
  {
    switch (midi)
    {
      case 48:
      case 49: return 14; // c, cis
      case 50:
      case 51: return 13; // d, dis
      case 52: return 12; // e
      case 53:
      case 54: return 11; // f, fis
      case 55:
      case 56: return 10;
      case 57:
      case 58: return 9;
      case 59: return 8;
      case 60:
      case 61: return 7;
      case 62:
      case 63: return 6;
      case 64: return 5;
      case 65:
      case 66: return 4;
      case 67:
      case 68: return 3;
      case 69:
      case 70: return 2;
      case 71: return 1;
      case 72: return 0;
      default: break;
    }
    return 0;
  }

  public SignState midiToSignState(int midi)
  {
    SignState st;
    st.act = false;
    st.pos = 0;
    switch (midi)
    {
      case 48: st.pos = 14; st.act = false; return st;  // c
      case 49: st.pos = 14; st.act = true; return st; //cis
      case 50: st.pos = 13; st.act = false; return st;  // d
      case 51: st.pos = 13; st.act = true; return st; // dis
      case 52: st.pos = 12; st.act = false; return st;  // e
      case 53: st.pos = 11; st.act = false; return st;  //f
      case 54: st.pos = 11; st.act = true; return st;
      case 55: st.pos = 10; st.act = false; return st;
      case 56: st.pos = 10; st.act = true; return st;
      case 57: st.pos = 9; st.act = false; return st;
      case 58: st.pos = 9; st.act = true; return st;
      case 59: st.pos = 8; st.act = false; return st;
      case 60: st.pos = 7; st.act = false; return st;
      case 61: st.pos = 7; st.act = true; return st;
      case 62: st.pos = 6; st.act = false; return st;
      case 63: st.pos = 6; st.act = true; return st;
      case 64: st.pos = 5; st.act = false; return st;
      case 65: st.pos = 4; st.act = false; return st;
      case 66: st.pos = 4; st.act = true; return st;
      case 67: st.pos = 3; st.act = false; return st;
      case 68: st.pos = 3; st.act = true; return st;
      case 69: st.pos = 2; st.act = false; return st;
      case 70: st.pos = 2; st.act = true; return st;
      case 71: st.pos = 1; st.act = false; return st;
      case 72: st.pos = 0; st.act = false; return st;
      default: break;
    }
    return st;
  }

  public void UpdateNoteStates(int playedNote, bool noteCorrect)
  {
    // check each key
    //int key = 0;
    //bool[] key_mask = getKeyMask();
    //foreach (bool mask in key_mask)
    //{
      //if (mask)
      //{
        cleanWrongNoteState(fightScale);
        int note_midi = keyToMidiMapping(playedNote);
        int note_pos = midiToContainerMapping(note_midi);
        if (noteCorrect)
        {
          note_state[c_pos][note_pos] = NoteState.RIGHT;
          sign_state[c_pos][note_pos] = midiToSignState(note_midi);
          c_pos++;
        }
        else
        {
          note_state[c_pos][note_pos] = NoteState.WRONG;
          sign_state[c_pos][note_pos] = midiToSignState(note_midi);
          error_counter++;
        }
      //}
      //key++;
    //}
    container.updateNoteContainer(note_state);
    container.updateSignContainer(sign_state);
  }

  //---end merge from learn scale


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
            correctNotePlayed = true;
            expectedNoteCounter++;
            UpdateNoteStates(playedNote, true);
        }
        else if (musicKeyPressed)
        {
            print("MISS");
            wrongNotePlayed = true;
            expectedNoteCounter = 0;
            UpdateNoteStates(playedNote, false);
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
      else {
        //reset
        c_pos = 0;
        error_counter = 0;
        resetNoteState();
        resetSignState();
      }
		}
	}


}
