using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

  public Text ScoreText;
  [HideInInspector]
  public LearnScale[] learn_scales;
  [HideInInspector]
  public Monster[] monsters;

  private int num_sNames = 17;
  private int[] num_scales = new int[17];
  private int[] cur_scales = new int[17];
  private int defeated_monsters;

  private int score_cur;
  private int score_total;
  private bool hideScoreTotal;

	// Use this for initialization
	void Start () {
    if(ScoreText == null){
      ScoreText = GameObject.Find("HUDCanvas/ScoreText").GetComponent<Text>();
    }
    // gameobj pointers
    monsters = (Monster[])GameObject.FindObjectsOfType(typeof(Monster));
    learn_scales = (LearnScale[]) GameObject.FindObjectsOfType(typeof(LearnScale));
    // set to zero
    for (int i = 0; i < num_sNames; i++){
      cur_scales[i] = 0;
      num_scales[i] = 0;
    }
    // get num scales from scene
    foreach (LearnScale l_scale in learn_scales){
      num_scales[(int)l_scale.scale_name]++;
    }
    defeated_monsters = 0;
    score_cur = 0;
    score_total = learn_scales.Length + monsters.Length;

    ScoreText.text = "Score: 0 / " + score_total;
	}
	
	// Update is called once per frame
	void Update () {

	}

  //Used by Monk.cs, do not remove
  public void UpdateNumScales(int scale_name){
    num_scales[scale_name]++;
  }

  //Used by Monk and EndlessTerrain
  public void SetScoreTotal(int score_total, bool hideScoreTotal){
    this.score_total = score_total;
    this.hideScoreTotal = hideScoreTotal;
    if (hideScoreTotal) {
      ScoreText.text = "Score: " + score_cur;
      Vector3 currentPos = ScoreText.gameObject.transform.parent.gameObject.transform.position;
      ScoreText.gameObject.transform.parent.gameObject.transform.position = new Vector3(currentPos.x + 20, currentPos.y, currentPos.z);    //make sure shorter scoreText is still centred
      print("I ran");
    }
    else ScoreText.text = "Score: " + score_cur + " / " + score_total;
  }

  // update scale scores, use in Learnin Scales
  public void updateScaleScore(ScaleNames s_name){
    cur_scales[(int)s_name]++;
    countCurScore();
    if (hideScoreTotal) ScoreText.text = "Score: " + score_cur;
    else ScoreText.text = "Score: " + score_cur + " / " + score_total;
  }

  // defeated a monster add to score
  public void updateDefMonster(){
    defeated_monsters++;
    countCurScore();
    if (hideScoreTotal) ScoreText.text = "Score: " + score_cur;
    else ScoreText.text = "Score: " + score_cur + " / " + score_total;
  }

  // count the current score
  public void countCurScore(){
    score_cur = 0;
    foreach (int cur_scale in cur_scales){
      score_cur += cur_scale;
    }
    score_cur += defeated_monsters;
  }
}
