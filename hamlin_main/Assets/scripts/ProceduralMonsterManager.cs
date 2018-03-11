using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ProceduralMonsterManager : NoteStateControl
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
  public EndlessTerrain terrainGenerator;

  [HideInInspector]
  public List<Monster> monsters;  //do NOT make this private

  // settings
  public float viewDistance = 2f;
  public float attackDistance = 2f;
  public float viewAngle = 60f;

  private bool activated;
  private bool fight_mode;
  private int currentMonsterId;
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
    gameOverScreen = GameObject.Find("GameOverScreen");
    gameOverScreen.SetActive(false);

    Monster[] tempMonsters = (Monster[])GameObject.FindObjectsOfType(typeof(Monster));  //there must be at least one monster already in the game!!! this is the baseMonster for PCG
    foreach (Monster monster in tempMonsters)
    {
      monsters.Add(monster);
    }
    fight_mode = false;
    activated = false;

    List<ScaleNames> pcgScaleNames = new List<ScaleNames>();
    List<NoteBaseKey> pcgBaseKeys = new List<NoteBaseKey>();
    for (int i = 0; i < numMonstersPerChunk; i++)
    {
      pcgScaleNames.Add((ScaleNames)Random.Range(0, 17));  //note added 1 to max because max val is exclusive
      pcgBaseKeys.Add((NoteBaseKey)Random.Range(48, 60));  //as above
    }
    //don't comment this out, this is important
    if (terrainGenerator)
    {
      terrainGenerator.scaleNames = pcgScaleNames;
      terrainGenerator.baseKeys = pcgBaseKeys;
      terrainGenerator.monsterManager = this;
      terrainGenerator.numMonstersPerChunk = numMonstersPerChunk;
      terrainGenerator.baseMonster = monsters[0];
    }

    monsters[0].defeated = true;
    monsters[0].gameObject.SetActive(false);

    print("proc monster manager init");

  }

  // update
  void Update()
  {
    bool monsterActivatedThisTurn = false;
    int result = 0;

    // current monster activated
    if (activated)
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
          print("destroying dying monster");
          int death_hash = Animator.StringToHash("Base Layer.death");
          AnimatorStateInfo stateInfo = monsters[i].anim.GetCurrentAnimatorStateInfo(0);
          if (stateInfo.fullPathHash == death_hash)
          {
            terrainGenerator.viewerPositionsOld.RemoveAt(terrainGenerator.allViewers.IndexOf(monsters[i].transform));
            terrainGenerator.allViewers.Remove(monsters[i].transform);
            Destroy(monsters[i].gameObject);
            monsters.RemoveAt(i);
          }
          continue;
        }

        // defeated monster
        // DO NOT DELETE BASE MONSTER (0);
        else if (i != 0 && monsters[i].defeated)
        {
          print("monster defeated");
          monsters[i].dying = true;
          player_controller.exitPlayMode();
          score.updateDefMonster();   //TODO: maybe move this back to destroying dying monster section
          monsters[i].anim.SetTrigger("die");
          //this is to get the stave to disappear - works but slow, would like a better method
          player_controller.hold_flute = false;
          player_controller.forceActivateCombat = true;
        }
        // player in view
        else if (!monsterActivatedThisTurn && !monsters[i].defeated && Vector3.Distance(player.position, monsters[i].transform.position) < viewDistance && Vector3.Angle(player.position - monsters[i].transform.position, monsters[i].transform.forward) < viewAngle)
        {
          print("found monster to activate");
          monsterActivatedThisTurn = true;
          currentMonsterId = i;
          base_key = monsters[i].base_key_monster;
          result = UpdateMonster(i);
          monsters[i].anim.SetTrigger("spotPlayer");
        }
      }
    }

    // lose
    if (result == 1 || health.GetHealthAmount() == 0)
    {
      StartCoroutine(LoseGame());
    }

  }

  //return values: 0 no action, 1 player lost
  int UpdateMonster(int id)
  {
    print("updating monster");
    Vector3 direction = player.position - monsters[id].transform.position;
    // start the scale
    if (!activated)
    {
      initMonster(id);
      initMonsterScale(id);
      print("activating");
    }
    else if (!fight_mode && direction.magnitude < attackDistance)
    {
      startFight(id);
      print("starting fight");
    }
    else if (fight_mode && player_controller.hamlinReadyToPlay())
    {
      print("playing scale");
      int key = 0;
      bool[] key_mask = getKeyMask();
      //player has beaten monster
      if (c_pos >= monsters[id].box_scale.Length)
      {
        print("winning fight");
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
        monsters[id].transform.LookAt(player);
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
              c_pos++;
            }
            // wrong note
            else
            {
              note_state[c_pos][note_pos] = NoteState.WRONG;
              sign_state[c_pos][note_pos] = midiToSignState(note_midi);
              player_controller.getAttacked();
              monsters[id].anim.SetTrigger("attack");
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
    container.updateScaleInd(monsters[id].scale_name, monsters[id].base_key_monster);
    string scaleText = castScale((int)monsters[id].scale_name);
    string baseKeyText = castBaseKey((int)(monsters[id].base_key_monster - 48));
    player_controller.changeScaleText(scaleText);
    player_controller.changeBaseKeyText(baseKeyText);
    //E.g. "A wild C Dorian monster appears!". A bit of fun :)
    infobox.text = "A wild " + baseKeyText + " " + scaleText + " monster appears!";
    info_image.SetActive(true);
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
  }

  // start the fight
  private void startFight(int id)
  {
    Debug.Log("Start Fight");
    fight_mode = true;
    sound_player.inCombat = true;
    player_controller.forceActivateCombat = true;
    monsters[id].anim.SetTrigger("fight");
    info_image.SetActive(true);
  }

  // exit fight
  private void exitFight(int id)
  {
    fight_mode = false;
    sound_player.inCombat = false;
    //player_controller.setMoveActivate(false);
    info_image.SetActive(false);
  }

  // exit Monster Scale
  private void exitMonsterScale(int id)
  {
    c_pos = 0;
    activated = false;
    //player_controller.setMoveActivate(true);
    exitFight(id);
    player_controller.exitPlayMode();
    resetNoteState();
    resetSignState();
    container.resetScaleInd();
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
  public void initMonster(int i)
  {
    base_key = monsters[i].base_key_monster;
    resetNoteState();
    resetSignState();
    // container position
    monsters[i].box_scale = allScales[(int)monsters[i].scale_name];
    monsters[i].box_midi = scaleToMidi(monsters[i].box_scale);
    container.updateNoteContainer(note_state);
    container.updateSignContainer(sign_state);
    monsters[i].gameObject.SetActive(true);
  }

}
