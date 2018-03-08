using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

  public ScaleNames scale_name;
  public NoteBaseKey base_key;
  public Health health;

  //these all need to be public so MonsterManager can access them
  //note that monster also implicitly has a transform
  [HideInInspector]
  public Animator anim;
  [HideInInspector]
  public UnityEngine.AI.NavMeshAgent nav;
  [HideInInspector]
  public int[] box_scale;
  [HideInInspector]
  public int[] box_midi;
  [HideInInspector]
  public bool defeated;
  [HideInInspector]
  public int playerDamageQueue;

  private void Awake()
  {
    anim = GetComponent<Animator>();
  }

  void Start () {
    nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    if(nav == null){
      print("nav is null!!!!");
    }
    defeated = false;
    playerDamageQueue = 0;
    if(health == null){
       health = GameObject.Find("Player").GetComponent<Health>();
    }
  }

  void Update () {
		 //intentionally empty
	}

  //Used by animation event to time health damage
  public void DamagePlayer()
  {
    health.takeDamage(1);
    //only attack once
    anim.SetBool("isAttacking", false);
    anim.SetBool("isIdle", true);
    playerDamageQueue--;
    //attack animation takes time - player may have played other wrong notes in the meantime
    if(playerDamageQueue > 0){
      DamagePlayer();
    }
  }

}
