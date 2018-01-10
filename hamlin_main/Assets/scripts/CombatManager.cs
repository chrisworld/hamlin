using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {

    public ScaleListener scaleListener;
    public SkeletonController monster;     //TODO: make this generic for all monsters
    public Health player;

    //NOTE: these values will need tweaking for each map!!! test them out in game
    public float monsterAttackDistance = 1;  //walk towards player if player detected and further away from this distance, else attack player
    public int monsterViewDistance = 2;    //these two vars define the space in which the monster can detect the player
    public int monsterViewAngle = 60;

    private int damage;

	void Start () {
        //int fightBaseKey = Random.Range(48, 55);  // Picking Random Base Key
        //int scale = Random.Range(0, 5);           // Picking Random Scale           NOTE: currently between 0 and 5 to keep it easier
        int fightBaseKey = 53;
        int scale = 0;
        scaleListener.SetFightBaseKey (fightBaseKey);
        scaleListener.SetScale (scale);
        string scaleInfo = scaleListener.GetScaleInfo();
        print(scaleInfo); //debug
        //TODO: show scaleInfo in GUI so player knows which scale to play
        monster.SetAttackDistance (monsterAttackDistance);
        monster.SetViewDistance (monsterViewDistance);
        monster.SetViewAngle (monsterViewAngle);
        damage = 0;
    }
	
	// Update is called once per frame
	void Update () {

        //monster determines if we are in combat or not
        if ( monster.GetInCombat() )
        {
            scaleListener.SetInCombat (true);                         //when in combat, activate scaleListener hit/miss/win mechanics; can still play notes at any time
            if ( monster.GetPlayerHealthDamaged() )
            {
               damage++;
               monster.ResetPlayerHealthDamaged();
            }
            if(damage % 60 == 0)
            {
                player.takeDamage(1);
            }
        }

        //scale listener determines if player has won or not
        if ( scaleListener.GetPlayerHasWon() )
        {
            monster.SetPlayerHasWon (true);
        }

	}
}
