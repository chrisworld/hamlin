using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	public int hearts_amount;
	public int pos_x_offset;
	public float scale_gap;
	public GameObject heartNote;

	// Use this for initialization
	void Start () {
		// get width of heart image
		RectTransform re_heart = heartNote.GetComponent<RectTransform>();
    float heart_width = re_heart.rect.width;

		for (int i = 0; i < hearts_amount; i++) {
        Transform tr_heart = ((GameObject)(Instantiate(heartNote, this.transform))).transform;
        // change new heart position
        tr_heart.localPosition = tr_heart.localPosition + 
        	new Vector3(heart_width*scale_gap*i - pos_x_offset, -0*i, 0);
    }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
