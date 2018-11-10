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
  public bool dying;
  [HideInInspector]
  public int playerDamageQueue;

  private void Awake()
  {
    anim = GetComponent<Animator>();
    if (anim == null){
      anim = this.gameObject.transform.GetChild(0).GetComponent<Animator>();
    }
  }

  void Start () {
    nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
  //or with new monster just manually called? no attack anim
  public void DamagePlayer()
  {
    health.takeDamage(1);
    //only attack once
    //anim.SetBool("isAttacking", false);
    //anim.SetBool("isIdle", true);

    playerDamageQueue--;
    //attack animation takes time - player may have played other wrong notes in the meantime
    if(playerDamageQueue > 0){
      DamagePlayer();
    }
  }


}
