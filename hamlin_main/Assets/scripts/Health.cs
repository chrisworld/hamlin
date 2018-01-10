using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public int heart_amount;
	public Image[] health_images;
	public Sprite[] health_sprites;

	private int maxHeart_amount = 5;
	private int health_perHeart = 1;
	private int curHealth;
	private int maxHealth;

	void Start () {
		curHealth = heart_amount * health_perHeart;
		maxHealth = curHealth;
		checkHealthAmount();
	}
	
	void checkHealthAmount()
	{
		for (int i = 0; i < maxHeart_amount; i++) {
			if (heart_amount <= i){
				health_images[i].enabled = false;
			}
			else{
				health_images[i].enabled = true;
			}
    }
	}

	void updateHearts()
	{
		int i = 0;
		foreach (Image image in health_images){
			// full hearts
			if(curHealth > i){
				image.sprite = health_sprites[0];
			}
			// empty hearts
			else{
				image.sprite = health_sprites[1];
			}
			i++;
		}
	}

	public void takeDamage (int damage) {
		curHealth = curHealth - damage;
		curHealth = Mathf.Clamp (curHealth, 0, maxHealth);
		Debug.Log("Health: " + curHealth);
		updateHearts();
	}

	public void healHearts (int heal) {
		curHealth = curHealth + heal;
		curHealth = Mathf.Clamp (curHealth, 0, maxHealth);
		Debug.Log("Health: " + curHealth);
		updateHearts();
	}
}



/* 
//Heart instantiate
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
	*/