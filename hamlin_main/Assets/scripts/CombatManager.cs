using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {

    public ScaleListener scaleListener;
    public SkeletonController monster;     //TODO: make this generic for all monsters
    public Animator anim;

    public int monsterAttackDistance = 10;  //walk towards player if player detected and further away from this distance, else attack player
    public int monsterViewDistance = 20;    //these two vars define the space in which the monster can detect the player
    public int monsterViewAngle = 30;

	void Start () {
        anim = GetComponent<Animator>();
        scaleListener = GetComponent<ScaleListener>();
        monster = GetComponent<SkeletonController>();

        int fightBaseKey = Random.Range(48, 55);  // Picking Random Base Key
        int scale = Random.Range(0, 5);		      // Picking Random Scale           NOTE: currently between 0 and 5 to keep it easier
        scaleListener.fightBaseKey = fightBaseKey;
        scaleListener.scale = scale;
        string scaleInfo = scaleListener.GetScaleInfo();
        //TODO: show scaleInfo in GUI so player knows which scale to play

        monster.attackDistance = monsterAttackDistance;
        monster.viewDistance = monsterViewDistance;
        monster.viewAngle = monsterViewAngle;
    }
	
	// Update is called once per frame
	void Update () {

        //monster determines if we are in combat or not
        if (monster.inCombat)
        {
            scaleListener.inCombat = true;                         //when in combat, activate scaleListener hit/miss/win mechanics; can still play notes at any time
        }

        //scale listener determines if player has won or not
        if (scaleListener.playerHasWon)
        {
            monster.playerHasWon = true;
        }
	}
}
