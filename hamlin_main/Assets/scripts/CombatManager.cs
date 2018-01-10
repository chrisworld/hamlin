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
    private bool gameOver;
    private bool startedPlaying;

	void Start () {
        int fightBaseKey = Random.Range(48, 55);  // Picking Random Base Key
        int scale = Random.Range(0, 5);           // Picking Random Scale           NOTE: currently between 0 and 5 to keep it easier
        
        //uncomment to test win condition
        //fightBaseKey = 53;        
        //scale = 0;
        //print("DEBUG: to test win condition, press V, B, N, J, comma, R, T, Z");
        scaleListener.SetFightBaseKey (fightBaseKey);
        scaleListener.SetScale (scale);
        monster.SetAttackDistance (monsterAttackDistance);
        monster.SetViewDistance (monsterViewDistance);
        monster.SetViewAngle (monsterViewAngle);
        damage = 0;
        gameOver = false;
        startedPlaying = false;
    }
	
	// Update is called once per frame
	void Update () {

        if(!startedPlaying && Input.anyKey)                            //player has started playing
        {
            string scaleInfo = scaleListener.GetScaleInfo();
            print(scaleInfo);  //TODO: this should be displayed in GUI e.g. musical notes on stave
            startedPlaying = true;
        }

        //monster determines if we are in combat or not
        if ( !gameOver && monster.GetInCombat() )
        {
            scaleListener.SetInCombat (true);                         //when in combat, activate scaleListener hit/miss/win mechanics; can still play notes at any time
            if ( monster.GetPlayerHealthDamaged() )
            {
               damage++;
               monster.ResetPlayerHealthDamaged();
            }
            if(damage != 0 && damage % 120 == 0)
            {
                player.takeDamage(1);
                damage = 0; //reset count
            }
            if(player.GetHealthAmount() == 0)
            {
                print("YOU LOSE");
                gameOver = true;
                //TODO: show end game screen
                //TODO: disable input
            }
        }

        //scale listener determines if player has won or not
        if ( !gameOver && scaleListener.GetPlayerHasWon() )
        {
            monster.SetPlayerHasWon (true);
            gameOver = true;
            //TODO: show win end game screen
            //TODO: disable input
        }

	}
}
