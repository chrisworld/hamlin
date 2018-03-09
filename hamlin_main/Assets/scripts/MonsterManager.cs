﻿using System.Collections;
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

  private bool activated;
  //private bool chasing;
  private int currentMonsterId;
  private bool initialisedMonsters;
  private bool proceduralMode;

  private int c_pos;
  private int error_counter;

  void Start()
  {
    // objects inits
    if(player == null){
      player = GameObject.Find("Player").GetComponent<Transform>();
      player_controller = player.GetComponent<PlayerController>();
      health = player.GetComponent<Health>();
    }
    if(sound_player == null){
      sound_player = GameObject.Find("SoundPlayer").GetComponent<SoundPlayer>();
    }
    if(container == null){
      container = GameObject.Find("ContainerManager").GetComponent<ContainerManager>();
    }
    if(score == null){
      score = GameObject.Find("GameState").GetComponent<Score>();
    }
    if(hud == null){
      hud = GameObject.Find("HUDCanvas").GetComponent<Transform>();
      info_image = hud.transform.GetChild(3).gameObject;
      
      //infobox = info_image.GetComponent<Text>();
    }
    // init vars
    info_image.SetActive(false);
    proceduralMode = (numMonstersPerChunk > 0) ? true : false;

    Monster[] tempMonsters = (Monster[])GameObject.FindObjectsOfType(typeof(Monster));  //there must be at least one monster already in the game!!! this is the baseMonster for PCG
    foreach (Monster monster in tempMonsters){
      monsters.Add(monster);
    }   
    //chasing = false;
    activated = false;

    //TODO: have list of learnt scales and this random selection should be limited to the list of scales the player has learnt
    //right now we use any scale and any base key
    List<ScaleNames> pcgScaleNames = new List<ScaleNames>();
    List<NoteBaseKey> pcgBaseKeys = new List<NoteBaseKey>();
    for(int i=0; i < numMonstersPerChunk; i++){
      pcgScaleNames.Add( (ScaleNames) Random.Range(0, 17) );  //note added 1 to max because max val is exclusive
      pcgBaseKeys.Add( (NoteBaseKey) Random.Range(48, 60) );  //as above
    }
    //don't comment this out, this is important
    if (terrainGenerator){
      terrainGenerator.scaleNames = pcgScaleNames;
      terrainGenerator.baseKeys = pcgBaseKeys;
      terrainGenerator.monsterManager = this;
      terrainGenerator.numMonstersPerChunk = numMonstersPerChunk;
      terrainGenerator.baseMonster = monsters[0];
    }

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
    if (!initialisedMonsters && monsters != null){
      initMonsters();
      return;
    }
    // monster update
    if (activated)
    {           
      //update just the active monster, activated = false when combat has finished
      result = UpdateMonster(currentMonsterId);
      monsterActivatedThisTurn = true;
    }
    else
    {                  
      //cycle through all monsters and activate first one which is within combat distance, if any
      for (int i = 0; i < monsters.Count; i++)
      { 
        // monsters dying
        if (monsters[i].dying){
          continue;
        }
        // defeated monser
        // DO NOT DELETE BASE MONSTER (0);
        if (i != 0 && monsters[i].defeated){
          // ToDo anim
          monsters[i].dying = true;
          Camera.main.fieldOfView = 60f;
          player_controller.exitPlayMode();
          score.updateDefMonster();
          Destroy(monsters[i].gameObject, 1);
        }
        // something
        else if (!monsterActivatedThisTurn && Vector3.Distance(player.position, monsters[i].transform.position) < viewDistance && Vector3.Angle(player.position - monsters[i].transform.position, monsters[i].transform.forward) < viewAngle)
        {
          monsterActivatedThisTurn = true;
          currentMonsterId = i;
          result = UpdateMonster(i);
        }
        //don't do this for disabled baseMonster
        else if (monsters[i].gameObject.activeSelf)  
        {
          //deactivate everything
          monsters[i].anim.SetBool("isRunning", false);
          monsters[i].anim.SetBool("isWalking", false);
          monsters[i].anim.SetBool("isAttacking", false);
          monsters[i].anim.SetBool("isIdle", true);
          if (i == (monsters.Count - 1) && !monsterActivatedThisTurn)
          {
            //this fixes a bug where monsters were stopping then instantly reactivating due to player proximity
            activated = false;
          }
        }
      }
    }
    // loose
    if (result == 1 || health.GetHealthAmount() == 0){
      looseGame();
    }

    //load next level if player has defeated all the monsters
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

  //TODO: add key listener for buttons that activate scales
  // set soundPlayer.inLearning = true;
  //should let player play notes without activating combat 
  }

  //return values: 0 no action, 1 player lost
  int UpdateMonster(int id) {

    //do nothing if player has already defeated monster
    if(monsters[id].defeated == true){
      //TODO: make monster run away / disappear
      // update score
      score.updateDefMonster();
      return 0;
    }

    Vector3 direction = player.position - monsters[id].transform.position;
    // start the scale
    if (!activated && Input.anyKey)
    {
      activated = true;
      // put scale
      base_key = monsters[id].base_key; //hack to avoid refactoring NoteStateControl
      setNoteStateToScale(monsters[id].box_scale);
      setSignStateToScale(monsters[id].box_scale);
      c_pos = 0;
      error_counter = 0;
      sound_player.inCombat = true;
			player_controller.changeScaleText (castScale((int)monsters[id].scale_name));
			player_controller.changeBaseKeyText (castBaseKey((int)(monsters[id].base_key-48)));
     }
     // stop the scale
     else if (activated && player_controller.checkValidJumpKey())
     {
        activated = false;
        player_controller.setMoveActivate(true);
        c_pos = 0;
        error_counter = 0;
        resetNoteState();
        resetSignState();
        sound_player.inCombat = false;
        if(!proceduralMode) monsters[id].nav.isStopped = false;
        return 0;
     }
      else if (activated && (direction.magnitude > attackDistance))     //too far away to attack, chase player
      {
        monsters[id].anim.SetBool("isRunning", false);
        monsters[id].anim.SetBool("isWalking", true);
        monsters[id].anim.SetBool("isAttacking", false);
        monsters[id].anim.SetBool("isIdle", false);
        if(!proceduralMode)  monsters[id].nav.destination = player.position;  //chasing currently NOT implemented for proceduralMode as no nav mesh
        //chasing = true; 
      }
      // play the scales
      else if (activated)
      {
        monsters[id].anim.SetBool("isRunning", false);
        monsters[id].anim.SetBool("isWalking", false);
        monsters[id].anim.SetBool("isAttacking", false);
        monsters[id].anim.SetBool("isIdle", true);
      if (!proceduralMode && !monsters[id].nav.isStopped)
        {
          monsters[id].nav.ResetPath();
          monsters[id].nav.isStopped = true;
        }
        player_controller.setMoveActivate(false);
        int key = 0;
        bool[] key_mask = getKeyMask();
        if (c_pos >= monsters[id].box_scale.Length)          //player has beaten monster
        {
          activated = false;
          StartCoroutine(ShowMessage("You defeated the monster!", 3f, false));
          resetNoteState();
          resetSignState();
          container.resetScaleInd();
          //player_controller.setMoveActivate(true);
          player_controller.exitPlayMode();
          monsters[id].defeated = true;
          return 0;
        }
        else if (health.GetHealthAmount() == 0)         //game ends if they run out of health
        {
          print("player out of health");
          activated = false;
          return 1;
        }
        else
        {
          //zoom in camera to go into 'combat mode'
          Camera.main.fieldOfView = 40f;
          monsters[id].transform.LookAt(player);   //TODO: we need this but need to change the player transform somehow as it's the wrong angle

          // check each key
          foreach (bool mask in key_mask)
          {
            if (mask)
            {
              cleanWrongNoteState(monsters[id].box_scale);
              int note_midi = keyToMidiMapping(key);
              int note_pos = midiToContainerMapping(note_midi);
              if (note_midi == monsters[id].box_midi[c_pos])
              {
                note_state[c_pos][note_pos] = NoteState.RIGHT;
                sign_state[c_pos][note_pos] = midiToSignState(note_midi);
                c_pos++;
              }
              else
              {
                note_state[c_pos][note_pos] = NoteState.WRONG;
                sign_state[c_pos][note_pos] = midiToSignState(note_midi);
                error_counter++;
                player_controller.getAttacked();
                monsters[id].anim.SetBool("isAttacking", true);
                monsters[id].anim.SetBool("isIdle", false);
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

  // init monster
  public void initMonsters(){
    for (int i = 0; i < monsters.Count; i++)
      {
        resetNoteState();
        resetSignState();
        // container position
        c_pos = 0;
        error_counter = 0;
        base_key = monsters[i].base_key;  //hack to avoid refactoring NoteStateControl
        monsters[i].box_scale = allScales[(int)monsters[i].scale_name];
        monsters[i].box_midi = scaleToMidi(monsters[i].box_scale);
        setNoteStateToScale(monsters[i].box_scale);
        setSignStateToScale(monsters[i].box_scale);
        container.updateScaleInd(monsters[i].scale_name, monsters[i].base_key);

        container.updateNoteContainer(note_state);
        container.updateSignContainer(sign_state);
      }
      initialisedMonsters = true;
  }

  // loose condition
  private void looseGame(){
    resetNoteState();
    resetSignState();
    player_controller.setMoveActivate(true);
    StartCoroutine(ShowMessage("You lose :'(", 3f, true));
  }

  //do not call directly, call with StartCoroutine(ShowMessage(...))
  IEnumerator ShowMessage(string message, float delay, bool endGame)
  {
    infobox.text = message;
    info_image.SetActive(true);
    yield return new WaitForSeconds(delay);
    info_image.SetActive(false);
    if(endGame){
      SceneManager.LoadScene("MainMenu_pablo");
    }

  }
	public string castScale(int scale){

		if (scale == 0) {
			return "CHROMATIC";
		}
		if (scale == 1){
			return "MAJOR";
		}
		if (scale == 2) {
			return "MINOR";
		}
		if (scale == 3) {
			return "HARMONIC MINOR";
		}
		if (scale == 4) {
			return "MELODIC MINOR";
		}
		if (scale == 5) {
			return "NATURAL MINOR";
		}
		if (scale == 6) {
			return "DIATONIC MINOR";
		}
		if (scale == 7) {
			return "AEOLIAN";
		}
		if (scale == 8) {
			return "PHRYGIAN";
		}
		if (scale == 9) {
			return "LOCRIAN";
		}
		if (scale == 10) {
			return "DORIAN";
		}
		if (scale == 11) {
			return "LYDIAN";
		}
		if (scale == 12) {
			return "MIXOLYDIAN";
		}
		if (scale == 13) {
			return "PENTATONIC";
		}
		if (scale == 14) {
			return "BLUES";
		}
		if (scale == 15) {
			return "TURKISH";
		}
		if (scale == 16) {
			return "INDIAN";
		}
		else{
			return "";
		}
	}

	// cast Base Key
	public string castBaseKey(int key){

		if (key == 0) {
			return "C";
		}
		if (key == 1){
			return "C#/Db";
		}
		if (key == 2) {
			return "D";
		}
		if (key == 3) {
			return "D#/Eb";
		}
		if (key == 4) {
			return "E";
		}
		if (key == 5) {
			return "F";
		}
		if (key == 6) {
			return "F#/Gb";
		}
		if (key == 7) {
			return "G";
		}
		if (key == 8) {
			return "G#/Ab";
		}
		if (key == 9) {
			return "A";
		}
		if (key == 10) {
			return "A#/Bb";
		}
		if (key == 11) {
			return "B";
		}
		else{
			return "";
		}
	}
}
