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

  void Start () {
    anim = GetComponent<Animator>();
    nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    defeated = false;
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
  }

}
