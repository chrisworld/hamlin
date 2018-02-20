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
      cur_scales[i] = 0;
      num_scales[i] = 0;
    }
    // get num scales from scene
    foreach (LearnScale l_scale in learn_scales){
      num_scales[(int)l_scale.scale_name]++;
    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void updateScaleScore(ScaleNames s_name){
    cur_scales[(int)s_name]++;
  }
}
