using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

  [HideInInspector]
  public LearnScale[] learn_scales; 

  private int num_sNames = 17;
  private int[] num_scales = new int[17];
  private int[] cur_scales = new int[17];


	// Use this for initialization
	void Start () {
    learn_scales = (LearnScale[]) GameObject.FindObjectsOfType(typeof(LearnScale));
    Debug.Log("Amount of Learn Scales: " + learn_scales.Length);
    // set to zero
    for (int i = 0; i < num_sNames; i++){
      //num_scales = new int[Enum.GetNames(typeof(ScaleNames)).Length];
      //cur_scales = new int[Enum.GetNames(typeof(ScaleNames)).Length];
      cur_scales[i] = 0;
      num_scales[i] = 0;
    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void updateScaleScore(ScaleNames scale_name){

  }
}
