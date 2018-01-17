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

    private int damage;
    private bool gameOver;
    private bool startedPlaying;
    private float fov;
    private PlayerController playerController;

	void Start () {
        int fightBaseKey = Random.Range(48, 55);  // Picking Random Base Key
        int scale = Random.Range(0, 5);           // Picking Random Scale           NOTE: currently between 0 and 5 to keep it easier
        scaleListener.InitFightScale(fightBaseKey, scale);
        monster.attackDistance = monsterAttackDistance;
        monster.viewDistance = monsterViewDistance;
        monster.viewAngle = monsterViewAngle;
        damage = 0;
        gameOver = false;
        startedPlaying = false;
        infowindow.SetActive(false);
        fov = Camera.main.fieldOfView;
        playerController = playerRef.GetComponent<PlayerController>();
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
        scaleListener.inCombat = true;                          //when in combat, activate scaleListener hit/miss/win mechanics; can still play notes at any time
        Camera.main.fieldOfView = 40f;                          //zoom in camera to go into 'combat mode'
        playerController.setMoveActivate(false);

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
