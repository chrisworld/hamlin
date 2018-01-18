using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour {

    public ScaleListener scaleListener;
    public SkeletonController monster;     //TODO: make this generic for all monsters
    public Health player;
    public ContainerManager container;
    public GameObject infowindow;
    public Text infobox;
    public Transform playerRef;

    //NOTE: these values will need tweaking for each map!!! test them out in game
    public float monsterAttackDistance = 1;  //walk towards player if player detected and further away from this distance, else attack player
    public int monsterViewDistance = 2;    //these two vars define the space in which the monster can detect the player
    public int monsterViewAngle = 60;

    private bool gameOver;
    private bool startedPlaying;
    private PlayerController playerController;
    private bool initCombat;

	void Start () {
        int fightBaseKey = Random.Range(48, 55);  // Picking Random Base Key
        int scale = Random.Range(0, 5);           // Picking Random Scale           NOTE: currently between 0 and 5 to keep it easier
        scaleListener.InitFightScale(fightBaseKey, scale);
        monster.attackDistance = monsterAttackDistance;
        monster.viewDistance = monsterViewDistance;
        monster.viewAngle = monsterViewAngle;
        gameOver = false;
        startedPlaying = false;
        infowindow.SetActive(false);
        playerController = playerRef.GetComponent<PlayerController>();
        //TODO: fix, this doesn't seem to get called again, also we lose player ref in editor
        //onLevelFinishedLoading did not help :/
        initCombat = true;
  }

  void onEnable(){
    SceneManager.sceneLoaded += OnLevelFinishedLoading;
  }

  void onDisable(){
    SceneManager.sceneLoaded -= OnLevelFinishedLoading;
  }

  void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
    //TODO: get reference 
    //TODO: move this into start()
    if(playerRef == null){
      GameObject.Find
    }
    Start();
  }

  // Update is called once per frame
  void Update()
  {

    if (!startedPlaying && Input.anyKey)                            //player has started playing
    {
      container.setScale(scaleListener.fightScale);
      print(scaleListener.GetScaleInfo());  //TODO: this should be displayed in GUI e.g. musical notes on stave
      startedPlaying = true;
      monster.gameOver = false;
    }

    if (!gameOver)
    {
      //monster determines if we are in combat or not
      if (monster.inCombat)
      {
        
        if(initCombat){
          playerController.setMoveActivate(false);
          initCombat = false;
        }
        else if(!initCombat && playerController.getMoveActivate())    //player has jumped to escape combat
        {
          monster.inCombat = false;
          //monster.status = "idle";
          initCombat = true;
          return;
        }

        scaleListener.inCombat = true;                          //when in combat, activate scaleListener hit/miss/win mechanics; can still play notes at any time
        Camera.main.fieldOfView = 40f;                          //zoom in camera to go into 'combat mode'

        if (player.GetHealthAmount() == 0)
        {
          gameOver = true;
          infowindow.SetActive(true);
          infobox.text = "You lose :(";
          SceneManager.LoadScene("MainMenu");
        }

        if (scaleListener.playerHasWon)
        {
          monster.status = "run away";
          gameOver = true;
          infowindow.SetActive(true);
          infobox.text = "You win!";
          SceneManager.LoadScene("MainMenu");
        }

        if (scaleListener.wrongNotePlayed)
        {
          print("set monster status to attack");
          monster.status = "attack";
          scaleListener.wrongNotePlayed = false;     //reset
          //TODO: make sure it only attacks once
        }

        if (scaleListener.correctNotePlayed)
        {
          monster.status = "damaged";
          scaleListener.correctNotePlayed = false;    //reset
        }

        if (monster.playerHealthDamaged)
        {
          player.takeDamage(1);
          monster.playerHealthDamaged = false;       //reset
        }

      }
      else {
        playerController.setMoveActivate(true);
      }

    }

  }
}
