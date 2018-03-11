using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MonsterManager : NoteStateControl
{
  // GameObjects
  public Transform player;
  public Transform hud;
  public PlayerController player_controller;
  public Health health;
  public GameObject info_image;
  public Text infobox;
  public Object nextScene;
  public Score score;

  public bool autoLoadNextScene = false;
  public int numMonstersPerChunk = 0;
  public bool hideBaseMonster = false;  //if generating monsters, we need a template monster already in the scene - this bool determines if this monster is hidden or not
  public EndlessTerrain terrainGenerator;

  [HideInInspector]
  public List<Monster> monsters;  //do NOT make this private

  // settings
  public float viewDistance = 2f;
  public float attackDistance = 1f;
  public float viewAngle = 60f;

  [HideInInspector]
  public ScaleNames scale_name;

  private bool activated;
  private bool chasing;
  private bool fight_mode;
  private int currentMonsterId;
  private bool initialisedMonsters;
  private bool proceduralMode;
  private GameObject gameOverScreen;

  private int c_pos;

  void Start()
  {
    // objects inits
    if (player == null)
    {
      player = GameObject.Find("Player").GetComponent<Transform>();
      player_controller = player.GetComponent<PlayerController>();
      health = player.GetComponent<Health>();
    }
    if (sound_player == null)
    {
      sound_player = GameObject.Find("SoundPlayer").GetComponent<SoundPlayer>();
    }
    if (container == null)
    {
      container = GameObject.Find("ContainerManager").GetComponent<ContainerManager>();
    }
    if (score == null)
    {
      score = GameObject.Find("GameState").GetComponent<Score>();
    }
    if (hud == null)
    {
      hud = GameObject.Find("HUDCanvas").GetComponent<Transform>();
      info_image = hud.transform.GetChild(3).gameObject;
    }
    if (infobox == null)
    {
      infobox = GameObject.Find("HUDCanvas/Info/Text").GetComponent<Text>();
    }
    infobox.text = "message";
    // init vars
    info_image.SetActive(false);
    proceduralMode = (numMonstersPerChunk > 0) ? true : false;
    gameOverScreen = GameObject.Find("MonsterScreen");
    gameOverScreen.SetActive(false);

    Monster[] tempMonsters = (Monster[])GameObject.FindObjectsOfType(typeof(Monster));  //there must be at least one monster already in the game!!! this is the baseMonster for PCG
    foreach (Monster monster in tempMonsters)
    {
      monsters.Add(monster);
    }
    chasing = false;
    fight_mode = false;
    activated = false;

    //TODO: have list of learnt scales and this random selection should be limited to the list of scales the player has learnt
    //right now we use any scale and any base key
    List<ScaleNames> pcgScaleNames = new List<ScaleNames>();
    List<NoteBaseKey> pcgBaseKeys = new List<NoteBaseKey>();
    for (int i = 0; i < numMonstersPerChunk; i++)
    {
      pcgScaleNames.Add((ScaleNames)Random.Range(0, 17));  //note added 1 to max because max val is exclusive
      pcgBaseKeys.Add((NoteBaseKey)Random.Range(48, 60));  //as above
    }
    //if (terrainGenerator){
    //terrainGenerator.scaleNames = pcgScaleNames;
    //terrainGenerator.baseKeys = pcgBaseKeys;
    //terrainGenerator.monsterManager = this;
    //terrainGenerator.numMonstersPerChunk = numMonstersPerChunk;
    //terrainGenerator.baseMonster = monsters[0];
    //}

    if (hideBaseMonster)
    {
      monsters[0].defeated = true;
      monsters[0].gameObject.SetActive(false);
    }
  }

  // update
  void Update()
  {
    bool monsterActivatedThisTurn = false;
    int result = 0;

    // init
    if (!initialisedMonsters && monsters != null)
    {
      initMonsters();
      return;
    }
    // current monster activated
    else if (activated)
    {
      //update just the active monster, activated = false when combat has finished
      result = UpdateMonster(currentMonsterId);
      monsterActivatedThisTurn = true;
    }
    // search for monster to activate
    else
    {
      //cycle through all monsters and activate first one which is within combat distance, if any
      for (int i = 0; i < monsters.Count; i++)
      {
        // trash dying monsters
        if (monsters[i].dying)
        {
          int death_hash = Animator.StringToHash("Base Layer.death");
          AnimatorStateInfo stateInfo = monsters[i].anim.GetCurrentAnimatorStateInfo(0);
          if (stateInfo.fullPathHash == death_hash)
          {
            score.updateDefMonster();
            Destroy(monsters[i].gameObject);
            monsters.RemoveAt(i);
          }
          continue;
        }

        // defeated monster
        // DO NOT DELETE BASE MONSTER (0);
        else if (i != 0 && monsters[i].defeated)
        {

          monsters[i].dying = true;
          Camera.main.fieldOfView = 60f;
          player_controller.exitPlayMode();
          monsters[i].anim.SetTrigger("die");
        }
        // player in view
        else if (!monsterActivatedThisTurn && !monsters[i].defeated && Vector3.Distance(player.position, monsters[i].transform.position) < viewDistance && Vector3.Angle(player.position - monsters[i].transform.position, monsters[i].transform.forward) < viewAngle)
        {
          monsterActivatedThisTurn = true;
          currentMonsterId = i;
          base_key = monsters[i].base_key_monster;
          result = UpdateMonster(i);
          monsters[i].anim.SetTrigger("spotPlayer");
          sound_player.monster_spot.Play();
          Debug.Log("Player spotted");
        }
        //don't do this for disabled baseMonster
        /*
        else if (monsters[i].gameObject.activeSelf)  
        {
          //deactivate everything
          //monsters[i].anim.SetBool("isRunning", false);
          //monsters[i].anim.SetBool("isWalking", false);
          //monsters[i].anim.SetBool("isAttacking", false);
          //monsters[i].anim.SetBool("isIdle", true);
          monsters[i].anim.SetTrigger("goIdle");
          if (i == (monsters.Count - 1) && !monsterActivatedThisTurn)
          {
            //this fixes a bug where monsters were stopping then instantly reactivating due to player proximity
            activated = false;
          }
        }
        */
      }
    }

    // lose
    if (result == 1 || health.GetHealthAmount() == 0)
    {
      StartCoroutine(LoseGame());
    }

    //load next level if player has defeated all the monsters
    /*
    bool allMonstersDefeated = true;
    foreach (Monster monster in monsters){
      if(monster.defeated == false){
        allMonstersDefeated = false;
        break;
      }
    }
    if(allMonstersDefeated && autoLoadNextScene){
      SceneManager.LoadScene(nextScene.name);
    }
    */

    //TODO: add key listener for buttons that activate scales
    // set soundPlayer.inLearning = true;
    //should let player play notes without activating combat 
  }

  //return values: 0 no action, 1 player lost
  int UpdateMonster(int id)
  {

    Vector3 direction = player.position - monsters[id].transform.position;
    // start the scale
    if (!activated)
    {
      initMonsterScale(id);
    }
    //too far away to attack, chase player
    else if (activated && !chasing && !fight_mode)
    {
      Debug.Log("chase");
      if (direction.magnitude > attackDistance)
      {
        chasing = true;
      }
    }
    // chasing update
    else if (activated && chasing && !fight_mode)
    {
      if (!proceduralMode) monsters[id].nav.destination = player.position;
      // stop chasing
      if (direction.magnitude > viewDistance)
      {
        Debug.Log("Out of Sight");
        monsters[id].nav.destination = monsters[id].transform.position;
        chasing = false;
        activated = false;
        monsters[id].anim.SetTrigger("calm");
      }
      // start fight
      else if (direction.magnitude < attackDistance)
      {
        //monsters[id].nav.destination = monsters[id].transform.position;
        startFight(id);
        monsters[id].nav.ResetPath();
        monsters[id].nav.isStopped = true;
      }
    }

    // stop the fight
    /*
    else if (fight_mode && run)
    {
      exitMonsterScale(id);
      return 0;
    }
    */
    // play the scale
    else if (fight_mode && player_controller.hamlinReadyToPlay())
    {
      //monsters[id].anim.SetBool("isRunning", false);
      //monsters[id].anim.SetBool("isWalking", false);
      //monsters[id].anim.SetBool("isAttacking", false);
      //monsters[id].anim.SetBool("isIdle", true);

      if (!proceduralMode && !monsters[id].nav.isStopped)
      {
        monsters[id].nav.ResetPath();
        monsters[id].nav.isStopped = true;
      }
      int key = 0;
      bool[] key_mask = getKeyMask();
      //player has beaten monster
      if (c_pos >= monsters[id].box_scale.Length)
      {
        winFight(id);
        return 0;
      }
      //game ends if they run out of health
      else if (health.GetHealthAmount() == 0)
      {
        print("player out of health");
        activated = false;
        return 1;
      }
      // run the scale
      else
      {
        //zoom in camera to go into 'combat mode'
        Camera.main.fieldOfView = 40f;
        monsters[id].transform.LookAt(player);
        //TODO: we need this but need to change the player transform somehow as it's the wrong angle
        // check each key
        foreach (bool mask in key_mask)
        {
          if (mask)
          {
            cleanWrongNoteState(monsters[id].box_scale);
            int note_midi = keyToMidiMapping(key);
            int note_pos = midiToContainerMapping(note_midi);
            // right note
            if (note_midi == monsters[id].box_midi[c_pos])
            {
              note_state[c_pos][note_pos] = NoteState.RIGHT;
              sign_state[c_pos][note_pos] = midiToSignState(note_midi);
              monsters[id].anim.SetTrigger("hurt");
              sound_player.monster_hurt.Play();
              c_pos++;
            }
            // wrong note
            else
            {
              note_state[c_pos][note_pos] = NoteState.WRONG;
              sign_state[c_pos][note_pos] = midiToSignState(note_midi);
              player_controller.getAttacked();
              monsters[id].anim.SetTrigger("attack");
              sound_player.monster_attack.Play();
              //monsters[id].anim.SetBool("isAttacking", true);
              //monsters[id].anim.SetBool("isIdle", false);
              monsters[id].playerDamageQueue++;
            }
          }
          key++;
        }
      }
      container.updateNoteContainer(note_state);
      container.updateSignContainer(sign_state);
    }
    return 0;
  }

  // init Monster Scale
  private void initMonsterScale(int id)
  {
    c_pos = 0;
    activated = true;
    //sound_player.activate_sound.Play();
    // put scale
    setNoteStateToScale(monsters[id].box_scale);
    setSignStateToScale(monsters[id].box_scale);
    container.updateScaleInd(scale_name, base_key);
    string scaleText = castScale((int)monsters[id].scale_name);
    string baseKeyText = castBaseKey((int)(monsters[id].base_key_monster - 48));
    player_controller.changeScaleText(scaleText);
    player_controller.changeBaseKeyText(baseKeyText);
    if (proceduralMode)
    {
      //E.g. "A wild C Dorian monster appears!". A bit of fun :)
      infobox.text = "A wild " + baseKeyText + " " + scaleText + " monster appears!";
      info_image.SetActive(true);
    }
  }

  // start the fight
  private void winFight(int id)
  {
    //ToDo win sound
    //sound_player.activate_sound.Play();
    //player_controller.setMoveActivate(true);
    StartCoroutine(ShowMessage("You win!", 3f, false));
    monsters[id].defeated = true;
    exitMonsterScale(id);
    sound_player.monster_die.Play();
    sound_player.monster_win.Play();
  }

  // start the fight
  private void startFight(int id)
  {
    Debug.Log("Start Fight");
    fight_mode = true;
    chasing = false;
    sound_player.inCombat = true;
    player_controller.forceActivateCombat = true;
    monsters[id].anim.SetTrigger("fight");
    if (proceduralMode) info_image.SetActive(true);
  }

  // start the fight
  private void exitFight(int id)
  {
    fight_mode = false;
    sound_player.inCombat = false;
    //player_controller.setMoveActivate(false);
    if (proceduralMode) info_image.SetActive(false);
  }

  // exit Monster Scale
  private void exitMonsterScale(int id)
  {
    c_pos = 0;
    activated = false;
    chasing = false;
    //player_controller.setMoveActivate(true);
    exitFight(id);
    player_controller.exitPlayMode();
    resetNoteState();
    resetSignState();
    container.resetScaleInd();
    if (!proceduralMode) monsters[id].nav.isStopped = false;
  }

  // show "lost game" end game screen
  IEnumerator LoseGame()
  {
    gameOverScreen.SetActive(true);
    yield return new WaitForSecondsRealtime(10);
    SceneManager.LoadScene("MainMenu_pablo");
  }

  //do not call directly, call with StartCoroutine(ShowMessage(...))
  IEnumerator ShowMessage(string message, float delay, bool endGame)
  {
    infobox.text = message;
    info_image.SetActive(true);
    yield return new WaitForSeconds(delay);
    info_image.SetActive(false);
    if (endGame)
    {
      SceneManager.LoadScene("MainMenu_pablo");
    }
  }

  // init monster
  public void initMonsters()
  {
    for (int i = 0; i < monsters.Count; i++)
    {
      base_key = monsters[i].base_key_monster;
      resetNoteState();
      resetSignState();
      // container position
      monsters[i].box_scale = allScales[(int)monsters[i].scale_name];
      monsters[i].box_midi = scaleToMidi(monsters[i].box_scale);
      container.updateNoteContainer(note_state);
      container.updateSignContainer(sign_state);
    }
    initialisedMonsters = true;
  }

}
