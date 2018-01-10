using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {

    public ScaleListener scaleListener;
    public SkeletonController skeleton;     //TODO: make this generic for all monsters
    public Animator anim;
    public int monsterAttackDistance = 10;  //walk towards player if player detected and further away from this distance, else attack player

    //these define the space in which the monster can detect the player
    public int monsterViewDistance = 20;
    public int monsterViewAngle = 30;

	void Start () {
        anim = GetComponent<Animator>();
        scaleListener = GetComponent<ScaleListener>();
        skeleton = GetComponent<SkeletonController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
