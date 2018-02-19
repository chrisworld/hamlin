using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerManager : MonoBehaviour {

	int counter = 0;
	bool gotData = false;
	bool initialised = false;

	int[] f_scale;
	public Sprite[] note_sprites;

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
	public Image[] note_container12;

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
	public Image[] sign_container12;

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
		note_container_big.Add(note_container12);

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
		sign_container_big.Add(sign_container12);

    resetContainers ();
		resetCleffs ();

	}

	// Update is called once per frame
	void Update () {

    if (gotData)
    {
        configContainers(f_scale);
        configContainersSigns(f_scale);
        gotData = false;
        initialised = true;
    }
    else if(initialised)
    {
    		// Debug: ToDo
        if (Input.GetKeyDown(KeyCode.Period))
        {
            note_container1[counter].enabled = true;
            counter++;
            if (counter == 15)
            {
                counter = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            resetContainers();
            resetCleffs();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            configContainers(f_scale);
            configContainersSigns(f_scale);
        }
    }
		
	}

	// sets the scale on screen
	public void setScale(int[] newScale){
      f_scale = newScale;
      gotData = true;
  }

	void resetContainer () {
		for (int j = 0; j < 15; j++) {
			note_container1[j].enabled = false;
		}
	}

	public void resetContainers () {
		foreach (Image[] note_container in note_container_big) {
			for (int j = 0; j < 15; j++) {
				note_container [j].enabled = false;
			}
		}
	}

	public void resetCleffs () {
		foreach (Image[] sign_container in sign_container_big) {
			for (int k = 0; k < 15; k++) {
				sign_container [k].enabled = false;
			}
		}
	}

	public void updateNoteContainer(NoteState[][] note_state){

    int c = 0;
		foreach (NoteState[] notes in note_state){
			int n = 0;
			Image[] note_container = (Image[])note_container_big[c];
			foreach (NoteState note in notes){
				switch (note)
				{
					case NoteState.DISABLED: 
						note_container[n].enabled = false; 
						break;
					case NoteState.NORMAL:
            //Chris - this access to note_sprites (and the other NoteState cases too) is what's causing the index out of bounds error. Max index for note_sprites is 2 unless you add more sprites.
            //I have commented out the lineoffset accesses for now as I'm not sure how to fix this - I don't understand what the line offsets were for.

            //note_container[n].sprite = note_sprites[0 + lineOffset(n)];
            note_container[n].sprite = note_sprites[0];
            note_container[n].enabled = true;
						//Debug.Log("normal: " + c +"/"+ n);
						break;
					case NoteState.WRONG:
						//note_container[n].sprite = note_sprites[1 + lineOffset(n)];
            note_container[n].sprite = note_sprites[1];
            note_container[n].enabled = true;
						//Debug.Log("wrong: " + c +"/"+ n);
						break;
					case NoteState.RIGHT:
						//note_container[n].sprite = note_sprites[2 + lineOffset(n)];
            note_container[n].sprite = note_sprites[2];
            note_container[n].enabled = true;
						//Debug.Log("right: " + c +"/"+ n);
						break;
					default: break;
				}
				n++;
			}
			c++;
		}
	}

	public int lineOffset(int note_pos){
		int offset = 2;  //was 3 but causing index out of bounds errors as max index of sprites array is 2
		switch (note_pos)
		{
			case 0: return offset;
			case 2: return offset;
			case 4: return offset;
			case 6: return offset;
			case 8: return offset;
			case 10: return offset;
			case 12: return offset;
			case 14: return offset;
			default: break;
		}
		return 0;
	}

	public void updateSignContainer(SignState[][] sign_state){
		int c = 0;
		foreach (SignState[] signs in sign_state){
			int n = 0;
			foreach (SignState sign in signs){
				Image[] sign_container = (Image[])sign_container_big[c];
				sign_container[n].enabled = sign.act;
				n++;
			}
			c++;
		}
	}

	// write note container
	void configContainers ( int[] fightScale ) {

		int i = 0;
		foreach (Image[] note_container in note_container_big) {

			if (i < fightScale.Length){

				if (fightScale [i] == 48) {  // C
					note_container [14].enabled = true;
				}
				else if (fightScale [i] == 49) {  // cis
					note_container [14].enabled = true;
				}
				else if (fightScale [i] == 50) {	// d
					note_container [13].enabled = true;
				}
				else if (fightScale [i] == 51) {	// dis
					note_container [13].enabled = true;
				}
				else if (fightScale [i] == 52) {	// e
					note_container [12].enabled = true;
				}
				else if (fightScale [i] == 53) {  // f
					note_container [11].enabled = true;
				}
				else if (fightScale [i] == 54) {	// fis
					note_container [11].enabled = true;
				}
				else if (fightScale [i] == 55) { // g
					note_container [10].enabled = true;
				}
				else if (fightScale [i] == 56) {	// gis
					note_container [10].enabled = true;
				}
				else if (fightScale [i] == 57) { // a
					note_container [9].enabled = true;
				}
				else if (fightScale [i] == 58) {
					note_container [9].enabled = true;
				}
				else if (fightScale [i] == 59) {  // h
					note_container [8].enabled = true;
				}
				else if (fightScale [i] == 60) {  // c
					note_container [7].enabled = true;
				}
				else if (fightScale [i] == 61) { 
					note_container [7].enabled = true;
				}
				else if (fightScale [i] == 62) {  // d
					note_container [6].enabled = true;
				}
				else if (fightScale [i] == 63) {
					note_container [6].enabled = true;
				}
				else if (fightScale [i] == 64) { // e
					note_container [5].enabled = true;
				}
				else if (fightScale [i] == 65) { // f
					note_container [4].enabled = true;
				}
				else if (fightScale [i] == 66) {
					note_container [4].enabled = true;
				}
				else if (fightScale [i] == 67) { // g
					note_container [3].enabled = true;
				}
				else if (fightScale [i] == 68) {
					note_container [3].enabled = true;
				}
				else if (fightScale [i] == 69) {  // a
					note_container [2].enabled = true;
				}
				else if (fightScale [i] == 70) {
					note_container [2].enabled = true;
				}
				else if (fightScale [i] == 71) { // h
					note_container [1].enabled = true;
				}
				else if (fightScale [i] == 72) { // c
					note_container [0].enabled = true;
				}
				i++;
			}
		}
	}

	// write signs container
	void configContainersSigns ( int[] fightScale ) {
		int i = 0;
		foreach (Image[] sign_container in sign_container_big) {

			if (i < fightScale.Length){	

				if (fightScale [i] == 48) { 				// c
					sign_container[14].enabled = false;
				}
				else if (fightScale [i] == 49) {
					sign_container[14].enabled = true;
				}
				else if (fightScale [i] == 50) {   // d
					sign_container[13].enabled = false;
				}
				else if (fightScale [i] == 51) {
					sign_container[13].enabled = true;
				}
				else if (fightScale [i] == 52) {   // e
					sign_container[12].enabled = false;
				}
				else if (fightScale [i] == 53) {		// f
					sign_container[11].enabled = false;
				}
				else if (fightScale [i] == 54) {
					sign_container[11].enabled = true;
				}
				else if (fightScale [i] == 55) {   // g
					sign_container[10].enabled = false;
				}
				else if (fightScale [i] == 56) {
					sign_container[10].enabled = true;
				}
				else if (fightScale [i] == 57) {   // a
					sign_container[9].enabled = false;
				}
				else if (fightScale [i] == 58) {
					sign_container[9].enabled = true;
				}
				else if (fightScale [i] == 59) {  // h
					sign_container[8].enabled = false;
				}
				else if (fightScale [i] == 60) {  // c
					sign_container[7].enabled = false;
				}
				else if (fightScale [i] == 61) { 
					sign_container[7].enabled = true;
				}
				else if (fightScale [i] == 62) {  // d
					sign_container[6].enabled = false;
				}
				else if (fightScale [i] == 63) {
					sign_container[6].enabled = true;
				}
				else if (fightScale [i] == 64) {   // e
					sign_container[5].enabled = false;
				}
				else if (fightScale [i] == 65) {  // f
					sign_container[4].enabled = false;
				}
				else if (fightScale [i] == 66) {  
					sign_container[4].enabled = true;
				}
				else if (fightScale [i] == 67) { // g
					sign_container[3].enabled = false;
				}
				else if (fightScale [i] == 68) {
					sign_container[3].enabled = true;
				}
				else if (fightScale [i] == 69) {  // a
					sign_container[2].enabled = false;
				}
				else if (fightScale [i] == 70) {
					sign_container[2].enabled = true;
				}
				else if (fightScale [i] == 71) {  // h
					sign_container[1].enabled = false;
				}
				else if (fightScale [i] == 72) {  //
					sign_container[0].enabled = false;
				}
				
				i++;
			}
		}
	}
}

