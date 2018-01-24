using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Structs
public struct SignState  
{  
    public bool act;
    public int pos;
}

// Enums
public enum NoteState {
	DISABLED = 0,
	NORMAL = 1,
	RIGHT = 2,
	WRONG = 3
};

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

public class Definitions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
