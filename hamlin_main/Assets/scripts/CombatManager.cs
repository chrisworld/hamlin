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
    public SoundPlayer soundPlayer;

    //NOTE: these values will need tweaking for each map!!! test them out in game
    public float monsterAttackDistance = 1;  //walk towards player if player detected and further away from this distance, else attack player
    public int monsterViewDistance = 2;    //these two vars define the space in which the monster can detect the player
    public int monsterViewAngle = 60;

    public ScaleNames scale_name;
    public NoteBaseKey base_key;

    private bool gameOver;
    private bool startedPlaying;
    private PlayerController playerController;
    private bool initCombat;

	void Start () 
  {
    //NOTE: currently between 0 and 5 to keep it easier
    //int fightBaseKey = Random.Range(48, 55);  // Picking Random Base Key
    //int scale = Random.Range(0, 2);           // Picking Random Scale           
    int fightBaseKey = (int)base_key;
    int scale = (int)scale_name;
    scaleListener.InitFightScale(fightBaseKey, scale);

    monster.attackDistance = monsterAttackDistance;
    monster.viewDistance = monsterViewDistance;
    monster.viewAngle = monsterViewAngle;
    gameOver = false;
    startedPlaying = false;
    infowindow.SetActive(false);
    playerController = playerRef.GetComponent<PlayerController>();
    initCombat = true;
  }

  // Update is called once per frame
  void Update()
  {
    if (!startedPlaying && Input.anyKey)           //player has started playing
    {
      //container.setScale(scaleListener.fightScale);
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
          print("combat");
          scaleListener.setNoteStateToScale(scaleListener.getFullScale(scale_name));
          playerController.setMoveActivate(false);                    //stop player moving
          //TODO: make player and monster face each other
          initCombat = false;
        }
        else if(!initCombat && playerController.getMoveActivate())    //player has jumped to escape combat
        {
          setAllCombatStatus(false);
          initCombat = true;
          return;
        }

        setAllCombatStatus(true);
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

        //combat rework: monster will only attack if wrong note played, and then only attacks once
        if (scaleListener.wrongNotePlayed)
        {
          monster.status = "attack";
          scaleListener.wrongNotePlayed = false;     //reset
        }

        if (scaleListener.correctNotePlayed)
        {
          monster.status = "damaged";
          scaleListener.correctNotePlayed = false;    //reset
        }

        if (monster.playerHealthDamaged)
        {
          player.takeDamage(1);
          monster.playerHealthDamaged = false;        //reset
        }

      }
      else {
        playerController.setMoveActivate(true);
      }

    }

  }

  // functions
  void OnEnable(){
    SceneManager.sceneLoaded += OnLevelFinishedLoading;
  }

  void OnDisable(){
    SceneManager.sceneLoaded -= OnLevelFinishedLoading;
  }

  void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
    if(playerRef == null){
      playerRef = GameObject.Find("Player").GetComponent<Transform>();
      playerController = playerRef.GetComponent<PlayerController>();
    }
    if(soundPlayer == null){
      soundPlayer = GameObject.Find("SoundPlayer").GetComponent<SoundPlayer>();
    }
  }

  void setAllCombatStatus(bool status){
    monster.inCombat = status;
    scaleListener.inCombat = status;
    soundPlayer.inCombat = status;
  }
}
